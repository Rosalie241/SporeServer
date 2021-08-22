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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SporeServer.Services
{
    public interface IAssetCommentManager
    {
        /// <summary>
        ///     Adds comment for asset from author
        /// </summary>
        /// <param name="asset"></param>
        /// <param name="author"></param>
        /// <param name="comment"></param>
        /// <returns></returns>
        public Task<bool> AddAsync(SporeServerAsset asset, SporeServerUser author, string comment);

        /// <summary>
        ///     Approves comment
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        public Task<bool> ApproveAsync(SporeServerAssetComment comment);

        /// <summary>
        ///     Removes comment
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        public Task<bool> RemoveAsync(SporeServerAssetComment comment);

        /// <summary>
        ///     Finds comment with given id, returns null when not found
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<SporeServerAssetComment> FindByIdAsync(Int64 id);

        /// <summary>
        ///     Finds all approved comments for asset
        /// </summary>
        /// <param name="asset"></param>
        /// <returns></returns>
        public Task<SporeServerAssetComment[]> FindAllApprovedByAssetAsync(SporeServerAsset asset);

        /// <summary>
        ///     Finds all approved comments for asset author
        /// </summary>
        /// <param name="author"></param>
        /// <returns></returns>
        public Task<SporeServerAssetComment[]> FindAllApprovedByAssetAuthorAsync(SporeServerUser author);

        /// <summary>
        ///     Finds all unapproved comments for asset author
        /// </summary>
        /// <param name="author"></param>
        /// <returns></returns>
        public Task<SporeServerAssetComment[]> FindAllUnApprovedByAssetAuthorAsync(SporeServerUser author);

        /// <summary>
        ///     Finds all comments for assets from author
        /// </summary>
        /// <param name="author"></param>
        /// <returns></returns>
        public Task<SporeServerAssetComment[]> FindAllByAssetAuthorAsync(SporeServerUser author);
    }
}
