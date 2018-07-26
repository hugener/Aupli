// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KeyInput.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Aupli.SystemBoundaries.Connectors.UserInterface.Input
{
    /// <summary>
    /// Defines the various keys.
    /// </summary>
    public enum KeyInput
    {
        /// <summary>
        /// The ok key.
        /// </summary>
        Ok,

        /// <summary>
        /// The left key.
        /// </summary>
        Left,

        /// <summary>
        /// The right key.
        /// </summary>
        Right,

        /// <summary>
        /// The select key.
        /// </summary>
        Select,

        /// <summary>
        /// The Up key.
        /// </summary>
        Up,

        /// <summary>
        /// The Down key.
        /// </summary>
        Down,

        /// <summary>
        /// The menu key.
        /// </summary>
        Menu,

        /// <summary>
        /// The play pause key.
        /// </summary>
        PlayPause,

        /// <summary>
        /// The next key.
        /// </summary>
        Next,

        /// <summary>
        /// The previous key.
        /// </summary>
        Previous,

        /// <summary>
        /// The stop key
        /// </summary>
        Stop,

        /// <summary>
        /// The unknown key.
        /// </summary>
        Unknown = 0xFFFF,
    }
}