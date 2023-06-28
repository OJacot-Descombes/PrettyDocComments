using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Xml.Linq;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;
using PrettyDocComments.Helpers;
using PrettyDocComments.Model;

namespace PrettyDocComments.Services;

internal sealed partial class FormatParser
{
    private static readonly string[] _levelBullets = { "●", "■", "○" };
    private static readonly WidthEstimator _estimator = new();

    private readonly double _emSize;
    private readonly IWpfTextView _view; // TODO: remove when ParseTable is done.

    private List<TextBlock> _textBlocks;
    private int _listLevel = -1;

    private readonly FormatAccumulator _accumulator;
    public FormatAccumulator Accumulator => _accumulator;

    public FormatParser(double indent, double emSize, double width, double fontAspect, IWpfTextView view)
    {
        _accumulator = new FormatAccumulator(view, indent, width, fontAspect);
        _emSize = emSize;
        _view = view;
    }

    public List<TextBlock> Parse(XElement node)
    {
        _textBlocks = new List<TextBlock>();
        ParseElement(node);
        CloseBlock();
        var temp = _textBlocks;
        _textBlocks = null;
        return temp;
    }

    private void ParseElement(XElement node, bool normalizeWS = true)
    {
        string previousTag = null;
        foreach (XNode child in node.Nodes()) {
            switch (child) {
                case XText xText:
                    _accumulator.Add(xText.Value.NormalizeSpace(normalizeWS));
                    previousTag = null;
                    break;
                case XElement el:
                    string normalizedTag = el.Name.LocalName.ToLowerInvariant();
                    switch (normalizedTag) {
                        case "a" or "embed" or "frame" or "img" or "paramref" or "permission" or "typeparamref":
                            Reference(el); // "name"
                            break;
                        case "b" or "strong":
                            using (_accumulator.CreateBoldScope()) {
                                ParseElement(el);
                            }
                            break;
                        case "big":
                            using (_accumulator.CreateFontAspect(_accumulator.FontAspect * 1.2)) {
                                ParseElement(el);
                            }
                            break;
                        case "blockquote":
                            CloseBlock();
                            using (_accumulator.CreateItalicScope())
                            using (_accumulator.CreateIndentScope(_emSize)) {
                                ParseElement(el);
                                CloseBlock();
                            }
                            break;
                        case "body" or "figure" or "html" or "label" or "nav" or "summary" or "time":
                            // Parse the children and IGNORE the tag itself.
                            // Note: Top-level <summary> tags are handled in the ShapeParser.
                            ParseElement(el);
                            break;
                        case "br":
                            _accumulator.AddLineBreak();
                            break;
                        case "button":
                            _accumulator.Add(" [ "); ParseElement(el); _accumulator.Add(" ] ");
                            break;
                        case "c" or "kbd" or "samp" or "tt":
                            using (_accumulator.CreateCodeScope()) {
                                ParseElement(el, normalizeWS: false);
                            }
                            break;
                        case "center":
                            CloseBlock();
                            using (_accumulator.CreateAlignmentScope(TextAlignment.Center)) {
                                ParseElement(el);
                                CloseBlock();
                            }
                            break;
                        case "cite" or "dfn" or "em" or "i" or "var":
                            using (_accumulator.CreateItalicScope()) {
                                ParseElement(el);
                            }
                            break;
                        case "code":
                            CloseBlock();
                            using (_accumulator.CreateCodeScope())
                            using (_accumulator.CreateWidthScope(_accumulator.Width - Options.Padding.Right)) {
                                RemoveTagIndent(el);
                                ParseElement(el, normalizeWS: false);
                                CloseBlock(BackgroundType.CodeBlock);
                            }
                            break;
                        case "del" or "s" or "strike":
                            using (_accumulator.CreateStrikethroughScope()) {
                                ParseElement(el);
                            }
                            break;
                        case "description" when _listLevel >= 0:
                            ParseElement(el);
                            break;
                        case "details": // Display like an open disclosure widget. (non-functional)
                            if (el.Element("summary") is { } summary) {
                                summary.AddFirst("▼ ");
                            }
                            ParseElement(el);
                            break;
                        case "dialog" or "fieldset":
                            CloseBlock();
                            using (_accumulator.CreateWidthScope(_accumulator.Width - Options.Padding.Right)) {
                                ParseElement(el);
                                CloseBlock(BackgroundType.Framed);
                            }
                            break;
                        case "dir" or "dl" or "list" or "menu" or "ol" or "table" or "ul":
                            ParseList(el, normalizedTag);
                            break;
                        case "div" or "figcaption" or "footer" or "form" or "header" or "section":
                            // Insert line break before and after <div> as most browsers do. No processing of CSS styles so far.
                            CloseBlock();
                            ParseElement(el);
                            CloseBlock();
                            break;
                        case "example":
                            ParseWithTitle(el, "Example: ", 1.7 * _emSize);
                            break;
                        case "h1":
                            HtmlHeading(el, 1.8);
                            break;
                        case "h2":
                            HtmlHeading(el, 1.53);
                            break;
                        case "h3":
                            HtmlHeading(el, 1.3);
                            break;
                        case "h4":
                            HtmlHeading(el, 1.11);
                            break;
                        case "h5":
                            HtmlHeading(el, 0.94);
                            break;
                        case "h6":
                            HtmlHeading(el, 0.8);
                            break;
                        case "head" or "datalist":
                            // IGNORE the tag completely (so far).
                            // The <head> element is a container for metadata and is not displayed.
                            // The <datalist> element provides items for the autocomplete feture of an <input> element.
                            break;
                        case "hr":
                            CloseBlock();
                            int count = (int)(0.9 * _accumulator.RemainingWidth / _view.FormattedLineSource.ColumnWidth);
                            _accumulator.Add(new string('─', count));
                            CloseBlock();
                            break;
                        case "include" or "inheritdoc":
                            using (_accumulator.CreateTextColorScope(Options.SpecialTextBrush)) {
                                _accumulator.Add(normalizedTag);
                                _accumulator.Add("(");
                                _accumulator.Add(String.Join(" ", el.Attributes().Select(a => $"{a.Name}='{a.Value}'")));
                                _accumulator.Add(")");
                            }
                            break;
                        case "input":
                            HtmlInputField(el);
                            break;
                        case "ins" or "u":
                            using (_accumulator.CreateUnderlineScope()) {
                                ParseElement(el);
                            }
                            break;
                        case "legend":
                            CloseBlock();
                            ParseElement(el);
                            CloseBlock(BackgroundType.Framed);
                            break;
                        case "mark": // We cannot set the background color in FormattedText. Let's do something else:
                            using (_accumulator.CreateBoldScope())
                            using (_accumulator.CreateItalicScope()) {
                                ParseElement(el);
                            }
                            break;
                        case "output":
                            _accumulator.Add("_______");
                            break;
                        case "p" or "para":
                            CloseBlock();

                            // Don't add space twice between two param tags. Don't add space before first element.
                            if (previousTag is not ("para" or "p") && el.PreviousNode is not null) {
                                _accumulator.Add(" ");
                                CloseBlock(height: _emSize / 2.5);
                            }
                            ParseElement(el);
                            CloseBlock();
                            if (el.NextNode is not null) { // Don't add space after last element.
                                _accumulator.Add(" ");
                                CloseBlock(height: _emSize / 2.5);
                            }
                            break;
                        case "param":
                            // We expect the doc-comment-<param> to be a top-level element and handle it in ShapeParser.
                            // In case it is nested, we consider it here as HTML-<param> and doc-comment-<param>.
                            string name = el.Attributes("name").FirstOrDefault()?.Value;
                            string title = (name is { Length: > 0 } ? name : normalizedTag);
                            if (el.Value is { Length: > 0 }) { // XML-doc-comment version
                                CloseBlock();
                                using (_accumulator.CreateBoldScope()) {
                                    _accumulator.Add(title + ":  ");
                                }
                                ParseElement(el);
                                CloseBlock();
                            } else { // HTML version
                                _accumulator.Add(title);
                                if (el.Attribute("value")?.Value is { Length: > 0 } v) {
                                    _accumulator.Add(" = " + v);
                                }
                            }
                            break;
                        case "pre": // We display it like <code> but without a special background color.
                            CloseBlock();
                            using (_accumulator.CreateCodeScope()) {
                                RemoveTagIndent(el);
                                ParseElement(el, normalizeWS: false);
                                CloseBlock();
                            }
                            break;
                        case "q":
                            _accumulator.Add("“");
                            ParseElement(el);
                            _accumulator.Add("”");
                            break;
                        case "remarks":
                            ParseWithTitle(el, "Remarks");
                            break;
                        case "see" or "seealso":
                            // Let's treat <seealso> like <see> if it is nested
                            // (according to Mahmoud Al-Qudsi: https://stackoverflow.com/a/69947292/880990)
                            Reference(el); // "cref", "langword", "href"
                            break;
                        case "select":
                            _accumulator.Add("[_____|v]");
                            break;
                        case "small":
                            using (_accumulator.CreateFontAspect(_accumulator.FontAspect / 1.2)) {
                                ParseElement(el);
                            }
                            break;
                        case "sub":
                            Subscript(el);
                            break;
                        case "sup":
                            Superscript(el);
                            break;
                        case "term" when _listLevel >= 0:
                            using (_accumulator.CreateBoldScope()) {
                                ParseElement(el);
                                _accumulator.Add(" – ");
                            }
                            break;
                        case "textarea":
                            CloseBlock();
                            using (_accumulator.CreateWidthScope(_accumulator.Width - Options.Padding.Right)) {
                                //RemoveTagIndent(el);
                                //ParseElement(el);
                                _accumulator.Add(String.Concat(el.Nodes()));
                                CloseBlock(BackgroundType.Framed);
                            }
                            break;
                        case "wbr":
                            _accumulator.Add("\u200B"); // Zero-length space.
                            break;
                        default: // UNSUPPORTED tags. Display tag name as a label plus the first Attribute if available.
                            using (_accumulator.CreateItalicScope()) {
                                string tagCapitalized = normalizedTag.FirstCap();
                                if (el.Attributes().FirstOrDefault()?.Value is { Length: > 0 } attributeValue) {
                                    _accumulator.Add($"""{tagCapitalized} "{attributeValue}": """);
                                } else {
                                    _accumulator.Add(tagCapitalized + ": ");
                                }
                            }
                            ParseElement(el);
                            break;
                    }
                    previousTag = normalizedTag;
                    break;
                case XComment comment:
                    CloseBlock();
                    using (_accumulator.CreateTextColorScope(Options.CommentTextBrush)) {
                        _accumulator.Add(comment.Value);
                    }
                    CloseBlock();
                    break;
                default:
                    _accumulator.Add(child.ToString().NormalizeSpace(normalizeWS));
                    break;
            }
        }
    }

