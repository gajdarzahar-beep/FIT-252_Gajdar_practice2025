using Xunit;
using task10;
using task10.Plugins;
using System;
using System.Reflection;
using System.Linq;

namespace task10tests;

public class PluginLoaderTests
{
    [Fact]
    public void PluginLoadAttribute_ShouldStoreDependencies()
    {
        var attr = new PluginLoadAttribute("PluginA", "PluginB");
        Assert.Equal(2, attr.Dependencies.Length);
        Assert.Contains("PluginA", attr.Dependencies);
        Assert.Contains("PluginB", attr.Dependencies);
    }

    [Fact]
    public void PluginA_ShouldHaveNoDependencies()
    {
        var attr = typeof(PluginA).GetCustomAttribute<PluginLoadAttribute>();
        Assert.NotNull(attr);
        Assert.Empty(attr.Dependencies);
    }

    [Fact]
    public void PluginB_ShouldDependOnPluginA()
    {
        var attr = typeof(PluginB).GetCustomAttribute<PluginLoadAttribute>();
        Assert.NotNull(attr);
        Assert.Contains("PluginA", attr.Dependencies);
    }

    [Fact]
    public void PluginC_ShouldDependOnPluginB()
    {
        var attr = typeof(PluginC).GetCustomAttribute<PluginLoadAttribute>();
        Assert.NotNull(attr);
        Assert.Contains("PluginB", attr.Dependencies);
    }

    [Fact]
    public void PluginA_Execute_ShouldNotThrow()
    {
        var plugin = new PluginA();
        var exception = Record.Exception(() => plugin.Execute());
        Assert.Null(exception);
    }

    [Fact]
    public void LoadPlugins_FromDirectory_ShouldLoadAllPlugins()
    {
        string pluginDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "TestPlugins");
        pluginDir = Path.GetFullPath(pluginDir);

        var plugins = PluginLoader.LoadPlugins(pluginDir);
        Assert.Equal(3, plugins.Count);
    }
}