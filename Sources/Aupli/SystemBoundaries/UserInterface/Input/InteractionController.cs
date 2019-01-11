// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InteractionController.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Aupli.SystemBoundaries.UserInterface.Input
{
    using System;
    using Aupli.SystemBoundaries.Bridges.Interaction;
    using Aupli.SystemBoundaries.UserInterface.Input.Ari;
    using Sundew.Pi.IO.Devices.InfraredReceivers.Lirc;
    using Sundew.Pi.IO.Devices.RfidTransceivers;
    using Sundew.Pi.IO.Devices.RotaryEncoders;
    using Sundew.TextView.ApplicationFramework.Input;

    /// <summary>
    /// Maps hardware inputs to application input.
    /// </summary>
    public class InteractionController : IInteractionController
    {
        private readonly InputControls inputControls;
        private readonly IInputManager inputManager;
        private readonly IInteractionControllerReporter interactionControllerReporter;

        /// <summary>
        /// Initializes a new instance of the <see cref="InteractionController" /> class.
        /// </summary>
        /// <param name="inputControls">The input controls.</param>
        /// <param name="inputManager">The input manager.</param>
        /// <param name="interactionControllerReporter">The interaction controller reporter.</param>
        public InteractionController(InputControls inputControls, IInputManager inputManager, IInteractionControllerReporter interactionControllerReporter)
        {
            this.inputControls = inputControls;
            this.inputManager = inputManager;
            this.interactionControllerReporter = interactionControllerReporter;
            this.interactionControllerReporter?.SetSource(typeof(InteractionController));
            this.KeyInputEvent = inputManager.CreateEvent<KeyInputArgs>();
            this.TagInputEvent = inputManager.CreateEvent<TagInputArgs>();
            this.inputControls.PlayPauseButton.Pressed += this.OnPlayPauseButtonPressed;
            this.inputControls.NextButton.Pressed += this.OnNextButtonPressed;
            this.inputControls.PreviousButton.Pressed += this.OnPreviousButtonPressed;
            this.inputControls.MenuButton.Pressed += this.OnMenuButtonPressed;
            this.inputControls.RotaryEncoder.Pressed += this.OnKy040ConnectionPressed;
            this.inputControls.RotaryEncoder.Rotated += this.OnKy040ConnectionRotated;
            this.inputControls.RemoteControl.CommandReceived += this.OnLircConnectionLircCommand;
            this.inputControls.RfidTransceiver.TagDetected += this.OnRfidTransceiverTagDetected;
        }

        /// <summary>
        /// Gets the key input event.
        /// </summary>
        /// <value>
        /// The key input event.
        /// </value>
        public InputEvent<KeyInputArgs> KeyInputEvent { get; }

        /// <summary>
        /// Gets the tag input event.
        /// </summary>
        /// <value>
        /// The tag input event.
        /// </value>
        public InputEvent<TagInputArgs> TagInputEvent { get; }

        /// <summary>
        /// Starts listening for IR command and scanning for RFIDs.
        /// </summary>
        public void Start()
        {
            this.inputControls.RemoteControl.StartListening();
            this.inputControls.RfidTransceiver.StartScanning();
            this.interactionControllerReporter?.Started();
        }

        private static KeyInput GetInput(LircCommandEventArgs e)
        {
            switch (e.KeyCode)
            {
                case LircKeyCodes.KeyOk:
                    return KeyInput.Ok;
                case LircKeyCodes.KeyLeft:
                    return KeyInput.Left;
                case LircKeyCodes.KeyRight:
                    return KeyInput.Right;
                case LircKeyCodes.KeyUp:
                    return KeyInput.Up;
                case LircKeyCodes.KeyDown:
                    return KeyInput.Down;
                case LircKeyCodes.KeyPlayPause:
                    return KeyInput.PlayPause;
                case LircKeyCodes.KeyPrevious:
                    return KeyInput.Previous;
                case LircKeyCodes.KeyNextSong:
                    return KeyInput.Next;
                case LircKeyCodes.KeyStop:
                    return KeyInput.Stop;
                case LircKeyCodes.KeyMenu:
                    return KeyInput.Menu;
                default:
                    return KeyInput.Unknown;
            }
        }

        private void OnPlayPauseButtonPressed(object sender, EventArgs eventArgs)
        {
            this.RaiseInput(KeyInput.Ok);
        }

        private void OnNextButtonPressed(object sender, EventArgs eventArgs)
        {
            this.RaiseInput(KeyInput.Right);
        }

        private void OnPreviousButtonPressed(object sender, EventArgs eventArgs)
        {
            this.RaiseInput(KeyInput.Left);
        }

        private void OnMenuButtonPressed(object sender, EventArgs eventArgs)
        {
            this.RaiseInput(KeyInput.Menu);
        }

        private void OnKy040ConnectionRotated(object sender, RotationEventArgs e)
        {
            this.RaiseInput(e.EncoderDirection == EncoderDirection.Clockwise ? KeyInput.Up : KeyInput.Down);
        }

        private void OnKy040ConnectionPressed(object sender, EventArgs e)
        {
            this.RaiseInput(KeyInput.Select);
        }

        private void OnLircConnectionLircCommand(object sender, LircCommandEventArgs e)
        {
            this.RaiseInput(GetInput(e));
        }

        private void OnRfidTransceiverTagDetected(object sender, TagDetectedEventArgs e)
        {
            var uid = e.Tag.ToString();
            this.interactionControllerReporter?.TagInputEvent(uid);
            this.inputManager.Raise(this.TagInputEvent, this, new TagInputArgs(uid));
        }

        private void RaiseInput(KeyInput keyInput)
        {
            this.interactionControllerReporter?.KeyInputEvent(keyInput);
            this.inputManager.Raise(this.KeyInputEvent, this, new KeyInputArgs(keyInput));
        }
    }
}