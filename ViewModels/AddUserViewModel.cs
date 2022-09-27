﻿using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace BuyU.ViewModels
{
    public class AddUserViewModel
    {

        [Required]
        [StringLength(50 , ErrorMessage ="The {0} must be at least {2} and at max {1} characters long.",MinimumLength = 3)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }



        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }


        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

    
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        public List<RoleViewModel> Roles { get; set; }
    }
}
