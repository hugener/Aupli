// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VolumeValue.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.Volume
{
    using Aupli.Numeric;

    /// <summary>
    /// Represents a volume value.
    /// </summary>
    public class VolumeValue
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VolumeValue"/> class.
        /// </summary>
        /// <param name="absoluteValue">The actual value.</param>
        /// <param name="percentage">The percentage.</param>
        public VolumeValue(byte absoluteValue, Percentage percentage)
        {
            this.AbsoluteValue = absoluteValue;
            this.Percentage = percentage;
        }

        /// <summary>
        /// Gets the actual value.
        /// </summary>
        /// <value>
        /// The actual value.
        /// </value>
        public byte AbsoluteValue { get; }

        /// <summary>
        /// Gets the percentage.
        /// </summary>
        /// <value>
        /// The percentage.
        /// </value>
        public Percentage Percentage { get; }
    }
}