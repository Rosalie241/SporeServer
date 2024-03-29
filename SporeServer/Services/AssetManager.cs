﻿/*
 * SporeServer - https://github.com/Rosalie241/SporeServer
 *  Copyright (C) 2021 Rosalie Wanders <rosalie@mailbox.org>
 *
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU Affero General Public License version 3.
 *  You should have received a copy of the GNU Affero General Public License
 *  along with this program. If not, see <https://www.gnu.org/licenses/>.
 */
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SporeServer.Areas.Identity.Data;
using SporeServer.Data;
using SporeServer.Models;
using SporeServer.Models.Xml;
using SporeServer.SporeTypes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;

namespace SporeServer.Services
{
    public class AssetManager : IAssetManager
    {
        private struct SporeAssetDirectories
        {
            public string ModelDirectory { get; set; }
            public string ThumbDirectory { get; set; }
            public string ImageDirectory { get; set; }
        }

        private struct SporeAssetFiles
        {
            public string ModelFile { get; set; }
            public string ThumbFile { get; set; }
            public string ImageFile { get; set; }
            public string ImageFile2 { get; set; }
            public string ImageFile3 { get; set; }
            public string ImageFile4 { get; set; }
        }

        private readonly SporeServerContext _context;
        private readonly string _staticDirectory;
        private readonly ILogger<AssetManager> _logger;
        private readonly UserManager<SporeServerUser> _userManager;
        private readonly Random _random;

        public AssetManager(SporeServerContext context, IWebHostEnvironment env, ILogger<AssetManager> logger, UserManager<SporeServerUser> userManager)
        {
            _context = context;
            _staticDirectory = Path.Combine(env.WebRootPath, "static");
            _logger = logger;
            _userManager = userManager;
            _random = new Random();
        }

        /// <summary>
        ///     Turns an asset id into a directory structure
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private string GetAssetDirectory(Int64 id)
        {
            string idString = id.ToString();

            // verify string length
            if (idString.Length < 6)
            {
                throw new Exception("id is too short!");
            }

            string[] splitId = new string[3];
            for (int i = 0; i < 3; i++)
            {
                splitId[i] = idString.Substring(i * 3, 3);
            }

            return Path.Combine(splitId[0], splitId[1], splitId[2]);
        }

        /// <summary>
        ///     Returns all directories for the given asset id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private SporeAssetDirectories GetAssetDirectories(Int64 id)
        {
            string assetDirectory = GetAssetDirectory(id);
            var directories = new SporeAssetDirectories()
            {
                ModelDirectory = Path.Combine(_staticDirectory, "model", assetDirectory),
                ThumbDirectory = Path.Combine(_staticDirectory, "thumb", assetDirectory),
                ImageDirectory = Path.Combine(_staticDirectory, "image", assetDirectory),
            };

            // make sure the directories exist
            foreach (string directory in new string[] { directories.ModelDirectory, 
                                                        directories.ThumbDirectory, 
                                                        directories.ImageDirectory })
            {
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
            }

            return directories;
        }

        /// <summary>
        ///     Returns all files for the given asset id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private SporeAssetFiles GetAssetFiles(Int64 id)
        {
            var directories = GetAssetDirectories(id);
            return new SporeAssetFiles()
            {
                ModelFile = Path.Combine(directories.ModelDirectory, $"{id}.xml"),
                ThumbFile = Path.Combine(directories.ThumbDirectory, $"{id}.png"),
                ImageFile = Path.Combine(directories.ImageDirectory, $"{id}_lrg.png"),
                ImageFile2 = Path.Combine(directories.ImageDirectory, $"{id}_2_lrg.png"),
                ImageFile3 = Path.Combine(directories.ImageDirectory, $"{id}_3_lrg.png"),
                ImageFile4 = Path.Combine(directories.ImageDirectory, $"{id}_4_lrg.png"),
            };
        }

        /// <summary>
        ///     Opens a stream to the given IFormFile
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private Stream OpenFormFileStream(IFormFile file)
        {
            var stream = file.OpenReadStream();
            var decompressedStream = stream;

            // handle gzip compressed streams aswell
            if (file.ContentType == "application/x-gzip")
            {
                decompressedStream = new GZipStream(stream, CompressionMode.Decompress);
            }

            return decompressedStream;
        }

