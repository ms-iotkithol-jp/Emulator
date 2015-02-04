using System;
using Microsoft.SPOT;

namespace EGIoTKitEmulator.Modules
{
    class SimulatorLogger : EGIoTKit.Gadgeteer.ILogger
    {
        Microsoft.SPOT.Hardware.I2CDevice i2cDevice;

        public SimulatorLogger(Microsoft.SPOT.Hardware.I2CDevice i2cDev)
        {
            i2cDevice = i2cDev;
        }
        void EGIoTKit.Gadgeteer.ILogger.Write(string log)
        {
            var order = Microsoft.SPOT.Hardware.I2CDevice.CreateWriteTransaction(new byte[] { 0x03 });
            var read = Microsoft.SPOT.Hardware.I2CDevice.CreateReadTransaction(Convert.FromBase64String(log));
            i2cDevice.Execute(new Microsoft.SPOT.Hardware.I2CDevice.I2CTransaction[] { order, read }, 100);
        }

       void EGIoTKit.Gadgeteer.ILogger.WriteLine(string log)
        {
            var msg = log + EGIoTKit.Gadgeteer.IoTKitBoard.EOL;
        }
    }
}
