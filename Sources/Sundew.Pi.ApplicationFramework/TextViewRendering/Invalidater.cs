// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Invalidater.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.Pi.ApplicationFramework.TextViewRendering
{
    internal class Invalidater : IInvalidater
    {
        private readonly ViewTimerCache viewTimerCache;
        private bool invalidate = true;

        public Invalidater(ViewTimerCache viewTimerCache)
        {
            this.viewTimerCache = viewTimerCache;
        }

        public IViewTimer CreateTimer()
        {
            return this.viewTimerCache.GetOrCreate();
        }

        public bool IsRenderRequiredAndReset()
        {
            var shouldInvalidate = this.invalidate;
            this.invalidate = false;
            return shouldInvalidate;
        }

        public void Invalidate()
        {
            this.invalidate = true;
        }
    }
}