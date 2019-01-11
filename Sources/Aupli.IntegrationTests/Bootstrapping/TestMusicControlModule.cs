// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestMusicControlModule.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.IntegrationTests.Bootstrapping
{
    using SystemBoundaries.MusicControl;
    using SystemBoundaries.MusicControl.Ari;
    using global::NSubstitute;
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
            this.MpcConnection = Substitute.For<IMpcConnection>();
            return this.MpcConnection;
        }
    }
}