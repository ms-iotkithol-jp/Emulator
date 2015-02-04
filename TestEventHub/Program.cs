using System;

using Microsoft.SPOT;
using Microsoft.SPOT.Input;
using Microsoft.SPOT.Presentation;
using Microsoft.SPOT.Presentation.Controls;

namespace TestEventHub
{
    public class Program : Microsoft.SPOT.Application
    {
        public static void Main()
        {
            Program myApplication = new Program();

            Window mainWindow = myApplication.CreateWindow();

            myApplication.TestEventHub();

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

            return mainWindow;
        }

        private void OnButtonUp(object sender, RoutedEventArgs evt)
        {
            ButtonEventArgs e = (ButtonEventArgs)evt;

            
            // Print the button code to the Visual Studio output window.
            Debug.Print(e.Button.ToString());
        }

        const string ehName = "egemulatortest";
        const string accessKey="fmEFzya9YktHPknfUzIKMxy71be+ZVZ68mGhnfxuB5k=";
        const string ehNSName="egms-sensorhub-ns";
        const string policyName="RootManageSharedAccessKey";
//        const string amqpAddress="amqps://RootManageSharedAccessKey:fmEFzya9YktHPknfUzIKMxy71be+ZVZ68mGhnfxuB5k=@egms-sensorhub-ns.servicebus.windows.net"
        private void TestEventHub()
        {
            string amqpAddress = "amqps://" + policyName + ":" + EGIoTKit.Utility.HttpUtility.UrlEncode(accessKey) + "@" + ehNSName + ".servicebus.windows.net";
            var address = new Amqp.Address(amqpAddress);
            var connection = new Amqp.Connection(address);
            var session = new Amqp.Session(connection);
            var amqpSender = new Amqp.SenderLink(session, "send-link" + ehName, ehName + "/Patritions/1");
            var message = new Amqp.Message(System.Text.UTF8Encoding.UTF8.GetBytes("Hello EG Iot Kit Emulator"));
            amqpSender.Send(message);
        }
    }
}
