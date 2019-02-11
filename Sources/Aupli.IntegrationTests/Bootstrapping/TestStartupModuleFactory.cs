// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestStartupModule.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.IntegrationTests.Bootstrapping
{
    using System.Threading.Tasks;
    using SystemBoundaries;
    using SystemBoundaries.Bridges.Lifecycle;
    using SystemBoundaries.Pi.Display.Api;
    using JustMock;
    using Pi.IO.GeneralPurpose;
    using Sundew.Base.Computation;
    using Sundew.TextView.ApplicationFramework;
    using Sundew.TextView.ApplicationFramework.Input;
    using Sundew.TextView.ApplicationFramework.TextViewRendering;
    using Telerik.JustMock;

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
            var displayFactory = Mock.Create<IDisplayFactory>();
            var display = Mock.Arrange(() => displayFactory.Create(Arg.IsAny<IGpioConnectionDriverFactory>(), Arg.AnyBool)).ReturnsMock();
            Mock.Arrange(() => display.TryCreateCharacterContext()).Returns(Result.Success(Mock.Create<ICharacterContext>()));
            return displayFactory;
        }

        protected override IGreetingProvider CreateGreetingProvider()
        {
            return Mock.Create<IGreetingProvider>();
        }

        protected override Task<ILifecycleConfiguration> GetLifecycleConfigurationAsync()
        {
            var lifeCycleConfiguration = Mock.Create<ILifecycleConfiguration>();
            return Task.FromResult(lifeCycleConfiguration);
        }
    }
}