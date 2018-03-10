// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MenuTextView.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.Menu
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using Aupli.Input;
    using Aupli.OperationSystem;
    using Sundew.Base.Text;
    using Sundew.Pi.ApplicationFramework.TextViewRendering;

    /// <summary>
    /// View for showing the menu.
    /// </summary>
    /// <seealso cref="ITextView" />
    public class MenuTextView : ITextView
    {
        private readonly NetworkDeviceProvider networkDeviceProvider;
        private IReadOnlyList<NetworkDevice> networkDevices;
        private IInvalidater invalidater;
        private IPAddress ipAddress;
        private int ipAddressIndex;
        private string tag;

        /// <summary>
        /// Initializes a new instance of the <see cref="MenuTextView" /> class.
        /// </summary>
        /// <param name="networkDeviceProvider">The ip address provider.</param>
        /// <param name="menuController">The menu controller.</param>
        public MenuTextView(NetworkDeviceProvider networkDeviceProvider, MenuController menuController)
        {
            this.networkDeviceProvider = networkDeviceProvider;
            menuController.TagInput += this.OnMenuControllerTagInput;
        }

        /// <inheritdoc />
        public void OnShowing(IInvalidater invalidater, ICharacterContext characterContext)
        {
            this.invalidater = invalidater;
            this.networkDevices = this.networkDeviceProvider.GetSystemNetworkDevices().Where(
                x => !Equals(x.IpAddress, IPAddress.Loopback)
                && !Equals(x.IpAddress, IPAddress.None)
                && !Equals(x.IpAddress, IPAddress.Any)).ToList();
            invalidater.Timer.Tick += this.OnTimerTick;
            invalidater.Timer.Interval = TimeSpan.FromSeconds(1);
            invalidater.Timer.Start(TimeSpan.FromSeconds(1));
        }

        /// <inheritdoc />
        public void Render(IRenderContext renderContext)
        {
            renderContext.Home();

            renderContext.WriteLine(this.ipAddress != null
                ? this.ipAddress.ToString().LimitAndPadRight(renderContext.Width, ' ')
                : "No IP found".LimitAndPadRight(renderContext.Width, ' '));

            renderContext.WriteLine(this.tag != null
                ? $"Tag: {this.tag}".LimitAndPadRight(renderContext.Width, ' ')
                : "No tag present".LimitAndPadRight(renderContext.Width, ' '));
        }

        /// <inheritdoc />
        public void OnClosing()
        {
            this.invalidater.Timer.Tick -= this.OnTimerTick;
            this.invalidater.Timer.Stop();
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            if (this.ipAddressIndex >= this.networkDevices.Count)
            {
                this.ipAddressIndex = 0;
            }

            this.ipAddress = this.networkDevices[this.ipAddressIndex++].IpAddress;
            this.invalidater.Invalidate();
        }

        private void OnMenuControllerTagInput(object sender, TagInputArgs e)
        {
            this.tag = e.Uid;
            this.invalidater.Invalidate();
        }
    }
}