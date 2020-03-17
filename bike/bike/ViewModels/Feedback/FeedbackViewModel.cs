using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using bike.Models.Feedback;
using Prism.Mvvm;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace bike.ViewModels.Feedback
{
    /// <summary>
    /// ViewModel for feedback page.
    /// </summary>
    [Preserve(AllMembers = true)]
    public class FeedbackViewModel : BindableBase
    {
        #region Constructor

        public FeedbackViewModel()
        {
            var randomNum = new Random(0123456789);
            this.FeedbackInfo = new ObservableCollection<Review>
            {
                new Review
                {
                    CustomerName = "Jessica Park",
                    Comment = "These boots are stunning and I look stunning in them.",
                    ReviewedDate = DateTime.Now.AddDays(randomNum.Next(0, 1000)),
                    CustomerImage = ImageSource.FromResource("1.jpeg", typeof(FeedbackViewModel).GetTypeInfo().Assembly),
                    Rating = randomNum.Next(0, 5),
                    Images = new List<ImageSource>
                    {
                        ImageSource.FromResource("bike2.jpg", typeof(FeedbackViewModel).GetTypeInfo().Assembly),
                        ImageSource.FromResource("bike.jpg", typeof(FeedbackViewModel).GetTypeInfo().Assembly),
                    }
                },
                new Review
                {
                    CustomerName = "Alice",
                    Comment = "Greatest purchase I have ever made in my life. No lie.",
                    ReviewedDate = DateTime.Now.AddDays(randomNum.Next(0, 1000)),
                    CustomerImage = ImageSource.FromResource("2.png", typeof(FeedbackViewModel).GetTypeInfo().Assembly),
                    Rating = randomNum.Next(0, 5)
                },
                new Review
                {
                    CustomerName = "John",
                    Comment = "Absolutely love them! Can’t stop wearing!",
                    ReviewedDate = DateTime.Now.AddDays(randomNum.Next(0, 1000)),
                    CustomerImage = ImageSource.FromResource("3.png", typeof(FeedbackViewModel).GetTypeInfo().Assembly),
                    Rating = randomNum.Next(0, 5)
                },
                new Review
                {
                    CustomerName = "Lisa",
                    Comment = "These boots are very much comfortable for wearing.",
                    ReviewedDate = DateTime.Now.AddDays(randomNum.Next(0, 1000)),
                    CustomerImage = ImageSource.FromResource("4.jpg", typeof(FeedbackViewModel).GetTypeInfo().Assembly),
                    Rating = randomNum.Next(0, 5),
                    Images = new List<ImageSource>
                    {
                        ImageSource.FromResource("bike2.jpg", typeof(FeedbackViewModel).GetTypeInfo().Assembly),
                        ImageSource.FromResource("bike3.jpg", typeof(FeedbackViewModel).GetTypeInfo().Assembly),
                        ImageSource.FromResource("bike4.jpg", typeof(FeedbackViewModel).GetTypeInfo().Assembly),
                    }
                },
                new Review
                {
                    CustomerName = "Rebacca",
                    Comment = "Absolutely love them! Can’t stop wearing!",
                    ReviewedDate = DateTime.Now.AddDays(randomNum.Next(0, 1000)),
                    CustomerImage = ImageSource.FromResource("5.jpg", typeof(FeedbackViewModel).GetTypeInfo().Assembly),
                    Rating = randomNum.Next(0, 5)
                },
                new Review
                {
                    CustomerName = "Jessica Park",
                    Comment = "Happy purchasing!",
                    ReviewedDate = DateTime.Now.AddDays(randomNum.Next(0, 1000)),
                    CustomerImage = ImageSource.FromResource("6.png", typeof(FeedbackViewModel).GetTypeInfo().Assembly),
                    Rating = randomNum.Next(4, 5)
                },
            };

            this.FilterCommand = new Command(this.OnFilterTapped);
            this.SortCommand = new Command(this.OnSortTapped);
            this.ItemSelectedCommand = new Command(this.ItemSelected);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the value for feedback info.
        /// </summary>
        public ObservableCollection<Review> FeedbackInfo { get; set; }

        /// <summary>
        /// Gets or sets the value for filter command.
        /// </summary>
        public Command FilterCommand { get; set; }

        /// <summary>
        /// Gets or sets the value for sort command.
        /// </summary>
        public Command SortCommand { get; set; }

        /// <summary>
        /// Gets or sets the command that will be executed when an item is selected.
        /// </summary>
        public Command ItemSelectedCommand { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Invoked when the sort button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private void OnSortTapped(object obj)
        {
            // Do something
        }

        /// <summary>
        /// Invoked when the filter button is clicked.
        /// </summary>
        /// <param name="obj">The Object</param>
        private void OnFilterTapped(object obj)
        {
            // Do something
        }

        /// <summary>
        /// Invoked when an item is selected.
        /// </summary>
        private void ItemSelected(object selectedItem)
        {
            // Do something
        }

        #endregion
    }
}