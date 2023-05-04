using System.Windows;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Editor;
using PrettyDocComments.Helpers;

namespace PrettyDocComments.Model;

internal class FormatAccumulator
{
    public FormatAccumulator(IWpfTextView view, double indent)
    {
        _view = view;
        _indent = indent;
    }

    private readonly IWpfTextView _view;

    private readonly List<FormatRun> _runs = new();

    public readonly struct Memento : IDisposable
    {
        private readonly FormatAccumulator _originator;

        private readonly double _indent;
        private readonly bool _bold;
        private readonly bool _italic;
        private readonly bool _strikethrough;
        private readonly bool _underline;
        private readonly bool _code;
        private readonly Brush _textColor;

        internal Memento(FormatAccumulator originator)
        {
            _originator = originator;

            _indent = originator._indent;
            _bold = originator._bold;
            _italic = originator._italic;
            _strikethrough = originator._strikethrough;
            _underline = originator._underline;
            _code = originator._code;
            _textColor = originator._textColor;
        }

        void IDisposable.Dispose()
        {
            _originator._indent = _indent;
            _originator._bold = _bold;
            _originator._italic = _italic;
            _originator._strikethrough = _strikethrough;
            _originator._underline = _underline;
            _originator._code = _code;
            _originator._textColor = _textColor;
        }
    }

    private double _indent;
    private bool _bold;
    private bool _italic;
    private bool _strikethrough;
    private bool _underline;
    private bool _code;
    private Brush _textColor = Options.DefaultTextColor;

    public double Indent { get => _indent; set => _indent = value; }
    public bool Bold { get => _bold; set => _bold = value; }
    public bool Italic { get => _italic; set => _italic = value; }
    public bool Strikethrough { get => _strikethrough; set => _strikethrough = value; }
    public bool Underline { get => _underline; set => _underline = value; }
    public bool Code { get => _code; set => _code = value; }
    public Brush TextColor { get => _textColor; set => _textColor = value; }

    public bool HasText => _runs.Count > 0;

    public Memento CreateFormatScope() => new(this);

    public void Add(string text)
    {
        _runs.Add(new FormatRun(text, _bold, _italic, _strikethrough, _underline, _code, _textColor));
    }

    public FormattedText GetFormattedText()
    {
        _runs[0].TrimStart();
        _runs[_runs.Count - 1].TrimEnd();
        string text = String.Concat(_runs.Select(r => r.Text));
        FormattedText formattedText = Factory.CreateFormattedText(text, Options.NormalTypeFace, _indent, _view);
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
