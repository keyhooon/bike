using System;

using SkiaSharp;

namespace bike.Controls.SkColorDispersionMap
{
    public interface IShape
    {
        TimeSpan Time { get; }

        SKRect BoundingBox { get; }

        void UpdateOpacity(double opacity);

        void Draw(SKCanvas canvas, SKPaint paint);
    }
}