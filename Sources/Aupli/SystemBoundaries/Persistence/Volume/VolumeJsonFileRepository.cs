// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VolumeJsonFileRepository.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.Persistence.Volume
{
    using System.IO;
    using System.Threading.Tasks;
    using Aupli.ApplicationServices.RequiredInterface.Volume;
    using Newtonsoft.Json;
    using Sundew.Base.Numeric;

    /// <summary>
    /// Volume repository that stores the volume in json to a file.
    /// </summary>
    /// <seealso cref="IVolumeRepository" />
    public class VolumeJsonFileRepository : IVolumeRepository
    {
        private readonly string filePath;

        /// <summary>
        /// Initializes a new instance of the <see cref="VolumeJsonFileRepository"/> class.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        public VolumeJsonFileRepository(string filePath)
        {
            this.filePath = filePath;
        }

        /// <summary>
        /// Gets or sets the volume.
        /// </summary>
        /// <value>
        /// The volume.
        /// </value>
        public Percentage Volume { get; set; }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        /// <returns>An async task.</returns>
        public async Task InitializeAsync()
        {
            this.Volume = JsonConvert.DeserializeObject<Percentage>(await File.ReadAllTextAsync(this.filePath));
        }

        /// <summary>
        /// Saves this instance.
        /// </summary>
        /// <returns>An async task.</returns>
        public async Task SaveAsync()
        {
            await File.WriteAllTextAsync(this.filePath, JsonConvert.SerializeObject(this.Volume));
        }
    }
}