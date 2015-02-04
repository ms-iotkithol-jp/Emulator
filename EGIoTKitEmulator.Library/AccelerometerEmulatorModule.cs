using System;
using Microsoft.SPOT;

namespace EGIoTKitEmulator.Library
{
    public class AccelerometerEmulatorModul : EGIoTKit.Gadgeteer.Modules.AccelerometerSensorModule
    {
        public AccelerometerEmulatorModul()
        {

        }

        public override void StartTakingMeasurements()
        {
            throw new NotImplementedException();
        }

        public override void StopTakingMeasurements()
        {
            throw new NotImplementedException();
        }

        public override EGIoTKit.Gadgeteer.Modules.AccelerometerSensorModule.Measurement TakeMeasurements()
        {
            throw new NotImplementedException();
        }
    }
}
