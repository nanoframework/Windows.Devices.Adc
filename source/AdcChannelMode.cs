//
// Copyright (c) 2017 The nanoFramework project contributors
// See LICENSE file in the project root for full license information.
//

namespace Windows.Devices.Adc
{
    /// <summary>
    /// Describes the channel modes that the ADC controller can use for input.
    /// </summary>
    public enum AdcChannelMode
    {
        /// <summary>
        /// Difference between two pins.
        /// </summary>
        Differential,
        /// <summary>
        /// Simple value of a particular pin.
        /// </summary>
        SingleEnded
    }
}
