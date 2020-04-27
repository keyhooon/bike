using Mapsui.Styles;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace bike.Controls.SkColorDispersionMap
{
    public class MultiWeightedVectorStyle : Style, IEnumerable<(int Weight, VectorStyle Style)>
    {

        public IList<(int Weight, VectorStyle Style)> WeightedVectorStyles { get; set; }

        public MultiWeightedVectorStyle()
        {

        }

        public (int Weight, VectorStyle Style) this[int i] => WeightedVectorStyles[i];


        public override bool Equals(object obj)
        {
            if (!(obj is MultiWeightedVectorStyle))
            {
                return false;
            }
            return Equals((MultiWeightedVectorStyle)obj);
        }

        public bool Equals(MultiWeightedVectorStyle vectorStyles)
        {
            if (!base.Equals(vectorStyles))
            {
                return false;
            }
            for (int i = 0; i < WeightedVectorStyles.Count; i ++)
            {
                if ((WeightedVectorStyles[i].Style.Line == null) ^ (vectorStyles[i].Style.Line == null))
                {
                    return false;
                }

                if (WeightedVectorStyles[i].Style.Line != null && !WeightedVectorStyles[i].Style.Line.Equals(vectorStyles[i].Style.Line))
                {
                    return false;
                }

                if ((WeightedVectorStyles[i].Style.Outline == null) ^ (vectorStyles[i].Style.Outline == null))
                {
                    return false;
                }

                if (WeightedVectorStyles[i].Style.Outline != null && !WeightedVectorStyles[i].Style.Outline.Equals(vectorStyles[i].Style.Outline))
                {
                    return false;
                }

                if ((WeightedVectorStyles[i].Style.Fill == null) ^ (vectorStyles[i].Style.Fill == null))
                {
                    return false;
                }

                if (WeightedVectorStyles[i].Style.Fill != null && !WeightedVectorStyles[i].Style.Fill.Equals(vectorStyles[i].Style.Fill))
                {
                    return false;
                }
            }
     

            return true;
        }

        public override int GetHashCode()
        {
            int res = 0;
            foreach (var vectorStyle in WeightedVectorStyles)
            {
                res ^= vectorStyle.GetHashCode();
            }
            return res ^ base.GetHashCode();
        }

        public IEnumerator<(int Weight, VectorStyle Style)> GetEnumerator()
        {
            for (int i = 0; i < WeightedVectorStyles.Count; i++)
            {
                yield return this[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return WeightedVectorStyles.GetEnumerator();
        }

        public static bool operator ==(MultiWeightedVectorStyle vectorStyle1, MultiWeightedVectorStyle vectorStyle2)
        {
            return Equals(vectorStyle1, vectorStyle2);
        }

        public static bool operator !=(MultiWeightedVectorStyle vectorStyle1, MultiWeightedVectorStyle vectorStyle2)
        {
            return !Equals(vectorStyle1, vectorStyle2);
        }
    }
}
