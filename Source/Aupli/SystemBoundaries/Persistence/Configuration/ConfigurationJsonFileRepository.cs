// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConfigurationJsonFileRepository.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.Persistence.Configuration
{
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using Aupli.SystemBoundaries.Persistence.Configuration.Api;
    using Newtonsoft.Json;
    using Sundew.Base.Threading;

    /// <summary>
    /// Repository for persisting the configuration.
    /// </summary>
    public class ConfigurationJsonFileRepository : IConfigurationRepository
    {
        private readonly string configurationFilePath;

        private readonly AsyncLazy<Configuration> configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationJsonFileRepository"/> class.
        /// </summary>
        /// <param name="configurationFilePath">The configuration file path.</param>
        public ConfigurationJsonFileRepository(string configurationFilePath)
        {
            this.configurationFilePath = configurationFilePath;
            this.configuration = new AsyncLazy<Configuration>(
                async () =>
                {
                    var settings = await File.ReadAllTextAsync(configurationFilePath).ConfigureAwait(false);
                    return JsonConvert.DeserializeObject<Configuration>(settings);
                }, true);
        }

        /// <summary>
        /// Gets the settings asynchronous.
        /// </summary>
        /// <returns>A get lifecycle configuration task.</returns>
        public async Task<Configuration> GetConfigurationAsync()
        {
            return await this.configuration;
        }
    }
}