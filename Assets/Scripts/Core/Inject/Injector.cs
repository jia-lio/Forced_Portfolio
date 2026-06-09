using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Nullbytes
{
    public static class Injector
    {
        private static readonly Dictionary<Type, FieldInfo[]> cachedInjectFields = new();

        public static void Inject(MonoBehaviour target, ManagerContainer container)
        {
            if (target == null || container == null)
                return;

            var targetType = target.GetType();
            if (cachedInjectFields.TryGetValue(targetType, out var fields) == false)
            {
                fields = GetAllInjectFields(targetType);
                cachedInjectFields[targetType] = fields;
            }

            foreach (var field in fields)
            {
                var dependency = container.Resolve(field.FieldType);
                if (dependency != null)
                {
                    field.SetValue(target, dependency);
                }
                else
                {
                    Logger.Log(null, LogType.Error, $"[Injector] Failed to resolve {field.FieldType.Name} for {targetType.Name}");
                }
            }
        }

        private static FieldInfo[] GetAllInjectFields(Type type)
        {
            List<FieldInfo> injectFields = new();

            while (type != null && type != typeof(object))
            {
                var fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                                 .Where(field => Attribute.IsDefined(field, typeof(InjectAttribute)));

                injectFields.AddRange(fields);
                type = type.BaseType;
            }

            return injectFields.ToArray();
        }
    }
}