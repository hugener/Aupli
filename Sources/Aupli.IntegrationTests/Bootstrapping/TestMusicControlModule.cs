// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestMusicControlModule.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.IntegrationTests.Bootstrapping
{
    using Aupli.SystemBoundaries.MusicControl;
    using Aupli.SystemBoundaries.MusicControl.Ari;
    using global::NSubstitute;
    using Moq;
    using MpcNET;

    public class TestMusicControlModule : MusicControlModule
    {
        public TestMusicControlModule(IMusicPlayerReporter musicPlayerReporter)
            : base(musicPlayerReporter)
        {
        }

        public IMpcConnection MpcConnection { get; private set; }

        protected override IMpcConnection CreateMpcConnection(IMpcConnectionReporter mpcConnectionReporter)
        {
            this.MpcConnection = New.Mock<IMpcConnection>();
            return this.MpcConnection;
        }
    }
}