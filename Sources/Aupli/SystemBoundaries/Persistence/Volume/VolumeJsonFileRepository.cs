// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VolumeJsonFileRepository.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.Persistence.Volume
{
    using System.Globalization;
    using System.IO;
    using System.Threading.Tasks;
    using Aupli.ApplicationServices.Volume.Ari;
    using Sundew.Base.Numeric;
    using Sundew.Base.Threading;

    /// <summary>
    /// Volume repository that stores the volume in json to a file.
    /// </summary>
    /// <seealso cref="IVolumeRepository" />
    public class VolumeJsonFileRepository : IVolumeRepository
    {
        private readonly AsyncLock lockObject = new AsyncLock();
        private readonly string filePath;
        private Percentage volume;
        private Task saveTask = default!;

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
        public Percentage Volume
        {
            get
            {
                using (this.lockObject.Lock())
                {
                    return this.volume;
                }
            }

            set
            {
                using (this.lockObject.Lock())
                {
                    this.volume = value;
                }
            }
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        /// <returns>An async task.</returns>
        public async ValueTask InitializeAsync()
        {
            using (await this.lockObject.LockAsync().ConfigureAwait(false))
            {
                this.volume =
                    new Percentage(double.Parse(await File.ReadAllTextAsync(this.filePath).ConfigureAwait(false)));
            }
        }

        /// <summary>
        /// Saves this instance.
        /// </summary>
        /// <returns>An async task.</returns>
        public async Task SaveAsync()
        {
            using (await this.lockObject.LockAsync().ConfigureAwait(false))
            {
                this.saveTask = this.saveTask?.ContinueWith(async task => await this.PrivateSaveAsync()) ??
                                Task.Run(async () => await this.PrivateSaveAsync());
            }
        }

        private async Task PrivateSaveAsync()
        {
            await File.WriteAllTextAsync(this.filePath, this.volume.Value.ToString(CultureInfo.InvariantCulture))
                .ConfigureAwait(false);
        }
    }
}