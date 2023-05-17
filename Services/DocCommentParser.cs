using System.Windows.Media;
using System.Xml.Linq;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;
using PrettyDocComments.Helpers;
using PrettyDocComments.Model;

namespace PrettyDocComments.Services;

internal sealed class DocCommentParser
{
    private static readonly string[] _levelBullets = { "●", "■", "○" };
    private static readonly WidthEstimator _estimator = new();

    private readonly FormatAccumulator _accumulator;
    private readonly double _emSize;
    private readonly IWpfTextView _view; // TODO: remove when ParseTable is done.

    private List<TextBlock> _textBlocks;
    private int _listLevel = -1;

    public DocCommentParser(double indent, double width, IWpfTextView view)
    {
        _accumulator = new FormatAccumulator(view, indent, width);
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

    private void ParseElement(XElement node, bool normalizeWS = true)
    {
        foreach (XNode child in node.Nodes()) {
            switch (child) {
                case XText xText:
                    _accumulator.Add(xText.Value.NormalizeSpace(normalizeWS));
                    break;
                case XElement el:
                    string normalizedTag = el.Name.LocalName.ToLowerInvariant();
                    switch (normalizedTag) {
                        case "br":
                            _accumulator.Add("\r\n");
                            break;
                        case "b" or "strong":
                            using (_accumulator.CreateBoldScope()) {
                                ParseElement(el);
                            }
                            break;
                        case "i":
                            using (_accumulator.CreateItalicScope()) {
                                ParseElement(el);
                            }
                            break;
                        case "u":
                            using (_accumulator.CreateUnderlineScope()) {
                                ParseElement(el);
                            }
                            break;
                        case "s" or "strike":
                            using (_accumulator.CreateStrikethroughScope()) {
                                ParseElement(el);
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
                            using (_accumulator.CreateBoldScope()) {
                                ParseElement(el);
                                _accumulator.Add(" – ");
                            }
                            break;
                        case "description" when _listLevel >= 0:
                            ParseElement(el);
                            break;
                        case "paramref" or "typeparamref":
                            Reference(el); // "name"
                            break;
                        case "see" or "seealso":
                            // Let's treat <seealso> like <see> if it is nested
                            // (according to Mahmoud Al-Qudsi: https://stackoverflow.com/a/69947292/880990)
                            Reference(el); // "cref", "langword", "href"
                            break;
                        case "include":
                            using (_accumulator.CreateTextColorScope(Options.SpecialTextColor)) {
                                _accumulator.Add("include(");
                                _accumulator.Add(String.Join(" ", el.Attributes().Select(a => $"{a.Name}='{a.Value}'")));
                                _accumulator.Add(")");
                            }
                            break;
                        default:
                            _accumulator.Add(el.ToString().NormalizeSpace(normalizeWS));
                            break;
                    }
                    break;
                default:
                    _accumulator.Add(child.ToString().NormalizeSpace(normalizeWS));
                    break;
            }
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

    private void ParseList(XElement el, string listTag)
    {
        // The <list> tag denotes a doc-comment type list, other types (ul, ol, dl, menu) are HTML type lists.

        const string Nul = null;

        string type = el.Attributes("type").FirstOrDefault()?.Value;
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
        foreach (var listItem in el.Elements()) {
            if (listItem.Name.LocalName == "listheader") {
                CloseBlock();
                using (_accumulator.CreateUnderlineScope()) {
                    foreach (var headerElement in listItem.Elements()) {
                        ParseElement(headerElement);
                        _accumulator.Add("\r\n");
                    }
                }
            } else { // textBlock
                CloseBlock();
                string actualBullet = numberType switch {
                    null => bullet ?? _levelBullets[_listLevel % _levelBullets.Length],
                    "A" => number.ToAlphabet() + ". ",
                    "a" => number.ToAlphabet(lowerCase: true) + ". ",
                    "I" => number.ToRoman() + ". ",
                    "i" => number.ToRoman(lowerCase: true) + ". ",
                    _ => number.ToString() + ". "
                };
                _accumulator.Add(actualBullet);
                TextBlock? textBlock = CloseBlock(height: 0.0);
                double itemIndent = type == "table" ? 0.0 : Math.Max(textBlock?.Text.Width ?? 0.0, _emSize) + 0.5 * _emSize;
                using (_accumulator.CreateIndentScope(itemIndent)) {
                    ParseElement(listItem);
                    CloseBlock();
                };
                number++;
            }
        }
        _listLevel--;
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

    private void ParseTable(XElement el)
    {
        double MinRowHeight = _view.FormattedLineSource.LineHeight / 2;

        List<Row> rows = GatherTableElements(el);
        double[] columnWidths = _estimator.EstimateColumnWidths(rows);
        if (columnWidths is null) {
            return;
        }
        ScaleColumnWidths(columnWidths, _accumulator.RemainingWidth);

        var parser = new DocCommentParser(_accumulator.Indent + Options.Padding.Left, 0, _view);

        CloseBlock();
        foreach (Row row in rows) {
            parser.ParseCells(row, columnWidths);
            double rowHeight = Math.Max(MinRowHeight, row.Cells
                .Select(c => c.Height)
                .DefaultIfEmpty(0)
                .Max());
            BackgroundType backgroundType = row.IsHeader ? BackgroundType.FramedShaded : BackgroundType.Framed;
            double left = _accumulator.Indent;
            for (int i = 0; i < columnWidths.Length; i++) {
                double columnWidth = columnWidths[i];

                // Add single TextBlock as Frame.
                _textBlocks.Add(new TextBlock(
                    "".AsFormatted(Options.NormalTypeFace, columnWidth - Options.Padding.GetWidth(), _view),
                    left, +Options.Padding.Top, height: rowHeight, backgroundType));
                double deltaY;
                if (row.Cells.Count > i) {
                    Cell cell = row.Cells[i];
                    _textBlocks.AddRange(cell.TextBlocks);
                    deltaY = -(cell.Height - Options.Padding.Bottom); // Jump upwards for next column's cell.
                } else {
                    deltaY = -Options.Padding.Top;
                }
                TextBlock tb = _textBlocks.Last();
                deltaY += tb.DeltaY;
                if (i == columnWidths.Length - 1) { // Jump down to next row.
                    deltaY += rowHeight;
                }

                _textBlocks[_textBlocks.Count - 1] = new TextBlock(tb.Text, tb.Left, deltaY, tb.Height, tb.BackgroundType);

                left += columnWidth;
            }
        }

        static List<Row> GatherTableElements(XElement el)
        {
            var rows = new List<Row>();
            foreach (var listItem in el.Elements()) {
                var row = new Row { IsHeader = listItem.Name.LocalName == "listheader" };
                rows.Add(row);
                foreach (XElement term in listItem.Elements()) { // We assume it's a <term>-tag.
                    row.Cells.Add(new Cell { Element = term });
                }
            }

            return rows;
        }

        static void ScaleColumnWidths(double[] columnWidths, double remainingWidth)
        {
            double columnScaling = (remainingWidth - Options.Padding.Right) / columnWidths.Sum();
            for (int i = 0; i < columnWidths.Length; i++) {
                columnWidths[i] *= columnScaling;
            }
        }
    }

    public void ParseCells(Row row, double[] columnWidths)
    {
        double deltaIndent = 0;
        for (int i = 0; i < columnWidths.Length; i++) {
            double columnWidth = columnWidths[i];
            using (_accumulator.CreateIndentScope(deltaIndent))
            using (_accumulator.CreateWidthScope(_accumulator.Indent + columnWidth - Options.Padding.GetWidth())) {
                if (row.Cells.Count > i) {
                    Cell cell = row.Cells[i];
                    cell.TextBlocks = Parse(cell.Element);
                }
            }
            deltaIndent += columnWidth;
        }
    }
}
