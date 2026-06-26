using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace task05;

public class ClassAnalyzer
{
    private Type _type;

    public ClassAnalyzer(Type type)
    {
        _type = type;
    }

    public IEnumerable<string> GetPublicMethods()
    {
        return _type.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                    .Select(m => m.Name)
                    .Distinct();
    }

    public IEnumerable<string> GetMethodParams(string methodName)
    {
        MethodInfo method = _type.GetMethod(methodName, BindingFlags.Public | BindingFlags.Instance);

        if (method == null)
            return Enumerable.Empty<string>();

        List<string> result = new List<string>();
        result.Add($"ReturnType: {method.ReturnType.Name}");

        ParameterInfo[] parameters = method.GetParameters();

        if (parameters.Length == 0)
        {
            result.Add("No parameters");
        }
        else
        {
            foreach (ParameterInfo param in parameters)
            {
                result.Add($"{param.ParameterType.Name} {param.Name}");
            }
        }

        return result;
    }

    public IEnumerable<string> GetAllFields()
    {
        return _type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                    .Select(f => f.Name);
    }

    public IEnumerable<string> GetProperties()
    {
        return _type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Select(p => p.Name);
    }

    public bool HasAttribute<T>() where T : Attribute
    {
        object[] attributes = _type.GetCustomAttributes(typeof(T), true)!;
        return attributes.Length > 0;
    }
}
