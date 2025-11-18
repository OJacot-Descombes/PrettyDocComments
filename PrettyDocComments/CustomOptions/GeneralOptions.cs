using System.ComponentModel;
using System.Drawing.Design;
using PrettyDocComments.CustomOptions.Design;

namespace PrettyDocComments.CustomOptions;

internal class GeneralOptions : BaseOptionModel<GeneralOptions>
{
    private const string GeneralCategory = "General";
    private const string TextColorsCategory = "Text Colors";
    private const string SizingCategory = "Sizing";
    private const string ShapeColorsCategory = "Shape Colors";
    private const string FontsCategory = "Fonts";

    [Category(GeneralCategory)]
    [DisplayName("Enable extension")]
    [Description("Whether this extention is enabled and overlays XML doc comments with a rendered image.\r\nYou can also toggle it from the Edit menu and assign it a key shortcut.")]
    [DefaultValue(true)]
    public bool Enabled { get; set; } = true;

    [Category(SizingCategory)]
    [DisplayName("Font scaling")]
    [Description("By how much the editor font size is multiplied to get the comment base font size (default is 0.8).")]
    [DefaultValue(0.8)]
    public double FontScaling { get; set; } = 0.8;

    [Category(SizingCategory)]
    [DisplayName("Minimum comment width in columns")]
    [Description("Minimum width of the rendered doc comment in columns (default is 80).")]
    [DefaultValue(80)]
    public int CommentWidthInColumns { get; set; } = 80;

    [Category(SizingCategory)]
    [DisplayName("Right edge of comment")]
    [Description("Right edge of the rendered doc comment in columns (default is 120).")]
    [DefaultValue(120)]
    public int RightMarginInColumns { get; set; } = 120;

    [Category(SizingCategory)]
    [DisplayName("Adjust width to view port")]
    [Description("Automatically adjusts the width of the comment to the editor window within the limits set by the minimum width and the right margin settings (default is true).")]
    [DefaultValue(true)]
    public bool AdjustWidthToViewPort { get; set; } = true;

    [Category(SizingCategory)]
    [DisplayName("Collapse Comments to Summary")]
    [Description("Whether to show only the Summary or the whole comments.\r\nYou can also toggle it from the Edit menu and assign it a key shortcut.")]
    [DefaultValue(false)]
    public bool CollapseToSummary { get; set; }

    [Category(SizingCategory)]
    [DisplayName("Compensate line height rounding (experimental)")]
    [Description("Compensate for Visual Studio's line height rounding (experimental).")]
    [DefaultValue(true)]
    public bool CompensateLineHeight { get; set; } = true;

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

    [Category(TextColorsCategory)]
    [DisplayName("Text color")]
    [Description("Color of normal text (default is Black).")]
    [TypeConverter(typeof(System.Drawing.ColorConverter))]
    public System.Drawing.Color TextColor { get; set; } = System.Drawing.Color.Black;

    [Category(TextColorsCategory)]
    [DisplayName("Special text color")]
    [Description("Color of special text, e.g. links and references (default is DarkSlateBlue).")]
    [TypeConverter(typeof(System.Drawing.ColorConverter))]
    public System.Drawing.Color SpecialTextColor { get; set; } = System.Drawing.Color.DarkSlateBlue;

    [Category(TextColorsCategory)]
    [DisplayName("HTML comment text color")]
    [Description("Color of HTML <!-- comment --> text (default is ForestGreen).")]
    [TypeConverter(typeof(System.Drawing.ColorConverter))]
    public System.Drawing.Color HtmlCommentTextColor { get; set; } = System.Drawing.Color.ForestGreen;

    [Category(TextColorsCategory)]
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
