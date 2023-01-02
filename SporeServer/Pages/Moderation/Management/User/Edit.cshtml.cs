using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;

namespace SporeServer.Pages.Moderation.Management.User
{
    public class EditModel : PageModel
    {
        public async Task<IActionResult> OnGet(Int64 id)
        {
            return Page();
        }
    }
}
