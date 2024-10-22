// <authors> [Ethan Perkins] </authors>
// <date> [9/13/2024] </date>

namespace CS3500.DevelopmentTests;

using CS3500.DependencyGraph;

/// <summary>
/// This is a test class for DependencyGraph and is intended
/// to contain all DependencyGraph Unit Tests
/// </summary>
[TestClass]
public class DependencyGraphTests
{
    //Proper Construction Set up

    /// <summary>
    /// Tests whether the initial size of a new graph is 0 (there are no elements in the graph)
    /// </summary>
    [TestMethod]
    public void Constructor_NewDependencyGraph_Valid()
    {
        DependencyGraph graph = new DependencyGraph();
        Assert.AreEqual(graph.Size, 0);
    }

    //Size tests

    /// <summary>
    /// Tests whether size increases when a dependency is added.
    /// </summary>
    [TestMethod]
    public void Size_DependencyAdd_Valid()
    {
        DependencyGraph graph = new DependencyGraph();
        graph.AddDependency("a", "b");
        Assert.AreEqual(graph.Size, 1);
    }

    /// <summary>
    /// Tests whether size increases with multiple distinct pairs.
    /// </summary>
    [TestMethod]
    public void Size_DependencyAddDifferent_Valid()
    {
        DependencyGraph graph = new DependencyGraph();
        graph.AddDependency("a", "b");
        graph.AddDependency("c", "d");
        Assert.AreEqual(graph.Size, 2);
    }

    /// <summary>
    /// Tests whether size increases with pairs of the same key.
    /// </summary>
    [TestMethod]
    public void Size_DependencyAddDifferentSameKey_Valid()
    {
        DependencyGraph graph = new DependencyGraph();
        graph.AddDependency("a", "b");
        graph.AddDependency("a", "c");
        Assert.AreEqual(graph.Size, 2);
    }

    /// <summary>
    /// Tests whether size increases with pairs of the same value.
    /// </summary>
    [TestMethod]
    public void Size_DependencyAddDifferentSameValue_Valid()
    {
        DependencyGraph graph = new DependencyGraph();
        graph.AddDependency("a", "b");
        graph.AddDependency("c", "b");
        Assert.AreEqual(graph.Size, 2);
    }

    /// <summary>
    /// Tests whether size does NOT increase with duplicate pairs.
    /// </summary>
    [TestMethod]
    public void Size_DependencyAddDuplicate_Valid()
    {
        DependencyGraph graph = new DependencyGraph();
        graph.AddDependency("a", "b");
        graph.AddDependency("a", "b");
        Assert.AreEqual(graph.Size, 1);
    }

    /// <summary>
    /// Tests whether size decreases when removing a pair.
    /// </summary>
    [TestMethod]
    public void Size_DependencyToEmpty_Valid()
    {
        DependencyGraph graph = new DependencyGraph();
        graph.AddDependency("a", "b");
        graph.RemoveDependency("a", "b");
        Assert.AreEqual(graph.Size, 0);
    }

    /// <summary>
    /// Tests whether size does not go decrease when a non-existent pair is "removed".
    /// </summary>
    [TestMethod]
    public void Size_NotGoToNegative_Valid()
    {
        DependencyGraph graph = new DependencyGraph();
        graph.RemoveDependency("a", "b");
        Assert.AreEqual(graph.Size, 0);
    }

    //HasDependents & HasDependees tests

    /// <summary>
    /// Tests that HasDependents and HasDependees returns false for an empty graph.
    /// </summary>
    [TestMethod]
    public void HasMethods_Empty_Valid()
    {
        DependencyGraph graph = new DependencyGraph();
        Assert.IsFalse(graph.HasDependents("a"));
        Assert.IsFalse(graph.HasDependees("a"));
    }

    /// <summary>
    /// Tests proper return values for a graph of one pair.
    /// </summary>
    [TestMethod]
    public void HasMethods_OnePair_Valid()
    {
        DependencyGraph graph = new DependencyGraph();
        graph.AddDependency("a", "b");
        Assert.IsTrue(graph.HasDependents("a"));
        Assert.IsTrue(graph.HasDependees("b"));
    }