    private TextBlock? CloseBlock(double height, BackgroundType backgroundType = BackgroundType.Default)
    {
        if (_accumulator.HasText) {
            double padding = backgroundType is BackgroundType.Default ? 0.0 : Options.Padding.GetWidth();
            FormattedText text = _accumulator.GetFormattedText(backgroundType != BackgroundType.CodeBlock, padding);
            var textBlock = new TextBlock(text, _accumulator.Indent, height, height, backgroundType);
            _textBlocks.Add(textBlock);
            return textBlock;
        }
        return null;
    }

    private void CloseBlock(BackgroundType backgroundType = BackgroundType.Default)
    {
        if (_accumulator.HasText) {
            double padding = backgroundType is BackgroundType.Default ? 0.0 : Options.Padding.GetWidth();
            FormattedText text = _accumulator.GetFormattedText(backgroundType != BackgroundType.CodeBlock, padding);
            _textBlocks.Add(new TextBlock(text, _accumulator.Indent, backgroundType));
        }
    }

    private void Reference(XElement el)
    {
        using (_accumulator.CreateTextColorScope(Options.SpecialTextBrush)) {
            if (el.Value is { Length: > 0 }) {
                ParseElement(el);
            } else {
                string text =
                     (el.Attribute("src") ?? el.Attributes().FirstOrDefault())?.Value is { Length: > 0 } attributeValue
                          ? attributeValue
                          : el.Name.LocalName;
                _accumulator.Add(text);
            }
        }
    }

