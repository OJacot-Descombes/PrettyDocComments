using System.Xml.Linq;
using PrettyDocComments.Helpers;
using PrettyDocComments.Model;

namespace PrettyDocComments.Services;

internal class WidthEstimator
{
    // Takes word wrapping into account and prevents extreme column width ratios.
    const int MinCellChars = 10, MaxCellChars = 100;

    public double[] EstimateColumnWidths(List<Row> rows)
    {
        // We only return the realtive column sizes. Later, these values will be scaled to make the total width of
        // the table fit the available space. Therefore we only estimate the number of characters per line. 


        if (rows.Count == 0) {
            return null;
        }
        int columnCount = rows.Max(r => r.Cells.Count);
        if (columnCount == 0) {
            return null;
        }
        double[] columnWidths = new double[columnCount];

        for (int i = 0; i < columnCount; i++) {
            int numChars = MinCellChars;
            foreach (Row row in rows) {
                if (i < row.Cells.Count) {
                    numChars = Math.Max(numChars, EstimateElementWidth(row.Cells[i].Element));
                    if (numChars >= MaxCellChars) {
                        break;
                    }
                }
            }
            columnWidths[i] = Math.Min(MaxCellChars, numChars);
        }

        return columnWidths;
    }

    private static int EstimateElementWidth(XElement node, bool normalizeWS = true, int listLevel = 0)
    {
        int maxNumChars = 0, currentLineWidth = 0;
        foreach (XNode child in node.Nodes()) {
            switch (child) {
                case XText xText:
                    currentLineWidth += xText.Value.NormalizeSpace(normalizeWS).Length;
                    break;
                case XElement el:
                    string normalizedTag = el.Name.LocalName.ToLowerInvariant();
                    switch (normalizedTag) {
                        case "b" or "strong" or "i" or "u" or "s" or "strike":
                            currentLineWidth += EstimateElementWidth(el, normalizeWS);
                            break;
                        case "br":
                            maxNumChars = Math.Max(maxNumChars, currentLineWidth);
                            currentLineWidth = 0;
                            break;
                        case "c":
                            currentLineWidth += EstimateElementWidth(el, false);
                            break;
                        case "code":
                            //TODO: Estimate code width better after removing unnecessary indent and taking longest line. The latter must be done in the DocCommentParser as well.
                            //    ParseElement(el, normalizeWS: false);
                            //    CloseBlock(BackgroundType.Shaded);
                            break;
                        case "remarks" or "example" or "para" or "seealso":
                            maxNumChars = Math.Max(maxNumChars, currentLineWidth);
                            maxNumChars = Math.Max(maxNumChars, EstimateElementWidth(el, normalizeWS));
                            currentLineWidth = 0;
                            break;
                        case "list" or "ul" or "ol" or "dl" or "menu":// TODO: estimate width of tables
                            listLevel++;
                            break;
                        case "listheader" or "item" when listLevel >= 0:
                            maxNumChars = Math.Max(maxNumChars, currentLineWidth);
                            maxNumChars = Math.Max(maxNumChars,
                                EstimateElementWidth(el, normalizeWS, listLevel) + 3 * listLevel);
                            currentLineWidth = 0;
                            break;
                        case "term" or "dt" when listLevel >= 0:
                            currentLineWidth += EstimateElementWidth(el) + 3;
                            break;
                        case "description" when listLevel >= 0:
                            currentLineWidth += EstimateElementWidth(el);
                            break;
                        case "paramref" or "typeparamref" or "see":
                            currentLineWidth += EstimateReferenceWidth(el);
                            break;
                        case "include":
                            currentLineWidth += "include(".Length +
                                el.Attributes().Sum(a => a.Name.ToString().Length + 1 + a.Value.Length) + 2;
                            break;
                        default:
                            maxNumChars = Math.Max(maxNumChars, el.ToString().NormalizeSpace(normalizeWS).Length);
                            break;
                    }
                    break;
                default:
                    maxNumChars = Math.Max(maxNumChars, child.ToString().NormalizeSpace(normalizeWS).Length);
                    break;
            }
            if (maxNumChars >= MaxCellChars) {
                break;
            }
        }
        return Math.Max(maxNumChars, currentLineWidth);

        static int EstimateReferenceWidth(XElement el)
        {
            return
                (String.IsNullOrEmpty(el.Value)
                    ? el.Attributes().FirstOrDefault()?.Value is { Length: > 0 } attributeValue
                          ? attributeValue
                          : el.Name.LocalName
                    : el.Value
                ).Length;
        }
    }
}