        /// <summary>
        ///     Loads a SporeModel
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        private bool LoadSporeModel(Stream stream, out SporeModel model)
        {
            try
            {
                model = SporeModel.SerializeFromXml(stream);
                return true;
            }
            catch(Exception e)
            {
                model = null;
                _logger.LogError($"LoadSporeModel: Failed To Serialize SporeModel: {e}");
                return false;
            }
        }

        /// <summary>
        ///     Validate the given SporeModel
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private bool ValidateSporeModel(SporeModel model)
        {
            try
            {
                SporeModel.Validate(model);
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError($"ValidateSporeModel: Failed To Validate SporeModel: {e}");
                return false;
            }
        }

        /// <summary>
        ///     Retrieves the archetypes for a given SporeModel
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private SporeArcheType[] GetSporeModelArcheTypes(SporeModel model)
        {
            try
            {
                return SporeModel.GetArcheTypes(model);
            }
            catch (Exception e)
            {
                _logger.LogError($"GetSporeModelArcheTypes: Failed To Retrieve Archetypes: {e}");
                return new SporeArcheType[] { };
            }
        }

        /// <summary>
        ///     Deserialize SporeModel to xml
        /// </summary>
        /// <param name="model"></param>
        /// <param name="xml"></param>
        /// <returns></returns>
        private bool DeserializeSporeModel(SporeModel model, out string xml)
        {
            try
            {
                xml = SporeModel.DeserializeToxml(model);
                return true;
            }
            catch(Exception e)
            {
                xml = null;
                _logger.LogError($"DeserializeSporeModel: Failed To Deserialize SporeModel: {e}");
                return false;
            }
        }

        /// <summary>
        ///     Saves IFormFile to path
        /// </summary>
        /// <param name="file"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        private async Task SaveFormFile(IFormFile file, string path)
        {
            using var saveStream = File.OpenWrite(path);
            using (var stream = file.OpenReadStream())
            {
                await stream.CopyToAsync(saveStream);
                await saveStream.FlushAsync();
            }
        }

        /// <summary>
        ///     Turns path into a relative url, returns null when path doesn't exist
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private string GetRelativeUrlFromPath(string path)
        {
            if (!File.Exists(path))
            {
                return null;
            }

            return "static/" + Path.GetRelativePath(_staticDirectory, path).Replace('\\', '/');
        }

        /// <summary>
        ///     Returns a random number
        /// </summary>
        /// <param name="minimum"></param>
        /// <param name="maximum"></param>
        /// <returns></returns>
        private int GetRandomNumber(int minimum, int maximum)
        {
            // TODO:
            // use RandomNumberGenerator.GetInt32 when .NET6 is out
            return _random.Next(minimum, maximum);
        }

