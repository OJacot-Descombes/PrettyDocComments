using System.Windows.Media;

namespace PrettyDocComments;

internal readonly struct TextBlock
{
    public TextBlock(FormattedText text, double left, Brush background = default)
    {
        Text = text;
        Left = left;
        Height = text.Height;
        Background = background;
    }

    public TextBlock(FormattedText text, double left, double height, Brush background = default)
    {
        Text = text;
        Left = left;
        Height = height;
        Background = background;
    }

    public readonly FormattedText Text;

    public readonly double Left;

    public readonly double Height;

    public readonly Brush Background;
}
