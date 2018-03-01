// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TagDetectedEventArgs.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.Pi.IO.Components.RfidTransceivers.Mfrc522
{
    using System;

    /// <summary>
    /// Event arguments for when a tag is detected.
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public class TagDetectedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TagDetectedEventArgs"/> class.
        /// </summary>
        /// <param name="uid">The uid.</param>
        public TagDetectedEventArgs(Uid uid)
        {
            this.Uid = uid;
        }

        /// <summary>
        /// Gets the uid.
        /// </summary>
        /// <value>
        /// The uid.
        /// </value>
        public Uid Uid { get; }
    }
}