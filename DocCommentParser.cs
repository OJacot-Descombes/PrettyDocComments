using System.Windows.Media;
using System.Xml.Linq;
using PrettyDocComments.Helpers;

namespace PrettyDocComments;

internal sealed class DocCommentParser
{
    private readonly FormatAccumulator _accumulator;
    private readonly List<TextBlock> _textBlocks = new();
    private readonly double _emSize;
    private double _indent;
    private int _listLevel = -1;
    private static readonly string[] _levelBullets = { "●", "■", "○" };

    public DocCommentParser(double emSize, double pixelsPerDip, double indent)
    {
        _accumulator = new FormatAccumulator(emSize, pixelsPerDip);
        _emSize = emSize;
        _indent = indent;
    }

    public IEnumerable<TextBlock> Parse(XElement node)
    {
        ParseElement(node);
        CloseBlock();
        foreach (TextBlock block in _textBlocks) {
            yield return block;
        }
        _textBlocks.Clear();
    }

    private TextBlock? CloseBlock(double height, Brush background = default)
    {
        if (_accumulator.HasText) {
            var textBlock = new TextBlock(_accumulator.GetFormattedText(), _indent, height, background);
            _textBlocks.Add(textBlock);
            return textBlock;
        }
        return null;
    }

    private void CloseBlock(Brush background = default)
    {
        if (_accumulator.HasText) {
            _textBlocks.Add(new TextBlock(_accumulator.GetFormattedText(), _indent, background));
        }
    }

    private void ParseElement(XElement node)
    {
        foreach (XNode child in node.Nodes()) {
            switch (child) {
                case XText xText:
                    _accumulator.Add(xText.Value);
                    break;
                case XElement el:
                    string normalizedTag = el.Name.LocalName.ToLowerInvariant();
                    switch (normalizedTag) {
                        case "br":
                            _accumulator.Add("\r\n");
                            break;
                        case "b" or "strong":
                            _accumulator.Bold = true;
                            ParseElement(el);
                            _accumulator.Bold = false;
                            break;
                        case "i":
                            _accumulator.Italic = true;
                            ParseElement(el);
                            _accumulator.Italic = false;
                            break;
                        case "u":
                            _accumulator.Underline = true;
                            ParseElement(el);
                            _accumulator.Underline = false;
                            break;
                        case "s" or "strike":
                            _accumulator.Strikethrough = true;
                            ParseElement(el);
                            _accumulator.Strikethrough = false;
                            break;
                        case "c":
                            _accumulator.Code = true;
                            ParseElement(el);
                            _accumulator.Code = false;
                            break;
                        case "code":
                            CloseBlock();
                            _accumulator.Code = true;
                            _indent += _emSize;
                            ParseElement(TrimLineBreaks(el));
                            CloseBlock(Options.CodeBackground);
                            _indent -= _emSize;
                            _accumulator.Code = false;
                            TrimStartLineBreakOfNextElement(el);
                            break;
                        case "example":
                            _accumulator.Bold = true;
                            _accumulator.Add(normalizedTag + ": ");
                            _accumulator.Bold = false;
                            ParseElement(el);
                            break;
                        case "list" or "ul" or "ol" or "dl" or "menu":
                            ParseList(el, normalizedTag);
                            break;
                        case "term" or "dt" when _listLevel >= 0:
                            _accumulator.Bold = true;
                            ParseElement(el);
                            _accumulator.Add(" – ");
                            _accumulator.Bold = false;
                            break;
                        case "description" when _listLevel >= 0:
                            ParseElement(el);
                            break;
                        case "see" when el.Attribute("cref").Value is { Length: > 0 } crefText:
                            var currentTextColor = _accumulator.TextColor;
                            _accumulator.TextColor = Options.CRefTextColor;
                            _accumulator.Add(crefText);
                            _accumulator.TextColor = currentTextColor;
                            break;
                        default:
                            _accumulator.Add(el.ToString());
                            break;
                    }
                    break;
                default:
                    _accumulator.Add(child.ToString());
                    break;
            }
        }
    }

    private XElement TrimLineBreaks(XElement el)
    {
        var children = el.Nodes().ToList();
        if (children.Count > 0) {
            if (children[0] is XText first) {
                first.Value = first.Value.TrimStart('\r', '\n');
            }
            if (children[children.Count - 1] is XText last) {
                last.Value = last.Value.TrimEnd('\r', '\n');
            }
        }
        return el;
    }

    private void TrimStartLineBreakOfNextElement(XElement el)
    {
        if (el.NextNode is XText text) {
            text.Value = text.Value.TrimStart('\r', '\n');
        }
    }

    private void ParseList(XElement el, string listTag)
    {
        // The <list> tag denotes a doc-comment type list, other types (ul, ol, dl, menu) are HTML type lists.

        const string Nul = null;

        _listLevel++;
        string type = el.Attributes("type").FirstOrDefault()?.Value;
        (string bullet, string numberType) = (listTag, type) switch {
            ("list", "number") => (Nul, "1"),
            ("list", "table") => ("", Nul),
            ("list", "bullet") => ("●", Nul),
            ("ul", "disk") => ("●", Nul),
            ("ul", "square") => ("■", Nul),
            ("ul", "circle") => ("○", Nul),
            ("ol", null) => (Nul, "1"),
            ("ol", _) => (Nul, type),
            _ => (Nul, Nul)
        };
        int number = 1;
        foreach (var listItem in el.Elements()) {
            if (listItem.Name.LocalName == "listheader") {
                _accumulator.Underline = true;
                foreach (var headerElement in listItem.Elements()) {
                    ParseElement(headerElement);
                    _accumulator.Add("\r\n");
                }
                _accumulator.Underline = false;
            } else { // textBlock
                CloseBlock();
                string actualBullet = numberType switch {
                    null => bullet ?? _levelBullets[_listLevel % _levelBullets.Length],
                    "A" => Helpers.Number.ToAlphabet(number) + ". ",
                    "a" => Helpers.Number.ToAlphabet(number, lowerCase: true) + ". ",
                    "I" => Helpers.Number.ToRoman(number) + ". ",
                    "i" => Helpers.Number.ToRoman(number, lowerCase: true) + ". ",
                    _ => number.ToString() + ". "
                };
                _accumulator.Add(actualBullet);
                TextBlock? textBlock = CloseBlock(height: 0.0);
                double itemIndent = Math.Max(textBlock?.Text.Width ?? 0.0, _emSize) + 0.5 * _emSize;
                _indent += itemIndent;
                ParseElement(listItem);
                CloseBlock();
                _indent -= itemIndent;
                number++;
            }
        }
        _listLevel--;
    }
}
