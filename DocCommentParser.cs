using System.Collections.Generic;
using System.Windows.Media;
using System.Xml.Linq;

namespace PrettyDocComments;

internal sealed class DocCommentParser
{
    private readonly FormatAccumulator _accumulator;
    private readonly List<TextBlock> _textBlocks = new();
    private readonly double _emSize;
    private double _indent;

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

    private void CloseBlock(double height, Brush background = default)
    {
        if (_accumulator.HasText) {
            _textBlocks.Add(new TextBlock(_accumulator.GetFormattedText(), _indent, height, background));
        }
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

        const string nul = null;

        string type = el.Attributes("type").FirstOrDefault()?.Value;
        (string bullet, string numberType) = (listTag, type) switch {
            ("list", "number") => (nul, "1"),
            ("list", "table") => ("  ", nul),
            ("ul", "square") => ("■ ", nul),
            ("ul", "circle") => ("○ ", nul),
            ("ol", null) => (nul, "1"),
            ("ol", _) => (nul, type),
            _ => ("● ", nul)
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
            } else { // item
                _accumulator.Add(numberType switch {
                    null => bullet,
                    "A" => Helpers.Number.ToAlphabet(number) + ". ",
                    "a" => Helpers.Number.ToAlphabet(number, lowerCase: true) + ". ",
                    "I" => Helpers.Number.ToRoman(number) + ". ",
                    "i" => Helpers.Number.ToRoman(number, lowerCase: true) + ". ",
                    _ => number.ToString() + ". "
                }); ;
                foreach (var itemNode in listItem.Nodes()) {
                    switch (itemNode) {
                        case XElement { Name.LocalName: "term" or "dt" } term:
                            _accumulator.Bold = true;
                            ParseElement(term);
                            _accumulator.Add(" – ");
                            _accumulator.Bold = false;
                            break;
                        case XElement itemEl:
                            ParseElement(itemEl);
                            break;
                        case XText itemText:
                            _accumulator.Add(itemText.Value);
                            break;
                    }
                }
                _accumulator.Add("\r\n");
                number++;
            }
        }
    }
}
