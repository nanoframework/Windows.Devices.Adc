//
// Copyright (c) 2017 The nanoFramework project contributors
// See LICENSE file in the project root for full license information.
//

using System;

namespace Windows.Devices.Adc
{
    /// <summary>
    /// Represents a single ADC channel.
    /// </summary>
    public sealed class AdcChannel : IAdcChannel, IDisposable
    {
        /// <summary>
        /// Gets the ADC controller for this channel.
        /// </summary>
        /// <value>
        /// The ADC controller.
        /// </value>
        public AdcController Controller { get; }

        /// <summary>
        /// Closes the connection on this channel, making it available to be opened by others.
        /// </summary>
        public void Close()
        {
            // This member is not implemented in C#
            throw new NotImplementedException();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Reads the value as a percentage of the max value possible for this controller.
        /// </summary>
        /// <returns>
        /// The value as percentage of the max value.
        /// </returns>
        public double ReadRatio()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Reads the digital representation of the analog value from the ADC.
        /// </summary>
        /// <returns>
        /// The digital value.
        /// </returns>
        public int ReadValue()
        {
            throw new NotImplementedException();
        }
    }
}
