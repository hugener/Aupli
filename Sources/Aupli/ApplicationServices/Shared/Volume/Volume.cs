// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Volume.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.ApplicationServices.Shared.Volume
{
    using Sundew.Base.Numeric;

    /// <summary>
    /// Represents a volume value.
    /// </summary>
    public class Volume
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Volume" /> class.
        /// </summary>
        /// <param name="percentage">The percentage.</param>
        public Volume(Percentage percentage)
        {
            this.Percentage = percentage;
        }

        /// <summary>
        /// Gets the percentage.
        /// </summary>
        /// <value>
        /// The percentage.
        /// </value>
        public Percentage Percentage { get; }
    }
}