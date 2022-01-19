

using System.ComponentModel.DataAnnotations;

namespace Blockchain.Models
{
    public class Property
    {
        public int PropertyID { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter an address")]
        public string Address { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter the owner details")]
        [Display(Name = "Owner")]
        public string OwnerDetails { get; set; }
        [Required(ErrorMessage = "Please upload the building design")]
        [Display(Name = "Design File")]
        public BCFile BuildingDesign { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter your license")]
        [Display(Name = "License")]
        public string SellerLicense { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter the price")]
        public decimal Price { get; set; }
    }
}
