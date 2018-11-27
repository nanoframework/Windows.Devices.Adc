//
// Copyright (c) 2017 The nanoFramework project contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Collections;
using System.Runtime.CompilerServices;

namespace Windows.Devices.Adc
{
    /// <summary>
    /// Represents an ADC controller on the system
    /// </summary>
    public sealed class AdcController : IAdcController
    {
        // this is used as the lock object 
        // a lock is required because multiple threads can access the AdcController
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private readonly object _syncLock = new object();

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private readonly int _controllerId;

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private AdcChannelMode _channelMode;

        // backing field for DeviceCollection
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private Hashtable s_deviceCollection;

        /// <summary>
        /// Device collection associated with this <see cref="AdcController"/>.
        /// </summary>
        /// <remarks>
        /// This collection is for internal use only.
        /// </remarks>
        internal Hashtable DeviceCollection
        {
            get
            {
                if (s_deviceCollection == null)
                {
                    lock (_syncLock)
                    {
                        if (s_deviceCollection == null)
                        {
                            s_deviceCollection = new Hashtable();
                        }
                    }
                }

                return s_deviceCollection;
            }

            set
            {
                s_deviceCollection = value;
            }
        }

        internal AdcController(string adcController)
        {
            // check if this device is already opened
            if (!AdcControllerManager.ControllersCollection.Contains(adcController))
            {
                // the ADC id is an ASCII string with the format 'ADCn'
                // need to grab 'n' from the string and convert that to the integer value from the ASCII code (do this by subtracting 48 from the char value)
                _controllerId = adcController[3] - '0';

                // call native init to allow HAL/PAL inits related with ADC hardware
                // this is also used to check if the requested ADC actually exists
                NativeInit();

                // add ADC controller to collection
                AdcControllerManager.ControllersCollection.Add(adcController, this);
            }
            else
            {
                // this controller already exists: throw an exception
                throw new ArgumentException();
            }
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
        /// Gets the default ADC controller on the system. 
        /// </summary>
        /// <returns>
        /// The default ADC controller on the system, or null if the system has no ADC controller.
        /// </returns>
        public static AdcController GetDefault()
        {
            string controllersAqs = GetDeviceSelector();
            string[] controllers = controllersAqs.Split(',');

            if(controllers.Length > 0)
            {
                //////////////////////////////////////////////////////////////////
                // note that, by design, the default controller is              //
                // the first one in the collection returned from the native end //
                //////////////////////////////////////////////////////////////////

                if (AdcControllerManager.ControllersCollection.Contains(controllers[0]))
                {
                    // controller is already open
                    return (AdcController)AdcControllerManager.ControllersCollection[controllers[0]];
                }
                else
                {
                    // there is no controller yet, create one
                    return new AdcController(controllers[0]);
                }
            }

            // the system has no ADC controller 
            return null;
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
            NativeOpenChannel(channelNumber);

            return new AdcChannel(this, channelNumber);
        }

        #region Native Calls
 
        /// <summary>
        /// Retrieves an string of all the ADC controllers on the system. 
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern string GetDeviceSelector();

        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern void NativeOpenChannel(Int32 channelNumber);

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

        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern void NativeInit();

        #endregion
    }
}
