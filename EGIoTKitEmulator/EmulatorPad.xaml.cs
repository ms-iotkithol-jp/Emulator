using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace EGIoTKitEmulator
{
    /// <summary>
    /// EmulatorPad.xaml の相互作用ロジック
    /// </summary>
    public partial class EmulatorPad : UserControl
    {
        public EmulatorPad()
        {
            InitializeComponent();
            this.Loaded += EmulatorPad_Loaded;
        }

        void EmulatorPad_Loaded(object sender, RoutedEventArgs e)
        {
            StartSensorEmulation();
        }

        Models.IoTKiTHardwareStatus iotKitHWStatus;

        public Models.IoTKiTHardwareStatus IotKitHWStatus
        {
            get { return iotKitHWStatus; }
            set
            {
                this.EmulatorPanel.DataContext = value;
                iotKitHWStatus = value;
            }
        }

        DispatcherTimer sensorEmulateTimer;
        TimeSpan sensorEmulateInterval = TimeSpan.FromMilliseconds(100);
        Random sensorValueRandom;
        public void StartSensorEmulation()
        {
            if (sensorEmulateTimer == null)
            {
                sensorValueRandom = new Random(DateTime.Now.Millisecond);
                sensorEmulateTimer = new DispatcherTimer();
                sensorEmulateTimer.Interval = sensorEmulateInterval;
                sensorEmulateTimer.Tick += sensorEmulateTimer_Tick;
            }
            sensorEmulateTimer.Start();
        }

        enum TemperatureSensorStatus
        {
            Untouched = 0,
            Touching,
            TouchedAndWarming,
            TouchedAndWarmed,
            Untouching,
            UntouchedAndCooling
        }
        long timeOfTemperatureSensorStatusChanged;

        TemperatureSensorStatus temperatureSensorStatus = TemperatureSensorStatus.Untouched;
        double deltaTemp = 0;

        void sensorEmulateTimer_Tick(object sender, EventArgs e)
        {
            if (iotKitHWStatus == null)
            {
                return;
            }
            long currentTick = DateTime.Now.Ticks;
            switch (temperatureSensorStatus)
            {
                case TemperatureSensorStatus.TouchedAndWarmed:
                case TemperatureSensorStatus.Untouched:
                    iotKitHWStatus.CurrentTemperature += iotKitHWStatus.TemperatureWhiteNoise * (0.5 - sensorValueRandom.NextDouble());
                    break;
                case TemperatureSensorStatus.Touching:
                    deltaTemp = (iotKitHWStatus.TargetMaxTemperature - iotKitHWStatus.TrueTemperature) / (iotKitHWStatus.ChangeDurationSec*TimeSpan.FromSeconds(1).Ticks / ( sensorEmulateInterval.Ticks));
                    temperatureSensorStatus = TemperatureSensorStatus.TouchedAndWarming;
                    break;
                case TemperatureSensorStatus.TouchedAndWarming:
                    iotKitHWStatus.TrueTemperature += deltaTemp;
                    if (iotKitHWStatus.TrueTemperature >= iotKitHWStatus.TargetMaxTemperature)
                    {
                        temperatureSensorStatus = TemperatureSensorStatus.TouchedAndWarmed;
                        cbTempSensorStatus.IsEnabled = true;
                    }
                    iotKitHWStatus.CurrentTemperature = iotKitHWStatus.TrueTemperature + iotKitHWStatus.TemperatureWhiteNoise * (0.5 - sensorValueRandom.NextDouble());
                    break;
                case  TemperatureSensorStatus.Untouching:
                    deltaTemp =(iotKitHWStatus.TargetMinTemperature - iotKitHWStatus.TrueTemperature) / (iotKitHWStatus.ChangeDurationSec * TimeSpan.FromSeconds(1).Ticks / (sensorEmulateInterval.Ticks));
                    temperatureSensorStatus = TemperatureSensorStatus.UntouchedAndCooling;
                    break;
                case TemperatureSensorStatus.UntouchedAndCooling:
                    iotKitHWStatus.TrueTemperature += deltaTemp;
                    if (iotKitHWStatus.TrueTemperature <= iotKitHWStatus.TargetMinTemperature)
                    {
                        temperatureSensorStatus = TemperatureSensorStatus.Untouched;
                        cbTempSensorStatus.IsEnabled = true;
                    }
                    iotKitHWStatus.CurrentTemperature = iotKitHWStatus.TrueTemperature + iotKitHWStatus.TemperatureWhiteNoise * (0.5 - sensorValueRandom.NextDouble());
                    break;
            }

            iotKitHWStatus.CurrentAccelX = iotKitHWStatus.TrueAccelX + iotKitHWStatus.AccelWhiteNoiseLevel * (0.5 - sensorValueRandom.NextDouble());
            iotKitHWStatus.CurrentAccelY = iotKitHWStatus.TrueAccelY + iotKitHWStatus.AccelWhiteNoiseLevel * (0.5 - sensorValueRandom.NextDouble());
            iotKitHWStatus.CurrentAccelZ = iotKitHWStatus.TrueAccelZ + iotKitHWStatus.AccelWhiteNoiseLevel * (0.5 - sensorValueRandom.NextDouble());
        }

        public void StopSensorEmulation()
        {
            if (sensorEmulateInterval != null)
            {
                sensorEmulateTimer.Stop();
            }
        }

        private void cbTempSensorStatus_CheckedChanged(object sender, RoutedEventArgs e)
        {
            timeOfTemperatureSensorStatusChanged = DateTime.Now.Ticks;
            if (((CheckBox)sender).IsChecked.Value)
            {
                if (temperatureSensorStatus == TemperatureSensorStatus.Untouched)
                {
                    temperatureSensorStatus = TemperatureSensorStatus.Touching;
                    ((CheckBox)sender).IsEnabled = false;
                }
            }
            else
            {
                if (temperatureSensorStatus == TemperatureSensorStatus.TouchedAndWarmed)
                {
                    temperatureSensorStatus = TemperatureSensorStatus.Untouching;
                    ((CheckBox)sender).IsEnabled = false;
                }
            }
        }

        DispatcherTimer accelerometerShakeTimer;

        enum AccelaratorSensorStatus
        {
            Stillness,
            Shake
        }
        double gravityX = 0;
        double gravityY = 0;
        double gravityZ = -1;

        AccelaratorSensorStatus accelaratorSensorStatus = AccelaratorSensorStatus.Stillness;
        private void cbAccelShake_CheckUpdated(object sender, RoutedEventArgs e)
        {
            if (accelerometerShakeTimer == null)
            {
                accelerometerShakeTimer = new DispatcherTimer();
                accelerometerShakeTimer.Interval = TimeSpan.FromMilliseconds(50);
                accelerometerShakeTimer.Tick += accelerometerShakeTimer_Tick;
            }
            if (((CheckBox)sender).IsChecked.Value)
            {
                accelaratorSensorStatus = AccelaratorSensorStatus.Shake;
                accelerometerShakeTimer.Start();
            }
            else
            {
                accelaratorSensorStatus = AccelaratorSensorStatus.Stillness;
                iotKitHWStatus.TrueAccelX = gravityX;
                iotKitHWStatus.TrueAccelY = gravityY;
                iotKitHWStatus.TrueAccelZ = gravityZ;
                accelerometerShakeTimer.Stop();
            }
        }

        void accelerometerShakeTimer_Tick(object sender, EventArgs e)
        {
            iotKitHWStatus.TrueAccelX = gravityX + (0.5 - sensorValueRandom.NextDouble()) * 4;
            iotKitHWStatus.TrueAccelY = gravityY + (0.5 - sensorValueRandom.NextDouble()) * 4;
            iotKitHWStatus.TrueAccelZ = gravityZ + (0.5 - sensorValueRandom.NextDouble()) * 4;
        }

        Vector3D lastVector = new Vector3D(0, 0, 1);
        double lastVectorLength = 1;

        private void UpdateRotation(double phai, double theata)
        {
            if (iotKitHWStatus != null)
            {
                double currentX;
                double currentY;
                double currentZ;
                double currentLength;
                double normX;
                double normY;
                double normZ;
                double angle;

                if (theata == 0)
                {
                    currentX = 0;
                    currentY = 0;
                    currentZ = 1;
                    angle = phai;
                    normX = 0;
                    normY = 1;
                    normZ = 0;
                    currentLength = 1;
                }
                else if (theata >= Math.PI)
                {
                    currentX = 0;
                    currentY = 0;
                    currentZ = -1;
                    angle = phai;
                    normX = 0;
                    normY = 0;
                    normZ = -1;
                    currentLength = 1;
                }
                else
                {
                    currentX = Math.Cos(phai) * Math.Sin(theata);
                    currentY = Math.Sin(phai) * Math.Sin(theata);
                    currentZ = Math.Cos(theata);

                    normX = lastVector.Y * currentZ - lastVector.Z * currentY;
                    normY = lastVector.Z * currentX - lastVector.X * currentZ;
                    normZ = lastVector.X * currentY - lastVector.Y * currentX;
                    currentLength = Math.Sqrt(currentX * currentX + currentY * currentY + currentZ * currentZ);
                    angle = Math.Acos((currentX * lastVector.X + currentY * lastVector.Y + currentZ * lastVector.Z) / (currentLength * lastVectorLength));
                }


                iotKitHWStatus.RotationMatrix = new AxisAngleRotation3D(new Vector3D(normX, normY, normZ), angle / Math.PI * 180);

                lastVector.X = currentX;
                lastVector.Y = currentY;
                lastVector.Z = currentZ;
                lastVectorLength = currentLength;

                iotKitHWStatus.TrueAccelX = -currentX;
                iotKitHWStatus.TrueAccelY = -currentY;
                iotKitHWStatus.TrueAccelZ = -currentZ;
            }
        }

   
        private void buttonResetAngle_Click(object sender, RoutedEventArgs e)
        {
            tbTheata.Text = "0";
            tbPhai.Text = "0";
            iotKitHWStatus.TrueAccelX = 0;
            iotKitHWStatus.TrueAccelY = 0;
            iotKitHWStatus.TrueAccelZ = -1;
            mtx3D = Matrix3D.Identity;
            boardMatrix.Matrix = mtx3D;
        }

        private void buttonResetPhai_Click(object sender, RoutedEventArgs e)
        {
            tbPhai.Text = "0";
        }

        Matrix3D mtx3D = Matrix3D.Identity;
        bool isDrag = false;
        Point mouseOffset;

        private void ModelUIElement3D_MouseDown(object sender, MouseButtonEventArgs e)
        {
            isDrag = true;
            mouseOffset = e.GetPosition(this);
        }

  //      Vector3D boardDirection = new Vector3D(0, 0, -1);
        private void ModelUIElement3D_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDrag && iotKitHWStatus != null)
            {
                var pt = e.GetPosition(this);
                mtx3D.Rotate(new Quaternion(new Vector3D(0, 0, 1), (pt.X - mouseOffset.X) / 10));
                mtx3D.Rotate(new Quaternion(new Vector3D(1, 0, 0), (pt.Y - mouseOffset.Y) / 10));
                boardMatrix.Matrix = mtx3D;

                double mx = mtx3D.M11 * iotKitHWStatus.TrueAccelX + mtx3D.M12 * iotKitHWStatus.TrueAccelY + mtx3D.M13 * iotKitHWStatus.TrueAccelZ;
                double my = mtx3D.M21 * iotKitHWStatus.TrueAccelX + mtx3D.M22 * iotKitHWStatus.TrueAccelY + mtx3D.M23 * iotKitHWStatus.TrueAccelZ;
                double mz = mtx3D.M31 * iotKitHWStatus.TrueAccelX + mtx3D.M32 * iotKitHWStatus.TrueAccelY + mtx3D.M33 * iotKitHWStatus.TrueAccelZ;

                double mlen = Math.Sqrt(mx * mx + my * my + mz * mz);
                double mxylen = Math.Sqrt(mx * mx + my * my);

                double mnx = -mx;
                double mny = -my;
                double mnz = -mz;

                tbTheata.Text = (Math.Acos(mnz / mlen) * 180 / Math.PI).ToString("#.#####");
                if (mxylen > 0)
                {
                    var phai = Math.Asin(mny / mxylen) * 180 / Math.PI;
                    if (phai < 0)
                    {
                        phai = 360 + phai;
                    }
                    tbPhai.Text = phai.ToString("#.#####");
                }
         //       iotKitHWStatus.LogString=("Theata=" + tbTheata.Text + ",Phai=" + tbPhai.Text);
                iotKitHWStatus.TrueAccelX = mx;
                iotKitHWStatus.TrueAccelY = my;
                iotKitHWStatus.TrueAccelZ = mz;
            }
        }

        private void ModelUIElement3D_MouseUp(object sender, MouseButtonEventArgs e)
        {
            isDrag = false;
        }


    }

    [ValueConversion(typeof(bool),typeof(BitmapImage))]
    public class RelayStatus2ImageConverter :IValueConverter
    {
        public object Convert(object value, Type targetType , object parameter, CultureInfo culture)
        {
            bool status = (bool)value;
            if (status)
            {
                return RelayOnImage;
            }
            else
            {
                return RelayOffImage;
            }
        }

        public object ConvertBack(object value,Type targetType, object parameter, CultureInfo culture)
        {
            BitmapImage image = (BitmapImage)value;
            if (image.Equals(RelayOnImage))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public BitmapImage RelayOnImage { get; set; }
        public BitmapImage RelayOffImage { get; set; }
    }
    
}
