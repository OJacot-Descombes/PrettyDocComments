using System.Windows.Media;

namespace PrettyDocComments.Model;

internal sealed class FormatRun(string text, bool bold, bool italic, bool strikethrough, bool underline, bool code,
    Brush textBrush, Brush highlightBrush, double fontAspect)
{
    public string Text { get; private set; } = text;
    public bool Bold { get; } = bold;
    public bool Italic { get; } = italic;
    public bool Strikethrough { get; } = strikethrough;
    public bool Underline { get; } = underline;
    public bool Code { get; } = code;
    public Brush TextBrush { get; } = textBrush;
    public Brush HighlightBrush { get; } = highlightBrush;
    public double FontAspect { get; } = fontAspect;

    public void TrimStart()
    {
        Text = Text.TrimStart(' ', '\n', '\r', '\t', '\f');
    }

    public void TrimEnd()
    {
        Text = Text.TrimEnd(' ', '\n', '\r', '\t', '\f');
    }

    public override string ToString()
    {
        return $"""
            "{Text}" {(Bold ? "b" : "")} {(Italic ? "i" : "")}, {(Strikethrough ? "strike" : "")}, {(Underline ? "u" : "")}, {(Code ? "c" : "")}
            """;
    }
}
