namespace PrettyDocComments.Model;

internal readonly struct RenderInfo
{

    public readonly List<Shape> Shapes;

    public readonly double CalculatedHeight;

    public readonly double VerticalScale;

    public readonly bool ContainsErrorHint;

    public RenderInfo(List<Shape> shapes, double calculatedHeight, double verticalScale, bool containsErrorHint = false)
    {
        Shapes = shapes;
        CalculatedHeight = calculatedHeight;
        VerticalScale = verticalScale;
        ContainsErrorHint = containsErrorHint;
    }
}
