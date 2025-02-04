﻿using System.Windows;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Editor;
using PrettyDocComments.Helpers;
using PrettyDocComments.Model;

namespace PrettyDocComments.Services;

internal class FormatAccumulator(IWpfTextView view, double indent, double width, double fontAspect)
{
    private readonly List<FormatRun> _runs = [];

    public readonly struct FontAspectMemento : IDisposable
    {
        private readonly FormatAccumulator _originator;

        private readonly double _fontAspect;

        internal FontAspectMemento(FormatAccumulator originator, double fontAspect)
        {
            _originator = originator;
            _fontAspect = originator.FontAspect;
            originator.FontAspect = fontAspect;
        }

        void IDisposable.Dispose() => _originator.FontAspect = _fontAspect;
    }

    public readonly struct IndentMemento : IDisposable
    {
        private readonly FormatAccumulator _originator;

        private readonly double _indent;

        internal IndentMemento(FormatAccumulator originator, double deltaIndent)
        {
            _originator = originator;
            _indent = originator.Indent;
            originator.Indent += deltaIndent;
        }

        void IDisposable.Dispose() => _originator.Indent = _indent;
    }

    public readonly struct WidthMemento : IDisposable
    {
        private readonly FormatAccumulator _originator;

        private readonly double _width;

        internal WidthMemento(FormatAccumulator originator, double width)
        {
            _originator = originator;
            _width = originator.Width;
            originator.Width = width;
        }

        void IDisposable.Dispose() => _originator.Width = _width;
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
        private readonly Brush _textColor;

        internal CodeMemento(FormatAccumulator originator, Brush textColor)
        {
            _originator = originator;

            _code = originator._code;
            originator._code = true;

            _textColor = originator._textBrush;
            originator._textBrush = textColor ?? _textColor; // Do not change if brush is null
        }

        void IDisposable.Dispose()
        {
            _originator._code = _code;
            _originator._textBrush = _textColor;
        }
    }

    public readonly struct TextColorMemento : IDisposable
    {
        private readonly FormatAccumulator _originator;
        private readonly Brush _textColor;

        internal TextColorMemento(FormatAccumulator originator, Brush textColor)
        {
            _originator = originator;
            _textColor = originator._textBrush;
            originator._textBrush = textColor;
        }

        void IDisposable.Dispose()
        {
            _originator._textBrush = _textColor;
        }
    }

    public readonly struct HighlightMemento : IDisposable
    {
        private readonly FormatAccumulator _originator;
        private readonly Brush _highlight;

        internal HighlightMemento(FormatAccumulator originator, Brush highlight)
        {
            _originator = originator;
            _highlight = originator._highlight;
            originator._highlight = highlight;
        }

        void IDisposable.Dispose()
        {
            _originator._highlight = _highlight;
        }
    }

    public readonly struct AlignmentMemento : IDisposable
    {
        private readonly FormatAccumulator _originator;
        private readonly TextAlignment _alignment;

        internal AlignmentMemento(FormatAccumulator originator, TextAlignment alignment)
        {
            _originator = originator;
            _alignment = originator._alignment;
            originator._alignment = alignment;
        }

        void IDisposable.Dispose()
        {
            _originator._alignment = _alignment;
        }
    }

    private bool _bold;
    private bool _italic;
    private bool _strikethrough;
    private bool _underline;
    private bool _code;
    private Brush _textBrush = Options.DefaultTextBrush;
    private Brush _highlight;
    private TextAlignment _alignment;

    private bool _trimNextStart;

    public double Indent { get; private set; } = indent;
    public double Width { get; private set; } = width;
    public double FontAspect { get; private set; } = fontAspect;

    public bool HasText => _runs.Count > 0;
    public double RemainingWidth => Width - Indent;

    public FontAspectMemento CreateFontAspect(double aspect) => new(this, aspect);
    public IndentMemento CreateIndentScope(double deltaIndent) => new(this, deltaIndent);
    public WidthMemento CreateWidthScope(double width) => new(this, width);
    public BoldMemento CreateBoldScope() => new(this);
    public ItalicMemento CreateItalicScope() => new(this);
    public StrikethroughMemento CreateStrikethroughScope() => new(this);
    public UnderlineMemento CreateUnderlineScope() => new(this);
    public CodeMemento CreateCodeScope(Brush textColor) => new(this, textColor);
    public TextColorMemento CreateTextColorScope(Brush textColor) => new(this, textColor);
    public HighlightMemento CreateHighlightScope(Brush highlight) => new(this, highlight);
    public AlignmentMemento CreateAlignmentScope(TextAlignment alignment) => new(this, alignment);

    public void Add(string text)
    {
        if (_trimNextStart) {
            _trimNextStart = false;
            text = text.TrimStart();
        }
        _runs.Add(new FormatRun(text, _bold, _italic, _strikethrough, _underline, _code, _textBrush, _highlight, FontAspect));
    }

    public void AddLineBreak()
    {
        _runs.Add(new FormatRun("\r\n", _bold, _italic, _strikethrough, _underline, _code, _textBrush, _highlight, FontAspect));
        _trimNextStart = true;
    }

    public FormattedTextEx GetFormattedText(bool trimStartEnd, double horizontalPadding)
    {
        if (trimStartEnd) {
            _runs[0].TrimStart();
            _runs[_runs.Count - 1].TrimEnd();
        }
        string text = String.Concat(_runs.Select(r => r.Text));
        FormattedTextEx formattedText = text.AsFormatted(Options.NormalTypeFace, Width - Indent - horizontalPadding, view);
        formattedText.TextAlignment = _alignment;
        int startIndex = 0;
        foreach (FormatRun run in _runs) {
            int length = run.Text?.Length ?? 0;
            formattedText.SetFontTypeface(run.Code ? Options.CodeTypeFace : Options.NormalTypeFace, startIndex, length);
            formattedText.SetFontStyle(run.Italic ? FontStyles.Italic : FontStyles.Normal, startIndex, length);
            formattedText.SetFontWeight(run.Bold ? FontWeights.Bold : FontWeights.Normal, startIndex, length);
            formattedText.SetForegroundBrush(run.TextBrush, startIndex, length);
            formattedText.SetHighlightBrush(run.HighlightBrush, startIndex, length);
            if (run.FontAspect != 1.0) {
                formattedText.SetFontSize(Options.GetNormalEmSize(view) * run.FontAspect, startIndex, length);
            }
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
