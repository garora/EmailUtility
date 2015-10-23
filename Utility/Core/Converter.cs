using System;
using System.Reflection;

namespace Utility.Core
{
    public class Converter
    {
        public static object ConvertObj<T>(object input)
        {
            if (input == null)
                return default(T);
            if (typeof(T) == typeof(int))
                return Convert.ToInt32(input);
            if (typeof(T) == typeof(long))
                return Convert.ToInt64(input);
            if (typeof(T) == typeof(string))
                return Convert.ToString(input);
            if (typeof(T) == typeof(bool))
                return (Convert.ToBoolean(input) ? 1 : 0);
            if (typeof(T) == typeof(double))
                return Convert.ToDouble(input);
            if (typeof(T) == typeof(DateTime))
                return Convert.ToDateTime(input);
            return default(T);
        }

        public static T ConvertTo<T>(object input)
        {
            object obj = default(T);
            if (input == null || input == DBNull.Value)
                return (T)obj;
            if (typeof(T) == typeof(int))
                obj = Convert.ToInt32(input);
            else if (typeof(T) == typeof(long))
                obj = Convert.ToInt64(input);
            else if (typeof(T) == typeof(string))
                obj = Convert.ToString(input);
            else if (typeof(T) == typeof(bool))
                obj = (Convert.ToBoolean(input) ? 1 : 0);
            else if (typeof(T) == typeof(double))
                obj = Convert.ToDouble(input);
            else if (typeof(T) == typeof(DateTime))
                obj = Convert.ToDateTime(input);
            return (T)obj;
        }

        public static object ConvertTo(Type type, object input)
        {
            object obj = null;
            if (input == null || input == DBNull.Value)
                return null;
            if (type == typeof(int))
                obj = Convert.ToInt32(input);
            else if (type == typeof(long))
                obj = Convert.ToInt64(input);
            else if (type == typeof(string))
                obj = Convert.ToString(input);
            else if (type == typeof(bool))
                obj = (Convert.ToBoolean(input) ? 1 : 0);
            else if (type == typeof(double))
                obj = Convert.ToDouble(input);
            else if (type == typeof(DateTime))
                obj = Convert.ToDateTime(input);
            return obj;
        }

        public static bool CanConvertTo<T>(string val)
        {
            return CanConvertTo(typeof(T), val);
        }

        public static bool CanConvertTo(Type type, string val)
        {
            try
            {
                if (type == typeof(int))
                {
                    var result = 0;
                    return int.TryParse(val, out result);
                }
                if (type == typeof(string))
                    return true;
                if (type == typeof(double))
                {
                    var result = 0.0;
                    return double.TryParse(val, out result);
                }
                if (type == typeof(long))
                {
                    var result = 0L;
                    return long.TryParse(val, out result);
                }
                if (type == typeof(float))
                {
                    var result = 0.0f;
                    return float.TryParse(val, out result);
                }
                if (type == typeof(bool))
                {
                    var result = false;
                    return bool.TryParse(val, out result);
                }
                if (type == typeof(DateTime))
                {
                    var result = DateTime.MinValue;
                    return DateTime.TryParse(val, out result);
                }
                if (type.BaseType == typeof(Enum))
                    Enum.Parse(type, val, true);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public static bool CanConvertToCorrectType(PropertyInfo propInfo, object val)
        {
            try
            {
                if (propInfo.PropertyType == typeof(int))
                    Convert.ToInt32(val);
                else if (propInfo.PropertyType == typeof(double))
                    Convert.ToDouble(val);
                else if (propInfo.PropertyType == typeof(long))
                {
                    double num1 = Convert.ToInt64(val);
                }
                else if (propInfo.PropertyType == typeof(float))
                {
                    double num2 = Convert.ToSingle(val);
                }
                else if (propInfo.PropertyType == typeof(bool))
                    Convert.ToBoolean(val);
                else if (propInfo.PropertyType == typeof(DateTime))
                    Convert.ToDateTime(val);
                else if (propInfo.PropertyType.BaseType == typeof(Enum) && val is string)
                    Enum.Parse(propInfo.PropertyType, (string)val, true);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public static bool CanConvertToCorrectType(PropertyInfo propInfo, string val)
        {
            try
            {
                if (propInfo.PropertyType == typeof(int))
                {
                    var result = 0;
                    return int.TryParse(val, out result);
                }
                if (propInfo.PropertyType == typeof(string))
                    return true;
                if (propInfo.PropertyType == typeof(double))
                {
                    var result = 0.0;
                    return double.TryParse(val, out result);
                }
                if (propInfo.PropertyType == typeof(long))
                {
                    var result = 0L;
                    return long.TryParse(val, out result);
                }
                if (propInfo.PropertyType == typeof(float))
                {
                    var result = 0.0f;
                    return float.TryParse(val, out result);
                }
                if (propInfo.PropertyType == typeof(bool))
                {
                    var result = false;
                    return bool.TryParse(val, out result);
                }
                if (propInfo.PropertyType == typeof(DateTime))
                {
                    var result = DateTime.MinValue;
                    return DateTime.TryParse(val, out result);
                }
                if (propInfo.PropertyType.BaseType == typeof(Enum))
                    Enum.Parse(propInfo.PropertyType, val, true);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public static object ConvertToSameType(PropertyInfo propInfo, object val)
        {
            object obj = null;
            if (propInfo.PropertyType == typeof(int))
                obj = Convert.ChangeType(val, typeof(int));
            else if (propInfo.PropertyType == typeof(double))
                obj = Convert.ChangeType(val, typeof(double));
            else if (propInfo.PropertyType == typeof(long))
                obj = Convert.ChangeType(val, typeof(long));
            else if (propInfo.PropertyType == typeof(float))
                obj = Convert.ChangeType(val, typeof(float));
            else if (propInfo.PropertyType == typeof(bool))
                obj = Convert.ChangeType(val, typeof(bool));
            else if (propInfo.PropertyType == typeof(DateTime))
                obj = Convert.ChangeType(val, typeof(DateTime));
            else if (propInfo.PropertyType == typeof(string))
                obj = Convert.ChangeType(val, typeof(string));
            else if (propInfo.PropertyType.BaseType == typeof(Enum) && val is string)
                obj = Enum.Parse(propInfo.PropertyType, (string)val, true);
            return obj;
        }

        public static bool IsSameType(PropertyInfo propInfo, object val)
        {
            return propInfo.PropertyType == typeof(int) && val is int ||
                   propInfo.PropertyType == typeof(bool) && val is bool ||
                   (propInfo.PropertyType == typeof(string) && val is string ||
                    propInfo.PropertyType == typeof(double) && val is double) ||
                   (propInfo.PropertyType == typeof(long) && val is long ||
                    propInfo.PropertyType == typeof(float) && val is float ||
                    (propInfo.PropertyType == typeof(DateTime) && val is DateTime ||
                     propInfo.PropertyType != null && propInfo.PropertyType.GetType() == val.GetType()));
        }
    }
}