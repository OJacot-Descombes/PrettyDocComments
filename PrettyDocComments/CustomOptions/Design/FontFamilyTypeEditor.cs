using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms.Design;

namespace PrettyDocComments.CustomOptions.Design;

internal class FontFamilyTypeEditor : UITypeEditor
{
    private IWindowsFormsEditorService _editorService;

    public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
    {
        return UITypeEditorEditStyle.DropDown;
    }

    public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
    {
        _editorService = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

        var lb = new FontFamilyListBox();
        lb.SelectedValueChanged += OnListBoxSelectedValueChanged;
        lb.Fill(value as string);
        _editorService.DropDownControl(lb);
        return lb.SelectedItem ?? value;
    }

    private void OnListBoxSelectedValueChanged(object sender, EventArgs e)
    {
        _editorService.CloseDropDown();
    }
}
