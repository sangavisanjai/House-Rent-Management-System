
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HouseRentManagementSystem.ViewModel
{
    public class Addproperty
    {

        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int PropId { get; set; }


        public int BookId { get; set; }


        [Required(AllowEmptyStrings = false)]
        [RegularExpression("^[a-zA-Z0-9_ ]*$", ErrorMessage = "Enter a Valid Name")]

        [Display(Name = "Property Name")]
        public string PropName { get; set; }


        [Required]
        public string PropType { get; set; }


        [Required]
        public string PropStatus { get; set; }


        [Required]
        [Display(Name = "Property Rent")]
        public int Proprent { get; set; }


        [Required]
        public string PropAddress { get; set; }


        [Required]
        public string PropDescription { get; set; }


        [Required]
        [RegularExpression(@"^([0]|\+91)?[6789]\d{9}$", ErrorMessage = "Please Enter a Valid Mobile Number")]
        public long PropContact { get; set; }


        [Required]
        public string PropSize { get; set; }


        [Required]
        public string PropFacing { get; set; }


        [Required]
        public string PetFriendly { get; set; }


        [Required]
        public string AvailableFor { get; set; }


        [Required]
        public string Flooring { get; set; }


        [Required]
        public string AvailableFrom { get; set; }


        [Required]
        [Display(Name = "Image")]
        public byte[] HImg1 { get; set; }
        
        

    }
}