    /// <summary>
    /// Tests that the opposite relationship has not been mistakenly added/seen in the graph.
    /// </summary>
    [TestMethod]
    public void HasMethods_OnePairOpposite_Valid()
    {
        DependencyGraph graph = new DependencyGraph();
        graph.AddDependency("a", "b");
        Assert.IsFalse(graph.HasDependees("a"));
        Assert.IsFalse(graph.HasDependents("b"));
    }

    /// <summary>
    /// Tests that dependencies are properly removed.
    /// </summary>
    [TestMethod]
    public void HasMethods_RemovePair_Valid()
    {
        DependencyGraph graph = new DependencyGraph();
        graph.AddDependency("a", "b");
        graph.RemoveDependency("a", "b");
        Assert.IsFalse(graph.HasDependents("a"));
        Assert.IsFalse(graph.HasDependees("b"));
    }

    /// <summary>
    /// Tests that dependencies are properly removed even after a duplicate addition.
    /// </summary>
    [TestMethod]
    public void HasMethods_AddTwoRemoveOnePair_Valid()
    {
        DependencyGraph graph = new DependencyGraph();
        graph.AddDependency("a", "b");
        graph.AddDependency("a", "b");
        graph.RemoveDependency("a", "b");
        Assert.IsFalse(graph.HasDependents("a"));
        Assert.IsFalse(graph.HasDependees("b"));
    }

    /// <summary>
    /// Tests HasDependees/HasDependents with multiple pairs of the same key.
    /// </summary>
    [TestMethod]
    public void HasMethods_MultiplePairsSameKey_Valid()
    {
        DependencyGraph graph = new DependencyGraph();
        graph.AddDependency("a", "b");
        graph.AddDependency("a", "c");
        Assert.IsTrue(graph.HasDependents("a"));
        Assert.IsTrue(graph.HasDependees("b"));
        Assert.IsTrue(graph.HasDependees("c"));
    }

    /// <summary>
    /// Tests HasDependees/HasDependents with multiple pairs of the same value.
    /// </summary>
    [TestMethod]
    public void HasMethods_MultiplePairsSameValue_Valid()
    {
        DependencyGraph graph = new DependencyGraph();
        graph.AddDependency("a", "b");
        graph.AddDependency("c", "b");
        Assert.IsTrue(graph.HasDependents("a"));
        Assert.IsTrue(graph.HasDependents("c"));
        Assert.IsTrue(graph.HasDependees("b"));
    }

    /// <summary>
    /// Tests HasDependees/HasDependents after the removal of one pair with similar keys.
    /// </summary>
    [TestMethod]
    public void HasMethods_MultiplePairsSameKeyRemovePair_Valid()
    {
        DependencyGraph graph = new DependencyGraph();
        graph.AddDependency("a", "b");
        graph.AddDependency("a", "c");
        graph.RemoveDependency("a", "c");
        Assert.IsTrue(graph.HasDependents("a"));
        Assert.IsTrue(graph.HasDependees("b"));
        Assert.IsFalse(graph.HasDependees("c"));
    }

    /// <summary>
    /// Tests HasDependees/HasDependents after the removal of one pair with similar values.
    /// </summary>
    [TestMethod]
    public void HasMethods_MultiplePairsSameValueRemovePair_Valid()
    {
        DependencyGraph graph = new DependencyGraph();
        graph.AddDependency("a", "b");
        graph.AddDependency("c", "b");
        graph.RemoveDependency("c", "b");
        Assert.IsTrue(graph.HasDependents("a"));
        Assert.IsFalse(graph.HasDependents("c"));
        Assert.IsTrue(graph.HasDependees("b"));
    }

    //GetDependents & GetDependees tests

    /// <summary>
    /// Tests GetDependees/GetDependents with an empty graph.
    /// </summary>
    [TestMethod]
    public void GetMethods_Empty_Valid()
    {
        DependencyGraph graph = new DependencyGraph();
        Assert.AreEqual(graph.GetDependents("a").Count(), 0);
        Assert.AreEqual(graph.GetDependees("a").Count(), 0);
    }

    /// <summary>
    /// Tests GetDependees/GetDependents with a graph with one pair.
    /// </summary>
    [TestMethod]
    public void GetMethods_OnePair_Valid()
    {
        DependencyGraph graph = new DependencyGraph();
        graph.AddDependency("a", "b");
        Assert.IsTrue(graph.GetDependents("a").Contains("b"));
        Assert.IsTrue(graph.GetDependees("b").Contains("a"));
    }

