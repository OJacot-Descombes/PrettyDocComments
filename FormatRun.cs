namespace PrettyDocComments;

internal sealed class FormatRun
{
    private readonly string _text;
    private readonly bool _bold;
    private readonly bool _italic;
    private readonly bool _strikethrough;
    private readonly bool _underline;
    private readonly bool _code;

    public FormatRun(string text, bool bold, bool italic, bool strikethrough, bool underline, bool code)
    {
        _text = text;
        _bold = bold;
        _italic = italic;
        _strikethrough = strikethrough;
        _underline = underline;
        _code = code;
    }

    public string Text => _text;
    public bool Bold => _bold;
    public bool Italic => _italic;
    public bool Strikethrough => _strikethrough;
    public bool Underline => _underline;
    public bool Code => _code;

    public override string ToString()
    {
        return $"""
            "{_text}" {(_bold ? "b" : "")} {(_italic ? "i" : "")}, {(_strikethrough ? "strike" : "")}, {(_underline ? "u" : "")}, {(_code ? "c" : "")}
            """;
    }
}
