using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RedMango_API.Models.Dto
{
    public class OrderHeaderCreateDTO
    {
       
        [Required]
        public string PickupName { get; set; }
        [Required]
        public string PickupPhoneNumber{get; set; }
        [Required]
        public int PickupEmail { get; set; }
        public string ApplicationUserId{get; set;}
        public double OrderTotal { get; set; }

   
        public string StripePaymentIntentID{get; set;}
        public string status{get; set;}
        public int TotalItems{get; set; }

        public IEnumerable<OrderDetailsCreateDTO> OrderDetailsDTO { get; set; }
    }
}
