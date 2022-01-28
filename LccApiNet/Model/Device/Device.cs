using Ardalis.SmartEnum.JsonNet;

using Newtonsoft.Json;
using System.ComponentModel;

namespace LccApiNet.Model.Device
{
    /// <summary>
    /// Represents remote device that is used to control league client
    /// </summary>
    public class Device : INotifyPropertyChanged
    {
        private int _id;
        private DeviceType _type = null!;
        private string _name = null!;
        private bool _isOnline = false;
        
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Unique identifier of the device
        /// </summary>
        [JsonProperty("id")]
        public int Id
        {
            get => _id;
            set
            {
                if (value != _id)
                {
                    _id = value;
                    OnPropertyChanged(nameof(Id));
                }
            }
        }

        /// <summary>
        /// Type of the device
        /// </summary>
        [JsonProperty("type")]
        [JsonConverter(typeof(SmartEnumNameConverter<DeviceType, int>))]
        public DeviceType Type
        {
            get => _type;
            set
            {
                if (value != _type)
                {
                    _type = value;
                    OnPropertyChanged(nameof(Type));
                }
            }
        }

        /// <summary>
        /// Name of the device
        /// </summary>
        [JsonProperty("name")]
        public string Name
        {
            get => _name;
            set
            {
                if (value != _name)
                {
                    _name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        /// <summary>
        /// Whether device is online
        /// </summary>
        [JsonProperty("isOnline")]
        public bool IsOnline
        {
            get => _isOnline;
            set
            {
                if (value != _isOnline)
                {
                    _isOnline = value;
                    OnPropertyChanged(nameof(IsOnline));
                }
            }
        }

        private void OnPropertyChanged(string? propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
