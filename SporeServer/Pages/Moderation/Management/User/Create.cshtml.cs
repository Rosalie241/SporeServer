using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;

namespace SporeServer.Pages.Moderation.Management.User
{
    [Authorize(Roles = "Admin,Moderator")]
    public class CreateModel : PageModel
    {
        public async Task<IActionResult> OnGet()
        {
            string actionQuery = Request.Query["action"];

            if (!String.IsNullOrEmpty(actionQuery))
            {
                if (actionQuery == "Back")
                {
                    return Redirect("https://spore.com/Moderation/Management/Users");
                }
            }

            return Page();
        }
    }
}
