using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace PrettyDocComments;

internal class FormatAccumulator
{
    private static readonly Typeface _normalTypeFace;
    private static readonly Typeface _codeTypeFace;

    static FormatAccumulator()
    {
        _normalTypeFace = new Typeface(new FontFamily("Segoe UI"),
            FontStyles.Normal, FontWeights.Normal, FontStretches.Normal, new FontFamily("Arial"));

        _codeTypeFace = new Typeface(new FontFamily("Cascadia Mono"), FontStyles.Normal, FontWeights.Normal, FontStretches.Normal,
            new FontFamily("Consolas"));
    }

    public FormatAccumulator(double emSize, double pixelsPerDip)
    {
        _pixelsPerDip = pixelsPerDip;
        _emSize = emSize;
    }

    private readonly double _emSize;
    private readonly double _pixelsPerDip;

    private readonly List<FormatRun> _runs = new();

    private bool _bold;
    private bool _italic;
    private bool _strikethrough;
    private bool _underline;
    private bool _code;

    public bool Bold { get => _bold; set => _bold = value; }
    public bool Italic { get => _italic; set => _italic = value; }
    public bool Strikethrough { get => _strikethrough; set => _strikethrough = value; }
    public bool Underline { get => _underline; set => _underline = value; }
    public bool Code { get => _code; set => _code = value; }

    public void Add(string text)
    {
        _runs.Add(new FormatRun(text, _bold, _italic, _strikethrough, _underline, _code));
    }

    public FormattedText GetFormattedText()
    {
        string text = String.Concat(_runs.Select(r => r.Text));
        var formattedText = new FormattedText(text, CultureInfo.InvariantCulture, FlowDirection.LeftToRight,
            _normalTypeFace, _emSize, Brushes.Black, _pixelsPerDip);
        int startIndex = 0;
        foreach (var run in _runs) {
            int length = run.Text?.Length ?? 0;

            formattedText.SetFontTypeface(run.Code ? _codeTypeFace : _normalTypeFace, startIndex, length);
            formattedText.SetFontStyle(run.Italic ? FontStyles.Italic : FontStyles.Normal, startIndex, length);
            formattedText.SetFontWeight(run.Bold ? FontWeights.Bold : FontWeights.Normal, startIndex, length);

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
        return formattedText;
    }
}
