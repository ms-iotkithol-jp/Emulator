using System;
using Microsoft.SPOT;

namespace EGIoTKitEmulator.Modules
{
    public class EGIoTKiTEmulatorMainboard : EGIoTKit.Gadgeteer.IoTKitBoard
    {
        public EGIoTKiTEmulatorMainboard()
        {
            var hwFactory = HardwareFactory.Current;
            accelerometerSensor = hwFactory.AccelelometerSensor;
            temperatureSensor = hwFactory.TemperatureSensor;
            relay = hwFactory.Relay;
        }

        public override void EnsureRgbSocketPinsAvailable()
        {
            throw new NotImplementedException();
        }

        public override string[] GetStorageDeviceVolumeNames()
        {
            throw new NotImplementedException();
        }

        public override string MainboardName
        {
            get { return "EGIoTKit Emulator"; }
        }

        public override string MainboardVersion
        {
            get { return "1.0.0.0"; }
        }

        public override bool MountStorageDevice(string volumeName)
        {
            throw new NotImplementedException();
        }

        protected override void OnOnboardControllerDisplayConnected(string displayModel, int width, int height, int orientationDeg, Gadgeteer.Modules.Module.DisplayModule.TimingRequirements timing)
        {
            throw new NotImplementedException();
        }

        public override void PostInit()
        
        {
            Debug.Print("PostInit Called");
        }

        public override void SetDebugLED(bool on)
        {
            if (on)
                Debug.Print("Debug LED:ON");
            else
                Debug.Print("Debug LED:OFF");
        }

        public override void SetProgrammingMode(Gadgeteer.Mainboard.ProgrammingInterface programmingInterface)
        {
            throw new NotImplementedException();
        }

        public override bool UnmountStorageDevice(string volumeName)
        {
            throw new NotImplementedException();
        }

        EGIoTKit.Gadgeteer.Modules.AccelerometerSensorModule accelerometerSensor;
        public override EGIoTKit.Gadgeteer.Modules.AccelerometerSensorModule AccelerometerSensor
        {
            get { return accelerometerSensor; }
        }

        EGIoTKit.Gadgeteer.Modules.TemperatureSensorModule temperatureSensor;
        public override EGIoTKit.Gadgeteer.Modules.TemperatureSensorModule TemperatureSensor
        {
            get { return temperatureSensor; }
        }

        EGIoTKit.Gadgeteer.Modules.RelayModule relay;
        public override EGIoTKit.Gadgeteer.Modules.RelayModule Relay
        {
            get { return relay; }
        }
    }

    class HardwareFactory
    {
        private HardwareFactory()
        {
            var config = new Microsoft.SPOT.Hardware.I2CDevice.Configuration(0x51, 400);
            i2cDevice = new Microsoft.SPOT.Hardware.I2CDevice(config);
            accelSensor = new AccelerometerSensor(i2cDevice);
            tempSensor = new TemperatureSensor(i2cDevice);
            relay = new Relay();
            emulatorLogger = new SimulatorLogger(i2cDevice);
        }
        private static HardwareFactory instance;
        Microsoft.SPOT.Hardware.I2CDevice i2cDevice;

       static  public HardwareFactory Current
        {
            get
            {
                if (instance == null) instance = new HardwareFactory();
                return instance;
            }
        }

        private AccelerometerSensor accelSensor;
        private TemperatureSensor tempSensor;
        private Relay relay;
        private EGIoTKit.Gadgeteer.ILogger emulatorLogger;

        public AccelerometerSensor AccelelometerSensor { get { return accelSensor; } }
        public TemperatureSensor TemperatureSensor { get { return tempSensor; } }
        public Relay Relay { get { return relay; } }

        public EGIoTKit.Gadgeteer.ILogger Logger { get { return emulatorLogger; } }
    }
}
