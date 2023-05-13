using System.Windows.Media;
using PrettyDocComments.Helpers;

namespace PrettyDocComments.Model;

internal readonly struct TextBlock
{
    public TextBlock(FormattedText text, double left, BackgroundType backgroundType = BackgroundType.Default)
    {
        Text = text;
        Left = left;
        DeltaY = Height = text.Height;
        BackgroundType = backgroundType;
    }

    public TextBlock(FormattedText text, double left, double deltaY, double height, BackgroundType backgroundType = BackgroundType.Default)
    {
        Text = text;
        Left = left;
        DeltaY = deltaY;
        Height = height;
        BackgroundType = backgroundType;
    }

    public readonly FormattedText Text;

    public readonly double Left;

    public readonly double DeltaY;

    public readonly double Height;

    public readonly BackgroundType BackgroundType;

    public readonly Brush Fill => BackgroundType switch {
        BackgroundType.Shaded or BackgroundType.FramedShaded => Options.CodeBackground,
        _ => null,
    };

    public readonly Pen Stroke => BackgroundType switch {
        BackgroundType.Framed or BackgroundType.FramedShaded => Options.FrameStroke,
        _ => null,
    };

    public override string ToString()
    {
        return $"{nameof(Text)}: \"{Text.Text}\", {nameof(Left)}: {Left}, {nameof(DeltaY)}: {DeltaY}, {nameof(Height)}: {Height}, {nameof(BackgroundType)}: {BackgroundType}";
    }
}
