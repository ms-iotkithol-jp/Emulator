using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace EGIoTKitEmulator.Adaptors
{
    public class AccerelometerSensorAdaptor : Microsoft.SPOT.Emulator.I2c.I2cDevice
    {
        public enum Command
        {
            NoCommand = 0,
            ReadAccelerometer = 1
        }

        Models.IoTKiTHardwareStatus hwStatus;
        Dispatcher uiDispatcher;
        public Models.IoTKiTHardwareStatus HwStatus
        {
            get { return hwStatus; }
            set { hwStatus = value; }
        }

        public Dispatcher UiDispatcher
        {
            get { return uiDispatcher; }
            set { uiDispatcher = value; }
        }

        public AccerelometerSensorAdaptor()
        {

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
            if (data.Length < 8*3)
            {
                new ArgumentOutOfRangeException("Buffer size should be more than 8*3!");
            }
            if (hwStatus == null)
            {
                new ArgumentNullException("Shoud be set HwStatus!");
            }
            switch (currentOrder)
            {
                case Command.ReadAccelerometer:
                    BitConverter.GetBytes(hwStatus.CurrentAccelX).CopyTo(data, 0);
                    BitConverter.GetBytes(hwStatus.CurrentAccelY).CopyTo(data, 8);
                    BitConverter.GetBytes(hwStatus.CurrentAccelZ).CopyTo(data, 16);
                    break;
            }
            base.DeviceRead(data);
            currentOrder = Command.NoCommand;
        }

        Command currentOrder = Command.NoCommand;
        protected override void DeviceWrite(byte[] data)
        {
            currentOrder = (Command)data[0];
            base.DeviceWrite(data);
        }
    }
}