        public async Task<bool> AddAsync(AssetUploadForm form, SporeServerAsset asset, SporeServerAsset parentAsset, bool slurped, SporeAssetType type)
        {
            try
            {
                var files = GetAssetFiles(form.AssetId);

                // load, validate & deserialize SporeModel
                SporeModel model = null;
                string modelXml = null;
                using (Stream modelStream = OpenFormFileStream(form.ModelData))
                {
                    if (!LoadSporeModel(modelStream, out model))
                    {
                        throw new Exception("LoadSporeModel Failed");
                    }
                    if (!ValidateSporeModel(model))
                    {
                        throw new Exception("ValidateSporeModel Failed");
                    }
                    if (!DeserializeSporeModel(model, out modelXml))
                    {
                        throw new Exception("DeserializeSporeModel Failed");
                    }
                }

                // save files
                File.WriteAllText(files.ModelFile, modelXml);
                await SaveFormFile(form.ThumbnailData, files.ThumbFile);
                if (form.ImageData != null)
                {
                    await SaveFormFile(form.ImageData, files.ImageFile);
                }
                if (form.ImageData_2 != null)
                {
                    await SaveFormFile(form.ImageData_2, files.ImageFile2);
                }
                if (form.ImageData_3 != null)
                {
                    await SaveFormFile(form.ImageData_3, files.ImageFile3);
                }
                if (form.ImageData_4 != null)
                {
                    await SaveFormFile(form.ImageData_4, files.ImageFile4);
                }

                // update database
                asset.Used = true;
                asset.Timestamp = DateTime.Now;
                asset.OriginalAssetId = 0;
                asset.ParentAssetId = 0;
                if (parentAsset != null)
                {
                    // when original asset id of parent is 0,
                    // the parent id is the original asset id
                    // else follow the original asset id specified
                    if (parentAsset.OriginalAssetId == 0)
                    {
                        asset.OriginalAssetId = parentAsset.AssetId;
                    }
                    else
                    {
                        asset.OriginalAssetId = parentAsset.OriginalAssetId;
                    }
                    asset.ParentAssetId = parentAsset.AssetId;
                }
                asset.Name = form.ModelData.FileName;

                var tags = new List<SporeServerAssetTag>();
                if (form.Tags != null)
                {
                    foreach (string tagString in form.Tags.Split(","))
                    {
                        string trimmedTagString = tagString.TrimStart().TrimEnd();

                        tags.Add(new SporeServerAssetTag()
                        {
                            Asset = asset,
                            Tag = trimmedTagString
                        });
                    }
                }
                asset.Tags = tags;

                var traits = new List<SporeServerAssetTrait>();
                if (form.TraitGuids != null)
                {
                    foreach (string traitString in form.TraitGuids.Split(","))
                    {
                        string trimmedTraitString = traitString.TrimStart()
                                                                .TrimStart('0', 'x')
                                                                .TrimEnd();

                        Int64 traitType = Int64.Parse(trimmedTraitString, NumberStyles.HexNumber);

                        // make sure the trait id is valid
                        if (!Enum.IsDefined(typeof(SporeAssetTraitType), traitType))
                        {
                            throw new Exception($"Invalid Trait Id: {traitType}");
                        }

                        traits.Add(new SporeServerAssetTrait()
                        {
                            Asset = asset,
                            TraitType = (SporeAssetTraitType)traitType
                        });
                    }
                }
                asset.Traits = traits;

                var archeTypes = new List<SporeServerAssetArcheType>();
                foreach (var archeType in GetSporeModelArcheTypes(model))
                {
                    archeTypes.Add(new SporeServerAssetArcheType()
                    {
                        Asset = asset,
                        ArcheType = archeType
                    });
                }
                asset.ArcheTypes = archeTypes;

                asset.Description = form.Description;
                asset.Size = form.ThumbnailData.Length;
                asset.Slurped = slurped;
                // TODO, put this in a struct or whatever?
                asset.ModelFileUrl = GetRelativeUrlFromPath(files.ModelFile);
                asset.ThumbFileUrl = GetRelativeUrlFromPath(files.ThumbFile);
                asset.ImageFileUrl = GetRelativeUrlFromPath(files.ImageFile);
                asset.ImageFile2Url = GetRelativeUrlFromPath(files.ImageFile2);
                asset.ImageFile3Url = GetRelativeUrlFromPath(files.ImageFile3);
                asset.ImageFile4Url = GetRelativeUrlFromPath(files.ImageFile4);
                asset.ModelType = (SporeModelType)model.Properties.ModelType;
                asset.Type = type;
                _context.Assets.Update(asset);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"AddAsync: Added Asset {asset.AssetId}");
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError($"AddAsync: Failed To Add Asset {asset.AssetId}: {e}");
                return false;
            }
        }

