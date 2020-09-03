using Syncfusion.ListView.XForms;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace bike.Infrastructure
{
    public class ItemGeneratorExt : ItemGenerator
    {
        public SfListView listView;

        public ItemGeneratorExt(SfListView listView) : base(listView)
        {
            this.listView = listView;
        }

        protected override ListViewItem OnCreateListViewItem(int itemIndex, ItemType type, object data = null)
        {
            if (type == ItemType.Record)
                return new ListViewItemExt(this.listView);
            return base.OnCreateListViewItem(itemIndex, type, data);
        }
    }
    public class ListViewItemExt : ListViewItem
    {
        private SfListView listView;

        public ListViewItemExt(SfListView listView)
        {
            this.listView = listView;
        }

        protected override void OnItemAppearing()
        {
            this.Opacity = 0;
            //this.Scale = 0;
            this.TranslationX = 100;
            this.TranslateTo(0, 0, 600, Easing.SinInOut);
            this.FadeTo(1, 600, Easing.CubicInOut);
            base.OnItemAppearing();
        }
    }
}
