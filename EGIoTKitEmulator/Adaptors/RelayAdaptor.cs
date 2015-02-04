using Microsoft.SPOT.Emulator.Gpio;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace EGIoTKitEmulator.Adaptors
{
    class RelayAdaptor
    {
        private GpioPort relayPort;
        private Models.IoTKiTHardwareStatus hwStatus;
        Dispatcher uiDispatcher;

        public RelayAdaptor(GpioPort port, Models.IoTKiTHardwareStatus hw, Dispatcher dispatcher)
        {
            relayPort = port;
            hwStatus = hw;
            uiDispatcher = dispatcher;
            relayPort.OnGpioActivity += relayPort_OnGpioActivity;
        }

        void relayPort_OnGpioActivity(GpioPort sender, bool edge)
        {
            Debug.WriteLine("OnGpioActivity");
            uiDispatcher.Invoke(() =>
            {
                hwStatus.RelayStatus = edge;
            });
        }
    }
}
