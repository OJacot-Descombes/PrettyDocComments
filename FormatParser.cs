using System.Xml.Linq;
using Microsoft.Internal.VisualStudio.PlatformUI;

namespace PrettyDocComments;

internal sealed class FormatParser
{
    public static readonly FormatParser Instance = new();

    private FormatParser() { }

    public void ParseElement(FormatAccumulator accumulator, XElement node)
    {
        foreach (XNode child in node.Nodes()) {
            switch (child) {
                case XText xText:
                    accumulator.Add(xText.Value);
                    break;
                case XElement el:
                    string normalizedTag = el.Name.LocalName.ToLowerInvariant();
                    switch (normalizedTag) {
                        case "br":
                            accumulator.Add("\r\n");
                            break;
                        case "b" or "strong":
                            accumulator.Bold = true;
                            ParseElement(accumulator, el);
                            accumulator.Bold = false;
                            break;
                        case "i":
                            accumulator.Italic = true;
                            ParseElement(accumulator, el);
                            accumulator.Italic = false;
                            break;
                        case "u":
                            accumulator.Underline = true;
                            ParseElement(accumulator, el);
                            accumulator.Underline = false;
                            break;
                        case "s" or "strike":
                            accumulator.Strikethrough = true;
                            ParseElement(accumulator, el);
                            accumulator.Strikethrough = false;
                            break;
                        case "c":
                            accumulator.Code = true;
                            ParseElement(accumulator, el);
                            accumulator.Code = false;
                            break;
                        case "code":
                            accumulator.Code = true;
                            el.Value = "  " + el.Value.Trim().Replace("\n", "\n  ");
                            ParseElement(accumulator, el);
                            accumulator.Code = false;
                            break;
                        case "example":
                            accumulator.Bold = true;
                            accumulator.Add(normalizedTag + ": ");
                            accumulator.Bold = false;
                            ParseElement(accumulator, el);
                            break;
                        case "list" or "ul" or "ol" or "dl" or "menu":
                            ParseList(accumulator, el, normalizedTag);
                            break;
                        default:
                            accumulator.Add(el.ToString());
                            break;
                    }
                    break;
                default:
                    accumulator.Add(child.ToString());
                    break;
            }
        }
    }

    private void ParseList(FormatAccumulator accumulator, XElement el, string listTag)
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
                accumulator.Underline = true;
                foreach (var headerElement in listItem.Elements()) {
                    ParseElement(accumulator, headerElement);
                    accumulator.Add("\r\n");
                }
                accumulator.Underline = false;
            } else { // item
                accumulator.Add(numberType switch {
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
                            accumulator.Bold = true;
                            ParseElement(accumulator, term);
                            accumulator.Add(" – ");
                            accumulator.Bold = false;
                            break;
                        case XElement itemEl:
                            ParseElement(accumulator, itemEl);
                            break;
                        case XText itemText:
                            accumulator.Add(itemText.Value);
                            break;
                    }
                }
                accumulator.Add("\r\n");
                number++;
            }
        }
    }
}
