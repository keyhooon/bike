using Mapsui;
using Mapsui.Layers;
using Mapsui.Providers;
using Mapsui.Rendering;
using Mapsui.Rendering.Skia;
using Mapsui.Rendering.Skia.SkiaStyles;
using Mapsui.Styles;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace bike.Controls.SkColorDispersionMap
{
    public class MultiWeightedLineStringRenderer : ISkiaStyleRenderer
    {
        public bool Draw(SKCanvas canvas, IReadOnlyViewport viewport, ILayer layer, IFeature feature, IStyle style, ISymbolCache symbolCache)
        {
            MultiWeightedVectorStyle multiVectorStyle = (MultiWeightedVectorStyle)style;
            MultiWeightedLineString multiWeigthLineString = (MultiWeightedLineString)feature.Geometry;


            var paints = multiVectorStyle.OrderByDescending(o=>o.Weight).Select(o => (o.Weight, paint: new SKPaint
                {
                    IsAntialias = true,
                    IsStroke = true,
                    StrokeWidth = (float)(o.Style).Line.Width,
                    Color = o.Style.Line.Color.ToSkia((float)layer.Opacity * o.Style.Opacity),
                    StrokeCap = o.Style.Line.PenStrokeCap.ToSkia(),
                    StrokeJoin = o.Style.Line.StrokeJoin.ToSkia(),
                    StrokeMiter = o.Style.Line.StrokeMiterLimit,
                    PathEffect = o.Style.Line.PenStyle.ToSkia((float)o.Style.Line.Width, o.Style.Line.DashArray)
                })).ToDictionary(o=>o.Weight,o=>o.paint);



            foreach (var weigthlineString in multiWeigthLineString)
            {
                using (var path = weigthlineString.LineString.Vertices.ToSkiaPath(viewport, canvas.LocalClipBounds))
                { 
                    canvas.DrawPath(path, paints[weigthlineString.Weight]);
                }
            }
            return true;
        }
    }
}
