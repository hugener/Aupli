// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMenuRequester.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.UserInterface
{
    using System;

    /// <summary>
    /// Interface for implementing a menu requester.
    /// </summary>
    public interface IMenuRequester
    {
        /// <summary>
        /// Occurs when [menu requested].
        /// </summary>
        event EventHandler MenuRequested;
    }
}