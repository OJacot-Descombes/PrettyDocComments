﻿using Microsoft.VisualStudio.Utilities;
using PrettyDocComments.Helpers;
using PrettyDocComments.Model;
using System.Windows;
using System.Xml.Linq;

namespace PrettyDocComments.Services;

internal sealed partial class FormatParser
{
    private void ParseTable(XElement el)
    {
        double MinRowHeight = _view.FormattedLineSource.LineHeight / 2;

        List<Row> rows = GatherTableElements(el);
        double[] columnWidths = _estimator.EstimateColumnWidths(rows);
        if (columnWidths is null) {
            return;
        }
        ScaleColumnWidths(columnWidths, _accumulator.RemainingWidth);

        var parser = new FormatParser(_accumulator.Indent + Options.Padding.Left, _emSize, 0, _accumulator.FontAspect, _view);

        CloseBlock();
        foreach (Row row in rows) {
            if (row.IsCaption) {
                using (_accumulator.CreateWidthScope(_accumulator.Indent + columnWidths.Sum()))
                using (_accumulator.CreateAlignmentScope(TextAlignment.Center)) {
                    ParseElement(row.Cells[0].Element);
                    CloseBlock();
                }
                continue;
            }
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
                string localName = listItem.Name.LocalName;
                switch (localName) {
                    case "colgroup":
                        continue; // Ignore HTML tag containing column formatting information (so far).
                    case "thead" or "tbody" or "tfoot": // Ignore and skip one tag-level .
                        foreach (var item in listItem.Elements()) {
                            GatherCells(rows, item, item.Name.LocalName);
                        }
                        break;
                    default:
                        GatherCells(rows, listItem, localName);
                        break;
                }
            }

            return rows;

            static void GatherCells(List<Row> rows, XElement listItem, string localName)
            {
                var row = new Row {
                    IsHeader = localName is "listheader" || listItem.Elements("th").Any() ||
                        listItem.Parent.Name.LocalName is "tfoot", // Format like header
                    IsCaption = localName == "caption"
                };
                rows.Add(row);
                if (row.IsCaption) {
                    row.Cells.Add(new Cell { Element = listItem });
                } else {
                    foreach (XElement term in listItem.Elements()) { // We assume it's a <term>-tag.
                        row.Cells.Add(new Cell { Element = term });
                    }
                }
            }
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
