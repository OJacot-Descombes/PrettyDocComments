using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Media;
using PrettyDocComments.CustomOptions.Design;

namespace PrettyDocComments.CustomOptions;

internal class GeneralOptions : BaseOptionModel<GeneralOptions>
{
    private const string TestColorsCategory = "Text Colors";
    private const string SizingCategory = "Sizing";
    private const string ShapeColorsCategory = "Shape Colors";
    private const string FontsCategory = "Fonts";

    [Category(SizingCategory)]
    [DisplayName("Font scaling")]
    [Description("By how much the editor font size is multiplied to get the comment base font size (default is 0.8).")]
    [DefaultValue(0.8)]
    public double FontScaling { get; set; } = 0.8;

    [Category(SizingCategory)]
    [DisplayName("Comment width in columns")]
    [Description("Width of the rendered doc comment in columns (default is 80).")]
    [DefaultValue(80)]
    public int CommentWidthInColumns { get; set; } = 80;

    [Category(ShapeColorsCategory)]
    [DisplayName("Comment background color")]
    [Description("Background color of rendred comments (default is LightGoldenrodYellow).")]
    [TypeConverter(typeof(System.Drawing.ColorConverter))]
    public System.Drawing.Color CommentBackColor { get; set; } = System.Drawing.Color.LightGoldenrodYellow;

    [Category(ShapeColorsCategory)]
    [DisplayName("Comment line color")]
    [Description("Color of comment frame and separator lines (default is DarkKhaki).")]
    [TypeConverter(typeof(System.Drawing.ColorConverter))]
    public System.Drawing.Color CommentLineColor { get; set; } = System.Drawing.Color.DarkKhaki;

    [Category(ShapeColorsCategory)]
    [DisplayName("Code block background color")]
    [Description("Background color of <code>blocks</code> (default is Gainsboro).")]
    [TypeConverter(typeof(System.Drawing.ColorConverter))]
    public System.Drawing.Color CodeBlockBackColor { get; set; } = System.Drawing.Color.Gainsboro;

    [Category(ShapeColorsCategory)]
    [DisplayName("Highlight color")]
    [Description("Default background color of <mark>blocks</mark> (default is Yellow).")]
    [TypeConverter(typeof(System.Drawing.ColorConverter))]
    public System.Drawing.Color HighlightColor { get; set; } = System.Drawing.Color.Yellow;

    [Category(TestColorsCategory)]
    [DisplayName("Text color")]
    [Description("Color of normal text (default is Black).")]
    [TypeConverter(typeof(System.Drawing.ColorConverter))]
    public System.Drawing.Color TextColor { get; set; } = System.Drawing.Color.Black;

    [Category(TestColorsCategory)]
    [DisplayName("Special text color")]
    [Description("Color of special text, e.g. links and references (default is DarkSlateBlue).")]
    [TypeConverter(typeof(System.Drawing.ColorConverter))]
    public System.Drawing.Color SpecialTextColor { get; set; } = System.Drawing.Color.DarkSlateBlue;

    [Category(TestColorsCategory)]
    [DisplayName("HTML comment text color")]
    [Description("Color of HTML <!-- comment --> text (default is ForestGreen).")]
    [TypeConverter(typeof(System.Drawing.ColorConverter))]
    public System.Drawing.Color HtmlCommentTextColor { get; set; } = System.Drawing.Color.ForestGreen;

    [Category(TestColorsCategory)]
    [DisplayName("Error text and frame color")]
    [Description("Color used to display XML parsing errors (default is Red).")]
    [TypeConverter(typeof(System.Drawing.ColorConverter))]
    public System.Drawing.Color ErrorTextColor { get; set; } = System.Drawing.Color.Red;

    [Category(FontsCategory)]
    [DisplayName("Default Font")]
    [Description("Font used to display the comment text (default is Segoe UI).")]
    [Editor(typeof(FontFamilyTypeEditor), typeof(UITypeEditor))]
    public string TextFont { get; set; } = "Segoe UI";

    [Category(FontsCategory)]
    [DisplayName("Code Font")]
    [Description("Font used to display code (default is Cascadia Mono).")]
    [Editor(typeof(FontFamilyTypeEditor), typeof(UITypeEditor))]
    public string CodeFont { get; set; } = "Cascadia Mono";
}
