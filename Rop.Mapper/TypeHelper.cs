using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Rop.Mapper
{
    public static class TypeHelper
    {
        public static bool CanBeNullable(this PropertyInfo pinfo)
        {
            return (!pinfo.PropertyType.IsValueType && pinfo.GetCustomAttributes().Any(a => a.GetType().Name.Contains("NullableAttribute")))
                   || (Nullable.GetUnderlyingType(pinfo.PropertyType) != null);
        }
        public static object? GetDefaultValue(this Type t)
        {
            if (t.IsValueType && Nullable.GetUnderlyingType(t) == null)
                return Activator.CreateInstance(t);
            else
                return null;
        }

        public static Type? HasGenericInterface(this Type type, Type genericinterface)
        {
            var ie=type.GetInterfaces()
                .FirstOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == genericinterface);
            return ie?.GetGenericArguments()[0];
        }

        public static bool IsIEnumerableOfT(this Type type,out Type? subtype)
        {
            subtype = type.HasGenericInterface(typeof(IEnumerable<>));
            return subtype != null;
        }
        public static bool IsIListOfT(this Type type, out Type? subtype)
        {
            subtype = type.HasGenericInterface(typeof(IList<>));
            return subtype != null;
        }
        private static List<T> _createList<T>(IEnumerable<T> origen)=>origen.ToList();
        private static T[] _createArray<T>(IEnumerable<T> origen)=>origen.ToArray<T>();
        private static readonly MethodInfo _createListMethod;
        private static readonly MethodInfo _createArrayMethod;
        static TypeHelper()
        {
            _createListMethod =
                typeof(TypeHelper).GetMethod(nameof(_createList), BindingFlags.NonPublic | BindingFlags.Static)!;
            _createArrayMethod =
                typeof(TypeHelper).GetMethod(nameof(_createArray), BindingFlags.NonPublic | BindingFlags.Static)!;

        }
        public static object CreateList(IEnumerable origen,Type destino)
        {
            var generic = _createListMethod.MakeGenericMethod(destino);
            return generic.Invoke(null,new []{origen})!;
        }
        public static object CreateArray(IEnumerable origen, Type destino)
        {
            var generic = _createArrayMethod.MakeGenericMethod(destino);
            return generic.Invoke(null, new[] { origen })!;
        }
        

    }
}
