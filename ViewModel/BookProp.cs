
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HouseRentManagementSystem.ViewModel
{
    public class BookProp
    {
        public int PropId { get; set; }
        public int BookId { get; set; }
        public string PropName { get; set; }
        public string PropType { get; set; }
        public string PropStatus { get; set; }
        public int PropRent { get; set; }
        public string PropAddress { get; set; }
        public string PropDescription { get; set; }
        public long PropContact { get; set; }
        public string PropSize { get; set; }
        public string PropFacing { get; set; 
        }
        public string PetFriendly { get; set; }
        public string AvailableFor { get; set; }
        public string Flooring { get; set; }
        public string AvailableFrom { get; set; }
        public string Book { get; set; }
        public string CustEmail { get; set; }
        public string SellEmail { get; set; }
        public byte[] HImg1 { get; set; }

        public string Email { get; set; }

    }
}