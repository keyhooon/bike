using SkiaSharp;

namespace bike.Controls.SkColorDispersionMap
{
    public interface ISinglePointShape : IShape
    {
        SKPoint Point { get; }
    }
}