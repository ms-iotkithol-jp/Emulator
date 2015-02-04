using System;
using Microsoft.SPOT;

namespace EGIoTKitEmulator.Modules
{
    public class TemperatureSensor:EGIoTKit.Gadgeteer.Modules.TemperatureSensorModule
    {
        Microsoft.SPOT.Hardware.I2CDevice i2cSensor;
        System.Threading.Timer timer;

        public TemperatureSensor(Microsoft.SPOT.Hardware.I2CDevice i2cDevice)
        {
            i2cSensor = i2cDevice;
            measurementInterval = TimeSpan.FromTicks(TimeSpan.TicksPerMillisecond * 100);
        }
        public override void StartTakingMeasurements()
        {
            if (timer == null)
            {
                timer = new System.Threading.Timer(UpdateValue, null, TimeSpan.MinValue, this.MeasurementInterval);
            }
            else
            {
                timer.Change(TimeSpan.MinValue, this.MeasurementInterval);
            }
        }

        public override void StopTakingMeasurements()
        {
            if (timer != null)
            {
                timer.Dispose();
                timer = null;
            }
        }

        public override EGIoTKit.Gadgeteer.Modules.TemperatureSensorModule.Measurement TakeMeasurements()
        {
            var order = Microsoft.SPOT.Hardware.I2CDevice.CreateWriteTransaction(new byte[] { 0x01 });
            var read = Microsoft.SPOT.Hardware.I2CDevice.CreateReadTransaction(new byte[8]);
            i2cSensor.Execute(new Microsoft.SPOT.Hardware.I2CDevice.I2CTransaction[] { order, read }, 100);

            var temperature = BitConverter.ToDouble(read.Buffer, 0);

            currentValue = new Measurement() { Temperature = temperature };
            return currentValue;
        }

        private void UpdateValue(object state)
        {
            currentValue = TakeMeasurements();
            OnMeasurementComplete();
        }
    }
}