    /// <summary>
    /// Tests GetDependees/GetDependents does not recognize the opposite dependency.
    /// </summary>
    [TestMethod]
    public void GetMethods_OnePairOpposite_Valid()
    {
        DependencyGraph graph = new DependencyGraph();
        graph.AddDependency("a", "b");
        Assert.IsFalse(graph.GetDependees("a").Contains("b"));
        Assert.IsFalse(graph.GetDependents("b").Contains("a"));
    }

    /// <summary>
    /// Tests GetDependees/GetDependents after the removal of a pair.
    /// </summary>
    [TestMethod]
    public void GetMethods_RemovePair_Valid()
    {
        DependencyGraph graph = new DependencyGraph();
        graph.AddDependency("a", "b");
        graph.RemoveDependency("a", "b");
        Assert.IsFalse(graph.GetDependents("a").Contains("b"));
        Assert.IsFalse(graph.GetDependees("b").Contains("a"));
    }

    /// <summary>
    /// Tests GetDependees/GetDependents after the removal of a pair even after a duplicate insertion.
    /// </summary>
    [TestMethod]
    public void GetMethods_AddTwoRemoveOnePair_Valid()
    {
        DependencyGraph graph = new DependencyGraph();
        graph.AddDependency("a", "b");
        graph.AddDependency("a", "b");
        graph.RemoveDependency("a", "b");
        Assert.IsFalse(graph.GetDependents("a").Contains("b"));
        Assert.IsFalse(graph.GetDependees("b").Contains("a"));
    }

    /// <summary>
    /// Tests GetDependees/GetDependents with pairs with similar keys.
    /// </summary>
    [TestMethod]
    public void GetMethods_MultiplePairsSameKey_Valid()
    {
        DependencyGraph graph = new DependencyGraph();
        graph.AddDependency("a", "b");
        graph.AddDependency("a", "c");
        Assert.IsTrue(graph.GetDependents("a").Contains("b"));
        Assert.IsTrue(graph.GetDependents("a").Contains("c"));
        Assert.IsTrue(graph.GetDependees("b").Contains("a"));
        Assert.IsTrue(graph.GetDependees("c").Contains("a"));
    }

    /// <summary>
    /// Tests GetDependees/GetDependents with pairs with similar values.
    /// </summary>
    [TestMethod]
    public void GetMethods_MultiplePairsSameValue_Valid()
    {
        DependencyGraph graph = new DependencyGraph();
        graph.AddDependency("a", "b");
        graph.AddDependency("c", "b");
        Assert.IsTrue(graph.GetDependents("a").Contains("b"));
        Assert.IsTrue(graph.GetDependees("b").Contains("a"));
        Assert.IsTrue(graph.GetDependees("b").Contains("c"));
        Assert.IsTrue(graph.GetDependents("c").Contains("b"));
    }

    /// <summary>
    /// Tests GetDependees/GetDependents after removal of a pair with similar keys.
    /// </summary>
    [TestMethod]
    public void GetMethods_MultiplePairsSameKeyRemovePair_Valid()
    {
        DependencyGraph graph = new DependencyGraph();
        graph.AddDependency("a", "b");
        graph.AddDependency("a", "c");
        graph.RemoveDependency("a", "c");
        Assert.IsTrue(graph.GetDependents("a").Contains("b"));
        Assert.IsTrue(graph.GetDependees("b").Contains("a"));
        Assert.IsFalse(graph.GetDependents("a").Contains("c"));
        Assert.IsFalse(graph.GetDependees("c").Contains("a"));
    }

    /// <summary>
    /// Tests GetDependees/GetDependents after removal of a pair with similar values.
    /// </summary>
    [TestMethod]
    public void GetMethods_MultiplePairsSameValueRemovePair_Valid()
    {
        DependencyGraph graph = new DependencyGraph();
        graph.AddDependency("a", "b");
        graph.AddDependency("c", "b");
        graph.RemoveDependency("c", "b");
        Assert.IsTrue(graph.GetDependents("a").Contains("b"));
        Assert.IsTrue(graph.GetDependees("b").Contains("a"));
        Assert.IsFalse(graph.GetDependents("c").Contains("b"));
        Assert.IsFalse(graph.GetDependees("b").Contains("c"));
    }

    //AddDependency tests (Most tests for this method have already been done above, so to remove redunancy many are not listed here.)

