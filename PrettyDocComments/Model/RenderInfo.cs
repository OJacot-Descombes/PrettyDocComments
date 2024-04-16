namespace PrettyDocComments.Model;

internal readonly struct RenderInfo(List<Shape> shapes, double calculatedHeight, double verticalScale,
    bool containsErrorHint = false)
{
    public readonly List<Shape> Shapes = shapes;

    public readonly double CalculatedHeight = calculatedHeight;

    public readonly double VerticalScale = verticalScale;

    public readonly bool ContainsErrorHint = containsErrorHint;
}
