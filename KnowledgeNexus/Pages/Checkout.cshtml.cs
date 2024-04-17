using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json;

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

        // Inject HttpClient into the page model 
        private readonly HttpClient _httpClient;

        public CheckoutModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public void OnGet(decimal total)
        {
            // Set the total purchase price from the parameter
            TotalPrice = total;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                // If model validation fails, redisplay the page with validation errors
                return Page();
            }

            // Prepare data to send to the API 
            var requestData = new
            {
                FirstName,
                LastName,
                Address,
                City,
                Province,
                PostalCode,
                ccNumber = CardNumber,
                ccExpiryDate = ExpirationDate,
                cvv = Cvc,
                // Convert products list to comma-separated string
                products = String.Join("-", Request.Cookies["products"].Split("-"))
            };

            // Serialize data to JSON
            var json = JsonSerializer.Serialize(requestData);

            // Send data to the Purchase API 
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("https://purchasesapi20240407212852.azurewebsites.net", content);

            if (!response.IsSuccessStatusCode)
            {
                //Handle API error
                return RedirectToPage("/Error"); 
            }
            // Model validation succeeded, process the order
            // Implement order processing logic here

            // Redirect to the order confirmation page
            return RedirectToPage("/OrderConfirmation");
        }
    }
}