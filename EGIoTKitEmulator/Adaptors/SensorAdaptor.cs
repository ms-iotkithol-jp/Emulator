using Microsoft.SPOT.Emulator.I2c;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace EGIoTKitEmulator.Adaptors
{
    class SensorAdaptor : Microsoft.SPOT.Emulator.I2c.I2cDevice
    {
        public enum Command
        {
            NoCommand = 0,
            ReadTemperature = 1,
            ReadAccelerometer = 2,
            Logging = 3
        }
        Dispatcher uiDispatcher;
        Models.IoTKiTHardwareStatus hwStatus;

        public Dispatcher UiDispatcher
        {
            get { return uiDispatcher; }
            set { uiDispatcher = value; }
        }
        public Models.IoTKiTHardwareStatus HwStatus
        {
            get { return hwStatus; }
            set { hwStatus = value; }
        }

        public SensorAdaptor()
        {

        }

        public SensorAdaptor(Models.IoTKiTHardwareStatus hw, Dispatcher dispatcher)
        {
            hwStatus = hw;
            uiDispatcher = dispatcher;
        }

        protected override void DeviceBeginTransaction()
        {
            base.DeviceBeginTransaction();
        }

        protected override void DeviceEndTransaction()
        {
            base.DeviceEndTransaction();
        }

        protected override void DeviceRead(byte[] data)
        {
            if (hwStatus == null)
            {
                new ArgumentNullException("Shoud be set HwStatus!");
            }
            switch (CurrentOrder)
            {
                case Command.ReadTemperature:
                    if (data.Length < 8)
                    {
                        new ArgumentOutOfRangeException("Buffer size should be more than 8!");
                    }
                    BitConverter.GetBytes(hwStatus.CurrentTemperature).CopyTo(data, 0);
                    break;
                case Command.ReadAccelerometer:
                    if (data.Length < 8 * 3)
                    {
                        new ArgumentOutOfRangeException("Buffer size should be more than 8*3!");
                    }
                    BitConverter.GetBytes(hwStatus.CurrentAccelX).CopyTo(data, 0);
                    BitConverter.GetBytes(hwStatus.CurrentAccelY).CopyTo(data, 8);
                    BitConverter.GetBytes(hwStatus.CurrentAccelZ).CopyTo(data, 16);
                    break;
                case Command.Logging:
                    var msg = Convert.ToBase64String(data);
                    hwStatus.LogString = msg;
                    break;
            }
            base.DeviceRead(data);
            CurrentOrder = Command.NoCommand;
        }

        private Command CurrentOrder;

        protected override void DeviceWrite(byte[] data)
        {
            CurrentOrder = (Command)data[0];
            base.DeviceWrite(data);
        }
    }
}
