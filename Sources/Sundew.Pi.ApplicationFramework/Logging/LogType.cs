// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogType.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.Pi.ApplicationFramework.Logging
{
    using System;

    /// <summary>
    /// Specifies the type of logging.
    /// </summary>
    [Flags]
    public enum LogType
    {
        /// <summary>
        /// Specifies logging to the console.
        /// </summary>
        Console,

        /// <summary>
        /// Specifies logging to a file.
        /// </summary>
        File
    }
}