// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IInputAggregator.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.Pi.ApplicationFramework.Input
{
    /// <summary>
    /// Interface for implementing an event aggregator.
    /// </summary>
    public interface IInputAggregator : IActivityAggregator
    {
        /// <summary>
        /// Starts this instance.
        /// </summary>
        void Start();
    }
}