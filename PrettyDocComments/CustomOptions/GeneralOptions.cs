using System.ComponentModel;

namespace PrettyDocComments.CustomOptions;

internal class GeneralOptions : BaseOptionModel<GeneralOptions>
{
    [Category("Sizing")]
    [DisplayName("Font scaling")]
    [Description("By how much the editor font size is multiplied to get the comment base font size (default is 0.8).")]
    [DefaultValue(0.8)]
    public double FontScaling { get; set; } = 0.8;

    [Category("Sizing")]
    [DisplayName("Comment width in columns")]
    [Description("Width of the rendered doc comment in columns (default is 80).")]
    [DefaultValue(80)]
    public int CommentWidthInColumns { get; set; } = 80;

    [Category("Shape Colors")]
    [DisplayName("Comment background color")]
    [Description("Background color of rendred comments (default is LightGoldenrodYellow).")]
    [TypeConverter(typeof(System.Drawing.ColorConverter))]
    public System.Drawing.Color CommentBackColor { get; set; } = System.Drawing.Color.LightGoldenrodYellow;

    [Category("Shape Colors")]
    [DisplayName("Comment line color")]
    [Description("Color of comment frame and separator lines (default is DarkKhaki).")]
    [TypeConverter(typeof(System.Drawing.ColorConverter))]
    public System.Drawing.Color CommentLineColor { get; set; } = System.Drawing.Color.DarkKhaki;

    [Category("Shape Colors")]
    [DisplayName("Code block background color")]
    [Description("Background color of <code>blocks</code> (default is Gainsboro).")]
    [TypeConverter(typeof(System.Drawing.ColorConverter))]
    public System.Drawing.Color CodeBlockBackColor { get; set; } = System.Drawing.Color.Gainsboro;

    [Category("Text Colors")]
    [DisplayName("Text color")]
    [Description("Color of normal text (default is Black).")]
    [TypeConverter(typeof(System.Drawing.ColorConverter))]
    public System.Drawing.Color TextColor { get; set; } = System.Drawing.Color.Black;

    [Category("Text Colors")]
    [DisplayName("Special text color")]
    [Description("Color of special text, e.g. links and references (default is DarkSlateBlue).")]
    [TypeConverter(typeof(System.Drawing.ColorConverter))]
    public System.Drawing.Color SpecialTextColor { get; set; } = System.Drawing.Color.DarkSlateBlue;

    [Category("Text Colors")]
    [DisplayName("HTML comment text color")]
    [Description("Color of HTML <!-- comment --> text (default is ForestGreen).")]
    [TypeConverter(typeof(System.Drawing.ColorConverter))]
    public System.Drawing.Color HtmlCommentTextColor { get; set; } = System.Drawing.Color.ForestGreen;

    [Category("Text Colors")]
    [DisplayName("Error text and frame color")]
    [Description("Color used to display XML parsing errors (default is Red).")]
    [TypeConverter(typeof(System.Drawing.ColorConverter))]
    public System.Drawing.Color ErrorTextColor { get; set; } = System.Drawing.Color.Red;
}
