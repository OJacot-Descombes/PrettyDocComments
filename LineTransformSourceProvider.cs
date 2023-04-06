using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Formatting;
using Microsoft.VisualStudio.Utilities;

namespace PrettyDocComments;

/// <summary>
/// Provides a line transform source so that line transforms can be properly calcualted for contract adornments.
/// </summary>
[Export(typeof(ILineTransformSourceProvider))]
[ContentType("code")]
[TextViewRole(PredefinedTextViewRoles.Document)]
internal sealed class LineTransformSourceProvider : ILineTransformSourceProvider
{
    public ILineTransformSource Create(IWpfTextView textView)
    {
        return new LineTransformSource(textView);
    }
}