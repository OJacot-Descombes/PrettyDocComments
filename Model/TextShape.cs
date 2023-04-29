﻿using System.Windows;
using System.Windows.Media;

namespace PrettyDocComments.Model;

internal sealed class TextShape : Shape
{
    public TextShape(FormattedText text, Point origin, double deltaY)
        : base(origin, deltaY)
    {
        _text = text;
    }

    private readonly FormattedText _text;

    public override void Draw(DrawingContext dc)
    {
        dc.DrawText(_text, _origin);
    }
}
