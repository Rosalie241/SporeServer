using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SporeServer.Areas.Identity.Data;
using SporeServer.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SporeServer.Pages.Moderation.Management
{
    [Authorize(Roles = "Admin,Moderator")]
    public class UsersModel : PageModel
    {
        private readonly UserManager<SporeServerUser> _userManager;
        private readonly IUserSubscriptionManager _userSubscriptionManager;
        private readonly IAssetManager _assetManager;

        public UsersModel(UserManager<SporeServerUser> userManager, IUserSubscriptionManager userSubscriptionManager, IAssetManager assetManager)
        {
            _userManager = userManager;
            _userSubscriptionManager = userSubscriptionManager;
            _assetManager = assetManager;
        }

        /// <summary>
        ///     Search Results
        /// </summary>
        public SporeServerUser[] Users { get; set; }
        /// <summary>
        ///     Search String
        /// </summary>
        public string SearchString { get; set; }
        /// <summary>
        ///     Whether it actually performed the search
        /// </summary>
        public bool Searched { get; set; }

        public async Task<IActionResult> OnGet()
        {
            SearchString = Request.Query["searchText"];

            if (String.IsNullOrEmpty(SearchString))
            {
                Users = await _userManager.Users
                            .Where(u => u.UserName != null)
                            .OrderBy(u => u.UserName)
                            .Take(25)
                            .ToArrayAsync();
            }
            else
            {
                Users = await _userManager.Users
                               .Where(u =>
                                       u.UserName != null &&
                                       (
                                           (u.Id.ToString() == SearchString) ||
                                           (u.UserName.Contains(SearchString)) ||
                                           (u.Email == SearchString)
                                       )
                                   )
                               .OrderBy(u => u.UserName)
                               .Take(25)
                               .ToArrayAsync();

            }

            string actionQuery = Request.Query["action"];
            if (!String.IsNullOrEmpty(actionQuery))
            {
                if (actionQuery == "Create")
                {
                    return Redirect("https://spore.com/Moderation/Management/User/Create");
                }
            }

            return Page();
        }
    }
}
