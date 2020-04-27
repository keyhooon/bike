using Mapsui.Geometries;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace bike.Controls.SkColorDispersionMap
{
    public class MultiWeightedLineString : MultiLineString, IEnumerable<(int Weight, LineString LineString)>
    {
        private IList<int> _weights;
        public IReadOnlyCollection<int> Weights { get => new ReadOnlyCollection<int>(_weights); }

        public new IReadOnlyCollection<LineString> LineStrings { get => new ReadOnlyCollection<LineString> (base.LineStrings); }
        public MultiWeightedLineString(IList<(int Weight, Point Vertex)> WeightedVertics) :base()
        {
            IList<Point> LastVertices = new List<Point>(new Point[] { WeightedVertics[0].Vertex }) ;
            int lastWeight = WeightedVertics[0].Weight;
            for (int i = 1; i < WeightedVertics.Count; i ++)
            {
                if (WeightedVertics[i].Weight == lastWeight)
                    LastVertices.Add(WeightedVertics[i].Vertex);
                else
                {
                    _weights.Add(lastWeight);
                    base.LineStrings.Add(new LineString(LastVertices));
                    LastVertices = new List<Point>(new Point[] { WeightedVertics[i].Vertex });
                    lastWeight = WeightedVertics[i].Weight;
                }
            }
        }

        private MultiWeightedLineString(IList<int> weight, IList<LineString> lineStrings) : base()
        {
            var __weights = new int[ weight.Count] ;
            weight.CopyTo(__weights, 0);
            _weights = new List<int>(__weights);

            var __lineStrings = new LineString[lineStrings.Count];
            for (int i = 0; i < lineStrings.Count; i++)
                __lineStrings[i] = lineStrings[i].Clone();
            base.LineStrings = new List<LineString>(__lineStrings);
        }

        public new (int Weight, LineString LineString) this[int index] => (Weight: _weights[index], LineString: base.LineStrings[index] );


        /// <summary>
        ///     Return a copy of this geometry
        /// </summary>
        /// <returns>Copy of Geometry</returns>
        public new MultiWeightedLineString Clone()
        {
            return new MultiWeightedLineString(_weights, base.LineStrings);
        }
        public new IEnumerator<(int Weight, LineString LineString)> GetEnumerator()
        {
            for (int i = 0; i < Weights.Count && i < LineStrings.Count; i++)
            {
                yield return this[i];
            }
        }

        /// <summary>
        ///     Gets an enumerator for enumerating the geometries in the GeometryCollection
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            for (int i = 0; i < Weights.Count && i < LineStrings.Count; i++)
            {
                yield return this[i];
            }
        }


    }
 
}
