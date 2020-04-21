using System;

using SkiaSharp;

namespace bike.Controls.SkColorDispersionMap
{
    public abstract class AShape : IShape
    {
        private SKRect _boundingBox = SKRect.Empty;

        public TimeSpan Time { get; protected set; }

        public SKRect BoundingBox => _boundingBox == SKRect.Empty ? _boundingBox = ComputeBoundBox() : _boundingBox;

        public abstract void UpdateOpacity(double opacity);

        public abstract void Draw(SKCanvas canvas, SKPaint paint);

        protected abstract SKRect ComputeBoundBox();
    }
}