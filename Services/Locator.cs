using System.Text;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Formatting;
using PrettyDocComments.Model;

namespace PrettyDocComments.Services;

internal sealed class Locator
{
    private readonly Regex _docCommentRegex;
    private readonly double _averageCharWidth;

    public Locator(Regex docCommentRegex, System.Drawing.Font editorFont)
    {
        _docCommentRegex = docCommentRegex;
        _averageCharWidth = GetAverageCharWidth(editorFont);
    }

    public bool TryGetComment(ITextSnapshot textSnapshot, ITextViewLine line, out Comment<string> comment)
    {
        int lineNumber = textSnapshot.GetLineNumberFromPosition(line.Start.Position);
        Match match = _docCommentRegex.Match(textSnapshot.GetLineFromLineNumber(lineNumber).GetText());
        if (match.Success) {
            StringBuilder sb = new();
            int maxTextLength = 0;
            int lastLineNumber = 0;
            int commentLeftCharIndex = match.Groups[1].Index;

            // Find first comment line
            while (lineNumber > 0 &&
                _docCommentRegex.IsMatch(textSnapshot.GetLineFromLineNumber(lineNumber - 1).GetText())) {
                lineNumber--;
            }
            SnapshotPoint startPoint = textSnapshot.GetLineFromLineNumber(lineNumber).Start;
            int firstLineNumber = lineNumber;
            while (lineNumber < textSnapshot.LineCount) {
                string text = textSnapshot.GetLineFromLineNumber(lineNumber).GetText();
                match = _docCommentRegex.Match(text);
                if (!match.Success) {
                    break;
                }
                lastLineNumber = lineNumber;
                string commentText = text.Substring(match.Groups[2].Index);
                sb.AppendLine(commentText.Trim());
                maxTextLength = Math.Max(maxTextLength, commentText.Length);
                lineNumber++;
            }
            SnapshotPoint endPoint = textSnapshot.GetLineFromLineNumber(lastLineNumber).Start;
            comment = new Comment<string>(new SnapshotSpan(startPoint, endPoint), commentLeftCharIndex,
                firstLineNumber, lastLineNumber, _averageCharWidth * (maxTextLength + 3), sb.ToString());
            return true;
        }
        comment = default;
        return false;
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
