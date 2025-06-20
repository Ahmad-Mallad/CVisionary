// CVisionary.DTOs/PortfolioUpdateDTO.cs
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace CVisionary.DTOs
{
    public class PortfolioUpdateDTO
    {
        [Required]
        public int PortfolioId { get; set; }

        /// <summary>
        /// True if a profile image already exists in the DB
        /// </summary>
        public bool HasProfileImage { get; set; }

        /// <summary>
        /// New profile image (optional)
        /// </summary>
        public IFormFile? ProfileImage { get; set; }

        /// <summary>
        /// Freeform block of text from user, including bio, name, contact, etc.
        /// </summary>
        [Required, Display(Name = "Your Bio and Info")]
        public string PersonalInfoText { get; set; } = string.Empty;

        // Service & Projects stay as before:
        [Required, Display(Name = "Services")]
        public List<short> ServiceIds { get; set; } = new();

        public List<ProjectUpdateDTO> Projects { get; set; } = new();
    }

}
