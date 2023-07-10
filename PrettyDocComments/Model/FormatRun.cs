using System.Windows.Media;

namespace PrettyDocComments.Model;

internal sealed class FormatRun
{
    private string _text;
    private readonly bool _bold;
    private readonly bool _italic;
    private readonly bool _strikethrough;
    private readonly bool _underline;
    private readonly bool _code;
    private readonly Brush _textBrush;
    private readonly Brush _highlightBrush;
    private readonly double _fontAspect;

    public FormatRun(string text, bool bold, bool italic, bool strikethrough, bool underline, bool code, 
        Brush textBrush, Brush highlightBrush, double fontAspect)
    {
        _text = text;
        _bold = bold;
        _italic = italic;
        _strikethrough = strikethrough;
        _underline = underline;
        _code = code;
        _textBrush = textBrush;
        _highlightBrush = highlightBrush;
        _fontAspect = fontAspect;
    }

    public string Text => _text;
    public bool Bold => _bold;
    public bool Italic => _italic;
    public bool Strikethrough => _strikethrough;
    public bool Underline => _underline;
    public bool Code => _code;
    public Brush TextBrush => _textBrush;
    public Brush HighlightBrush => _highlightBrush;
    public double FontAspect => _fontAspect;

    public void TrimStart()
    {
        _text = _text.TrimStart(' ', '\n', '\r', '\t', '\f');
    }

    public void TrimEnd()
    {
        _text = _text.TrimEnd(' ', '\n', '\r', '\t', '\f');
    }

    public override string ToString()
    {
        return $"""
            "{_text}" {(_bold ? "b" : "")} {(_italic ? "i" : "")}, {(_strikethrough ? "strike" : "")}, {(_underline ? "u" : "")}, {(_code ? "c" : "")}
            """;
    }
}
