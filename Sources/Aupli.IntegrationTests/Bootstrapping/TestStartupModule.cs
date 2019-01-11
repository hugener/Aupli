// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestStartupModule.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.IntegrationTests.Bootstrapping
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using SystemBoundaries;
    using SystemBoundaries.Bridges.Lifecycle;
    using SystemBoundaries.Pi.Display.Api;
    using SystemBoundaries.SystemServices.Api;
    using SystemBoundaries.SystemServices.Ari;
    using global::NSubstitute;
    using NSubstitute;
    using Pi.IO.GeneralPurpose;
    using Sundew.Base.Computation;
    using Sundew.TextView.ApplicationFramework;
    using Sundew.TextView.ApplicationFramework.Input;
    using Sundew.TextView.ApplicationFramework.TextViewRendering;

    public class TestStartupModule : StartupModule
    {
        public TestStartupModule(
            IApplicationRendering application,
            IGpioConnectionDriverFactory gpioConnectionDriverFactory,
            string namePath = "name.val",
            string pin26FeaturePath = "pin26-feature.val",
            string greetingsPath = "greetings.csv",
            string lastGreetingPath = "last-greeting.val",
            ITextViewRendererReporter textViewRendererReporter = null,
            IInputManagerReporter inputManagerReporter = null,
            ISystemServicesAwaiterReporter systemServicesAwaiterReporter = null)
            : base(application, gpioConnectionDriverFactory, namePath, pin26FeaturePath, greetingsPath, lastGreetingPath, textViewRendererReporter, inputManagerReporter, systemServicesAwaiterReporter)
        {
        }

        protected override IDisplayFactory CreateDisplayFactory()
        {
            var displayFactory = Substitute.For<IDisplayFactory>();
            var display = displayFactory.Create(Arg.Any<IGpioConnectionDriverFactory>(), Arg.Any<bool>()).ReturnsSubstitute();
            display.TryCreateCharacterContext().Returns(Result.Success(Substitute.For<ICharacterContext>()));
            return displayFactory;
        }

        protected override IGreetingProvider CreateGreetingProvider()
        {
            return Substitute.For<IGreetingProvider>();
        }

        protected override ISystemServicesAwaiter CreateServicesAwaiter()
        {
            var systemServicesAwaiter = Substitute.For<ISystemServicesAwaiter>();
            systemServicesAwaiter.WaitForServicesAsync(Arg.Any<IEnumerable<string>>(), Arg.Any<TimeSpan>()).Returns(Task.FromResult(true));
            return systemServicesAwaiter;
        }

        protected override Task<ILifecycleConfiguration> GetLifecycleConfigurationAsync()
        {
            var lifeCycleConfiguration = Substitute.For<ILifecycleConfiguration>();
            return Task.FromResult(lifeCycleConfiguration);
        }
    }
}