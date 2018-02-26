//
// Copyright (c) 2017 The nanoFramework project contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Runtime.CompilerServices;

namespace Windows.Devices.Adc
{
    /// <summary>
    /// Represents a single ADC channel.
    /// </summary>
    public sealed class AdcChannel : IAdcChannel, IDisposable
    {
        private readonly int  _channelNumber;
        private AdcController _adcController;
        private int           _deviceId;

               // this is used as the lock object 
        // a lock is required because multiple threads can access the GpioPin
        private object _syncLock = new object();

        internal AdcChannel(AdcController controller, int deviveId, int channelNumber)
        {
            _adcController = controller;
            _deviceId = deviveId;
            _channelNumber = channelNumber;
        }

     
        
        /// <summary>
        /// Gets the ADC controller for this channel.
        /// </summary>
        /// <value>
        /// The ADC controller.
        /// </value>
       public AdcController Controller {
            get
            {
                return _adcController;
            }
        }

 
        /// <summary>
        /// Reads the value as a percentage of the max value possible for this controller.
        /// </summary>
        /// <returns>
        /// The value as percentage of the max value.
        /// </returns>
        public double ReadRatio()
        {
            return (double)ReadValue() / (double)_adcController.MaxValue;
        }

        /// <summary>
        /// Reads the digital representation of the analog value from the ADC.
        /// </summary>
        /// <returns>
        /// The digital value.
        /// </returns>
        public int ReadValue()
        {
            lock (_syncLock)
            {
                // check if pin has been disposed
                if (_disposedValue) { throw new ObjectDisposedException(); }

                return NativeReadValue(_deviceId, _channelNumber);
            }
        }

        private void ThowIfDisposed()
        {
            // check if pin has been disposed
            if (_disposedValue)
            {
                throw new ObjectDisposedException();
            }
        }

        #region IDisposable Support

        private bool _disposedValue;

        private void Dispose(bool disposing)
        {
            if (_adcController != null)
            {
                if (disposing)
                {
                    NativeDisposeChannel(_channelNumber);
                    _adcController = null;

                }

                _disposedValue = true;
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            lock (_syncLock)
            {
                if (!_disposedValue)
                {
                    Dispose(true);
                    GC.SuppressFinalize(this);
                }
            }
        }

        #pragma warning disable 1591
        ~AdcChannel()
        {
            Dispose(false);
        }

        #endregion

        #region Native Calls

        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern int NativeReadValue(int deviceId, int channelNumber);

         [MethodImpl(MethodImplOptions.InternalCall)]
        private extern void NativeDisposeChannel(int channelNumber);

        #endregion

    }
}
