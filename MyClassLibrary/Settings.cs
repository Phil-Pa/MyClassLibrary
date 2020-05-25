using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MyClassLibrary
{
    public class Settings
    {

        private readonly Dictionary<string, string> _settings = new Dictionary<string, string>();
        private readonly List<string> _properties = new List<string>();
        private readonly string _settingsFileName;
        private readonly Type _type;

        public Settings(string settingsFileName, Type type)
        {
            _settingsFileName = settingsFileName;
            _type = type;
            InitProperties();
            Load();
        }

        private void InitProperties()
        {
            var properties = _type.GetProperties();

            foreach (var propertyInfo in properties)
            {
                _properties.Add(propertyInfo.Name);
            }
        }

        public bool IsProperty(string propertyName)
        {
            return _properties.Contains(propertyName);
        }

        private void Load()
        {
            if (!File.Exists(_settingsFileName))
                return;

            var lines = File.ReadAllLines(_settingsFileName);
            foreach (var line in lines)
            {
                var keyValuePair = line.Split('=', StringSplitOptions.RemoveEmptyEntries);
                var key = keyValuePair[0];

                if (!IsProperty(key))
                    throw new Exception(key + " is not a property of type " + _type.FullName);

                var value = keyValuePair[1];

                _settings.Add(key, value);
            }
        }

        public void Save()
        {
            var sb = new StringBuilder();
            foreach (var (key, value) in _settings)
            {
                sb.Append(key).Append('=').Append(value).Append(Environment.NewLine);
            }

            File.WriteAllText(_settingsFileName, sb.ToString());
        }

        public string this[string index]
        {
            get
            {
                if (_settings.ContainsKey(index)) return _settings[index];
                else throw new ArgumentException();
            }
            set
            {
                if (_settings.ContainsKey(index))
                {
                    _settings[index] = value;
                }
                else
                {
                    _settings.Add(index, value);
                }
            }
        }
    }
}
