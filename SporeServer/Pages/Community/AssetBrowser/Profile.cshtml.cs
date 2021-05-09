using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SporeServer.Areas.Identity.Data;
using SporeServer.Services;

namespace SporeServer.Pages.Community.AssetBrowser
{
    public class ProfileModel : PageModel
    {
        private readonly UserManager<SporeServerUser> _userManager;
        private readonly ISubscriptionManager _subscriptionManager;

        public ProfileModel(UserManager<SporeServerUser> userManager, ISubscriptionManager subscriptionManager)
        {
            _userManager = userManager;
            _subscriptionManager = subscriptionManager;
        }

        /// <summary>
        ///     Profile User
        /// </summary>
        public SporeServerUser ProfileUser { get; set; }
        /// <summary>
        ///     Current user
        /// </summary>
        public SporeServerUser CurrentUser { get; set; }
        /// <summary>
        ///     Whether CurrentUser is subscribed to ProfileUser
        /// </summary>
        public bool Subscribed { get; set; }

        public async Task<IActionResult> OnGet(Int64 id)
        {
            ProfileUser = await _userManager.FindByIdAsync($"{id}");
            CurrentUser = await _userManager.GetUserAsync(User);

            Subscribed = _subscriptionManager.Find(CurrentUser, ProfileUser) != null;

            return Page();
        }
    }
}
