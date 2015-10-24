using Microsoft.Win32;

namespace Utility.Core
{
    public class RegistryHelper
    {
        private RegistryHelper()
        {
        }

        public static void SetLocalMachineRegistryValue(string path, string item, string newValue)
        {
            RegistryKey registryKey = null;
            try
            {
                registryKey = Registry.LocalMachine.OpenSubKey(path, true) ?? Registry.LocalMachine.CreateSubKey(path);
                if (registryKey == null)
                    return;
                registryKey.SetValue(item, newValue, RegistryValueKind.String);
            }
            finally
            {
                if (registryKey != null)
                    registryKey.Close();
            }
        }

        public static string GetLocalMachineRegistryValue(string path, string item)
        {
            RegistryKey registryKey = null;
            string str = null;
            try
            {
                registryKey = Registry.LocalMachine.OpenSubKey(path);
                if (registryKey != null)
                    str = (string) registryKey.GetValue(item);
            }
            catch
            {
            }
            finally
            {
                if (registryKey != null)
                    registryKey.Close();
            }
            return str;
        }

        public static string GetLocalMachineRegistryValue(string path, string item, string defaultString)
        {
            return GetLocalMachineRegistryValue(path, item) ?? defaultString;
        }

        public static void SetCurrentUserRegistryValue(string path, string item, string newValue)
        {
            RegistryKey registryKey = null;
            try
            {
                registryKey = Registry.CurrentUser.OpenSubKey(path, true) ?? Registry.CurrentUser.CreateSubKey(path);
                if (registryKey == null)
                    return;
                registryKey.SetValue(item, newValue, RegistryValueKind.String);
            }
            finally
            {
                if (registryKey != null)
                    registryKey.Close();
            }
        }

        public static string GetCurrentUserRegistryValue(string path, string item)
        {
            RegistryKey registryKey = null;
            var str = string.Empty;
            try
            {
                registryKey = Registry.CurrentUser.OpenSubKey(path);
                if (registryKey != null)
                    str = (string) registryKey.GetValue(item);
            }
            catch
            {
            }
            finally
            {
                if (registryKey != null)
                    registryKey.Close();
            }
            return str;
        }

        public static string GetCurrentUserRegistryValue(string path, string item, string defaultString)
        {
            var str = GetCurrentUserRegistryValue(path, item);
            if (str == null || str == string.Empty)
                str = defaultString;
            return str;
        }
    }
}