// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MenuRequester.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.UserInterface.Internal
{
    using System;
    using Aupli.SystemBoundaries.UserInterface.Player.Ari;

    /// <summary>
    /// Requests the menu by sending an event.
    /// </summary>
    /// <seealso cref="IMenuRequester" />
    internal class MenuRequester : IMenuRequester
    {
        /// <summary>
        /// Occurs when [menu requested].
        /// </summary>
        public event EventHandler? MenuRequested;

        /// <summary>
        /// Requests the menu asynchronous.
        /// </summary>
        public void RequestMenu()
        {
            this.MenuRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}