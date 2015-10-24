using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Utility.Logging;

namespace Utility.Core
{
    public class ReflectionHelper
    {
        private ReflectionHelper()
        {
        }

        public static object CreateInstance(string assemblyName, string type)
        {
            try
            {
                return Assembly.LoadFrom(assemblyName).CreateInstance(type, true);
            }
            catch (TargetInvocationException ex)
            {
                throw ex.InnerException;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static object CreateInstance(Type type, object[] args)
        {
            try
            {
                return Activator.CreateInstance(type, args);
            }
            catch (TargetInvocationException ex)
            {
                throw ex.InnerException;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static object CreateInstance(Type type)
        {
            return Activator.CreateInstance(type);
        }

        public static Type GetType(string assemblyName, string type)
        {
            try
            {
                return Assembly.LoadFrom(assemblyName).GetType(type, true);
            }
            catch (TargetInvocationException ex)
            {
                throw ex.InnerException;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static ArrayList LoadAllAssembliesForPath(string path)
        {
            var str = Assembly.GetExecutingAssembly().Location.ToLower();
            var arrayList = new ArrayList();
            foreach (var assemblyFile in Directory.GetFiles(path, "*.dll"))
            {
                if (!assemblyFile.ToLower().Equals(str))
                {
                    try
                    {
                        var assembly = Assembly.LoadFrom(assemblyFile);
                        arrayList.Add(assembly);
                    }
                    catch (Exception ex)
                    {
                        MessageLog.LogError(LogClass.CommonUtil, ex);
                    }
                }
            }
            var directoryName = new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName;
            if (StringHelper.Match(path, directoryName, true))
                arrayList.Add(Assembly.GetExecutingAssembly());
            return arrayList;
        }

        public static Type[] GetCreatableObjects(Type parentType, string path)
        {
            var arrayList1 = new ArrayList();
            var arrayList2 = LoadAllAssembliesForPath(path);
            try
            {
                foreach (Assembly assembly in arrayList2)
                {
                    Debug.WriteLine(assembly.Location);
                    foreach (var c in assembly.GetTypes())
                    {
                        if (parentType.IsAssignableFrom(c) && !c.IsAbstract && !c.IsInterface)
                            arrayList1.Add(c);
                        else if (c.IsAssignableFrom(parentType) && !c.IsAbstract && !c.IsInterface)
                            arrayList1.Add(c);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return (Type[]) arrayList1.ToArray(typeof (Type));
        }

        public static Hashtable GetFieldsByAttribute(Type objectType, Type attributeType)
        {
            try
            {
                var hashtable = new Hashtable();
                foreach (var fieldInfo in objectType.GetFields())
                {
                    var customAttributes = fieldInfo.GetCustomAttributes(attributeType, true);
                    if (customAttributes.Length > 0)
                        hashtable.Add(fieldInfo, customAttributes[0]);
                }
                return hashtable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Hashtable GetFieldsByAttribute(object o, Type attributeType)
        {
            try
            {
                var type = o.GetType();
                var hashtable = new Hashtable();
                foreach (var fieldInfo in type.GetFields())
                {
                    var customAttributes = fieldInfo.GetCustomAttributes(attributeType, true);
                    if (customAttributes.Length > 0)
                        hashtable.Add(fieldInfo, customAttributes[0]);
                }
                return hashtable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Hashtable GetPropertiesByAttribute(Type objectType, Type attributeType)
        {
            try
            {
                var hashtable = new Hashtable();
                foreach (var propertyInfo in objectType.GetProperties())
                {
                    var customAttributes = propertyInfo.GetCustomAttributes(attributeType, true);
                    if (customAttributes.Length > 0)
                        hashtable.Add(propertyInfo, customAttributes[0]);
                }
                return hashtable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Hashtable GetPropertiesByAttribute(object o, Type attributeType)
        {
            return GetPropertiesByAttribute(o.GetType(), attributeType);
        }

        public static Hashtable GetMethodsByAttribute(Type parentType, Type attributeType)
        {
            try
            {
                var hashtable = new Hashtable();
                foreach (var methodInfo in parentType.GetMethods())
                {
                    var customAttributes = methodInfo.GetCustomAttributes(attributeType, true);
                    if (customAttributes.Length > 0)
                        hashtable.Add(customAttributes[0], methodInfo);
                }
                return hashtable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Attribute GetSingleAttribute(Type attributeType, object instance)
        {
            try
            {
                var hashtable = new Hashtable();
                var attributeArray = (Attribute[]) instance.GetType().GetCustomAttributes(attributeType, true);
                if (attributeArray.Length > 0)
                    return attributeArray[0];
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Attribute GetSingleAttribute(Type attributeType, Type type)
        {
            try
            {
                var hashtable = new Hashtable();
                var attributeArray = (Attribute[]) type.GetCustomAttributes(attributeType, true);
                if (attributeArray.Length > 0)
                    return attributeArray[0];
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Attribute[] GetAttributes(Type attributeType, object instance)
        {
            try
            {
                var hashtable = new Hashtable();
                return (Attribute[]) instance.GetType().GetCustomAttributes(attributeType, true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool IsTypeOf(Type checkType, string baseType)
        {
            if (checkType == null)
                return false;
            while (checkType != null && checkType.ToString() != baseType)
                checkType = checkType.BaseType;
            return checkType != null;
        }

        public static bool IsTypeOf(Type checkType, Type baseType)
        {
            return IsTypeOf(checkType, baseType.ToString());
        }

        public static void DisplayProperties(object o)
        {
            foreach (MemberInfo memberInfo in o.GetType().GetProperties())
                Debug.WriteLine(memberInfo.Name);
        }

        public static void DisplayObjectTypes(object o)
        {
            foreach (object obj in o.GetType().GetNestedTypes())
                Debug.WriteLine(obj.ToString());
            foreach (object obj in o.GetType().GetInterfaces())
                Debug.WriteLine(obj.ToString());
            foreach (MemberInfo memberInfo in o.GetType().GetMethods())
                Debug.WriteLine(memberInfo.Name);
        }

        public static bool ImplementsInterface(object o, Type interfaceType)
        {
            foreach (var type in o.GetType().GetInterfaces())
            {
                if (type == interfaceType)
                    return true;
            }
            return false;
        }

        public static bool ImplementsInterface(Type objectType, Type interfaceType)
        {
            foreach (var type in objectType.GetInterfaces())
            {
                if (type == interfaceType ||
                    type.Name.Contains("`") &&
                    (type.Name == interfaceType.Name && type.Assembly == interfaceType.Assembly))
                    return true;
            }
            return false;
        }

        public static IEnumerable<Type> GetCreatableObjectByAttribute(Assembly assembly, Type attributeType)
        {
            var list = new List<Type>();
            try
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (((Attribute[]) type.GetCustomAttributes(attributeType, true)).Length > 0)
                        list.Add(type);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return list;
        }

        public static Type[] GetCreatableObjectImplementsInterface(Type interfaceType, Assembly assembly)
        {
            var arrayList = new ArrayList();
            try
            {
                foreach (var objectType in assembly.GetTypes())
                {
                    if (ImplementsInterface(objectType, interfaceType) && !objectType.IsAbstract &&
                        !objectType.IsInterface)
                        arrayList.Add(objectType);
                }
            }
            catch (Exception ex)
            {
                MessageLog.LogError(LogClass.CommonUtil, ex, "Could not load types for assembly: " + assembly.FullName);
            }
            return (Type[]) arrayList.ToArray(typeof (Type));
        }

        public static Type[] GetCreatableObjectImplementsInterface(Type interfaceType, string path)
        {
            var arrayList1 = new ArrayList();
            if (File.Exists(path))
            {
                try
                {
                    arrayList1.Add(Assembly.LoadFile(path));
                }
                catch (BadImageFormatException ex)
                {
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            else
                arrayList1 = LoadAllAssembliesForPath(StringHelper.EnsureDirectorySeperatorAtEnd(path));
            var arrayList2 = new ArrayList();
            try
            {
                foreach (Assembly assembly in arrayList1)
                {
                    try
                    {
                        foreach (var objectType in assembly.GetTypes())
                        {
                            if (ImplementsInterface(objectType, interfaceType) && !objectType.IsAbstract &&
                                !objectType.IsInterface)
                                arrayList2.Add(objectType);
                        }
                    }
                    catch (ReflectionTypeLoadException ex)
                    {
                        MessageLog.LogError(LogClass.CommonUtil, ex,
                            "Could not load types for assembly: " + assembly.FullName);
                        foreach (var exception in ex.LoaderExceptions)
                            MessageLog.LogError(LogClass.CommonUtil, ex, "Loader Exception: " + exception.Message);
                    }
                    catch (Exception ex)
                    {
                        MessageLog.LogError(LogClass.CommonUtil, ex,
                            "Could not load types for assembly: " + assembly.FullName);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return (Type[]) arrayList2.ToArray(typeof (Type));
        }
    }
}