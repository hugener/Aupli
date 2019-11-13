// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestSystemServicesModule.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.IntegrationTests.Bootstrapping
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Aupli.SystemBoundaries.SystemServices;
    using Aupli.SystemBoundaries.SystemServices.Api;
    using Aupli.SystemBoundaries.SystemServices.Ari;
    using global::NSubstitute;

    public class TestSystemServicesModule : SystemServicesModule
    {
        public TestSystemServicesModule(ISystemServicesAwaiterReporter systemServicesAwaiterReporter, IWifiConnecterReporter wifiConnecterReporter)
            : base(systemServicesAwaiterReporter, wifiConnecterReporter)
        {
        }

        protected override ISystemServicesAwaiter CreateServicesAwaiter()
        {
            var systemServicesAwaiter = Substitute.For<ISystemServicesAwaiter>();
            systemServicesAwaiter.WaitForServicesAsync(Arg.Any<IEnumerable<string>>(), Timeout.InfiniteTimeSpan).Returns(Task.FromResult(true));
            return systemServicesAwaiter;
        }

        protected override IWifiConnecter CreateWifiConnecter()
        {
            return Substitute.For<IWifiConnecter>();
        }
    }
}