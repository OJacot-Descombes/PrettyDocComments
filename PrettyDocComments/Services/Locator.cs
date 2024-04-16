using System.Text;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Formatting;
using PrettyDocComments.Model;

namespace PrettyDocComments.Services;

internal sealed class Locator(Regex docCommentRegex, IWpfTextView view)
{
    public bool TryGetComment(ITextSnapshot textSnapshot, ITextViewLine line, out Comment<string> comment)
    {
        int lineNumber = textSnapshot.GetLineNumberFromPosition(line.Start.Position);
        Match match = docCommentRegex.Match(textSnapshot.GetLineFromLineNumber(lineNumber).GetText());
        if (match.Success) {
            StringBuilder sb = new();
            int maxTextLength = 0;
            int lastLineNumber = 0;
            int commentLeftCharIndex = match.Groups[1].Index;

            // Find first comment line
            while (lineNumber > 0 &&
                docCommentRegex.IsMatch(textSnapshot.GetLineFromLineNumber(lineNumber - 1).GetText())) {
                lineNumber--;
            }
            SnapshotPoint startPoint = textSnapshot.GetLineFromLineNumber(lineNumber).Start;
            int firstLineNumber = lineNumber;
            while (lineNumber < textSnapshot.LineCount) {
                string text = textSnapshot.GetLineFromLineNumber(lineNumber).GetText();
                match = docCommentRegex.Match(text);
                if (!match.Success) {
                    break;
                }
                lastLineNumber = lineNumber;
                string commentText = text.Substring(match.Groups[2].Index);
                if (commentText.Length > 0 && commentText[0] == ' ') {
                    commentText = commentText.Substring(1);
                }
                sb.AppendLine(commentText);
                maxTextLength = Math.Max(maxTextLength, commentText.Length);
                lineNumber++;
            }
            SnapshotPoint endPoint = textSnapshot.GetLineFromLineNumber(lastLineNumber).Start;
            comment = new Comment<string>(new SnapshotSpan(startPoint, endPoint), commentLeftCharIndex,
                firstLineNumber, lastLineNumber, view.FormattedLineSource.ColumnWidth * (maxTextLength + 3),
                sb.Replace("&nbsp;", "\u00A0").ToString());
            return true;
        }
        comment = default;
        return false;
    }
}
