using SkiaSharp;

namespace bike.Controls.SkColorDispersionMap
{
    public abstract class ASinglePointShape : AShape, ISinglePointShape
    {
        public SKPoint Point { get; protected set; }
    }
}