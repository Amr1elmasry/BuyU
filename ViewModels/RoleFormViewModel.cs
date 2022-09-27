using System.ComponentModel.DataAnnotations;

namespace BuyU.ViewModels

{
    public class RoleFormViewModel
    {
        [Required, StringLength(256)]
        public string Name { get; set; }
    }
}