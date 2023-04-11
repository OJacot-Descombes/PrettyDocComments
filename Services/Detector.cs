using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Formatting;
using PrettyDocComments.Model;

namespace PrettyDocComments.Services;

internal class Detector
{
    private readonly IWpfTextView _view;
    private readonly Regex _docCommentRegex;
    private readonly double _averageCharWidth;

    public Detector(IWpfTextView view, Regex docCommentRegex, System.Drawing.Font editorFont)
    {
        _docCommentRegex = docCommentRegex;
        _view = view;
        _averageCharWidth = GetAverageCharWidth(editorFont);
    }

    public IEnumerable<OriginalComment> GetVisibleComments(ITextSnapshot textSnapshot)
    {
        int commentStartLineIndex = 0;
        int maxTextLength = 0;
        double left = -1;
        bool isInComment = false;

        var (commentStart, visibleStart, visibleEnd) = GetLineIndexRange(textSnapshot);
        int lineIndex = commentStart;


        SnapshotPoint visibleStartPoint = textSnapshot.GetLineFromLineNumber(visibleStart).Start;
        TextBounds visibleStartBounds = _view.TextViewLines.GetCharacterBounds(visibleStartPoint);

        SnapshotPoint visibleEndPoint = textSnapshot.GetLineFromLineNumber(visibleEnd).Start;
        double visibleBottom = _view.TextViewLines.GetCharacterBounds(visibleEndPoint).Bottom;

        StringBuilder sb = new();
        while (lineIndex <= (isInComment ? textSnapshot.LineCount : Math.Min(visibleEnd, textSnapshot.LineCount))) {
            ITextSnapshotLine textLine = textSnapshot.GetLineFromLineNumber(lineIndex);
            string text = textLine.GetText();
            Match match = _docCommentRegex.Match(text);
            if (match.Success) {
                if (!isInComment) { // Start of new doc comment.
                    commentStartLineIndex = lineIndex;
                    isInComment = true;
                    maxTextLength = 0;
                    left = -1;
                }

                string commentText = text.Substring(match.Groups[2].Index);
                sb.AppendLine(commentText.Trim());
                maxTextLength = Math.Max(maxTextLength, commentText.Length);
                if (left == -1 && lineIndex >= visibleStart && lineIndex <= visibleEnd) {
                    left = GetLeftBound(textSnapshot, lineIndex, match);
                }
            } else if (isInComment) { // Reached end of comment, return DocComment structure.
                double top, bottom;
                SnapshotPoint startPoint = textSnapshot.GetLineFromLineNumber(commentStartLineIndex).Start;
                if (commentStartLineIndex >= visibleStart) {
                    top = _view.TextViewLines.GetCharacterBounds(startPoint).Top;
                } else { // Line is not in TextViewLines buffer. 
                    top = visibleStartBounds.Top - _view.LineHeight * (visibleStart - commentStartLineIndex);
                }

                int previousIndex = lineIndex - 1;
                SnapshotPoint endPoint = textSnapshot.GetLineFromLineNumber(previousIndex).Start;
                if (previousIndex <= visibleEnd) {
                    bottom = _view.TextViewLines.GetCharacterBounds(endPoint).Bottom;
                } else {
                    bottom = visibleBottom + _view.LineHeight * (previousIndex - visibleEnd);
                }
                var rect = new Rect(left, top, _averageCharWidth * (maxTextLength + 3), bottom - top);
                var docComment = new OriginalComment(new SnapshotSpan(startPoint, endPoint), rect, sb.ToString());
                yield return docComment;
                isInComment = false;
                sb.Clear();
            }
            lineIndex++;
        }
    }

    private (int commentStart, int visibleStart, int visibleEnd) GetLineIndexRange(ITextSnapshot textSnapshot)
    {
        int visibleStart = _view.TextViewLines.FirstVisibleLine.Start.GetContainingLineNumber();
        int visibleEnd = _view.TextViewLines.LastVisibleLine.Start.GetContainingLineNumber();

        int commentStart = visibleStart;
        if (commentStart < textSnapshot.LineCount) {
            while (commentStart > 0 &&
                _docCommentRegex.IsMatch(textSnapshot.GetLineFromLineNumber(commentStart).GetText())) {

                commentStart--;
            }
        }

        return (commentStart, visibleStart, visibleEnd);
    }

    private double GetLeftBound(ITextSnapshot textSnapshot, int lineIndex, Match match)
    {
        double left;
        SnapshotPoint lineStart = textSnapshot.GetLineFromLineNumber(lineIndex).Start;
        int commentLeftCharIndex = match.Groups[1].Index;
        left = _view.TextViewLines.GetCharacterBounds(lineStart + commentLeftCharIndex).Left;
        return left;
    }

    private double GetAverageCharWidth(System.Drawing.Font font)
    {
        const string ExampleText = "The Quick Brown Fox Jumps Over The Lazy Dog";

        using var bmp = new System.Drawing.Bitmap(1, 1);
        using var g = System.Drawing.Graphics.FromImage(bmp);
        System.Drawing.SizeF size = g.MeasureString(ExampleText, font);
        return size.Width / ExampleText.Length;
    }
}
