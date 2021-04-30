/*
 * SporeServer - https://github.com/Rosalie241/SporeServer
 *  Copyright (C) 2021 Rosalie Wanders <rosalie@mailbox.org>
 *
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU Affero General Public License version 3.
 *  You should have received a copy of the GNU Affero General Public License
 *  along with this program. If not, see <https://www.gnu.org/licenses/>.
 */
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
        /// <param name="parentId"></param>
        /// <param name="slurped"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        Task<bool> AddAsync(AssetUploadForm form, SporeServerAsset asset, Int64 parentId, bool slurped, SporeAssetType type);

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
        ///     Tries to find asset from id (optionally includes author), returns null when not found
        /// </summary>
        /// <param name="id"></param>
        /// <param name="includeAuthor"></param>
        /// <returns></returns>
        Task<SporeServerAsset> FindByIdAsync(Int64 id, bool includeAuthor);

        /// <summary>
        ///     Tries to find asset from id, returns null when not found
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<SporeServerAsset> FindByIdAsync(Int64 id);
    }
}
