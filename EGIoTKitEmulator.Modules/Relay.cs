using System;
using Microsoft.SPOT;

namespace EGIoTKitEmulator.Modules
{
    public class Relay : EGIoTKit.Gadgeteer.Modules.RelayModule
    {
        Microsoft.SPOT.Hardware.OutputPort relayPort;

        public Relay()
        {
            relayPort = new Microsoft.SPOT.Hardware.OutputPort((Microsoft.SPOT.Hardware.Cpu.Pin)21, false);
            if (relayPort == null)
            {
                new NotImplementedException("This library is developed for EG IoT Kit Emulator");
            }
        }

        public override void TurnOff()
        {
            relayPort.Write(false);
        }

        public override void TurnOn()
        {
            relayPort.Write(true);
        }
    }
}
