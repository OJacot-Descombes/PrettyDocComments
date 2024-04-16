using PrettyDocComments.Helpers;
using PrettyDocComments.Model;
using System.Xml.Linq;

namespace PrettyDocComments.Services;

internal sealed partial class FormatParser
{
    private void ParseList(XElement el, string listTag)
    {
        // The <list> tag denotes a doc-comment type list, other types (ul, ol, dl, menu) are HTML type lists.

        const string Nul = null;

        string type = el.Name.LocalName == "table"
            ? "table" // HTML table
            : el.Attributes("type").FirstOrDefault()?.Value;
        if (type is "table") {
            ParseTable(el);
            return;
        }

        _listLevel++;
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
        string lastLocalName = null;
        double itemIndent = 0.0;
        foreach (var listItem in el.Elements()) {
            string localName = listItem.Name.LocalName;
            if (localName == "listheader") {
                CloseBlock();
                using (Accumulator.CreateUnderlineScope()) {
                    foreach (var headerElement in listItem.Elements()) {
                        if (headerElement.Name.LocalName is "term") {
                            using (Accumulator.CreateBoldScope()) {
                                ParseElement(headerElement);
                                Accumulator.Add(" – ");
                            }
                        } else {
                            ParseElement(headerElement);
                            Accumulator.Add("\r\n");
                        }
                    }
                }
            } else if (localName == "dd" || lastLocalName == "dt") {  // HTML description list description.
                using (Accumulator.CreateIndentScope(itemIndent)) {
                    ParseElement(listItem);
                    CloseBlock();
                }
            } else {
                CloseBlock();
                string actualBullet = numberType switch {
                    null => bullet ?? _levelBullets[_listLevel % _levelBullets.Length],
                    "A" => number.ToAlphabet() + ". ",
                    "a" => number.ToAlphabet(lowerCase: true) + ". ",
                    "I" => number.ToRoman() + ". ",
                    "i" => number.ToRoman(lowerCase: true) + ". ",
                    _ => number.ToString() + ". "
                };
                Accumulator.Add(actualBullet);
                TextBlock? textBlock = CloseBlock(height: 0.0);
                itemIndent = type == "table" ? 0.0 : Math.Max(textBlock?.Text.Width ?? 0.0, emSize) + 0.5 * emSize;
                using (Accumulator.CreateIndentScope(itemIndent)) {
                    if (localName == "dt") { // HTML description list term.
                        using (Accumulator.CreateBoldScope()) {
                            ParseElement(listItem);
                            Accumulator.Add(" – ");
                        }
                    } else {
                        ParseElement(listItem);
                        CloseBlock();
                    }
                }
                number++;
            }
            lastLocalName = localName;
        }
        _listLevel--;
    }
}
