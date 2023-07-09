using System.ComponentModel;
using Microsoft.VisualStudio.Shell;
using PrettyDocComments.Helpers;

namespace PrettyDocComments.CustomOptions;

public class OptionPageGrid : DialogPage
{
    private static readonly TypeConverter _colorConverter = new System.Drawing.ColorConverter();

    //[Category("General")]
    //[DisplayName("Enabled")]
    //[Description("Indicates whether this extension is enabled.")]
    //[DefaultValue(true)]
    //public bool Enabled { get; set; } = true;

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
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public DrawingColor CommentBackColor { get; set; } = DrawingColor.LightGoldenrodYellow;

    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public string CommentBackColorString { 
        get => (string)_colorConverter.ConvertTo(CommentBackColor,typeof(string)); 
        set => CommentBackColor = (DrawingColor)_colorConverter.ConvertFrom(value); 
    }

    [Category("Shape Colors")]
    [DisplayName("Comment line color")]
    [Description("Color of comment frame and separator lines (default is DarkKhaki).")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public DrawingColor CommentLineColor { get; set; } = DrawingColor.DarkKhaki;

    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public string CommentLineColorString
    {
        get => (string)_colorConverter.ConvertTo(CommentLineColor, typeof(string));
        set => CommentLineColor = (DrawingColor)_colorConverter.ConvertFrom(value);
    }

    [Category("Shape Colors")]
    [DisplayName("Code block background color")]
    [Description("Background color of <code>blocks</code> (default is Gainsboro).")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public DrawingColor CodeBlockBackColor { get; set; } = DrawingColor.Gainsboro;

    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public string CodeBlockBackColorString
    {
        get => (string)_colorConverter.ConvertTo(CodeBlockBackColor, typeof(string));
        set => CodeBlockBackColor = (DrawingColor)_colorConverter.ConvertFrom(value);
    }

    [Category("Text Colors")]
    [DisplayName("Text color")]
    [Description("Color of normal text (default is Black).")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public DrawingColor TextColor { get; set; } = DrawingColor.Black;

    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public string TextColorString
    {
        get => (string)_colorConverter.ConvertTo(TextColor, typeof(string));
        set => TextColor = (DrawingColor)_colorConverter.ConvertFrom(value);
    }

    [Category("Text Colors")]
    [DisplayName("Special text color")]
    [Description("Color of special text, e.g. links and references (default is DarkSlateBlue).")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public DrawingColor SpecialTextColor { get; set; } = DrawingColor.DarkSlateBlue;

    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public string SpecialTextColorString
    {
        get => (string)_colorConverter.ConvertTo(SpecialTextColor, typeof(string));
        set => SpecialTextColor = (DrawingColor)_colorConverter.ConvertFrom(value);
    }

    [Category("Text Colors")]
    [DisplayName("HTML comment text color")]
    [Description("Color of HTML <!-- comment --> text (default is ForestGreen).")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public DrawingColor HtmlCommentTextColor { get; set; } = DrawingColor.ForestGreen;

    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public string HtmlCommentTextColorString
    {
        get => (string)_colorConverter.ConvertTo(HtmlCommentTextColor, typeof(string));
        set => HtmlCommentTextColor = (DrawingColor)_colorConverter.ConvertFrom(value);
    }

    [Category("Text Colors")]
    [DisplayName("Error text and frame color")]
    [Description("Color used to display XML parsing errors (default is Red).")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public DrawingColor ErrorTextColor { get; set; } = DrawingColor.Red;

    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public string ErrorTextColorString
    {
        get => (string)_colorConverter.ConvertTo(ErrorTextColor, typeof(string));
        set => ErrorTextColor = (DrawingColor)_colorConverter.ConvertFrom(value);
    }

    public override void SaveSettingsToStorage()
    {
        base.SaveSettingsToStorage();
        Options.Refresh();
    }
}
