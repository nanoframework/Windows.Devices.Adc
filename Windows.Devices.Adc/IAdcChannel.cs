//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

namespace Windows.Devices.Adc
{
    internal interface IAdcChannel
    {
        int ReadValue();
        double ReadRatio();

        AdcController Controller { get; }
    }
}
