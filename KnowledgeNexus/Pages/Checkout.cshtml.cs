using KnowledgeNexus.Data;
using KnowledgeNexus.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json;

namespace KnowledgeNexus.Pages
{
    public class CheckoutModel : PageModel
    {
        private readonly ILogger<CheckoutModel> _logger;
        private readonly KnowledgeNexusContext _context;

        public IList<Books> Books { get; set; } = default!;
        public List<int> ProductIDs { get; set; } = new List<int>();

        public int CartSum { get; set; } = 0;
        //public decimal Subtotal { get; set; } = 0;
        public decimal Tax { get; set; } = 0;
        public decimal Shipping { get; set; } = 0;
        public decimal ShippingTax { get; set; } = 0;
        public decimal Total { get; set; } = 0;

        // Property to hold the total purchase price 
        public decimal TotalPrice { get; set; } = 0;

        public class PurchaseData
        {
            public string FirstName { get; set; } = string.Empty;
            public string LastName { get; set; } = string.Empty;
            public string Address { get; set; } = string.Empty;
            public string City { get; set; } = string.Empty;
            public string Province { get; set; } = string.Empty;
            public string PostalCode { get; set; } = string.Empty;
            
            public string CardNumber { get; set; } = string.Empty;
            public string ExpirationDate { get; set; } = string.Empty;
            public string Cvc { get; set; } = string.Empty;
            public string products { get; set; } = string.Empty;
        }

        [BindProperty]
        public PurchaseData purchaseData { get; set; } = default!;
        public CheckoutModel(ILogger<CheckoutModel> logger, KnowledgeNexusContext context)
        {
            _logger = logger;
            _context = context;
            purchaseData = new PurchaseData();
        }

        public async Task OnGetAsync()
        {
            string? cookieValue = Request.Cookies["ProductIDs"];

            if (cookieValue == null)
            {
                createCookie("");
            }
            else
            {
                CartSum = cookieValue.Split("-").Length;

                string[] ids = cookieValue.Split("-");

                ProductIDs.AddRange(ids.Select(int.Parse));

                purchaseData.products = cookieValue.Replace("-", ",");

            }

            Books = await _context.Books.Where(g => ProductIDs.Contains(g.BooksId)).ToListAsync();

            TotalPrice = Books.Sum(g => g.Price); ;

            Tax = Math.Round(TotalPrice * 0.15m, 2);

            Shipping = TotalPrice > 70 ? 0 : 5;

            // Calculate shipping tax
            ShippingTax = Shipping * 0.1m;

            // Calculate total
            Total = TotalPrice + Tax + Shipping + ShippingTax;

        }

        public async Task<IActionResult> OnPostAsync()
        {
            string? cookieValue = Request.Cookies["ProductIDs"]; 

            if(cookieValue == null)
            {
                createCookie("");
            }
            else
            {
                CartSum = cookieValue.Split("-").Length;

                string[] ids = cookieValue.Split("-");

                ProductIDs.AddRange(ids.Select(int.Parse));

                purchaseData.products = cookieValue.Replace("-", ",");

            }

            Books = await _context.Books.Where(g => ProductIDs.Contains(g.BooksId)).ToListAsync();

            //Subtotal = TotalPrice;

            Tax = Math.Round(TotalPrice * 0.15m, 2);

            Shipping = TotalPrice > 70 ? 0 : 5;

            // Calculate shipping tax
            ShippingTax = Shipping * 0.1m;

            // Calculate total
            Total = TotalPrice + Tax + Shipping + ShippingTax;

            if (!ModelState.IsValid)
            {
                return Page(); 
            }

            // Serialize purchaseData to JSON
            //string jsonData = JsonConvert.SerializeObject(purchaseData);

            // Hard code sample JSON payload
            string jsonPayload = "{\"FirstName\":\"John\",\"LastName\":\"Doe\",\"Address\":\"123 Main St\",\"City\":\"New York\",\"Province\":\"NY\",\"PostalCode\":\"B3L1X6\",\"ccNumber\":\"1111111111111111\",\"ccExpiryDate\":\"1225\",\"cvv\":\"123\",\"products\":\"1,2,3,4,5\"}";

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://purchasesapi20240407212852.azurewebsites.net");

                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                try
                {
                    HttpResponseMessage response = await client.PostAsync("/", new StringContent(jsonPayload, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                    {
                        string responseString = await response.Content.ReadAsStringAsync();

                        TempData["InvoiceNumber"] = responseString;
                        // Clear the cookie
                        Response.Cookies.Delete("ProductIDs");

                        return RedirectToPage("./Confirmation");
                    }
                    else
                    {
                        string responseString = await response.Content.ReadAsStringAsync();

                        ModelState.AddModelError("", "An error occurred while processing your request. Please try again later." + responseString);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while processing your request. Please try again later. Exception: " + ex.Message);
                }
            }
            return Page();

        }

        private void createCookie(string value)
        {
            Response.Cookies.Append("ProductIDs", value, new CookieOptions()
            {
                Expires = DateTime.Now.AddDays(1)
            });
        }
    }
}