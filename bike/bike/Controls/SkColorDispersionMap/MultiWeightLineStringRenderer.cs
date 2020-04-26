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
    public class MultiWeightLineStringRenderer : ISkiaStyleRenderer
    {
        public bool Draw(SKCanvas canvas, IReadOnlyViewport viewport, ILayer layer, IFeature feature, IStyle style, ISymbolCache symbolCache)
        {
            MultiVectorStyle multiVectorStyle = (MultiVectorStyle)style;
            MultiWeigthLineString multiWeigthLineString = (MultiWeigthLineString)feature.Geometry;


            double[] StyleUpperLimit = multiVectorStyle.Select(vectorStyle => vectorStyle.Weight).OrderByDescending(o=>o).ToArray();
            var paints = multiVectorStyle.Select(vectorStyle => 
            {
                var paint = new SKPaint
                {
                    IsAntialias = true,
                    IsStroke = true,
                    StrokeWidth = (float)(vectorStyle.Style).Line.Width,
                    Color = vectorStyle.Style.Line.Color.ToSkia((float)layer.Opacity * vectorStyle.Style.Opacity),
                    StrokeCap = vectorStyle.Style.Line.PenStrokeCap.ToSkia(),
                    StrokeJoin = vectorStyle.Style.Line.StrokeJoin.ToSkia(),
                    StrokeMiter = vectorStyle.Style.Line.StrokeMiterLimit
                };
                if (vectorStyle.Style.Line.PenStyle != PenStyle.Solid)
                    paint.PathEffect = vectorStyle.Style.Line.PenStyle.ToSkia((float)vectorStyle.Style.Line.Width, vectorStyle.Style.Line.DashArray);
                else
                    paint.PathEffect = null;
                return paint;
            }).ToArray();



            foreach (var weigthlineString in multiWeigthLineString)
            {
                using (var path = weigthlineString.LineString.Vertices.ToSkiaPath(viewport, canvas.LocalClipBounds))
                { 
                    int index = 0;
                    while (weigthlineString.Weight < StyleUpperLimit[index])
                        index++;
                    canvas.DrawPath(path, paints[index]);
                }
            }
            return true;
        }
    }
}
