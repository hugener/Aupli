// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlayerCustomCharacters.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.UserInterface.Player
{
    using Sundew.Pi.ApplicationFramework.TextViewRendering;

    internal static class PlayerCustomCharacters
    {
        /* private const int PlayTopLeft = 0;
         private const int PlayTopRight = 1;
         private const int PlayBottomLeft = 2;
         private const int PlayBottomRight = 3;*/
        private const int PauseLeftId = 0;
        private const int PauseRightId = 1;
        private const int MuteId = 2;
        private static readonly byte[] PauseLeftCharacter =
        {
            0b01110,
            0b01110,
            0b01110,
            0b01110,
            0b01110,
            0b01110,
            0b01110,
        };

        private static readonly byte[] PauseRightCharacter =
        {
            0b01110,
            0b01110,
            0b01110,
            0b01110,
            0b01110,
            0b01110,
            0b01110,
        };

        private static readonly byte[] MuteCharacter =
        {
            0b00000,
            0b00001,
            0b00011,
            0b01101,
            0b01101,
            0b00011,
            0b00001,
        };
        /*
        private static readonly byte[] PlayTopLeftCharacter =
        {
            0b10000,
            0b11000,
            0b11100,
            0b11110,
            0b11111,
            0b11111,
            0b11111,
            //0b11111
        };

        private static readonly byte[] PlayTopRightCharacter =
        {
            0b00000,
            0b00000,
            0b00000,
            0b00000,
            0b10000,
            0b11000,
            0b11100,
            //0b11110
        };

        private static readonly byte[] PlayBottomLeftCharacter =
        {
            0b11111,
            0b11111,
            0b11111,
            0b11111,
            0b11110,
            0b11000,
            0b10000,
            //0b00000
        };

        private static readonly byte[] PlayBottomRightCharacter =
        {
            0b11110,
            0b11100,
            0b11000,
            0b10000,
            0b00000,
            0b00000,
            0b00000,
            //0b00000
        };
        */

        public static string Pause => $" {(char)PauseLeftId}{(char)PauseRightId} ";

        public static string Mute => $"{(char)MuteId}";

        /*
        public static string PlayerTop => $" {(char)PlayTopLeft}{(char)PlayTopRight} ";

        public static string PlayerBottom => $" {(char)PlayBottomLeft}{(char)PlayBottomRight} ";
        */
        public static void SetCharacters(ICharacterContext characterContext)
        {
            /*    characterContext.SetCustomCharacter(0, PlayTopLeftCharacter);
                characterContext.SetCustomCharacter(1, PlayTopRightCharacter);
                characterContext.SetCustomCharacter(2, PlayBottomLeftCharacter);
                characterContext.SetCustomCharacter(3, PlayBottomRightCharacter);*/

            characterContext.SetCustomCharacter(PauseLeftId, PauseLeftCharacter);
            characterContext.SetCustomCharacter(PauseRightId, PauseRightCharacter);
            characterContext.SetCustomCharacter(MuteId, MuteCharacter);
        }
    }
}
