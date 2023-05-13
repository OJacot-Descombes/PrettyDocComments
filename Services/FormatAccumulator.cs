using System.Windows;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Editor;
using PrettyDocComments.Helpers;
using PrettyDocComments.Model;

namespace PrettyDocComments.Services;

internal class FormatAccumulator
{
    public FormatAccumulator(IWpfTextView view, double indent, double width)
    {
        _view = view;
        _indent = indent;
        _width = width;
    }

    private readonly IWpfTextView _view;

    private readonly List<FormatRun> _runs = new();

    public readonly struct IndentMemento : IDisposable
    {
        private readonly FormatAccumulator _originator;

        private readonly double _indent;

        internal IndentMemento(FormatAccumulator originator, double deltaIndent)
        {
            _originator = originator;
            _indent = originator._indent;
            originator._indent += deltaIndent;
        }

        void IDisposable.Dispose() => _originator._indent = _indent;
    }

    public readonly struct WidthMemento : IDisposable
    {
        private readonly FormatAccumulator _originator;

        private readonly double _width;

        internal WidthMemento(FormatAccumulator originator, double width)
        {
            _originator = originator;
            _width = originator._width;
            originator._width = width;
        }

        void IDisposable.Dispose() => _originator._width = _width;
    }

    public readonly struct BoldMemento : IDisposable
    {
        private readonly FormatAccumulator _originator;

        private readonly bool _bold;

        internal BoldMemento(FormatAccumulator originator)
        {
            _originator = originator;
            _bold = originator._bold;
            originator._bold = true;
        }

        void IDisposable.Dispose() => _originator._bold = _bold;
    }

    public readonly struct ItalicMemento : IDisposable
    {
        private readonly FormatAccumulator _originator;

        private readonly bool _italic;

        internal ItalicMemento(FormatAccumulator originator)
        {
            _originator = originator;
            _italic = originator._italic;
            originator._italic = true;
        }

        void IDisposable.Dispose() => _originator._italic = _italic;
    }

    public readonly struct StrikethroughMemento : IDisposable
    {
        private readonly FormatAccumulator _originator;
        private readonly bool _strikethrough;

        internal StrikethroughMemento(FormatAccumulator originator)
        {
            _originator = originator;
            _strikethrough = originator._strikethrough;
            originator._strikethrough = true;
        }

        void IDisposable.Dispose() => _originator._strikethrough = _strikethrough;
    }

    public readonly struct UnderlineMemento : IDisposable
    {
        private readonly FormatAccumulator _originator;
        private readonly bool _underline;

        internal UnderlineMemento(FormatAccumulator originator)
        {
            _originator = originator;
            _underline = originator._underline;
            originator._underline = true;
        }

        void IDisposable.Dispose() => _originator._underline = _underline;
    }

    public readonly struct CodeMemento : IDisposable
    {
        private readonly FormatAccumulator _originator;
        private readonly bool _code;

        internal CodeMemento(FormatAccumulator originator)
        {
            _originator = originator;
            _code = originator._code;
            originator._code = true;
        }

        void IDisposable.Dispose() => _originator._code = _code;
    }

    public readonly struct TextColorMemento : IDisposable
    {
        private readonly FormatAccumulator _originator;
        private readonly Brush _textColor;

        internal TextColorMemento(FormatAccumulator originator, Brush textColor)
        {
            _originator = originator;
            _textColor = originator._textColor;
            originator._textColor = textColor;
        }

        void IDisposable.Dispose()
        {
            _originator._textColor = _textColor;
        }
    }

    private double _indent;
    private double _width;
    private bool _bold;
    private bool _italic;
    private bool _strikethrough;
    private bool _underline;
    private bool _code;
    private Brush _textColor = Options.DefaultTextColor;

    public double Indent { get => _indent; }
    public double Width { get => _width; }
    public bool Bold { get => _bold; }
    public bool Italic { get => _italic; }
    public bool Strikethrough { get => _strikethrough; }
    public bool Underline { get => _underline; }
    public bool Code { get => _code; }
    public Brush TextColor { get => _textColor; }

    public bool HasText => _runs.Count > 0;
    public double RemainingWidth => _width - _indent;

    public IndentMemento CreateIndentScope(double deltaIndent) => new(this, deltaIndent);
    public WidthMemento CreateWidthScope(double width) => new(this, width);
    public BoldMemento CreateBoldScope() => new(this);
    public ItalicMemento CreateItalicScope() => new(this);
    public StrikethroughMemento CreateStrikethroughScope() => new(this);
    public UnderlineMemento CreateUnderlineScope() => new(this);
    public CodeMemento CreateCodeScope() => new(this);
    public TextColorMemento CreateTextColorScope(Brush textColor) => new(this, textColor);

    public void Add(string text)
    {
        _runs.Add(new FormatRun(text, _bold, _italic, _strikethrough, _underline, _code, _textColor));
    }

    public FormattedText GetFormattedText(double horizontalPadding = 0.0)
    {
        _runs[0].TrimStart();
        _runs[_runs.Count - 1].TrimEnd();
        string text = String.Concat(_runs.Select(r => r.Text));
        FormattedText formattedText = text.AsFormatted(Options.NormalTypeFace, _width - _indent - horizontalPadding, _view);
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