    /// <summary>
    /// Tests whether the addition of self-loops is valid.
    /// </summary>
    [TestMethod]
    public void AddDependency_SelfLoopPair_Valid()
    {
        DependencyGraph graph = new DependencyGraph();
        graph.AddDependency("a", "a");
        Assert.IsTrue(graph.GetDependents("a").Contains("a"));
        Assert.IsTrue(graph.GetDependees("a").Contains("a"));
        Assert.AreEqual(graph.Size, 1);
    }

    /// <summary>
    /// Tests the addition of multiple duplicate pairs.
    /// </summary>
    [TestMethod]
    public void AddDependency_MultipleDuplicatePairs_Valid()
    {
        DependencyGraph graph = new DependencyGraph();
        graph.AddDependency("a", "b");
        graph.AddDependency("a", "b");
        graph.AddDependency("b", "c");
        graph.AddDependency("b", "c");
        Assert.IsTrue(graph.GetDependents("a").Contains("b"));
        Assert.IsTrue(graph.GetDependees("b").Contains("a"));
        Assert.IsTrue(graph.GetDependents("b").Contains("c"));
        Assert.IsTrue(graph.GetDependees("c").Contains("b"));
        Assert.AreEqual(graph.Size, 2);
    }

    /// <summary>
    /// Tests the addition of a cycle.
    /// </summary>
    [TestMethod]
    public void AddDependency_Cycles_Valid()
    {
        DependencyGraph graph = new DependencyGraph();
        graph.AddDependency("a", "b");
        graph.AddDependency("b", "c");
        graph.AddDependency("c", "a");
        Assert.IsTrue(graph.GetDependents("a").Contains("b"));
        Assert.IsTrue(graph.GetDependees("b").Contains("a"));
        Assert.IsTrue(graph.GetDependents("b").Contains("c"));
        Assert.IsTrue(graph.GetDependees("c").Contains("b"));
        Assert.IsTrue(graph.GetDependents("c").Contains("a"));
        Assert.IsTrue(graph.GetDependees("a").Contains("c"));
        Assert.AreEqual(graph.Size, 3);
    }

    /// <summary>
    /// Tests the addition of ten pairs.
    /// </summary>
    [TestMethod]
    public void AddDependency_TenPairs_Valid()
    {
        DependencyGraph graph = new DependencyGraph();
        graph.AddDependency("a", "b");
        graph.AddDependency("a", "c");
        graph.AddDependency("b", "c");
        graph.AddDependency("b", "d");
        graph.AddDependency("d", "e");
        graph.AddDependency("e", "c");
        graph.AddDependency("f", "e");
        graph.AddDependency("f", "g");
        graph.AddDependency("g", "e");
        graph.AddDependency("g", "a");
        Assert.IsTrue(graph.GetDependents("a").Contains("b"));
        Assert.IsTrue(graph.GetDependents("a").Contains("c"));
        Assert.IsTrue(graph.GetDependents("b").Contains("c"));
        Assert.IsTrue(graph.GetDependents("b").Contains("d"));
        Assert.IsTrue(graph.GetDependents("d").Contains("e"));
        Assert.IsTrue(graph.GetDependents("e").Contains("c"));
        Assert.IsTrue(graph.GetDependents("f").Contains("e"));
        Assert.IsTrue(graph.GetDependents("f").Contains("g"));
        Assert.IsTrue(graph.GetDependents("g").Contains("e"));
        Assert.IsTrue(graph.GetDependents("g").Contains("a"));
        Assert.IsTrue(graph.GetDependees("a").Contains("g"));
        Assert.IsTrue(graph.GetDependees("b").Contains("a"));
        Assert.IsTrue(graph.GetDependees("c").Contains("a"));
        Assert.IsTrue(graph.GetDependees("c").Contains("b"));
        Assert.IsTrue(graph.GetDependees("c").Contains("e"));
        Assert.IsTrue(graph.GetDependees("d").Contains("b"));
        Assert.IsTrue(graph.GetDependees("e").Contains("d"));
        Assert.IsTrue(graph.GetDependees("e").Contains("f"));
        Assert.IsTrue(graph.GetDependees("e").Contains("g"));
        Assert.IsTrue(graph.GetDependees("g").Contains("f"));
        Assert.AreEqual(graph.Size, 10);
    }

