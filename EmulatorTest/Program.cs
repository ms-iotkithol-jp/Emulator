using System;

using Microsoft.SPOT;
using Microsoft.SPOT.Input;
using Microsoft.SPOT.Presentation;
using Microsoft.SPOT.Presentation.Controls;
using Microsoft.SPOT.Hardware;
using System.Net;

namespace EmulatorTest
{
    public class Program : Microsoft.SPOT.Application
    {
        public static void Main()
        {
            Program myApplication = new Program();

            Window mainWindow = myApplication.CreateWindow();
            myApplication.SetupDevice();

            // Create the object that configures the GPIO pins to buttons.
            GPIOButtonInputProvider inputProvider = new GPIOButtonInputProvider(null);

            // Start the application
            myApplication.Run(mainWindow);
        }

        private Window mainWindow;

        public Window CreateWindow()
        {
            // Create a window object and set its size to the
            // size of the display.
            mainWindow = new Window();
            mainWindow.Height = SystemMetrics.ScreenHeight;
            mainWindow.Width = SystemMetrics.ScreenWidth;

            // Create a single text control.
            Text text = new Text();

            text.Font = Resources.GetFont(Resources.FontResources.small);
            text.TextContent = Resources.GetString(Resources.StringResources.String1);
            text.HorizontalAlignment = Microsoft.SPOT.Presentation.HorizontalAlignment.Center;
            text.VerticalAlignment = Microsoft.SPOT.Presentation.VerticalAlignment.Center;

            // Add the text control to the window.
            mainWindow.Child = text;

            // Connect the button handler to all of the buttons.
            mainWindow.AddHandler(Buttons.ButtonUpEvent, new RoutedEventHandler(OnButtonUp), false);

            // Set the window visibility to visible.
            mainWindow.Visibility = Visibility.Visible;

            // Attach the button focus to the window.
            Buttons.Focus(mainWindow);

            try
            {
                var request = HttpWebRequest.Create("http://www.twitter.com") as HttpWebRequest;
                using (var response = request.GetResponse() as HttpWebResponse)
                {
                    Debug.Print("Status:" + response.StatusCode);
                }
            }
            catch (Exception ex)
            {

            }
            return mainWindow;
        }

        OutputPort relayPort;
        I2CDevice i2cSensor;

        public void SetupDevice()
        {
            relayPort = new OutputPort((Cpu.Pin)21, false);
            relayPort.Write(true);
            var i2cConfig = new I2CDevice.Configuration(0x51, 400);
            i2cSensor = new I2CDevice(i2cConfig);
            var xwsaction = I2CDevice.CreateWriteTransaction(new byte[] { 0x01 });
            var xrsaction = I2CDevice.CreateReadTransaction(new byte[8]);
            i2cSensor.Execute(new I2CDevice.I2CTransaction[] {xwsaction, xrsaction }, 100);
            
            var read = xrsaction.Buffer;

            var temperature = BitConverter.ToDouble(read, 0);

            var accelOrder = I2CDevice.CreateWriteTransaction(new byte[] { 0x02 });
            var accelRead = I2CDevice.CreateReadTransaction(new byte[24]);
            i2cSensor.Execute(new I2CDevice.I2CTransaction[] { accelOrder, accelRead }, 100);

            var accelX = BitConverter.ToDouble(accelRead.Buffer, 0);
            var accelY = BitConverter.ToDouble(accelRead.Buffer, 8);
            var accelZ = BitConverter.ToDouble(accelRead.Buffer, 16);
        }

        private void OnButtonUp(object sender, RoutedEventArgs evt)
        {
            ButtonEventArgs e = (ButtonEventArgs)evt;

            // Print the button code to the Visual Studio output window.
            Debug.Print(e.Button.ToString());
        }

        void UpdateRealy()
        {
            var relayGP = new Microsoft.SPOT.Hardware.InputPort((Microsoft.SPOT.Hardware.Cpu.Pin)20, true, Microsoft.SPOT.Hardware.Port.ResistorMode.PullDown);
        }
    }
}
