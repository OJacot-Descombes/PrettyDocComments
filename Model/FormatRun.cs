using System.Windows.Media;

namespace PrettyDocComments.Model;

internal sealed class FormatRun
{
    private readonly string _text;
    private readonly bool _bold;
    private readonly bool _italic;
    private readonly bool _strikethrough;
    private readonly bool _underline;
    private readonly bool _code;
    private readonly Brush _textBrush;

    public FormatRun(string text, bool bold, bool italic, bool strikethrough, bool underline, bool code, Brush textBrush)
    {
        _text = text;
        _bold = bold;
        _italic = italic;
        _strikethrough = strikethrough;
        _underline = underline;
        _code = code;
        _textBrush = textBrush;
    }

    public string Text => _text;
    public bool Bold => _bold;
    public bool Italic => _italic;
    public bool Strikethrough => _strikethrough;
    public bool Underline => _underline;
    public bool Code => _code;
    public Brush TextBrush => _textBrush;

    public override string ToString()
    {
        return $"""
            "{_text}" {(_bold ? "b" : "")} {(_italic ? "i" : "")}, {(_strikethrough ? "strike" : "")}, {(_underline ? "u" : "")}, {(_code ? "c" : "")}
            """;
    }
}
