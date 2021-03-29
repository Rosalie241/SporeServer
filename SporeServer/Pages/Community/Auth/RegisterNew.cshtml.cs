using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SporeServer.Areas.Identity.Data;
using SporeServer.Models;

namespace SporeServer.Pages.Community.Auth
{
    [AllowAnonymous]
    public class RegisterNewModel : PageModel
    {
        private readonly UserManager<SporeServerUser> _userManager;

        public RegisterNewModel(UserManager<SporeServerUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync(RegisterInfo info)
        {
            if (!ModelState.IsValid)
                return Page();

            IdentityResult result = await _userManager.CreateAsync(
                new SporeServerUser { 
                    UserName = info.DisplayName, 
                    Email = info.Email 
                }, info.Password);

            return LocalRedirect(Request.Path + "?success=" + result.Succeeded);
        }
    }
}
