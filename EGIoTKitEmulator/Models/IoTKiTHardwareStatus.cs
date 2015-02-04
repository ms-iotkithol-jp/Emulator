using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace EGIoTKitEmulator.Models
{
    public class IoTKiTHardwareStatus : INotifyPropertyChanged
    {
        public double TrueTemperature
        {
            get { return trueTemperature; }
            set
            {
                trueTemperature = value;
                OnPropertyChanged("TrueTemperature");
            }
        }
        public double CurrentTemperature
        {
            get { return currentTemperature; }
            set
            {
                currentTemperature = value;
                OnPropertyChanged("CurrentTemperature");
            }
        }
        public double TargetMaxTemperature
        {
            get { return targetMaxTemperature; }
            set
            {
                targetMaxTemperature = value;
                OnPropertyChanged("TargetMaxTemperature");
            }
        }
        public double TargetMinTemperature
        {
            get { return targetMinTemperature; }
            set
            {
                targetMinTemperature = value;
                OnPropertyChanged("TargetMinTemperature");
            }
        }
        public double ChangeDurationSec
        {
            get { return changeDurationSec; }
            set
            {
                changeDurationSec = value;
                OnPropertyChanged("ChangeDurationSec");
            }
        }
        public double TemperatureWhiteNoise
        {
            get { return temperatureWhiteNoiseLevel; }
            set
            {
                temperatureWhiteNoiseLevel = value;
                OnPropertyChanged("TemperatureWhiteNoise");
            }
        }

        public bool IsTouchedTemperatureSensor
        {
            get { return isTouchedTemperatureSensor; }
            set
            {
                isTouchedTemperatureSensor = true;
                OnPropertyChanged("IsTouchedTemperatureSensor");
            }
        }
        private double trueTemperature;
        private double currentTemperature;
        private double targetMaxTemperature;
        private double targetMinTemperature;
        private double changeDurationSec;
        private double temperatureWhiteNoiseLevel;
        private bool isTouchedTemperatureSensor;

        public double TrueAccelX
        {
            get { return trueAccelX; }
            set
            {
                trueAccelX = value;
                OnPropertyChanged("TrueAccelX");
            }
        }
        public double TrueAccelY
        {
            get { return trueAccelY; }
            set
            {
                trueAccelY = value;
                OnPropertyChanged("TrueAccelY");
            }
        }
        public double TrueAccelZ
        {
            get { return trueAccelZ; }
            set
            {
                trueAccelZ = value;
                OnPropertyChanged("TrueAccelZ");
            }
        }
        public double CurrentAccelX
        {
            get { return currentAccelX; }
            set
            {
                currentAccelX = value;
                OnPropertyChanged("CurrentAccelX");
            }
        }
        public double CurrentAccelY
        {
            get { return currentAccelY; }
            set
            {
                currentAccelY = value;
                OnPropertyChanged("CurrentAccelY");
            }
        }
        public double CurrentAccelZ
        {
            get { return currentAccelZ; }
            set
            {
                currentAccelZ = value;
                OnPropertyChanged("CurrentAccelZ");
            }
        }
        public double AccelWhiteNoiseLevel
        {
            get { return accelWhiteNoiseLevel; }
            set
            {
                accelWhiteNoiseLevel = value;
                OnPropertyChanged("AccelWhiteNoiseLevel");
            }
        }

        public Quaternion BoardQuaternion
        {
            get { return boardQuaternion; }
            private set
            {
                boardQuaternion = value;
                OnPropertyChanged("BoardQuaternion");
            }
        }

        Rotation3D rotationMatrix;
        public Rotation3D RotationMatrix
        {
            get { return rotationMatrix; }
            set
            {
                rotationMatrix = value;
                OnPropertyChanged("RotationMatrix");
            }
        }

        public void UpdateRotation()
        {
            var length = Math.Sqrt(trueAccelX * trueAccelX + trueAccelY * trueAccelY + trueAccelZ * trueAccelZ);

        }

        public bool RelayStatus
        {
            get { return relayStatus; }
            set
            {
                relayStatus = value;
                OnPropertyChanged("RelayStatus");
            }
        }

        private double trueAccelX;
        private double trueAccelY;
        private double trueAccelZ;
        private double currentAccelX;
        private double currentAccelY;
        private double currentAccelZ;
        private double accelWhiteNoiseLevel;

        private Quaternion boardQuaternion = new Quaternion(new Vector3D(0,0,1),0);

        private bool relayStatus;

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName){
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private StringBuilder logString = new StringBuilder();
        public string LogString
        {
            get { return logString.ToString(); }
            set
            {
                logString.Append(value);
                OnPropertyChanged("LogString");
            }
        }
    }
}
