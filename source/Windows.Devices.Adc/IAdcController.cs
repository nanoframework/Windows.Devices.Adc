//
// Copyright (c) 2017 The nanoFramework project contributors
// See LICENSE file in the project root for full license information.
//

namespace Windows.Devices.Adc
{
    internal interface IAdcController
    {
        bool IsChannelModeSupported(AdcChannelMode channelMode);
        AdcChannel OpenChannel(int channelNumber);

        int ChannelCount { get; }
        AdcChannelMode ChannelMode { get; set; }
        int MaxValue { get; }
        int MinValue { get; }
        int ResolutionInBits { get; }
    }
}
