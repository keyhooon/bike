using Mapsui.Styles;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace bike.Controls.SkColorDispersionMap
{
    public class MultiVectorStyle : Style, IEnumerable<(double Weight, VectorStyle Style)>
    {

        public IList<(double Weight, VectorStyle Style)> VectorStyles { get; set; }

        public MultiVectorStyle()
        {

        }

        public (double Weight, VectorStyle Style) this[int i] => VectorStyles[i];


        public override bool Equals(object obj)
        {
            if (!(obj is MultiVectorStyle))
            {
                return false;
            }
            return Equals((MultiVectorStyle)obj);
        }

        public bool Equals(MultiVectorStyle vectorStyles)
        {
            if (!base.Equals(vectorStyles))
            {
                return false;
            }
            for (int i = 0; i < VectorStyles.Count; i ++)
            {
                if ((VectorStyles[i].Style.Line == null) ^ (vectorStyles[i].Style.Line == null))
                {
                    return false;
                }

                if (VectorStyles[i].Style.Line != null && !VectorStyles[i].Style.Line.Equals(vectorStyles[i].Style.Line))
                {
                    return false;
                }

                if ((VectorStyles[i].Style.Outline == null) ^ (vectorStyles[i].Style.Outline == null))
                {
                    return false;
                }

                if (VectorStyles[i].Style.Outline != null && !VectorStyles[i].Style.Outline.Equals(vectorStyles[i].Style.Outline))
                {
                    return false;
                }

                if ((VectorStyles[i].Style.Fill == null) ^ (vectorStyles[i].Style.Fill == null))
                {
                    return false;
                }

                if (VectorStyles[i].Style.Fill != null && !VectorStyles[i].Style.Fill.Equals(vectorStyles[i].Style.Fill))
                {
                    return false;
                }
            }
     

            return true;
        }

        public override int GetHashCode()
        {
            int res = 0;
            foreach (var vectorStyle in VectorStyles)
            {
                res ^= vectorStyle.GetHashCode();
            }
            return res ^ base.GetHashCode();
        }

        public IEnumerator<(double Weight, VectorStyle Style)> GetEnumerator()
        {
            for (int i = 0; i < VectorStyles.Count; i++)
            {
                yield return this[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return VectorStyles.GetEnumerator();
        }

        public static bool operator ==(MultiVectorStyle vectorStyle1, MultiVectorStyle vectorStyle2)
        {
            return Equals(vectorStyle1, vectorStyle2);
        }

        public static bool operator !=(MultiVectorStyle vectorStyle1, MultiVectorStyle vectorStyle2)
        {
            return !Equals(vectorStyle1, vectorStyle2);
        }
    }
}
