using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace KnowledgeNexus.PurchaseService
{
    public class PurchaseService
    {
        private readonly HttpClient _httpClient;

        public PurchaseService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> SubmitPurchaseAsync(PurchaseModel purchaseData)
        {
            try
            {
                //Serialize the purchase data to JSON
                var json = JsonConvert.SerializeObject(purchaseData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Send a POST request to the Purchase API 
                var response = await _httpClient.PostAsync("https://purchasesapi20240407212852.azurewebsites.net", content);

                // Check id the request was successful 
                if (response.IsSuccessStatusCode)
                {
                    return true; 
                }
                else
                {
                    //Log or handle the error
                    return false; 
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception 
                return false;
            }
        }
    }

    // Define a model representing the data to be sent to the Purchase API 
    public class PurchaseModel
    {
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

        // Additional property to hold products, you might need to adjust the type based on your data structure
        public string Products { get; set; }
    }
}
