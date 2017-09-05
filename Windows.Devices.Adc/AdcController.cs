//
// Copyright (c) 2017 The nanoFramework project contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Collections.Generic;
using Windows.Devices.Adc.Provider;

namespace Windows.Devices.Adc
{
    /// <summary>
    /// Represents an ADC controller on the system
    /// </summary>
    public sealed class AdcController : IAdcController
    {
        /// <summary>
        /// The number of channels available on the ADC controller.
        /// </summary>
        /// <value>
        /// Number of channels.
        /// </value>
        public int ChannelCount { get; }

        /// <summary>
        /// Gets or sets the channel mode for the ADC controller.
        /// </summary>
        /// <value>
        /// The ADC channel mode.
        /// </value>
        public AdcChannelMode ChannelMode { get; set; }

        /// <summary>
        /// Gets the maximum value that the controller can report.
        /// </summary>
        /// <value>
        /// The maximum value.
        /// </value>
        public int MaxValue { get; }

        /// <summary>
        /// The minimum value the controller can report.
        /// </summary>
        /// <value>
        /// The minimum value.
        /// </value>
        public int MinValue { get; }

        /// <summary>
        /// Gets the resolution of the controller as number of bits it has. For example, if we have a 10-bit ADC, that means it can detect 1024 (2^10) discrete levels.
        /// </summary>
        /// <value>
        /// The number of bits the ADC controller has.
        /// </value>
        public int ResolutionInBits { get; }

        /// <summary>
        /// Gets all the controllers that are connected to the system asynchronously.
        /// </summary>
        /// <param name="provider">
        /// The ADC provider for the controllers on the system.
        /// </param>
        /// <returns>
        /// When the method completes successfully, it returns a list of values that represent the controllers available on the system.
        /// </returns>
        public static IReadOnlyList<AdcController> GetControllers(IAdcProvider provider)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the default ADC controller on the system.
        /// </summary>
        /// <returns>
        /// The default ADC controller on the system, or null if the system has no ADC controller.
        /// </returns>
        public static AdcController GetDefault()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Verifies that the specified channel mode is supported by the controller.
        /// </summary>
        /// <param name="channelMode">
        /// The channel mode.
        /// </param>
        /// <returns>
        /// True if the specified channel mode is supported, otherwise false.
        /// </returns>
        public bool IsChannelModeSupported(AdcChannelMode channelMode)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Opens a connection to the specified ADC channel.
        /// </summary>
        /// <param name="channelNumber">
        /// The channel to connect to.
        /// </param>
        /// <returns>
        /// The ADC channel.
        /// </returns>
        public AdcChannel OpenChannel(Int32 channelNumber)
        {
            throw new NotImplementedException();
        }
    }
}
