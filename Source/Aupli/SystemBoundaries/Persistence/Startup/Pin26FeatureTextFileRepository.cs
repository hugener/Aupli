// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Pin26FeatureTextFileRepository.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.Persistence.Startup
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Aupli.SystemBoundaries.Bridges.Lifecycle;

    /// <summary>
    /// Repository for getting the Pin 26 feature.
    /// </summary>
    public class Pin26FeatureTextFileRepository
    {
        private readonly string pin26FeaturePath;

        /// <summary>
        /// Initializes a new instance of the <see cref="Pin26FeatureTextFileRepository"/> class.
        /// </summary>
        /// <param name="pin26FeaturePath">The name path.</param>
        public Pin26FeatureTextFileRepository(string pin26FeaturePath)
        {
            this.pin26FeaturePath = pin26FeaturePath;
        }

        /// <summary>
        /// Gets the name asynchronous.
        /// </summary>
        /// <returns>The name in an async task.</returns>
        public async Task<Pin26Feature> GetPin26FeatureAsync()
        {
            return Enum.Parse<Pin26Feature>(await File.ReadAllTextAsync(this.pin26FeaturePath));
        }
    }
}