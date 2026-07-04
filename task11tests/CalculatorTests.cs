using Xunit;
using task11;

namespace task11tests;

public class CalculatorTests
{
    [Fact]
    public void GeneratedCalculator_Add_ShouldReturnSum()
    {
        var calculator = ClassGenerator.CreateCalculator();
        var result = calculator.GetType().GetMethod("Add").Invoke(calculator, new object[] { 5, 3 });
        Assert.Equal(8, result);
    }

    [Fact]
    public void GeneratedCalculator_Minus_ShouldReturnDifference()
    {
        var calculator = ClassGenerator.CreateCalculator();
        var result = calculator.GetType().GetMethod("Minus").Invoke(calculator, new object[] { 10, 4 });
        Assert.Equal(6, result);
    }

    [Fact]
    public void GeneratedCalculator_Mul_ShouldReturnProduct()
    {
        var calculator = ClassGenerator.CreateCalculator();
        var result = calculator.GetType().GetMethod("Mul").Invoke(calculator, new object[] { 7, 6 });
        Assert.Equal(42, result);
    }

    [Fact]
    public void GeneratedCalculator_Div_ShouldReturnQuotient()
    {
        var calculator = ClassGenerator.CreateCalculator();
        var result = calculator.GetType().GetMethod("Div").Invoke(calculator, new object[] { 15, 3 });
        Assert.Equal(5, result);
    }
}