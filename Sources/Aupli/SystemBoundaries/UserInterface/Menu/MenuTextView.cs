// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MenuTextView.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.UserInterface.Menu
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using Aupli.SystemBoundaries.Bridges.Interaction;
    using Aupli.SystemBoundaries.UserInterface.Menu.Ari;
    using Sundew.Base.Collections;
    using Sundew.Base.Text;
    using Sundew.TextView.ApplicationFramework.TextViewRendering;

    /// <summary>
    /// View for showing the menu.
    /// </summary>
    /// <seealso cref="ITextView" />
    public class MenuTextView : ITextView
    {
        private readonly INetworkDeviceInfoProvider networkDeviceInfoProvider;
        private readonly MenuController menuController;
        private IReadOnlyList<NetworkDeviceInfo> networkDevices;
        private IInvalidater invalidater;
        private IPAddress ipAddress;
        private int ipAddressIndex;
        private string tag;
        private IViewTimer timer;

        /// <summary>
        /// Initializes a new instance of the <see cref="MenuTextView" /> class.
        /// </summary>
        /// <param name="networkDeviceInfoProvider">The ip address provider.</param>
        /// <param name="menuController">The menu controller.</param>
        public MenuTextView(INetworkDeviceInfoProvider networkDeviceInfoProvider, MenuController menuController)
        {
            this.networkDeviceInfoProvider = networkDeviceInfoProvider;
            this.menuController = menuController;
            this.menuController.TagInput += this.OnMenuControllerTagInput;
        }

        /// <inheritdoc />
        public IEnumerable<object> InputTargets => this.menuController.ToEnumerable();

        /// <inheritdoc />
        public Task OnShowingAsync(IInvalidater invalidater, ICharacterContext characterContext)
        {
            this.invalidater = invalidater;
            this.networkDevices = this.networkDeviceInfoProvider.GetNetworkDeviceInfos().Where(
                x => !Equals(x.IpAddress, IPAddress.Loopback)
                && !Equals(x.IpAddress, IPAddress.None)
                && !Equals(x.IpAddress, IPAddress.Any)).ToList();
            this.timer = invalidater.CreateTimer();
            this.timer.Tick += this.OnTimerTick;
            this.timer.Start(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public void Render(IRenderContext renderContext)
        {
            renderContext.Home();

            renderContext.WriteLine(this.ipAddress != null
                ? this.ipAddress.ToString().LimitAndPadRight(renderContext.Size.Width, ' ')
                : "No IP found".LimitAndPadRight(renderContext.Size.Width, ' '));

            renderContext.WriteLine(this.tag != null
                ? $"Tag: {this.tag}".LimitAndPadRight(renderContext.Size.Width, ' ')
                : "No tag present".LimitAndPadRight(renderContext.Size.Width, ' '));
        }

        /// <inheritdoc />
        public Task OnClosingAsync()
        {
            this.timer.Tick -= this.OnTimerTick;
            this.timer.Stop();
            return Task.CompletedTask;
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            if (this.ipAddressIndex >= this.networkDevices.Count)
            {
                this.ipAddressIndex = 0;
            }

            if (this.networkDevices.Any())
            {
                this.ipAddress = this.networkDevices[this.ipAddressIndex++].IpAddress;
            }
            else
            {
                this.ipAddress = null;
            }

            this.invalidater.Invalidate();
        }

        private void OnMenuControllerTagInput(object sender, TagInputArgs e)
        {
            this.tag = e.Uid;
            this.invalidater.Invalidate();
        }
    }
}