﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Lms.MVC.Core.Entities;

namespace Lms.MVC.UI.Models.ViewModels.PublicationViewModels
{
    public class DeletePublicationViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }

        public string Url { get; set; }

        public Subject Subject { get; set; }

        public Level Level { get; set; }

        // Dropdown list

        public IEnumerable<Subject> Subjects { get; set; }

        // Author Name to be used
        [Display(Name = "Author First Name")]
        public string AuthorFirstName { get; set; }

        [Display(Name = "Author Last Name")]
        public string AuthorLastName { get; set; }

        //Subject Creation at Post
        public string SubjectTitle { get; set; }

        [Required]
        public ICollection<Author> Authors { get; set; }
    }
}