    private void ParseWithTitle(XElement el, string title, double indent = 0.0)
    {
        CloseBlock();
        using (_accumulator.CreateBoldScope()) {
            _accumulator.Add(title);
        }
        CloseBlock();
        using (_accumulator.CreateIndentScope(indent)) {
            ParseElement(el);
            CloseBlock();
        }
    }

    /// <summary>
    /// Removes the indentation of an XML element's value.
    /// </summary>
    /// <param name="el">Usually a &lt;code&gt; element</param>
    private void RemoveTagIndent(XElement el)
    {
        string[] lines = el.Value.Split('\n');

        // Find the first and last non-empty lines where the index 'last' points to the last such line + 1
        int first = 0;
        int last = lines.Length;
        while (first < lines.Length && String.IsNullOrWhiteSpace(lines[first])) {
            first++;
        }
        while (last > first && String.IsNullOrWhiteSpace(lines[last - 1])) {
            last--;
        }

        if (last > first) {
            // Calculate the minimum number of leading spaces in the non-empty lines
            int minSpaces = Int32.MaxValue;
            for (int i = first; i < last; i++) {
                lines[i] = lines[i].TrimEnd();
                string line = lines[i];
                if (line.Length > 0) {
                    minSpaces = Math.Min(minSpaces, line.TakeWhile(Char.IsWhiteSpace).Count());
                }
            }

            if (minSpaces > 0) {
                // Remove the leading spaces from each non-empty line
                for (int i = first; i < last; i++) {
                    string line = lines[i];
                    if (line.Length > minSpaces) {
                        lines[i] = line.Substring(minSpaces);
                    }
                }

                //// Join the lines and set the value
                el.Value = String.Join("\n", lines.Skip(first).Take(last - first));
            } else if (first > 0 || last < lines.Length) {
                el.Value = String.Join("\n", lines.Skip(first).Take(last - first));
            }
        } else {
            // If all lines are empty, set the value to a single space to get a shaded rectangle.
            el.Value = " ";
        }
    }

