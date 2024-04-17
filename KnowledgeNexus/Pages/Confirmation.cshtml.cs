using KnowledgeNexus.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KnowledgeNexus.Pages
{
    public class ConfirmationModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly KnowledgeNexusContext _context;

        public string InvoiceNumber { get; set; }

        public List<int> ProductIDs { get; set; } = new List<int>();

        public int CartSum { get; set; } = 0;

        public ConfirmationModel(ILogger<IndexModel> logger, KnowledgeNexusContext context)
        {
            _logger = logger;
            _context = context;
        }
        public void OnGet()
        {
            // Read the InvoiceNumber
            InvoiceNumber = TempData["InvoiceNumber"]?.ToString();

            //
            // Get the existing cookie value
            string? cookieValue = Request.Cookies["ProductIDs"]; // Can only read the cookie value 1st character, not the whole string

            // Cookie does not exist
            if (cookieValue == null)
            {
                // Create cookie and set its initial value to 0
                createCookie("");
            }
            else// If the cookie exists, parse its value into ProductIDs list
            {
                // Fix how to get the length of "9,3,2,8,9,9"

                CartSum = cookieValue.Split("-").Length;

            }
        }
        // A helper function to create a cookie and set its value to count
        private void createCookie(string value)
        {
            Response.Cookies.Append("ProductIDs", value, new CookieOptions()
            {
                Expires = DateTime.Now.AddDays(1)
            });
        }

    }
}