    //RemoveDependency tests (Most tests for this method have already been done above, so to remove redunancy many are not listed here.)

    /// <summary>
    /// Tests the removal of a pair from an empty graph.
    /// </summary>
    [TestMethod]
    public void RemoveDependency_EmptyGraph_Valid()
    {
        DependencyGraph graph = new DependencyGraph();
        Assert.AreEqual(0, graph.Size);
        graph.RemoveDependency("a", "b");
        Assert.AreEqual(0, graph.Size);
    }

    /// <summary>
    /// Tests the removal of the same pair twice.
    /// </summary>
    [TestMethod]
    public void RemoveDependency_RemoveTwiceToEmpty_Valid()
    {
        DependencyGraph graph = new DependencyGraph();
        graph.AddDependency("a", "b");
        Assert.AreEqual(1, graph.Size);
        graph.RemoveDependency("a", "b");
        graph.RemoveDependency("a", "b");
        Assert.AreEqual(0, graph.Size);
    }

    /// <summary>
    /// Tests the removal of the same pair twice (where other pairs exist in the graph).
    /// </summary>
    [TestMethod]
    public void RemoveDependency_RemoveTwiceToNonempty_Valid()
    {
        DependencyGraph graph = new DependencyGraph();
        graph.AddDependency("a", "b");        
        graph.AddDependency("a", "c");
        Assert.AreEqual(2, graph.Size);
        graph.RemoveDependency("a", "b");
        graph.RemoveDependency("a", "b");
        Assert.AreEqual(1, graph.Size);
    }

    /// <summary>
    /// Tests the removal of ten pairs.
    /// </summary>
    [TestMethod]
    public void RemoveDependency_TenPairs_Valid()
    {
        DependencyGraph graph = new DependencyGraph();
        graph.AddDependency("a", "b");
        graph.AddDependency("a", "c");
        graph.AddDependency("b", "c");
        graph.AddDependency("b", "d");
        graph.AddDependency("d", "e");
        graph.AddDependency("e", "c");
        graph.AddDependency("f", "e");
        graph.AddDependency("f", "g");
        graph.AddDependency("g", "e");
        graph.AddDependency("g", "a");
        string[] arr = ["a", "b", "d", "e", "f", "g"];
        int size = 10;
        foreach(string dependee in arr)
        {
            foreach(string dependent in graph.GetDependents(dependee))
            {
                graph.RemoveDependency(dependee, dependent);
                size--;
                Assert.AreEqual(graph.Size, size);
            }
        }
    }

    //ReplaceDependents tests

    /// <summary>
    /// Tests ReplaceDependents in an empty graph.
    /// </summary>
    [TestMethod]
    public void ReplaceDependents_Empty_Valid()
    {
        DependencyGraph graph = new DependencyGraph();
        graph.ReplaceDependents("a", ["b"]);
        Assert.IsTrue(graph.GetDependents("a").Contains("b"));
        Assert.IsTrue(graph.GetDependees("b").Contains("a"));
        Assert.AreEqual (graph.Size, 1);
    }

    /// <summary>
    /// Tests ReplaceDependents with one pair.
    /// </summary>
    [TestMethod]
    public void ReplaceDependents_OnePair_Valid()
    {
        DependencyGraph graph = new DependencyGraph();
        graph.AddDependency ("a", "b");
        graph.ReplaceDependents("a", ["c"]);
        Assert.IsTrue(graph.GetDependents("a").Contains("c"));
        Assert.IsTrue(graph.GetDependees("c").Contains("a"));
        Assert.IsFalse(graph.GetDependees("b").Contains("a"));
        Assert.AreEqual(graph.Size, 1);
    }

    /// <summary>
    /// Tests ReplaceDependents with a dependee that does not exist in the graph.
    /// </summary>
    [TestMethod]
    public void ReplaceDependents_NoneExist_Valid()
    {
        DependencyGraph graph = new DependencyGraph();
        graph.AddDependency("a", "b");
        graph.ReplaceDependents("b", ["c"]);
        Assert.IsTrue(graph.GetDependents("a").Contains("b"));
        Assert.IsTrue(graph.GetDependees("b").Contains("a"));
        Assert.IsTrue(graph.GetDependents("b").Contains("c"));
        Assert.IsTrue(graph.GetDependees("c").Contains("b"));
        Assert.AreEqual(graph.Size, 2);
    }