    private static Dictionary<char, char> _subScripts, _superScripts;

    private void Subscript(XElement el)
    {
        _subScripts ??= new() {
            { '0', '₀' }, { '1', '₁' }, { '2', '₂' }, { '3', '₃' }, { '4', '₄' },
            { '5', '₅' }, { '6', '₆' }, { '7', '₇' }, { '8', '₈' }, { '9', '₉' },
            { 'a', 'ₐ' }, /*'b', n/a */ /*'c', n/a */ /*'d', n/a */ { 'e', 'ₑ' },
            /*'f', n/a */ /*'g', n/a */ { 'h', 'ₕ' }, { 'i', 'ᵢ' }, { 'j', 'ⱼ' },
            { 'k', 'ₖ' }, { 'l', 'ₗ' },  { 'm', 'ₘ' }, { 'n', 'ₙ' }, { 'o', 'ₒ' },
            { 'p', 'ₚ' }, /*'q', n/a */ { 'r', 'ᵣ' }, { 's', 'ₛ' }, { 't', 'ₜ' },
            { 'u', 'ᵤ' }, { 'v', 'ᵥ' }, /*'w', n/a */ { 'x', 'ₓ' }, /*'y', n/a */
            /*'z', n/a */ { '+', '₊' }, { '-', '₋' }, { '=', '₌' }, { '(', '₍' },
            { ')', '₎' }, { 'β', 'ᵦ' }, { 'γ', 'ᵧ' }, { 'ρ', 'ᵨ' }, { 'ψ', 'ᵩ' },
            { 'χ', 'ᵪ' }, { 'ə', 'ₔ' }
        };
        if (el.Value.All(_subScripts.ContainsKey)) {
            var sb = new StringBuilder();
            foreach (char ch in el.Value) {
                sb.Append(_subScripts[ch]);
            }
            _accumulator.Add(sb.ToString());
        } else {
            using (_accumulator.CreateFontAspect(0.7 * _accumulator.FontAspect)) {
                ParseElement(el);
            }
        }
    }

