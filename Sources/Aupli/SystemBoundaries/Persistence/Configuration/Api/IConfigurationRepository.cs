// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IConfigurationRepository.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.Persistence.Configuration.Api
{
    using System.Threading.Tasks;

    /// <summary>
    /// Interface for a configuration repository.
    /// </summary>
    public interface IConfigurationRepository
    {
        /// <summary>
        /// Gets the settings asynchronous.
        /// </summary>
        /// <returns>A get lifecycle configuration task.</returns>
        Task<Configuration> GetConfigurationAsync();

        /// <summary>
        /// Saves the configuration asynchronously.
        /// </summary>
        /// <returns>A save task.</returns>
        Task SaveConfigurationAsync();
    }
}