    /// <summary>
    /// Tests ReplaceDependents with the same pair.
    /// </summary>
    [TestMethod]
    public void ReplaceDependents_SamePair_Valid()
    {
        DependencyGraph graph = new DependencyGraph();
        graph.AddDependency("a", "b");
        graph.ReplaceDependents("a", ["b"]);
        Assert.IsTrue(graph.GetDependents("a").Contains("b"));
        Assert.IsTrue(graph.GetDependees("b").Contains("a"));
        Assert.AreEqual(graph.Size, 1);
    }

    /// <summary>
    /// Tests ReplaceDependents from multiple pairs of the same key to multiple new pairs.
    /// </summary>
    [TestMethod]
    public void ReplaceDependents_MultiplePairsToMultiplePairs_Valid()
    {
        DependencyGraph graph = new DependencyGraph();
        graph.AddDependency("a", "b");
        graph.AddDependency("a", "c");
        graph.AddDependency("a", "d");
        graph.ReplaceDependents("a", ["e", "f", "g"]);
        Assert.IsFalse(graph.HasDependees("b"));
        Assert.IsFalse(graph.HasDependees("c"));
        Assert.IsFalse(graph.HasDependees("d"));
        Assert.IsTrue(graph.GetDependents("a").Contains("e"));
        Assert.IsTrue(graph.GetDependents("a").Contains("f"));
        Assert.IsTrue(graph.GetDependents("a").Contains("g"));
        Assert.IsTrue(graph.GetDependees("e").Contains("a"));
        Assert.IsTrue(graph.GetDependees("f").Contains("a"));
        Assert.IsTrue(graph.GetDependees("g").Contains("a"));
        Assert.IsFalse(graph.GetDependents("a").Contains("b"));
        Assert.IsFalse(graph.GetDependents("a").Contains("c"));
        Assert.IsFalse(graph.GetDependents("a").Contains("d"));
        Assert.IsFalse(graph.GetDependees("b").Contains("a"));
        Assert.IsFalse(graph.GetDependees("c").Contains("a"));
        Assert.IsFalse(graph.GetDependees("d").Contains("a"));
        Assert.AreEqual(graph.Size, 3);
    }

    /// <summary>
    /// Tests ReplaceDependents from one pair to multiple pairs.
    /// </summary>
    [TestMethod]
    public void ReplaceDependents_SinglePairToMultiplePairs_Valid()
    {
        DependencyGraph graph = new DependencyGraph();
        graph.AddDependency("a", "b");
        Assert.AreEqual(graph.Size, 1);
        graph.ReplaceDependents("a", ["c", "d", "e", "f", "g"]);
        Assert.IsFalse(graph.HasDependees("b"));
        Assert.AreEqual(graph.Size, 5);
    }

    /// <summary>
    /// Tests ReplaceDependents from multiple pairs of the same key to one pair.
    /// </summary>
    [TestMethod]
    public void ReplaceDependents_MultiplePairsToSinglePair_Valid()
    {
        DependencyGraph graph = new DependencyGraph();
        graph.AddDependency("a", "b");
        graph.AddDependency("a", "c");
        graph.AddDependency("a", "d");
        graph.AddDependency("a", "e");
        graph.AddDependency("a", "f");
        Assert.AreEqual(graph.Size, 5);
        graph.ReplaceDependents("a", ["g"]);
        Assert.IsFalse(graph.HasDependees("b"));
        Assert.IsFalse(graph.HasDependees("c"));
        Assert.IsFalse(graph.HasDependees("d"));
        Assert.IsFalse(graph.HasDependees("e"));
        Assert.IsFalse(graph.HasDependees("f"));
        Assert.AreEqual(graph.Size, 1);
    }

    /// <summary>
    /// Tests ReplaceDependents with an enumerable list containing duplicate elements.
    /// </summary>
    [TestMethod]
    public void ReplaceDependents_SameElementInEnumSet_Valid()
    {
        DependencyGraph graph = new DependencyGraph();
        graph.AddDependency("a", "b");
        graph.ReplaceDependents("a", ["c", "c"]);
        Assert.IsFalse(graph.HasDependees("b"));
        Assert.IsTrue(graph.HasDependees("c"));
        Assert.AreEqual(graph.Size, 1);
    }


