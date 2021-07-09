/*
 * SporeServer - https://github.com/Rosalie241/SporeServer
 *  Copyright (C) 2021 Rosalie Wanders <rosalie@mailbox.org>
 *
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU Affero General Public License version 3.
 *  You should have received a copy of the GNU Affero General Public License
 *  along with this program. If not, see <https://www.gnu.org/licenses/>.
 */
using Microsoft.EntityFrameworkCore;
using SporeServer.Areas.Identity.Data;
using SporeServer.Models;
using SporeServer.SporeTypes;
using System;
using System.Threading.Tasks;

namespace SporeServer.Services
{
    public interface IAssetManager
    {
        /// <summary>
        ///     Adds asset
        /// </summary>
        /// <param name="form"></param>
        /// <param name="asset"></param>
        /// <param name="parentAsset"></param>
        /// <param name="slurped"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        Task<bool> AddAsync(AssetUploadForm form, SporeServerAsset asset, SporeServerAsset parentAsset, bool slurped, SporeAssetType type);

        /// <summary>
        ///     Updates asset
        /// </summary>
        /// <param name="asset"></param>
        /// <returns></returns>
        Task<bool> UpdateAsync(SporeServerAsset asset);

        /// <summary>
        ///     Deletes asset
        /// </summary>
        /// <param name="asset"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync(SporeServerAsset asset);

        /// <summary>
        ///     Reserves new asset for user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<bool> ReserveAsync(SporeServerUser user);

        /// <summary>
        ///     Tries to find asset from id (optionally includes extras), returns null when not found
        /// </summary>
        /// <param name="id"></param>
        /// <param name="includeExtras"></param>
        /// <returns></returns>
        Task<SporeServerAsset> FindByIdAsync(Int64 id, bool includeExtras);

        /// <summary>
        ///     Tries to find asset from id, returns null when not found
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<SporeServerAsset> FindByIdAsync(Int64 id);

        /// <summary>
        ///     Tries to find all assets from author id, returns null when not found
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<SporeServerAsset[]> FindAllByUserIdAsync(Int64 authorId);

        /// <summary>
        ///     Returns random list of assets with specified type, excludes assets made by author id
        /// </summary>
        /// <param name="authorId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        Task<SporeServerAsset[]> GetRandomAssetsAsync(Int64 authorId, SporeModelType type);

        /// <summary>
        ///     Returns count of assets by author
        /// </summary>
        /// <param name="author"></param>
        /// <returns></returns>
        Task<Int32> GetCountByAuthorAsync(SporeServerUser author);

        /// <summary>
        ///     Returns all assets
        /// </summary>
        /// <returns></returns>
        DbSet<SporeServerAsset> GetAllAssets();
    }
}