    private void Superscript(XElement el)
    {
        _superScripts ??= new() {
            { '0', '⁰' }, { '1', '¹' }, { '2', '²' }, { '3', '³' }, { '4', '⁴' },
            { '5', '⁵' }, { '6', '⁶' }, { '7', '⁷' }, { '8', '⁸' }, { '9', '⁹' },
            { 'a', 'ᵃ' }, { 'b', 'ᵇ' }, { 'c', 'ᶜ' }, { 'd', 'ᵈ' }, { 'e', 'ᵉ' },
            { 'f', 'ᶠ' }, { 'g', 'ᵍ' }, { 'h', 'ʰ' }, { 'i', 'ⁱ' }, { 'j', 'ʲ' },
            { 'k', 'ᵏ' }, { 'l', 'ˡ' }, { 'm', 'ᵐ' }, { 'n', 'ⁿ' }, { 'o', 'ᵒ' },
            { 'p', 'ᵖ' }, /*'q', n/a */ { 'r', 'ʳ' }, { 's', 'ˢ' }, { 't', 'ᵗ' },
            { 'u', 'ᵘ' }, { 'v', 'ᵛ' }, { 'w', 'ʷ' }, { 'x', 'ˣ' }, { 'y', 'ʸ' },
            { 'z', 'ᶻ' },
            { 'A', 'ᴬ' }, { 'B', 'ᴮ' }, { 'C', 'ꟲ' }, { 'D', 'ᴰ' }, { 'E', 'ᴱ' },
            { 'F', 'ꟳ' }, { 'G', 'ᴳ' }, { 'H', 'ᴴ' }, { 'I', 'ᴵ' }, { 'J', 'ᴶ' },
            { 'K', 'ᴷ' }, { 'L', 'ᴸ' }, { 'M', 'ᴹ' }, { 'N', 'ᴺ' }, { 'O', 'ᴼ' },
            { 'P', 'ᴾ' }, { 'Q', 'ꟴ' }, { 'R', 'ᴿ' }, /*'S', n/a */ { 'T', 'ᵀ' },
            { 'U', 'ᵁ' }, { 'V', 'ⱽ' }, { 'W', 'ᵂ' }, /*'X', n/a */ /*'Y', n/a */
            /*'Z', n/a */ { '+', '⁺' }, { '-', '⁻' }, { '=', '⁼' }, { '(', '⁽' },
            { ')', '⁾' }, { 'β', 'ᵝ' }, { 'γ', 'ᵞ' }, { 'δ', 'ᵟ' }, {'ε','ᵋ' },
            { 'θ', 'ᶿ' }, { 'ι', 'ᶥ' }, { 'υ', 'ᶹ' }, { 'φ', 'ᶲ' }, { 'ψ', 'ᵠ' },
            {'χ','ᵡ' }, {'~','˜' }, {'ə','ₔ' }
        };
        if (el.Value.All(_superScripts.ContainsKey)) {
            var sb = new StringBuilder();
            foreach (char ch in el.Value) {
                sb.Append(_superScripts[ch]);
            }
            _accumulator.Add(sb.ToString());
        } else {
            _accumulator.Add("^(");
            ParseElement(el);
            _accumulator.Add(")");
        }
    }

    private void HtmlInputField(XElement el)
    {
        string text = el.Attribute("type")?.Value switch {
            "button" when el.Attribute("value")?.Value is var value => $"[ {value} ]",
            "checkbox" => "[X] ",
            "color" or "date" or "datetime-local" => "[_____|v]",
            "file" => "[ Browse... ]",
            "hidden" => "",
            "image" => el.Attribute("src")?.Value is { Length: > 0 } src ? $"[ {src} ]" : "[ ☺ ]",
            "number" => "[_____|↕]",
            "password" => "[••••___]",
            "radio" => "○ ",
            "range" => "═══○═════",
            "reset" => "[ Reset ]",
            "submit" => "[ Submit ]",
            _ => "[_______]"
        };
        _accumulator.Add(text);
    }

    private void HtmlHeading(XElement el, double aspectFactor)
    {
        CloseBlock();
        using (_accumulator.CreateBoldScope())
        using (_accumulator.CreateFontAspect(aspectFactor)) {
            ParseElement(el);
            CloseBlock();
        }
    }
}
