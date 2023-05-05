using System.Text.RegularExpressions;
using System.Windows.Media;
using System.Xml.Linq;
using Microsoft.VisualStudio.Text.Editor;
using PrettyDocComments.Helpers;
using PrettyDocComments.Model;

namespace PrettyDocComments.Services;

internal sealed class DocCommentParser
{
    private readonly FormatAccumulator _accumulator;
    private readonly List<TextBlock> _textBlocks = new();
    private readonly double _emSize;
    private int _listLevel = -1;
    private static readonly string[] _levelBullets = { "●", "■", "○" };
    private static readonly Regex _normalizeWhiteSpacesRegex = new(@"[ \n\r\t\f]+", RegexOptions.Compiled);

    public DocCommentParser(double indent, IWpfTextView view)
    {
        _accumulator = new FormatAccumulator(view, indent);
        _emSize = view.FormattedLineSource.DefaultTextProperties.FontRenderingEmSize * Options.FontScaling;
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
            var textBlock = new TextBlock(_accumulator.GetFormattedText(), _accumulator.Indent, height, background);
            _textBlocks.Add(textBlock);
            return textBlock;
        }
        return null;
    }

    private void CloseBlock(Brush background = default)
    {
        if (_accumulator.HasText) {
            _textBlocks.Add(new TextBlock(_accumulator.GetFormattedText(), _accumulator.Indent, background));
        }
    }

    private void ParseElement(XElement node, bool normalizeWS = true)
    {
        foreach (XNode child in node.Nodes()) {
            switch (child) {
                case XText xText:
                    _accumulator.Add(NormalizeSpace(xText.Value, normalizeWS));
                    break;
                case XElement el:
                    string normalizedTag = el.Name.LocalName.ToLowerInvariant();
                    switch (normalizedTag) {
                        case "br":
                            _accumulator.Add("\r\n");
                            break;
                        case "b" or "strong":
                            using (var scope = _accumulator.CreateFormatScope()) {
                                _accumulator.Bold = true;
                                ParseElement(el);
                            }
                            break;
                        case "i":
                            using (var scope = _accumulator.CreateFormatScope()) {
                                _accumulator.Italic = true;
                                ParseElement(el);
                            }
                            break;
                        case "u":
                            using (var scope = _accumulator.CreateFormatScope()) {
                                _accumulator.Underline = true;
                                ParseElement(el);
                            }
                            break;
                        case "s" or "strike":
                            using (var scope = _accumulator.CreateFormatScope()) {
                                _accumulator.Strikethrough = true;
                                ParseElement(el);
                            }
                            break;
                        case "c":
                            using (var scope = _accumulator.CreateFormatScope()) {
                                _accumulator.Code = true;
                                ParseElement(el, normalizeWS: false);
                            }
                            break;
                        case "code":
                            CloseBlock();
                            using (var scope = _accumulator.CreateFormatScope()) {
                                _accumulator.Code = true;
                                _accumulator.Indent += _emSize;
                                ParseElement(el, normalizeWS: false);
                                CloseBlock(Options.CodeBackground);
                            }
                            break;
                        case "remarks":
                            ParseWithTitle(el, "Remarks");
                            break;
                        case "example":
                            ParseWithTitle(el, "Example: ", 1.7 * _emSize);
                            break;
                        case "list" or "ul" or "ol" or "dl" or "menu":
                            ParseList(el, normalizedTag);
                            break;
                        case "para":
                            ParseElement(el);
                            CloseBlock();
                            _accumulator.Add(" ");
                            CloseBlock(height: _emSize / 2.5);
                            break;
                        case "term" or "dt" when _listLevel >= 0:
                            using (var scope = _accumulator.CreateFormatScope()) {
                                _accumulator.Bold = true;
                                ParseElement(el);
                                _accumulator.Add(" – ");
                            }
                            break;
                        case "description" when _listLevel >= 0:
                            ParseElement(el);
                            break;
                        case "paramref":
                            Reference(el, "name");
                            break;
                        case "see":
                            Reference(el, "cref");
                            break;
                        default:
                            _accumulator.Add(NormalizeSpace(el.ToString(), normalizeWS));
                            break;
                    }
                    break;
                default:
                    _accumulator.Add(NormalizeSpace(child.ToString(), normalizeWS));
                    break;
            }
        }
    }

    private void Reference(XElement el, string attributeName)
    {
        string text = el.Attribute(attributeName).Value is { Length: > 0 } attributeValue
            ? attributeValue
            : el.Name.LocalName;
        using (var scope = _accumulator.CreateFormatScope()) {
            _accumulator.TextColor = Options.SpecialTextColor;
            _accumulator.Add(text);
        }
    }

    private void ParseWithTitle(XElement el, string title, double indent = 0.0)
    {
        CloseBlock();
        using (var scope = _accumulator.CreateFormatScope()) {
            _accumulator.Bold = true;
            _accumulator.Add(title);
        }
        CloseBlock();
        using (var scope = _accumulator.CreateFormatScope()) {
            _accumulator.Indent += indent;
            ParseElement(el);
            CloseBlock();
        }
    }

    private void ParseList(XElement el, string listTag)
    {
        // TODO: correctly display table-type lists http://www.blackwasp.co.uk/DocumentationLists_2.aspx.

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
                CloseBlock();
                using (var scope = _accumulator.CreateFormatScope()) {
                    _accumulator.Underline = true;
                    foreach (var headerElement in listItem.Elements()) {
                        ParseElement(headerElement);
                        _accumulator.Add("\r\n");
                    }
                }
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
                double itemIndent = type == "table" ? 0.0 : Math.Max(textBlock?.Text.Width ?? 0.0, _emSize) + 0.5 * _emSize;
                using (var scope = _accumulator.CreateFormatScope()) {
                    _accumulator.Indent += itemIndent;
                    ParseElement(listItem);
                    CloseBlock();
                };
                number++;
            }
        }
        _listLevel--;
    }

    private static string NormalizeSpace(string s, bool normalizeWS)
        => normalizeWS ? _normalizeWhiteSpacesRegex.Replace(s, " ") : s;
}
