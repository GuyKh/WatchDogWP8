using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using WatchDOG.DataStructures;

namespace WatchDog.Helpers
{
    public class SettingsHelper
    {
        private const string SettingsDir = "Settings";
        private const string SettingsFile = "settings.xml";

        public static void SetSettings(Settings settings)
        {
            SaveSettingToFile<Settings>(SettingsDir, SettingsFile, settings);
        }

        public static Settings GetSettings()
        {
            return RetrieveSettingFromFile<Settings>(SettingsDir, SettingsFile);
        }

        private static T RetrieveSettingFromFile<T>(string dir, string file) where T : class
        {
            IsolatedStorageFile isolatedFileStore = IsolatedStorageFile.GetUserStoreForApplication();
            if (isolatedFileStore.DirectoryExists(dir))
            {
                try
                {
                    using (var stream = new IsolatedStorageFileStream(System.IO.Path.Combine(dir, file), FileMode.Open, isolatedFileStore))
                    {
                        return (T)SerializationHelper.DeserializeData<T>(stream);
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Could not retrieve file " + dir + "\\" + file + ". With Exception: " + ex.Message);
                }
            }
            return null;
        }

        private static void SaveSettingToFile<T>(string dir, string file, T data)
        {
            IsolatedStorageFile isolatedFileStore = IsolatedStorageFile.GetUserStoreForApplication();
            if (!isolatedFileStore.DirectoryExists(dir))
                isolatedFileStore.CreateDirectory(dir);
            try
            {
                string fn = System.IO.Path.Combine(dir, file);
                if (isolatedFileStore.FileExists(fn)) isolatedFileStore.DeleteFile(fn); //mostly harmless, used because isolatedFileStore is stupid :D

                using (var stream = new IsolatedStorageFileStream(fn, FileMode.CreateNew, FileAccess.ReadWrite, isolatedFileStore))
                {
                    SerializationHelper.SerializeData<T>(data, stream);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Could not save file " + dir + "\\" + file + ". With Exception: " + ex.Message);
            }
        }
    }

    public static class SerializationHelper
    {
        public static void SerializeData<T>(this T obj, Stream streamObject)
        {
            if (obj == null || streamObject == null)
                return;

            var ser = new DataContractJsonSerializer(typeof(T));
            ser.WriteObject(streamObject, obj);
        }

        public static T DeserializeData<T>(Stream streamObject)
        {
            if (streamObject == null)
                return default(T);

            var ser = new DataContractJsonSerializer(typeof(T));
            return (T)ser.ReadObject(streamObject);
        }
    }
}
