using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace KnowledgeNexus.Pages
{
    public class CheckoutModel : PageModel
    {
        // Properties to represent payment details
        [Required(ErrorMessage = "First Name is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }

        public string Address2 { get; set; }

        [Required(ErrorMessage = "City is required")]
        public string City { get; set; }

        [Required(ErrorMessage = "Province/Territory is required")]
        public string Province { get; set; }

        [Required(ErrorMessage = "Postal Code is required")]
        [RegularExpression(@"^[A-Za-z]\d[A-Za-z][ -]?\d[A-Za-z]\d$", ErrorMessage = "Invalid Postal Code")]
        public string PostalCode { get; set; }

        [Required(ErrorMessage = "Name on Card is required")]
        public string CardName { get; set; }

        [Required(ErrorMessage = "Credit Card Number is required")]
        [RegularExpression(@"^\d{16}$", ErrorMessage = "Invalid Credit Card Number")]
        public string CardNumber { get; set; }

        [Required(ErrorMessage = "Expiration Date is required")]
        [RegularExpression(@"^\d{2}/\d{4}$", ErrorMessage = "Invalid Expiration Date")]
        public string ExpirationDate { get; set; }

        [Required(ErrorMessage = "CVC is required")]
        [RegularExpression(@"^\d{3}$", ErrorMessage = "Invalid CVC")]
        public string Cvc { get; set; }

        // Property to hold the total purchase price 
        public decimal TotalPrice { get; set; }

        // Property to hold the cart sum 
        public int CartSum { get; set; }

        public void OnGet(decimal total)
        {
            // Set the total purchase price from the parameter
            TotalPrice = total;
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                // If model validation fails, redisplay the page with validation errors
                return Page();
            }

            // Model validation succeeded, process the order
            // Implement order processing logic here

            // Redirect to the order confirmation page
            return RedirectToPage("/OrderConfirmation");
        }
    }
}