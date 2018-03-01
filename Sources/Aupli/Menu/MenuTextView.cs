// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MenuTextView.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.Menu
{
    using Aupli.OperationSystem;
    using Sundew.Pi.ApplicationFramework.TextViewRendering;

    /// <summary>
    /// View for showing the menu.
    /// </summary>
    /// <seealso cref="ITextView" />
    public class MenuTextView : ITextView
    {
        private readonly IPAddressProvider ipAddressProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="MenuTextView"/> class.
        /// </summary>
        /// <param name="ipAddressProvider">The ip address provider.</param>
        public MenuTextView(IPAddressProvider ipAddressProvider)
        {
            this.ipAddressProvider = ipAddressProvider;
        }

        /// <inheritdoc />
        public void OnShowing(IInvalidater invalidater, ICharacterContext characterContext)
        {
        }

        /// <inheritdoc />
        public void Render(IRenderContext renderContext)
        {
            renderContext.Clear();
            renderContext.Home();
            if (this.ipAddressProvider.TryGetSystemIpAddress(out var ipAddress))
            {
                renderContext.WriteLine(ipAddress);
            }
            else
            {
                renderContext.WriteLine("No IP found");
            }

            renderContext.WriteLine("Menu to exit");
        }

        /// <inheritdoc />
        public void OnClosing()
        {
        }
    }
}