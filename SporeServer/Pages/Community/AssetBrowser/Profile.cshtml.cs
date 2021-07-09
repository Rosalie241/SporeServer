using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SporeServer.Areas.Identity.Data;
using SporeServer.Services;

namespace SporeServer.Pages.Community.AssetBrowser
{
    [Authorize]
    public class ProfileModel : PageModel
    {
        private readonly UserManager<SporeServerUser> _userManager;
        private readonly IUserSubscriptionManager _userSubscriptionManager;
        private readonly IAggregatorSubscriptionManager _aggregatorSubscriptionManager;
        private readonly IAchievementManager _achievementManager;
        private readonly IAssetManager _assetManager;
        private readonly IAggregatorManager _aggregatorManager;

        public ProfileModel(UserManager<SporeServerUser> userManager, IUserSubscriptionManager userSubscriptionManager, 
                    IAggregatorSubscriptionManager aggregatorSubscriptionManager, IAchievementManager achievementManager,
                    IAssetManager assetManager, IAggregatorManager aggregatorManager)
        {
            _userManager = userManager;
            _userSubscriptionManager = userSubscriptionManager;
            _aggregatorSubscriptionManager = aggregatorSubscriptionManager;
            _achievementManager = achievementManager;
            _assetManager = assetManager;
            _aggregatorManager = aggregatorManager;
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
        /// <summary>
        ///     Unlocked Achievement Ids
        /// </summary>
        public Int64[] UnlockedAchievementIds { get; set; }
        /// <summary>
        ///     Unlocked Achivements Count
        /// </summary>
        public Int32 UnlockedAchievementCount { get; set; }
        /// <summary>
        ///     Current Achievement Index
        /// </summary>
        public Int32 AchievementIndex { get; set; }
        /// <summary>
        ///     Next Achievement Index
        /// </summary>
        public Int32 NextAchievementIndex { get; set; }
        /// <summary>
        ///     Previous Achievement Index
        /// </summary>
        public Int32 PreviousAchievementIndex { get; set; }

        /// <summary>
        ///     Profile User Assets
        /// </summary>
        public SporeServerAsset[] Assets { get; set; }
        /// <summary>
        ///     Asset Count
        /// </summary>
        public Int32 AssetCount { get; set; }
        /// <summary>
        ///     Current Asset Index
        /// </summary>
        public Int32 AssetIndex { get; set; }
        /// <summary>
        ///     Next Asset Index
        /// </summary>
        public Int32 NextAssetIndex { get; set; }
        /// <summary>
        ///     Previous Asset Index
        /// </summary>
        public Int32 PreviousAssetIndex { get; set; }

        /// <summary>
        ///     Profile User Aggregators
        /// </summary>
        public SporeServerAggregator[] Aggregators { get; set; }
        /// <summary>
        ///     Current User Aggregator Subscriptions
        /// </summary>
        public Int64[] AggregatorSubscriptions { get; set; }
        /// <summary>
        ///     Aggregator Count
        /// </summary>
        public Int32 AggregatorCount { get; set; }
        /// <summary>
        ///     Current Aggregator Index
        /// </summary>
        public Int32 AggregatorIndex { get; set; }
        /// <summary>
        ///     Next Aggregator Index
        /// </summary>
        public Int32 NextAggregatorIndex { get; set; }
        /// <summary>
        ///     Previous Aggregator Index
        /// </summary>
        public Int32 PreviousAggregatorIndex { get; set; }

        public async Task<IActionResult> OnGet(Int64 id)
        {
            Console.WriteLine($"/community/assetBrowser/profile/{id}{Request.QueryString}");

            ProfileUser = await _userManager.FindByIdAsync($"{id}");
            CurrentUser = await _userManager.GetUserAsync(User);

            // make sure ProfileUser exists
            if (ProfileUser == null)
            {
                return NotFound();
            }

            Subscribed = _userSubscriptionManager.Find(CurrentUser, ProfileUser) != null;

            AchievementIndex = 0;
            NextAchievementIndex = 3;
            PreviousAchievementIndex = -1;

            AssetIndex = 0;
            NextAssetIndex = 10;
            PreviousAssetIndex = -1;

            AggregatorIndex = 0;
            NextAggregatorIndex = 5;
            PreviousAggregatorIndex = -1;

            var requestQuery = Request.Query;
            
            if (requestQuery.ContainsKey("achievementIndex") &&
                Int32.TryParse(requestQuery["achievementIndex"], out Int32 achievementIndex))
            {
                AchievementIndex = achievementIndex;
                NextAchievementIndex = achievementIndex + 3;
                PreviousAchievementIndex = achievementIndex - 3;
            }

            if (requestQuery.ContainsKey("assetIndex") &&
                Int32.TryParse(requestQuery["assetIndex"], out Int32 creationIndex))
            {
                AssetIndex = creationIndex;
                NextAssetIndex = creationIndex + 10;
                PreviousAssetIndex = creationIndex - 10;
            }

            if (requestQuery.ContainsKey("aggregatorIndex") &&
                Int32.TryParse(requestQuery["aggregatorIndex"], out Int32 sporecastIndex))
            {
                AggregatorIndex = sporecastIndex;
                NextAggregatorIndex = sporecastIndex + 5;
                PreviousAggregatorIndex = sporecastIndex - 5;
            }

            var unlockedAchievementIds = await _achievementManager.FindAllByAuthorAsync(ProfileUser);
            UnlockedAchievementCount = unlockedAchievementIds == null ? 0 : unlockedAchievementIds.Length;
            UnlockedAchievementIds = unlockedAchievementIds?
                                        .Reverse()
                                        .Skip(AchievementIndex)
                                        .Take(3)
                                        .ToArray();

            var assets = await _assetManager.FindAllByUserIdAsync(ProfileUser.Id);
            AssetCount = assets == null ? 0 : assets.Length;
            Assets = assets?
                        .OrderByDescending(a => a.Timestamp)
                        .Skip(AssetIndex)
                        .Take(10)
                        .ToArray();

            var aggregators = await _aggregatorManager.FindByAuthorAsync(ProfileUser);
            AggregatorCount = aggregators == null ? 0 : aggregators.Length;
            Aggregators = aggregators?
                                .OrderByDescending(a => a.Timestamp)
                                .Skip(AggregatorIndex)
                                .Take(5)
                                .ToArray();

            var aggregatorSubscriptions = await _aggregatorSubscriptionManager.FindAllByAuthorAsync(CurrentUser);
            AggregatorSubscriptions = aggregatorSubscriptions
                                            .Select(s => s.AggregatorId)
                                            .ToArray();

            return Page();
        }
    }
}
