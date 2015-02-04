using System;
using Microsoft.SPOT;
using EGIoTKit.Gadgeteer.Modules;

namespace EGIoTKitApps
{
    partial class Program
    {
        void ProgramStarted()
        {
            /*******************************************************************************************
             * 
             * Please write application below
             * 
            *******************************************************************************************/

            Gadgeteer.Timer timer = new Gadgeteer.Timer(5000);
            timer.Tick += timer_Tick;
            timer.Start();

            Debug.Print("Program Started");
        }

        bool relayStatus = false;
        void timer_Tick(Gadgeteer.Timer timer)
        {
            Debug.Print("Tick=" + DateTime.Now.Ticks);
            var tempVal = Mainboard.TemperatureSensor.TakeMeasurements();
            var accelVal = Mainboard.AccelerometerSensor.TakeMeasurements();
            Debug.Print("T=" + tempVal.Temperature);
            Debug.Print("Accelerometer:X=" + accelVal.X + ",Y=" + accelVal.Y + ",Z=" + accelVal.Z);

            if (relayStatus)
            {
                Mainboard.Relay.TurnOn();
            }
            else
            {
                Mainboard.Relay.TurnOff();
            }
            relayStatus = !relayStatus;
        }
    }
}
