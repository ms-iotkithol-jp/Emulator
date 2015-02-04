using System;
using Microsoft.SPOT;
using System.Threading;

namespace EGIoTKitApps
{
    public partial class Program : Gadgeteer.Program
    {
        public static void Main()
        {
            Program.Mainboard = new EGIoTKitEmulator.Modules.EGIoTKiTEmulatorMainboard();

            var program = new Program();
            program.InitializeModules();

            program.ProgramStarted();

            program.Run();
        }

        private void InitializeModules()
        {
            //
        }

        protected new static EGIoTKitEmulator.Modules.EGIoTKiTEmulatorMainboard Mainboard
        {
            get
            {
                return ((EGIoTKitEmulator.Modules.EGIoTKiTEmulatorMainboard)(Gadgeteer.Program.Mainboard));
            }
            set
            {
                Gadgeteer.Program.Mainboard = value;
            }
        }

    }
}
