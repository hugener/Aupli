// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IIdleControllerObserver.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.Pi.ApplicationFramework.Input
{
    /// <summary>
    /// Interface for implementing an observer for the <see cref="IdleController"/>.
    /// </summary>
    public interface IIdleControllerObserver
    {
        /// <summary>
        /// Called when the <see cref="IdleController"/> is started.
        /// </summary>
        void Started();

        /// <summary>
        /// Called when the <see cref="IdleController"/> receives input.
        /// </summary>
        void OnInputActivity();

        /// <summary>
        /// Called when the <see cref="IdleController"/> is activated.
        /// </summary>
        void Activated();

        /// <summary>
        /// Called when the <see cref="IdleController"/> receives system activity.
        /// </summary>
        void OnSystemActivity();

        /// <summary>
        /// Called when the <see cref="IdleController"/> does not receive input after some time.
        /// </summary>
        void OnInputIdle();

        /// <summary>
        /// Called when the <see cref="IdleController"/> does not receive activity after some time.
        /// </summary>
        void OnSystemIdle();
    }
}