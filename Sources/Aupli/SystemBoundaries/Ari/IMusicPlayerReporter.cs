// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMusicPlayerReporter.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.Ari
{
    using MpcNET;

    /// <summary>
    /// Interface for implementing a music player reporter and mpc connection reporter.
    /// </summary>
    /// <seealso cref="Aupli.SystemBoundaries.Mpc.IMusicPlayerReporter" />
    /// <seealso cref="MpcNET.IMpcConnectionReporter" />
    public interface IMusicPlayerReporter : Mpc.IMusicPlayerReporter, IMpcConnectionReporter
    {
    }
}