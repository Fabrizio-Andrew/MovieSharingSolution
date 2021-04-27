using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HW6MovieSharingSolution.Models
{
    /// <summary>
    /// Defines the movie model that represents the attributes of a movie entity
    /// </summary>
    public class Movie
    {
        /// <summary>
        /// Gets or sets the movie identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets the movie title.
        /// </summary>
        /// <value>The movie title.</value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the user movie category.
        /// </summary>
        /// <value>The user movie category.</value>
        public string Category { get; set; }

        /// <summary>
        /// Gets or sets the genre.
        /// </summary>
        /// <value>The genre.</value>
        public string Genre { get; set; }

        /// <summary>
        /// Gets or sets the name of the shared-with user.
        /// </summary>
        /// <value>The shared-with user's name.</value>
        public string SharedWithName { get; set; }

        /// <summary>
        /// Gets or sets the email of the shared-with user.
        /// </summary>
        /// <value>The shared-with user's email.</value>
        public string SharedWithEmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the shared date.
        /// </summary>
        /// <value>The shared date.</value>
        [Display(Name = "Shared Date")]
        [DataType(DataType.Date)]
        public DateTime SharedDate { get; set; }

        /// <summary>
        /// Gets or sets the name of the user who requested the movie.
        /// </summary>
        /// <value>The requestor's name.</value>
        public string RequestorName { get; set; }

        /// <summary>
        /// Gets or sets the email of the user who requested the movie.
        /// </summary>
        /// <value>The requestor's email.</value>
        public string RequestorEmail { get; set; }

        /// <summary>
        /// Gets or sets the email of the owner.
        /// </summary>
        /// <value>The owner's email.</value>
        public string OwnerEmailAddress { get; set; }
    }
}