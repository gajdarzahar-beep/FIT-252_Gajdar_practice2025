using Xunit;
using task03;

public class CustomCollectionTests
{
    [Fact]
    public void CustomCollection_GetEnumerator_ReturnsAllItems()
    {
        var collection = new CustomCollection<int>();
        collection.Add(1);
        collection.Add(2);

        var result = new System.Collections.Generic.List<int>();
        foreach (var item in collection)
        {
            result.Add(item);
        }

        Assert.Equal(new[] { 1, 2 }, result);
    }

    [Fact]
    public void GetReverseEnumerator_ReturnsItemsInReverseOrder()
    {
        var collection = new CustomCollection<int>();
        collection.Add(1);
        collection.Add(2);
        collection.Add(3);

        var result = collection.GetReverseEnumerator();
        var list = new System.Collections.Generic.List<int>();
        foreach (var item in result)
        {
            list.Add(item);
        }

        Assert.Equal(new[] { 3, 2, 1 }, list);
    }

    [Fact]
    public void GenerateSequence_ReturnsCorrectSequence()
    {
        var sequence = CustomCollection<int>.GenerateSequence(5, 3);
        var list = new System.Collections.Generic.List<int>();
        foreach (var item in sequence)
        {
            list.Add(item);
        }

        Assert.Equal(new[] { 5, 6, 7 }, list);
    }

    [Fact]
    public void FilterAndSort_ReturnsFilteredAndSortedItems()
    {
        var collection = new CustomCollection<int>();
        collection.Add(3);
        collection.Add(1);
        collection.Add(2);

        var result = collection.FilterAndSort(x => x > 1, x => x);
        var list = new System.Collections.Generic.List<int>();
        foreach (var item in result)
        {
            list.Add(item);
        }

        Assert.Equal(new[] { 2, 3 }, list);
    }
}