using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace bike.Models.Feedback
{
    /// <summary>
    /// Model for feedback list.
    /// </summary>
    [Preserve(AllMembers = true)]
    public class Review
    {
        /// <summary>
        /// Gets or sets the value for rating.
        /// </summary>
        public double Rating { get; set; }

        /// <summary>
        /// Gets or sets the value for date.
        /// </summary>
        public DateTime ReviewedDate { get; set; }

        /// <summary>
        /// Gets or sets the customer name.
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// Gets or sets the profile image.
        /// </summary>
        public ImageSource CustomerImage { get; set; }

        /// <summary>
        /// Gets or sets the comment.
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Gets or sets the list of images.
        /// </summary>
        public List<ImageSource> Images { get; set; }
    }
}
