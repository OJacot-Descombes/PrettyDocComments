using System.Windows;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Editor;
using PrettyDocComments.Helpers;

namespace PrettyDocComments.Model;

internal class FormatAccumulator
{
    public FormatAccumulator(IWpfTextView view)
    {
        _view = view;
    }

    private readonly IWpfTextView _view;

    private readonly List<FormatRun> _runs = new();

    private bool _bold;
    private bool _italic;
    private bool _strikethrough;
    private bool _underline;
    private bool _code;
    private Brush _textColor = Options.DefaultTextColor;

    public bool HasText => _runs.Count > 0;

    public bool Bold { get => _bold; set => _bold = value; }
    public bool Italic { get => _italic; set => _italic = value; }
    public bool Strikethrough { get => _strikethrough; set => _strikethrough = value; }
    public bool Underline { get => _underline; set => _underline = value; }
    public bool Code { get => _code; set => _code = value; }
    public Brush TextColor { get => _textColor; set => _textColor = value; }

    public void Add(string text)
    {
        _runs.Add(new FormatRun(text, _bold, _italic, _strikethrough, _underline, _code, _textColor));
    }

    public FormattedText GetFormattedText()
    {
        string text = String.Concat(_runs.Select(r => r.Text));
        FormattedText formattedText = Factory.CreateFormattedText(text, Options.NormalTypeFace, _view);
        int startIndex = 0;
        foreach (FormatRun run in _runs) {
            int length = run.Text?.Length ?? 0;
            formattedText.SetFontTypeface(run.Code ? Options.CodeTypeFace : Options.NormalTypeFace, startIndex, length);
            formattedText.SetFontStyle(run.Italic ? FontStyles.Italic : FontStyles.Normal, startIndex, length);
            formattedText.SetFontWeight(run.Bold ? FontWeights.Bold : FontWeights.Normal, startIndex, length);
            formattedText.SetForegroundBrush(run.TextBrush, startIndex, length);
            var textDecorations = new TextDecorationCollection(2);
            if (run.Underline) {
                textDecorations.Add(TextDecorations.Underline);
            }
            if (run.Strikethrough) {
                textDecorations.Add(TextDecorations.Strikethrough);
            }
            formattedText.SetTextDecorations(textDecorations, startIndex, length);

            startIndex += length;
        }
        _runs.Clear();
        return formattedText;
    }
}
