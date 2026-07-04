using System;
using System.Text;
using System.Reflection;

namespace task07;

public static class ReflectionHelper
{
    public static string PrintTypeInfo(Type type)
    {
        var sb = new StringBuilder();

        var displayName = type.GetCustomAttribute<DisplayNameAttribute>();
        sb.AppendLine(displayName != null
            ? $"Class DisplayName: {displayName.DisplayName}"
            : "Class DisplayName: not set");

        var version = type.GetCustomAttribute<VersionAttribute>();
        sb.AppendLine(version != null
            ? $"Version: {version.Major}.{version.Minor}"
            : "Version: not set");

        sb.AppendLine("Methods:");
        var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        foreach (var method in methods)
        {
            var name = method.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName;
            sb.AppendLine($"  {method.Name} - DisplayName: {name ?? "not set"}");
        }

        sb.AppendLine("Properties:");
        var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        foreach (var prop in props)
        {
            var name = prop.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName;
            sb.AppendLine($"  {prop.Name} - DisplayName: {name ?? "not set"}");
        }

        return sb.ToString();
    }
}
