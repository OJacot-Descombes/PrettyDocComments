using System.Diagnostics;
using System.Windows.Media;
using Microsoft.VisualStudio.Utilities;
using PrettyDocComments.Helpers;

namespace PrettyDocComments.Model;

[DebuggerDisplay("{nameof(Text)}: \"{Text.Text}\", {nameof(Left)}: {Left}, {nameof(DeltaY)}: {DeltaY}, {nameof(Height)}: {Height}, {nameof(BackgroundType)}: {BackgroundType}")]
internal readonly struct TextBlock
{
    public TextBlock(FormattedTextEx text, double left, BackgroundType backgroundType = BackgroundType.Default)
    {
        Text = text;
        Left = left;
        DeltaY = Height = text.Height + (backgroundType is BackgroundType.Default ? 0.0 : Options.Padding.GetHeight());
        BackgroundType = backgroundType;
    }

    public TextBlock(FormattedTextEx text, double left, double deltaY, double height, BackgroundType backgroundType = BackgroundType.Default)
    {
        Text = text;
        Left = left;
        DeltaY = deltaY;
        Height = height;
        BackgroundType = backgroundType;
    }

    public readonly FormattedTextEx Text;

    public readonly double Left;

    public readonly double DeltaY;

    public readonly double Height;

    public readonly BackgroundType BackgroundType;

    public readonly Brush Fill => BackgroundType switch {
        BackgroundType.CodeBlock or BackgroundType.FramedShaded => Options.CodeBackground,
        _ => null,
    };

    public readonly Pen Stroke => BackgroundType switch {
        BackgroundType.Framed or BackgroundType.FramedShaded => Options.FrameStroke,
        _ => null,
    };
}
