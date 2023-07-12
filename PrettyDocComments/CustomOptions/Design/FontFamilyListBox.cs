using System.Drawing;
using System.Windows.Forms;

namespace PrettyDocComments.CustomOptions.Design;

internal class FontFamilyListBox : ListBox
{
    public FontFamilyListBox()
    {
        IntegralHeight = false;
        SelectionMode = SelectionMode.One;
        ItemHeight = Math.Max(ItemHeight, 18);
        Height = 16 * ItemHeight + 3;
        DrawMode = DrawMode.OwnerDrawFixed;
    }

    public void Fill(string selectedValue)
    {
        foreach (var family in FontFamily.Families) {
            int index = Items.Add(family.Name);
            if (family.Name == selectedValue) {
                SelectedIndex = index;
            }
        }
    }

    protected override void OnDrawItem(DrawItemEventArgs e)
    {
        const string ExampleText = "Abc 123";

        string fontFamilyName = (string)Items[e.Index];
        using var itemFont = new Font(fontFamilyName, 12);
        using var brush = new SolidBrush(e.ForeColor);
        e.DrawBackground();
        e.Graphics.DrawString(itemFont.Name, Font, brush, e.Bounds.X, e.Bounds.Y);

        float textWidth = e.Graphics.MeasureString(itemFont.Name, Font).Width;
        float exampleWidth = e.Graphics.MeasureString(ExampleText, itemFont).Width;
        if (textWidth + exampleWidth + 3 < e.Bounds.Width) {
            e.Graphics.DrawString(ExampleText, itemFont, brush, e.Bounds.Right - exampleWidth - 1, e.Bounds.Y);
        }
        e.DrawFocusRectangle();
    }
}
