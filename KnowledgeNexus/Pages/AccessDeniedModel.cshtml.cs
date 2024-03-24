using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KnowledgeNexus.Pages
{
    [Authorize(Roles = "Administrator")]

    public class AccessDeniedModelModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
