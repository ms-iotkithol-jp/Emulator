using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.SPOT.Emulator;
using Microsoft.SPOT.Emulator.Gpio;

namespace EGIoTKitEmulator
{
    public partial class MainForm : Form
    {
        private Emulator _emulator;

        System.Windows.Forms.Integration.ElementHost wpfPageHost;
        public MainForm(Emulator emulator)
        {
            _emulator = emulator;
            
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            wpfPageHost = new System.Windows.Forms.Integration.ElementHost();
            wpfPageHost.Dock = DockStyle.Fill;
            var wpfPage = new EmulatorPad();

            var hwStatus = new Models.IoTKiTHardwareStatus()
            {
                TargetMinTemperature = 20,
                TargetMaxTemperature = 34,
                CurrentTemperature = 20,
                TrueTemperature = 20,
                TemperatureWhiteNoise = 0.0001,
                ChangeDurationSec = 5,
                CurrentAccelX = 0,
                CurrentAccelY = 0,
                CurrentAccelZ = -1,
                TrueAccelX = 0,
                TrueAccelY = 0,
                TrueAccelZ = -1,
                AccelWhiteNoiseLevel = 0.001,
                RelayStatus = false
            };
            wpfPage.IotKitHWStatus = hwStatus;

            wpfPageHost.Child = wpfPage;

            this.Controls.Add(wpfPageHost);

            var relayGP = _emulator.FindComponentById("RelayGP") as GpioPort;
            relayAdaptor = new Adaptors.RelayAdaptor(relayGP, hwStatus,wpfPage.Dispatcher);

            sensorAdopter = _emulator.FindComponentById("I2CSensor") as Adaptors.SensorAdaptor;
            sensorAdopter.HwStatus = hwStatus;

        }
        Adaptors.RelayAdaptor relayAdaptor;
        Adaptors.SensorAdaptor sensorAdopter;
    }
}
