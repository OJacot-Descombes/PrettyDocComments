using System.Windows;
using System.Windows.Media;
using System.Xaml;
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

    public FormatParser(double indent, double width, double fontAspect, IWpfTextView view)
    {
        _accumulator = new FormatAccumulator(view, indent, width, fontAspect);
        _emSize = view.FormattedLineSource.DefaultTextProperties.FontRenderingEmSize * Options.FontScaling;
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
                        case "big":
                            using (_accumulator.CreateFontAspect(_accumulator.FontAspect * 1.2)) {
                                ParseElement(el);
                            }
                            break;
                        case "button":
                            _accumulator.Add(" [ "); ParseElement(el); _accumulator.Add(" ] ");
                            break;
                        case "blockquote":
                            CloseBlock();
                            using (_accumulator.CreateItalicScope())
                            using (_accumulator.CreateIndentScope(_emSize)) {
                                ParseElement(el);
                                CloseBlock();
                            }
                            break;
                        case "br":
                            _accumulator.AddLineBreak();
                            break;
                        case "b" or "strong":
                            using (_accumulator.CreateBoldScope()) {
                                ParseElement(el);
                            }
                            break;
                        case "i" or "cite" or "dfn" or "em":
                            using (_accumulator.CreateItalicScope()) {
                                ParseElement(el);
                            }
                            break;
                        case "u" or "ins":
                            using (_accumulator.CreateUnderlineScope()) {
                                ParseElement(el);
                            }
                            break;
                        case "s" or "strike" or "del":
                            using (_accumulator.CreateStrikethroughScope()) {
                                ParseElement(el);
                            }
                            break;
                        case "center":
                            CloseBlock();
                            using (_accumulator.CreateAlignmentScope(TextAlignment.Center)) {
                                ParseElement(el);
                                CloseBlock();
                            }
                            break;
                        case "c":
                            using (_accumulator.CreateCodeScope()) {
                                ParseElement(el, normalizeWS: false);
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
                        case "div" or "figcaption" or "footer" or "form":
                            // Insert line break before and after <div> as most browsers do. No processing of CSS styles so far.
                            CloseBlock();
                            ParseElement(el);
                            CloseBlock();
                            break;
                        case "remarks":
                            ParseWithTitle(el, "Remarks");
                            break;
                        case "example":
                            ParseWithTitle(el, "Example: ", 1.7 * _emSize);
                            break;
                        case "list" or "ul" or "ol" or "dl" or "menu" or "table" or "dir":
                            ParseList(el, normalizedTag);
                            break;
                        case "para" or "p":
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
                        case "term" when _listLevel >= 0:
                            using (_accumulator.CreateBoldScope()) {
                                ParseElement(el);
                                _accumulator.Add(" – ");
                            }
                            break;
                        case "description" when _listLevel >= 0:
                            ParseElement(el);
                            break;
                        case "paramref" or "typeparamref" or "permission" or "a" or "embed" or "img" or "frame":
                            Reference(el); // "name"
                            break;
                        case "see" or "seealso":
                            // Let's treat <seealso> like <see> if it is nested
                            // (according to Mahmoud Al-Qudsi: https://stackoverflow.com/a/69947292/880990)
                            _accumulator.Add("See: ");
                            Reference(el); // "cref", "langword", "href"
                            break;
                        case "include" or "inheritdoc":
                            using (_accumulator.CreateTextColorScope(Options.SpecialTextColor)) {
                                _accumulator.Add(normalizedTag);
                                _accumulator.Add("(");
                                _accumulator.Add(String.Join(" ", el.Attributes().Select(a => $"{a.Name}='{a.Value}'")));
                                _accumulator.Add(")");
                            }
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
                        case "input":
                            HtmlInputField(el);
                            break;
                        case "summary" or "figure" or "label":
                            // Render without special handling but do not display the tag name.
                            ParseElement(el);
                            break;
                        default: // Unsupported tags. Display tag name as a label.
                            using (_accumulator.CreateItalicScope()) {
                                _accumulator.Add(normalizedTag.FirstCap() + ": ");
                            }
                            ParseElement(el);
                            break;
                    }
                    previousTag = normalizedTag;
                    break;
                case XComment comment:
                    CloseBlock();
                    using (_accumulator.CreateTextColorScope(Options.CommentTextColor)) {
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
        string text = String.IsNullOrEmpty(el.Value)
            ? el.Attributes().FirstOrDefault()?.Value is { Length: > 0 } attributeValue
                  ? attributeValue
                  : el.Name.LocalName
            : el.Value;
        using (_accumulator.CreateTextColorScope(Options.SpecialTextColor)) {
            _accumulator.Add(text);
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
    /// This method removes the indentation of an XML element's value.
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

    private void HtmlInputField(XElement el)
    {
        string text = el.Attribute("type")?.Value switch {
            "button" when el.Attribute("value")?.Value is var value => $"[ {value} ]",
            "checkbox" => "[X] ",
            "color" or "date" or "datetime-local" => "[____|▼]",
            "file" => "[ Browse... ]",
            "hidden" => "",
            "image" when el.Attribute("src")?.Value is var src => $"[ {src} ]",
            "number" => "[____|↕]",
            "password" => "[••••__]",
            "radio" => "◯ ",
            "range" => "━━⬤━━━━",
            "reset" => "[ Reset ]",
            "submit" => "[ Submit ]",
            _ => "[______]"
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
