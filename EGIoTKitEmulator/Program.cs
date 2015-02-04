using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;

using Microsoft.SPOT.Emulator;
using Microsoft.SPOT.Emulator.Gpio;

namespace EGIoTKitEmulator
{
    class Program : Emulator
    {
        public Program():base(new EmulatorNative())
        { 
        }

//        Microsoft.SPOT.Emulator.I2c.I2cBus sensorBus;

        public override void SetupComponent()
        {
        //    this.I2cBus = new Microsoft.SPOT.Emulator.I2c.I2cBus();
            var i2cSensor = new Adaptors.SensorAdaptor();
            i2cSensor.ComponentId = "I2CSensor";
            i2cSensor.Address = 0x51;
            i2cSensor.Scl = (Microsoft.SPOT.Hardware.Cpu.Pin)51;
            i2cSensor.Sda = (Microsoft.SPOT.Hardware.Cpu.Pin)52;
            this.RegisterComponent(i2cSensor);

   //         tempSensor.ComponentId = "TemperatureSensor";
          //  this.GpioPorts.MaxPorts = 128;
            base.SetupComponent();
        }

        public override void InitializeComponent()
        {
            base.InitializeComponent();

            // Start the UI in its own thread.
            Thread uiThread = new Thread(StartForm);
            uiThread.SetApartmentState(ApartmentState.STA);
            uiThread.Start();
        }

        public override void UninitializeComponent()
        {
            base.UninitializeComponent();

            // The emulator is stopped. Close the WinForm UI.
            Application.Exit();
        }

        private void StartForm()
        {
            // Some initial setup for the WinForm UI
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Start the WinForm UI. Run() returns when the form is closed.
            Application.Run(new MainForm(this));

            // When the user closes the WinForm UI, stop the emulator.
            Stop();
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            (new Program()).Start();
        }
    }

    class I2CTempSensor : Microsoft.SPOT.Emulator.I2c.I2cDevice
    {
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
            base.DeviceRead(data);
            data[0]=0x41;
        }

        protected override void DeviceWrite(byte[] data)
        {
            base.DeviceWrite(data);
        }
    }
}
