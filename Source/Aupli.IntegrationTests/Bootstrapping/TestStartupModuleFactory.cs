// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestStartupModuleFactory.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.IntegrationTests.Bootstrapping
{
    using System.Threading.Tasks;
    using Aupli.SystemBoundaries;
    using Aupli.SystemBoundaries.Bridges.Lifecycle;
    using Aupli.SystemBoundaries.Pi.Display.Api;
    using global::NSubstitute;
    using Moq;
    using Pi.IO.GeneralPurpose;
    using Sundew.Base.Computation;
    using Sundew.TextView.ApplicationFramework;
    using Sundew.TextView.ApplicationFramework.Input;
    using Sundew.TextView.ApplicationFramework.TextViewRendering;

    public class TestStartupModuleFactory : StartupModuleFactory
    {
        public TestStartupModuleFactory(
            IApplicationRendering application,
            IGpioConnectionDriverFactory gpioConnectionDriverFactory,
            string namePath = "name.val",
            string pin26FeaturePath = "pin26-feature.val",
            string greetingsPath = "greetings.csv",
            string lastGreetingPath = "last-greeting.val",
            ITextViewRendererReporter textViewRendererReporter = null,
            IInputManagerReporter inputManagerReporter = null)
            : base(application, gpioConnectionDriverFactory, namePath, pin26FeaturePath, greetingsPath, lastGreetingPath, textViewRendererReporter, inputManagerReporter)
        {
        }

        protected override IDisplayFactory CreateDisplayFactory()
        {
            var displayFactory = New.Mock<IDisplayFactory>().SetDefaultValue(DefaultValue.Mock);
            var display = displayFactory.Create(It.IsAny<IGpioConnectionDriverFactory>(), It.IsAny<bool>());
            display.Setup(x => x.TryCreateCharacterContext()).Returns(Result.Success(New.Mock<ICharacterContext>()));
            return displayFactory;
        }

        protected override IGreetingProvider CreateGreetingProvider()
        {
            return New.Mock<IGreetingProvider>();
        }

        protected override Task<ILifecycleConfiguration> GetLifecycleConfigurationAsync()
        {
            var lifeCycleConfiguration = New.Mock<ILifecycleConfiguration>();
            return Task.FromResult(lifeCycleConfiguration);
        }
    }
}