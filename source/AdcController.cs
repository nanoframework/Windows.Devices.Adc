//
// Copyright (c) 2017 The nanoFramework project contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Runtime.CompilerServices;

namespace Windows.Devices.Adc
{
    /// <summary>
    /// Represents an ADC controller on the system
    /// </summary>
    public sealed class AdcController : IAdcController
    {
        private readonly int _deviceId;
        private AdcChannelMode _channelMode;
        private string _adcController;


        internal AdcController(string adcController)
        {
            // Remove the "ADC" part of the string "ADCxx" to get the "xx" value
            _deviceId = (Convert.ToInt32(adcController.Substring(3)));    
            _adcController = adcController;
        }

        /// <summary>
        /// The number of channels available on the ADC controller.
        /// </summary>
        /// <value>
        /// Number of channels.
        /// </value>
        public int ChannelCount {
            get
            {
                return NativeGetChannelCount();
            }
        }

        /// <summary>
        /// Gets or sets the channel mode for the ADC controller.
        /// </summary>
        /// <value>
        /// The ADC channel mode.
        /// </value>
        public AdcChannelMode ChannelMode {
            get
            {
                return _channelMode;
            }
            set
            {
                _channelMode = value;
            }
        }

        /// <summary>
        /// Gets the maximum value that the controller can report.
        /// </summary>
        /// <value>
        /// The maximum value.
        /// </value>
        public int MaxValue {
            get
            {
                return NativeGetMaxValue();
            }
            
        }

        /// <summary>
        /// The minimum value the controller can report.
        /// </summary>
        /// <value>
        /// The minimum value.
        /// </value>
        public int MinValue {
            get
            {
                return NativeGetMinValue();
            }
        }

        /// <summary>
        /// Gets the resolution of the controller as number of bits it has. For example, if we have a 10-bit ADC, that means it can detect 1024 (2^10) discrete levels.
        /// </summary>
        /// <value>
        /// The number of bits the ADC controller has.
        /// </value>
        public int ResolutionInBits {
            get
            {
                return NativeGetResolutionInBits();
            }
        }

        /// <summary>
        /// Initializes a ADC controller instance based on the given DeviceInformation ID.
        /// </summary>
        /// <param name="deviceId">
        /// The acquired DeviceInformation ID.
        /// </param>
        /// <returns>
        /// AdcController
        /// </returns>       
        public static AdcController FromID(string deviceId)
        {
            return new AdcController(deviceId);
        }

        /// <summary>
        /// Gets the default ADC controller on the system. 
        /// </summary>
        /// <returns>
        /// The default ADC controller on the system, or null if the system has no ADC controller.
        /// </returns>
        public static AdcController GetDefault()
        {
            string controllers = GetDeviceSelector();
            string[] devices = controllers.Split(',');

            return new AdcController(devices[0]);
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
            return NativeIsChannelModeSupported((int)channelMode);
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
            NativeOpenChannel(_deviceId, channelNumber);

            return new AdcChannel(this, _deviceId, channelNumber);
        }

        #region Native Calls
 
        /// <summary>
        /// Retrieves an string of all the ADC controllers on the system. 
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern string GetDeviceSelector();

        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern void NativeOpenChannel(Int32 deviceId ,Int32 channelNumber);

        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern int NativeGetChannelCount();

        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern int NativeGetMaxValue();

        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern int NativeGetMinValue();

        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern bool NativeIsChannelModeSupported(int mode);

        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern int NativeGetResolutionInBits();
        
        #endregion

    }
}
