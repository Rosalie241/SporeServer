/*
 * SporeServer - https://github.com/Rosalie241/SporeServer
 *  Copyright (C) 2021 Rosalie Wanders <rosalie@mailbox.org>
 *
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU Affero General Public License version 3.
 *  You should have received a copy of the GNU Affero General Public License
 *  along with this program. If not, see <https://www.gnu.org/licenses/>.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SporeServer.Areas.Identity.Data;
using SporeServer.Services;
using Microsoft.EntityFrameworkCore;
using SporeServer.SporeTypes;
using System.Globalization;
using Microsoft.AspNetCore.Http;

namespace SporeServer.Pages.Community.AssetBrowser
{
    [Authorize]
    public class SingleDownloadModel : PageModel
    {
        private readonly IAssetManager _assetManager;

        public SingleDownloadModel(IAssetManager assetManager)
        {
            _assetManager = assetManager;
        }

        /// <summary>
        ///     BrowseType Query
        /// </summary>
        public string BrowseType { get; set; }
        /// <summary>
        ///     FilterView Query
        /// </summary>
        public string FilterView { get; set; }
        /// <summary>
        ///     Whether to show the big background or not
        /// </summary>
        public bool ShowBigBackground { get; set; }
        /// <summary>
        ///     Whether an action was performed
        /// </summary>
        public bool PerformedAction { get; set; }
        /// <summary>
        ///     Whether the management tools should be shown
        /// </summary>
        public bool ShowManageTools { get; set; }
        /// <summary>
        ///     Whether an asset was deleted
        /// </summary>
        public bool DeletedAsset { get; set; }
        /// <summary>
        ///     Custom query string
        /// </summary>
        public QueryString QueryString { get; set; }
        /// <summary>
        ///     Amount Of Filtered Items In Total
        /// </summary>
        public Int32 SearchCount { get; set; }
        /// <summary>
        ///     Searched Assets
        /// </summary>
        public SporeServerAsset[] Assets { get; set; }
        /// <summary>
        ///     Amount of Assets
        /// </summary>
        public Int32 AssetCount { get; set; }
        /// <summary>
        ///     Current Index
        /// </summary>
        public Int32 CurrentIndex { get; set; }
        /// <summary>
        ///     Next Index
        /// </summary>
        public Int32 NextIndex { get; set; }
        /// <summary>
        ///     Previous Index
        /// </summary>
        public Int32 PreviousIndex { get; set; }
        /// <summary>
        ///     Amount Of Items Per Page
        /// </summary>
        public Int32 PageSize { get; set; }

        public async Task<IActionResult> OnGet(Int32? index)
        {
            BrowseType = Request.Query["browseType"];
            FilterView = Request.Query["filterView"];
            QueryString = Request.QueryString;
            ShowBigBackground = false;
            PageSize = 8;
            DeletedAsset = false;

            // parse AppResolution header
            var resolution = Request.Headers["AppResolution"].FirstOrDefault();
            if (!String.IsNullOrEmpty(resolution))
            {
                // make sure it's in the right format
                var splitResolution = resolution.Split('x');

                if (splitResolution.Length == 2)
                {
                    // the official server
                    // only checks the width,
                    // so let's do the same here
                    if (Int32.TryParse(splitResolution[0], out int width))
                    {
                        // show big background
                        // and increase page size
                        // when the resolution width
                        // is large enough
                        if (width >= 1024)
                        {
                            ShowBigBackground = true;
                            PageSize = 15;
                        }
                    }
                }
            }

            // parse 'deleteAsset' query
            // and perform action if user has permission
            var deleteAssetQuery = Request.Query["deleteAsset"];
            if (Int64.TryParse(deleteAssetQuery, out Int64 assetId) &&
                (User.IsInRole("Moderator") || User.IsInRole("Admin")))
            {
                // remove deleteAsset query
                var filteredQueryString = Request.Query.ToList().Where(x => x.Key != "deleteAsset");
                QueryString = QueryString.Create(filteredQueryString);

                // try to find the asset from the ID
                var asset = await _assetManager.FindByIdAsync(assetId);
                if (asset != null)
                { // when found, attempt to delete the asset
                    if (!await _assetManager.DeleteAsync(asset))
                    {
                        return StatusCode(500);
                    }

                    DeletedAsset = true;
                }
            }

            // parse 'action' query
            // and perform action
            var requestQuery = Request.Query["action"];
            IOrderedQueryable<SporeServerAsset> orderedAssets = null;
            if (requestQuery == "SEARCH")
            {
                orderedAssets = PerformSearch();
            }
            else if (requestQuery == "BROWSE")
            {
                orderedAssets = PerformBrowse();
            }
            
            // check if page has manage query,
            // if so, verify if user has moderator or admin role,
            // then display them
            var manageQuery = Request.Query["manage"];
            if (manageQuery == "1" && 
                (User.IsInRole("Moderator") || User.IsInRole("Admin")))
            {
                ShowManageTools = true;
            }

            // when the action was performed,
            // set page information
            if (orderedAssets != null)
            {
                CurrentIndex = index ?? 0;
                NextIndex = CurrentIndex + PageSize;
                PreviousIndex = CurrentIndex - PageSize;
                SearchCount = await orderedAssets
                                    .CountAsync();
                Assets = await orderedAssets
                                .Skip(CurrentIndex)
                                .Take(PageSize)
                                .ToArrayAsync();
                AssetCount = Assets.Length;
                PerformedAction = true;
            }

            return Page();
        }

        /// <summary>
        ///     Performs a search when the requirements have been met
        /// </summary>
        public IOrderedQueryable<SporeServerAsset> PerformSearch()
        {
            var searchText = Request.Query["searchText"].ToString();

            // no need to do anything when 
            // there's no search string
            if (String.IsNullOrEmpty(searchText))
            {
                return null;
            }

            var searchFieldsQuery = Request.Query["searchFields"];
            bool searchName = searchFieldsQuery.Contains("name");
            bool searchAuthor = searchFieldsQuery.Contains("author");
            bool searchTags = searchFieldsQuery.Contains("tags");
            bool searchDescription = searchFieldsQuery.Contains("description");

            // no need to perform a search
            // when no search field has been specified
            if (!searchName &&
                !searchAuthor &&
                !searchTags &&
                !searchDescription)
            {
                return null;
            }

            // perform actual search 
            return _assetManager.GetAllAssets()
                        .Include(a => a.Author)
                        .Include(a => a.Tags)
                        .Where(a => a.Used &&
                            (
                                (searchName && a.Name.Contains(searchText)) ||
                                (searchAuthor && a.Author.UserName.Contains(searchText)) ||
                                (searchTags && a.Tags.Where(t => t.Tag.Contains(searchText)).FirstOrDefault() != null) ||
                                (searchDescription && a.Description.Contains(searchText))
                            )
                        ).OrderBy(a => a.Name);
        }

        /// <summary>
        ///     Performs a browse search when the requirements have been met
        /// </summary>
        public IOrderedQueryable<SporeServerAsset> PerformBrowse()
        {
            var modelTypeQuery = Request.Query["modelType"].ToString();
            var modelTypes = new List<SporeModelType>();
            bool filterModelType = true;

            // sadly there are 2 exceptions to this system,
            // first, when browseType is ALL, search for everything
            // second, when browseType is UFO, search for UFOs,
            // there's no dedicated UFO selection screen
            if (BrowseType == "ALL")
            { // don't filter on ModelType
                filterModelType = false;
            }
            else if (BrowseType == "UFO")
            {
                modelTypeQuery = $"{(Int64)SporeModelType.VehicleUFO:x}";
            }

            // only validate query,
            // when we need to filter ModelType
            if (filterModelType)
            {
                // make sure the query exists
                if (String.IsNullOrEmpty(modelTypeQuery))
                {
                    return null;
                }

                // validate values from query
                foreach (var modelTypeString in modelTypeQuery.Split(';'))
                {
                    // make sure we can parse the value,
                    // and that it's a valid ModelType
                    if (!Int64.TryParse(modelTypeString,
                            NumberStyles.HexNumber,
                            null,
                            out Int64 modelType) ||
                        !Enum.IsDefined(typeof(SporeModelType), modelType))
                    {
                        return null;
                    }

                    modelTypes.Add((SporeModelType)modelType);
                }
            }

            // perform actual search
            IOrderedQueryable<SporeServerAsset> orderedAssets;
            var assets = _assetManager.GetAllAssets()
                         .Include(a => a.Author)
                         .Include(a => a.Tags)
                         .Where(a => a.Used &&
                             (
                                (!filterModelType || modelTypes.Contains(a.ModelType))
                             )
                         );

            // TODO, support more FilterViews
            switch (FilterView)
            {
                case "NEWEST":
                    orderedAssets = assets.OrderByDescending(a => a.Timestamp);
                    break;

                case "OLDEST":
                    orderedAssets = assets.OrderBy(a => a.Timestamp);
                    break;

                case "RANDOM":
                default:
                    orderedAssets = assets.OrderBy(a => a.Name);
                    break;
            }

            return orderedAssets;
        }
    }
}
