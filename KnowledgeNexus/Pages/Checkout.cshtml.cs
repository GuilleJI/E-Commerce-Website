using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KnowledgeNexus.Pages
{
    public class CheckoutModel : PageModel
    {
        // Property to hold the total purchase price 
        public decimal TotalPrice { get; set; }

        // Property to hold the cart sum 
        public int CartSum { get; set; }
        public void OnGet(decimal total)
        {
            // Set the total purchase price from the parameter
            TotalPrice = total;
            
        }


        
    }
}
