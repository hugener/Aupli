// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TagInputArgs.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.Shared.UserInterface.Input
{
    using global::System;

    /// <summary>
    /// Input event args when a tag is detected.
    /// </summary>
    /// <seealso cref="EventArgs" />
    public class TagInputArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TagInputArgs" /> class.
        /// </summary>
        /// <param name="uid">The uid.</param>
        public TagInputArgs(string uid)
        {
            this.Uid = uid;
        }

        /// <summary>
        /// Gets the uid.
        /// </summary>
        /// <value>
        /// The uid.
        /// </value>
        public string Uid { get; }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return "Tag: " + this.Uid;
        }
    }
}