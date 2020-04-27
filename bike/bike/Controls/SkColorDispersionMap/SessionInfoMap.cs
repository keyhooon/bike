using bike.Helper;
using bike.Models;
using Mapsui.UI.Forms;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Infrastructure.Extension;
using Mapsui.UI.Objects;
using Mapsui.Providers;
using Mapsui.Styles;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;
using System.Linq;
using Mapsui.Geometries;

namespace bike.Controls.SkColorDispersionMap
{
    public class SessionInfoMap : BindableObject, IFeatureProvider
    {
        public static readonly BindableProperty WeightedPositionsProperty = BindableProperty.Create(nameof(WeightedPositions), typeof(ObservableRangeCollection<(int Weight, Position Position)>), typeof(SessionInfoMap), new ObservableRangeCollection<(int Weight, Position Position)>());
        public static readonly BindableProperty WeightedVectorStyleProperty = BindableProperty.Create(nameof(WeightedVectorStyle), typeof(MultiWeightedVectorStyle), typeof(SessionInfoMap), new MultiWeightedVectorStyle());

        public ObservableRangeCollection<(int Weight, Position Position)> WeightedPositions
        {
            get => (ObservableRangeCollection<(int Weight, Position Position)>)GetValue(WeightedPositionsProperty);
            set => SetValue(WeightedPositionsProperty, value);
        }

        public MultiWeightedVectorStyle WeightedVectorStyle
        {
            get => (MultiWeightedVectorStyle)GetValue(WeightedVectorStyleProperty);
            set => SetValue(WeightedVectorStyleProperty, value);
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="T:Mapsui.UI.Forms.Polyline"/> class.
        /// </summary>
        public SessionInfoMap()
        {
            CreateFeature();
        }


        private Feature feature;

        /// <summary>
        /// Mapsui Feature belonging to this drawable
        /// </summary>
        public Feature Feature
        {
            get
            {
                return feature;
            }
            set
            {
                if (feature == null || !feature.Equals(value))
                    feature = value;
            }
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            switch (propertyName)
            {
                case nameof(WeightedPositions):
                    Feature.Geometry = new MultiWeightedLineString(WeightedPositions.Select(p => (p.Weight, Vertex: p.Position.ToMapsui())).ToList());
                    WeightedPositions.CollectionChanged += (sender, e) => OnPropertyChanged(nameof(WeightedPositions)); ;
                    break;
                case nameof(WeightedVectorStyle):
                    Feature.Styles.Clear();
                    Feature.Styles.Add(WeightedVectorStyle);
                    break;
            }
        }

        private object sync = new object();

        /// <summary>
        /// Create feature
        /// </summary>
        private void CreateFeature()
        {
            lock (sync)
            {
                if (Feature == null)
                {
                    // Create a new one
                    Feature = new Feature
                    {
                        Geometry = new MultiWeightedLineString(WeightedPositions.Select(p => (p.Weight, Vertex: p.Position.ToMapsui())).ToList()),
                    };
                    Feature.Styles.Add(WeightedVectorStyle);
                }
            }
        }

    }
}