    //ReplaceDependees tests

    /// <summary>
    /// Tests ReplaceDependees in an empty graph.
    /// </summary>
    [TestMethod]
    public void ReplaceDependees_Empty_Valid()
    {
        DependencyGraph graph = new DependencyGraph();
        graph.ReplaceDependees("a", ["b"]);
        Assert.IsTrue(graph.GetDependents("b").Contains("a"));
        Assert.IsTrue(graph.GetDependees("a").Contains("b"));
        Assert.AreEqual(graph.Size, 1);
    }

    /// <summary>
    /// Tests ReplaceDependees with one pair.
    /// </summary>
    [TestMethod]
    public void ReplaceDependees_OnePair_Valid()
    {
        DependencyGraph graph = new DependencyGraph();
        graph.AddDependency("b", "a");
        graph.ReplaceDependees("a", ["c"]);
        Assert.IsTrue(graph.GetDependees("a").Contains("c"));
        Assert.IsTrue(graph.GetDependents("c").Contains("a"));
        Assert.IsFalse(graph.GetDependents("b").Contains("a"));
        Assert.AreEqual(graph.Size, 1);
    }

    /// <summary>
    /// Tests ReplaceDependees with a dependent that does not exist in the graph.
    /// </summary>
    [TestMethod]
    public void ReplaceDependees_NoneExist_Valid()
    {
        DependencyGraph graph = new DependencyGraph();
        graph.AddDependency("b", "a");
        graph.ReplaceDependees("b", ["c"]);
        Assert.IsTrue(graph.GetDependents("b").Contains("a"));
        Assert.IsTrue(graph.GetDependees("a").Contains("b"));
        Assert.IsTrue(graph.GetDependents("c").Contains("b"));
        Assert.IsTrue(graph.GetDependees("b").Contains("c"));
        Assert.AreEqual(graph.Size, 2);
    }

    /// <summary>
    /// Tests ReplaceDependees with the same pair.
    /// </summary>
    [TestMethod]
    public void ReplaceDependees_SamePair_Valid()
    {
        DependencyGraph graph = new DependencyGraph();
        graph.AddDependency("b", "a");
        graph.ReplaceDependees("a", ["b"]);
        Assert.IsTrue(graph.GetDependents("b").Contains("a"));
        Assert.IsTrue(graph.GetDependees("a").Contains("b"));
        Assert.AreEqual(graph.Size, 1);
    }

    /// <summary>
    /// Tests ReplaceDependees from multiple pairs of the same value to multiple new pairs.
    /// </summary>
    [TestMethod]
    public void ReplaceDependees_MultiplePairsToMultiplePairs_Valid()
    {
        DependencyGraph graph = new DependencyGraph();
        graph.AddDependency("b", "a");
        graph.AddDependency("c", "a");
        graph.AddDependency("d", "a");
        graph.ReplaceDependees("a", ["e", "f", "g"]);
        Assert.IsFalse(graph.HasDependents("b"));
        Assert.IsFalse(graph.HasDependents("c"));
        Assert.IsFalse(graph.HasDependents("d"));
        Assert.IsTrue(graph.GetDependees("a").Contains("e"));
        Assert.IsTrue(graph.GetDependees("a").Contains("f"));
        Assert.IsTrue(graph.GetDependees("a").Contains("g"));
        Assert.IsTrue(graph.GetDependents("e").Contains("a"));
        Assert.IsTrue(graph.GetDependents("f").Contains("a"));
        Assert.IsTrue(graph.GetDependents("g").Contains("a"));
        Assert.IsFalse(graph.GetDependees("a").Contains("b"));
        Assert.IsFalse(graph.GetDependees("a").Contains("c"));
        Assert.IsFalse(graph.GetDependees("a").Contains("d"));
        Assert.IsFalse(graph.GetDependents("b").Contains("a"));
        Assert.IsFalse(graph.GetDependents("c").Contains("a"));
        Assert.IsFalse(graph.GetDependents("d").Contains("a"));
        Assert.AreEqual(graph.Size, 3);
    }

    /// <summary>
    /// Tests ReplaceDependees from one pair to multiple pairs.
    /// </summary>
    [TestMethod]
    public void ReplaceDependees_SinglePairToMultiplePairs_Valid()
    {
        DependencyGraph graph = new DependencyGraph();
        graph.AddDependency("b", "a");
        Assert.AreEqual(graph.Size, 1);
        graph.ReplaceDependees("a", ["c", "d", "e", "f", "g"]);
        Assert.IsFalse(graph.HasDependents("b"));
        Assert.AreEqual(graph.Size, 5);
    }