        public async Task<bool> UpdateAsync(SporeServerAsset asset)
        {
            try
            {
                // update asset in database
                _context.Assets.Update(asset);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"UpdateAsync: Updated Asset {asset.AssetId}");
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError($"UpdateAsync: Failed To Update Asset {asset.AssetId}: {e}");
                return false;
            }
        }

        public async Task<bool> DeleteAsync(SporeServerAsset asset)
        {
            try
            {
                var files = GetAssetFiles(asset.AssetId);

                // delete files
                foreach (string file in new string[] { files.ModelFile, 
                                                        files.ThumbFile, 
                                                        files.ImageFile, 
                                                        files.ImageFile2, 
                                                        files.ImageFile3, 
                                                        files.ImageFile4 })
                {
                    if (File.Exists(file))
                    {
                        File.Delete(file);
                    }
                }

                // update database
                _context.Assets.Remove(asset);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"DeleteAsync: Deleted Asset {asset.AssetId}");
                return true;
            }
            catch(Exception e)
            {
                _logger.LogError($"DeleteAsync: Failed To Delete Asset {asset.AssetId}: {e}");
                return false;
            }
        }

        public async Task<bool> ReserveAsync(SporeServerUser user)
        {
            try
            {
                // create new empty asset
                var asset = new SporeServerAsset()
                {
                    Used = false,
                    Author = user
                };

                // add it to assets
                await _context.Assets.AddAsync(asset);
                await _context.SaveChangesAsync();

                // update user
                user.NextAssetId = asset.AssetId;
                await _userManager.UpdateAsync(user);

                _logger.LogInformation($"ReserveAsync: Reserved Asset {asset.AssetId} For User {user.Id}");
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError($"ReserveAsync: Failed To Reserve Asset For User {user.Id}: {e}");
                return false;
            }
        }

        public async Task<SporeServerAsset> FindByIdAsync(Int64 id, bool includeExtras)
        {
            try
            {
                if (includeExtras)
                {
                    return await _context.Assets.Include(a => a.Author)
                                                .Include(a => a.Tags)
                                                .Include(a => a.Traits)
                                                .OrderByDescending(a => a.AssetId)
                                                .AsSplitQuery()
                                                .FirstOrDefaultAsync(a => a.AssetId == id);
                }
                else
                {
                    return await _context.Assets.FindAsync(id);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"FindByIdAsync: Failed To Find Asset {id}: {e}");
                return null;
            }
        }
        
        public async Task<SporeServerAsset> FindByIdAsync(Int64 id)
        {
            return await FindByIdAsync(id, false);
        }

        public async Task<SporeServerAsset[]> FindAllByUserIdAsync(Int64 authorId)
        {
            try
            {
                // return a list of assets which are used & have the same author id
                return await _context.Assets.Where(a => a.Used && a.AuthorId == authorId)
                                            .Include(a => a.Tags)
                                            .Include(a => a.Traits)
                                            .OrderByDescending(a => a.AssetId)
                                            .AsSplitQuery()
                                            .ToArrayAsync();
            }
            catch (Exception e)
            {
                _logger.LogError($"FindAllByUserIdAsync: Failed To Find Assets For {authorId}: {e}");
                return null;
            }
            
        }

        public async Task<SporeServerAsset[]> GetRandomAssetsAsync(Int64 authorId, SporeModelType type)
        {
            try
            {
                // maximum of 5 assets per request
                int amountOfItems = GetRandomNumber(0, 5);

                // find only used assets which don't have the author specified by author id
                // and make sure it's the type we want
                return await _context.Assets
                        .Include(a => a.Author)
                        .Where(a => a.Used &&
                                a.AuthorId != authorId &&
                                a.ModelType == type)
                        .OrderBy(a => Guid.NewGuid()) // random order
                        .Take(amountOfItems)
                        .ToArrayAsync();
            }
            catch (Exception e)
            {
                _logger.LogError($"GetRandomAssets: Failed To Get random Assets: {e}");
                return null;
            }
        }

        public async Task<SporeServerAsset[]> GetRandomAssetsAsync(Int64 authorId, SporeArcheType type)
        {
            try
            {
                // maximum of 5 assets per request
                int amountOfItems = GetRandomNumber(0, 5);

                // find only used assets which don't have the author specified by author id
                // and make sure it's the type we want
                return await _context.Assets
                        .Include(a => a.Author)
                        .Include(a => a.ArcheTypes)
                        .Where(a => a.Used &&
                                a.AuthorId != authorId &&
                                a.ArcheTypes.Any(a => a.ArcheType == type))
                        .OrderBy(a => Guid.NewGuid()) // random order
                        .Take(amountOfItems)
                        .ToArrayAsync();
            }
            catch (Exception e)
            {
                _logger.LogError($"GetRandomAssets: Failed To Get random Assets: {e}");
                return null;
            }
        }

        public async Task<Int32> GetCountByAuthorAsync(SporeServerUser author)
        {
            try
            {
                return await _context.Assets
                                        .Where(a => 
                                            a.Used &&
                                            a.AuthorId == author.Id 
                                        ).CountAsync();
            }
            catch (Exception e)
            {
                _logger.LogError($"GetCountByAuthor: Failed To Get Count For {author.Id}: {e}");
                return 0;
            }
        }

        public DbSet<SporeServerAsset> GetAllAssets()
        {
            return _context.Assets;
        }
    }
}
