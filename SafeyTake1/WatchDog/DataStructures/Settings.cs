using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WatchDOG.DataStructures
{
    public class Settings
    {
        #region Constants

        readonly string SETTINGS_FILEPATH = "";

        #endregion 

        #region Private Properties

        // Our settings
        static IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;

        // The key names of our settings
        const string LowAlertColorKeyName = "LowAlertColorSetting";
        const string MediumAlertColorKeyName = "MediumAlertColorSetting";
        const string HighAlertColorKeyName = "HighAlertColorSetting";
        const string CurrentDriveKeyName = "CurrentDriverSetting";
        const string IsGpsEnabledKeyName = "IsGpsEnabledSetting";
        const string UnitsKeyName = "UnitsSetting";

        // The default value of our settings    
        static Color LowAlertColorDefault = Colors.Green;
        static Color MediumAlertColorDefault = Colors.Yellow;
        static Color HighAlertColorDefault = Colors.Red;
        const Driver CurrentDriveDefault = null;
        const bool IsGpsEnabledDefault = true;
        const Units UnitsDefault = Units.Metric;
        
        #endregion 

        #region Public Properties
        /// <summary>
        /// Property to get and set a Low Alert Color Setting Key.
        /// </summary>
        static public Color LowAlertColorSetting
        {
            get
            {
                return GetValueOrDefault<Color>(LowAlertColorKeyName, LowAlertColorDefault);
            }
            set
            {
                if (AddOrUpdateValue(LowAlertColorKeyName, value))
                {
                    Save();
                }
            }
        }

        /// <summary>
        /// Property to get and set a Medium Alert Color Setting Key.
        /// </summary>
        static public Color MediumAlertColorSetting
        {
            get
            {
                return GetValueOrDefault<Color>(MediumAlertColorKeyName, MediumAlertColorDefault);
            }
            set
            {
                if (AddOrUpdateValue(MediumAlertColorKeyName, value))
                {
                    Save();
                }
            }
        }

        /// <summary>
        /// Property to get and set a High Alert Color Setting Key.
        /// </summary>
        static public Color HighAlertColorSetting
        {
            get
            {
                return GetValueOrDefault<Color>(HighAlertColorKeyName, HighAlertColorDefault);
            }
            set
            {
                if (AddOrUpdateValue(HighAlertColorKeyName, value))
                {
                    Save();
                }
            }
        }

        /// <summary>
        /// Property to get and set a CurrentDriver Setting Key.
        /// </summary>
        static public Driver CurrentDriverSetting
        {
            get
            {
                return GetValueOrDefault<Driver>(CurrentDriveKeyName, CurrentDriveDefault);
            }
            set
            {
                if (AddOrUpdateValue(CurrentDriveKeyName, value))
                {
                    Save();
                }
            }
        }


        /// <summary>
        /// Property to get and set a Is GPS Enabled Setting Key.
        /// </summary>
        static public bool IsGpsEnabledSettings
        {
            get
            {
                return GetValueOrDefault<bool>(IsGpsEnabledKeyName, IsGpsEnabledDefault);
            }
            set
            {
                if (AddOrUpdateValue(IsGpsEnabledKeyName, value))
                {
                    Save();
                }
            }
        }

        /// <summary>
        /// Property to get and set a Units Setting Key.
        /// </summary>
        static public Units UnitsSetting
        {
            get
            {
                return GetValueOrDefault<Units>(UnitsKeyName, UnitsDefault);
            }
            set
            {
                if (AddOrUpdateValue(UnitsKeyName, value))
                {
                    Save();
                }
            }
        }


        #endregion

        #region Public Methods

        #endregion

        #region General Methods
        /// <summary>
        /// Update a setting value for our application. If the setting does not
        /// exist, then add the setting.
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        static public bool AddOrUpdateValue(string Key, Object value)
        {
            bool valueChanged = false;

            // If the key exists
            if (settings.Contains(Key))
            {
                // If the value has changed
                if (settings[Key] != value)
                {
                    // Store the new value
                    settings[Key] = value;
                    valueChanged = true;
                }
            }
            // Otherwise create the key.
            else
            {
                settings.Add(Key, value);
                valueChanged = true;
            }
           return valueChanged;
        }

        /// <summary>
        /// Get the current value of the setting, or if it is not found, set the 
        /// setting to the default setting.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        static public T GetValueOrDefault<T>(string Key, T defaultValue)
        {
            T value;

            // If the key exists, retrieve the value.
            if (settings.Contains(Key))
            {
                value = (T)settings[Key];
            }
            // Otherwise, use the default value.
            else
            {
                value = defaultValue;
            }
            return value;
        }

        /// <summary>
        /// Save the settings.
        /// </summary>
        static public void Save()
        {
            settings.Save();
        }

        #endregion
    }

    public enum Units{
        Metric,     // Meters
        Imperial    // Miles
    }
}