    /// <summary>
    /// Tests ReplaceDependees from multiple pairs of the same value to one pair.
    /// </summary>
    [TestMethod]
    public void ReplaceDependees_MultiplePairsToSinglePair_Valid()
    {
        DependencyGraph graph = new DependencyGraph();
        graph.AddDependency("b", "a");
        graph.AddDependency("c", "a");
        graph.AddDependency("d", "a");
        graph.AddDependency("e", "a");
        graph.AddDependency("f", "a");
        Assert.AreEqual(graph.Size, 5);
        graph.ReplaceDependees("a", ["g"]);
        Assert.IsFalse(graph.HasDependents("b"));
        Assert.IsFalse(graph.HasDependents("c"));
        Assert.IsFalse(graph.HasDependents("d"));
        Assert.IsFalse(graph.HasDependents("e"));
        Assert.IsFalse(graph.HasDependents("f"));
        Assert.AreEqual(graph.Size, 1);
    }

    /// <summary>
    /// Tests ReplaceDependees with an enumerable list containing duplicate elements.
    /// </summary>
    [TestMethod]
    public void ReplaceDependees_SameElementInEnumSet_Valid()
    {
        DependencyGraph graph = new DependencyGraph();
        graph.AddDependency("b", "a");
        graph.ReplaceDependees("a", ["c", "c"]);
        Assert.IsFalse(graph.HasDependents("b"));
        Assert.IsTrue(graph.HasDependents("c"));
        Assert.AreEqual(graph.Size, 1);
    }

    /// <summary>
    /// Stress tests the DependencyGraph class methods by running several within a certain time limit (2 seconds).
    /// Also double checks that the methods properly function for large numbers of dependencies.
    /// </summary>
    [TestMethod]
    [Timeout(2000)] // 2 second run time limit
    public void StressTest()
    {
        DependencyGraph dg = new();
        // Create a list of strings
        const int SIZE = 200;
        string[] letters = new string[SIZE];
        for (int i = 0; i < SIZE; i++)
        {
            letters[i] = string.Empty + ((char)('a' + i));
        }
        // Create two lists of string lists for dependencies we expect to see
        HashSet<string>[] dependents = new HashSet<string>[SIZE];
        HashSet<string>[] dependees = new HashSet<string>[SIZE];
        for (int i = 0; i < SIZE; i++)
        {
            dependents[i] = [];
            dependees[i] = [];
        }
        // Add a bunch of dependencies
        for (int i = 0; i < SIZE; i++)
        {
            for (int j = i + 1; j < SIZE; j++)
            {
                dg.AddDependency(letters[i], letters[j]);
                dependents[i].Add(letters[j]);
                dependees[j].Add(letters[i]);
            }
        }
        // Remove a bunch of dependencies
        for (int i = 0; i < SIZE; i++)
        {
            for (int j = i + 4; j < SIZE; j += 4)
            {
                dg.RemoveDependency(letters[i], letters[j]);
                dependents[i].Remove(letters[j]);
                dependees[j].Remove(letters[i]);
            }
        }
        // Add some back
        for (int i = 0; i < SIZE; i++)
        {
            for (int j = i + 1; j < SIZE; j += 2)
            {
                dg.AddDependency(letters[i], letters[j]);
                dependents[i].Add(letters[j]);
                dependees[j].Add(letters[i]);
            }
        }
        // Remove some more
        for (int i = 0; i < SIZE; i += 2)
        {
            for (int j = i + 3; j < SIZE; j += 3)
            {
                dg.RemoveDependency(letters[i], letters[j]);
                dependents[i].Remove(letters[j]);
                dependees[j].Remove(letters[i]);
            }
        }
        // Compare the resulting lists from the graph with the original correct lists.
        for (int i = 0; i < SIZE; i++)
        {
            Assert.IsTrue(dependents[i].SetEquals(new
            HashSet<string>(dg.GetDependents(letters[i]))));
            Assert.IsTrue(dependees[i].SetEquals(new
            HashSet<string>(dg.GetDependees(letters[i]))));
        }
    }
}