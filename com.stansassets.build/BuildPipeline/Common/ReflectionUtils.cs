using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace StansAssets.Build.Pipeline
{
    //TODO: move certain methods to the com.stansassets.foundation package
    public static class ReflectionUtils
    {
        static readonly string[] s_BuiltInAssemblyPrefixes = { "Mono.", "Unity.", "UnityEngine", "UnityEditor", "System", "mscorlib" };

        public static IEnumerable<Type> FindImplementationsOf<T>(bool ignoreBuiltIn = false)
        {
            var baseType = typeof(T);
            return FindImplementationsOf(baseType, ignoreBuiltIn);
        }

        public static IEnumerable<Type> FindImplementationsOf(Type baseType, bool ignoreBuiltIn = false)
        {
            var assemblies = GetAssembles(ignoreBuiltIn);

            return assemblies
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => baseType.IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract);
        }

        public static IEnumerable<MethodInfo> FindMethodsWithAttributes<T>(bool ignoreBuiltIn = false)
        {
            var assemblies = GetAssembles(ignoreBuiltIn);

            return assemblies
                .SelectMany(assembly => assembly.GetTypes())
                .SelectMany(type => type.GetMethods())
                .Where(methodInfo => methodInfo.GetCustomAttributes(typeof(T), false).Length > 0);

        }

        public static bool HasDefaultConstructor(Type type)
        {
            return type.GetConstructors().Any(constructor => !constructor.GetParameters().Any());
        }

        static IEnumerable<Assembly> GetAssembles(bool ignoreBuiltIn = false)
        {
            IEnumerable<Assembly> assemblies = AppDomain.CurrentDomain.GetAssemblies();

            if (ignoreBuiltIn)
            {
                assemblies = assemblies.Where(assembly => {
                    var assemblyName = assembly.GetName().Name;
                    return !s_BuiltInAssemblyPrefixes.Any(prefix => assemblyName.StartsWith(prefix));
                });
            }

            return assemblies;
        }

    }
}
