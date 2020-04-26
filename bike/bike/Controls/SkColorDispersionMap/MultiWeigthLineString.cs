using Mapsui.Geometries;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace bike.Controls.SkColorDispersionMap
{
    public class MultiWeigthLineString : MultiLineString, IEnumerable<(double Weight, LineString LineString)>
    {
        public IList<double> Weigths { get; set; }

        public MultiWeigthLineString() :base()
        {
            Weigths = new Collection<double>();
        }

        public new (double Weight, LineString LineString) this[int index] => (Weight: Weigths[index], LineString: LineStrings[index] );


        /// <summary>
        ///     Return a copy of this geometry
        /// </summary>
        /// <returns>Copy of Geometry</returns>
        public new MultiWeigthLineString Clone()
        {
            var geoms = new MultiWeigthLineString();
            for (int i = 0; i < LineStrings.Count; i++)
            {
                geoms.LineStrings.Add(LineStrings[i].Clone());
                geoms.Weigths.Add(Weigths[i]);
            }
            return geoms;
        }
        public new IEnumerator<(double Weight, LineString LineString)> GetEnumerator()
        {
            for (int i = 0; i < Weigths.Count && i < LineStrings.Count; i++)
            {
                yield return this[i];
            }
        }

        /// <summary>
        ///     Gets an enumerator for enumerating the geometries in the GeometryCollection
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            for (int i = 0; i < Weigths.Count && i < LineStrings.Count; i++)
            {
                yield return this[i];
            }
        }


    }
 
}
