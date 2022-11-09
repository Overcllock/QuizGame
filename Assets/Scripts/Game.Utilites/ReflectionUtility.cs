using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace Game.Utilities
{
    public static class ReflectionUtility
    {
        public static bool editorMode;

        private static readonly List<string> _allowedAssemblyTags = new List<string>
        {
            "CSharp",
            "Game"
        };

        public static Dictionary<Type, T> InstantiateAllTypesMap<T>(Action<T> activator = null) where T : class
        {
            List<Type> retrievedTypes = GetAllTypes<T>();
            Dictionary<Type, T> map = new Dictionary<Type, T>(retrievedTypes.Count);

            for (int i = 0; i < retrievedTypes.Count; i++)
            {
                Type type = retrievedTypes[i];
                if (TryCreateInstance(type, out T instance))
                {
                    map[type] = instance;
                    if (activator != null)
                    {
                        activator.Invoke(instance);
                    }
                }
            }

            return map;
        }

        public static List<T> InstantiateAllTypes<T>(Action<T> activator = null) where T : class
        {
            List<Type> retrievedTypes = GetAllTypes<T>();
            List<T> instantiated = new List<T>(retrievedTypes.Count);

            for (int i = 0; i < retrievedTypes.Count; i++)
            {
                Type type = retrievedTypes[i];
                if (TryCreateInstance(type, out T instance))
                {
                    instantiated.Add(instance);
                    if (activator != null)
                    {
                        activator.Invoke(instance);
                    }
                }
            }

            return instantiated;
        }

        public static List<Type> GetAllTypes<T>()
        {
            List<Type> list = new List<Type>();
            Type neededType = typeof(T);
            foreach (Assembly assembly in GetAllowedAssemblies())
            {
                foreach (Type type in assembly.GetTypes())
                {
                    if (neededType.IsAssignableFrom(type) &&
                        !type.IsInterface &&
                        !type.IsAbstract)
                    {
                        list.Add(type);
                    }
                }
            }

            return list;
        }

        public static IEnumerable<Assembly> GetAllowedAssemblies()
        {
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (_allowedAssemblyTags.Any(assembly.FullName.Contains))
                {
                    if (editorMode || !assembly.FullName.Contains("Editor")) yield return assembly;
                }
            }
        }

        public static bool TryCreateInstance<T>(Type type, out T instance)
        {
            try
            {
                instance = (T)Activator.CreateInstance(type);
                return true;
            }
            catch
            {
#if CLIENT
                UnityEngine.Debug.LogError($"Unable to create instance of [{type.Name}]");
#endif

                instance = default;
                return false;
            }
        }
    }
}