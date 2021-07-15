﻿//
// Copyright (c) .NET Foundation and Contributors
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
        // this is used as the lock object 
        // a lock is required because multiple threads can access the channel
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private readonly object _syncLock;

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private readonly int  _channelNumber;

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private AdcController _adcController;

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private bool _disposed;

        internal AdcChannel(AdcController controller, int channelNumber)
        {
            _adcController = controller;
            _channelNumber = channelNumber;

            _syncLock = new object();
        }

        /// <summary>
        /// Gets the ADC controller for this channel.
        /// </summary>
        /// <value>
        /// The ADC controller.
        /// </value>
       public AdcController Controller
       {
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
            return ReadValue() / (double)_adcController.MaxValue;
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
                if (_disposed) { throw new ObjectDisposedException(); }

                return NativeReadValue();
            }
        }

        private void ThowIfDisposed()
        {
            // check if pin has been disposed
            if (_disposed)
            {
                throw new ObjectDisposedException();
            }
        }

        #region IDisposable Support

        private void Dispose(bool disposing)
        {
            if (_adcController != null)
            {
                if (disposing)
                {
                    NativeDisposeChannel();
                    _adcController = null;

                }

                _disposed = true;
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            lock (_syncLock)
            {
                if (!_disposed)
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
        #pragma warning restore 1591

        #endregion

        #region Native Calls

        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern int NativeReadValue();

        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern void NativeDisposeChannel();

        #endregion

    }
}
