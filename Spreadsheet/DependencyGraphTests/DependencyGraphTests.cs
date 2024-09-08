namespace CS3500.DevelopmentTests;

using CS3500.DependencyGraph;

/// <summary>
///   This is a test class for DependencyGraphTest and is intended
///   to contain all DependencyGraphTest Unit Tests
/// </summary>
[TestClass]
public class DependencyGraphTests
{
    
    // --- Test Given By Instructor ---
    
    /// <summary>
    ///   TODO:  Explain carefully what this code tests.
    ///          Also, update in-line comments as appropriate.
    /// </summary>
    [TestMethod]
    [Timeout(2000)] // 2 second run time limit
    public void StressTest()
    {
        DependencyGraph dg = new();

        // A bunch of strings to use
        const int SIZE = 200;
        var letters = new string[SIZE];
        for (var i = 0; i < SIZE; i++)
        {
            letters[i] = string.Empty + ((char)('a' + i));
        }

        // The correct answers
        var dependents = new HashSet<string>[SIZE];
        var dependees = new HashSet<string>[SIZE];
        for (var i = 0; i < SIZE; i++)
        {
            dependents[i] = [];
            dependees[i] = [];
        }

        // Add a bunch of dependencies
        for (var i = 0; i < SIZE; i++)
        {
            for (var j = i + 1; j < SIZE; j++)
            {
                dg.AddDependency(letters[i], letters[j]);
                dependents[i].Add(letters[j]);
                dependees[j].Add(letters[i]);
            }
        }

        // Remove a bunch of dependencies
        for (var i = 0; i < SIZE; i++)
        {
            for (var j = i + 4; j < SIZE; j += 4)
            {
                dg.RemoveDependency(letters[i], letters[j]);
                dependents[i].Remove(letters[j]);
                dependees[j].Remove(letters[i]);
            }
        }

        // Add some back
        for (var i = 0; i < SIZE; i++)
        {
            for (var j = i + 1; j < SIZE; j += 2)
            {
                dg.AddDependency(letters[i], letters[j]);
                dependents[i].Add(letters[j]);
                dependees[j].Add(letters[i]);
            }
        }

        // Remove some more
        for (var i = 0; i < SIZE; i += 2)
        {
            for (var j = i + 3; j < SIZE; j += 3)
            {
                dg.RemoveDependency(letters[i], letters[j]);
                dependents[i].Remove(letters[j]);
                dependees[j].Remove(letters[i]);
            }
        }

        // Make sure everything is right
        for (var i = 0; i < SIZE; i++)
        {
            Assert.IsTrue(dependents[i].SetEquals(new HashSet<string>(dg.GetDependents(letters[i]))));
            Assert.IsTrue(dependees[i].SetEquals(new HashSet<string>(dg.GetDependees(letters[i]))));
        }
    }
    
    // --- Ps3 Canvas Example and DependencyGraph Xml comment Tests---
    
    [TestMethod]
    public void DependencyGraphTest_Ps3CanvasExampleRecreation_IsAccurate()
    {
        DependencyGraph dg = new();
        dg.AddDependency("A3", "A1");
        dg.AddDependency("A3", "A2");
        dg.AddDependency("A4", "A2");
        dg.AddDependency("A2", "A1");

        // --- HasDependents ---
        // IsTrue
        Assert.IsTrue(dg.HasDependents("A4"));
        Assert.IsTrue(dg.HasDependents("A3"));
        Assert.IsTrue(dg.HasDependents("A2"));
        // IsFalse
        Assert.IsFalse(dg.HasDependents("A1"));

        // --- HasDependees ---
        // IsTrue
        Assert.IsTrue(dg.HasDependees("A1"));
        Assert.IsTrue(dg.HasDependees("A2"));
        // IsFalse
        Assert.IsFalse(dg.HasDependees("A4"));
        Assert.IsFalse(dg.HasDependees("A3"));

        // --- GetDependents ---
        Assert.IsTrue(dg.GetDependents("A4").Contains("A2"));
        Assert.IsTrue(dg.GetDependents("A2").Contains("A1"));
        Assert.IsTrue(dg.GetDependents("A3").Contains("A2"));
        Assert.IsTrue(dg.GetDependents("A3").Contains("A1"));

        // --- GetDependees ---
        Assert.IsTrue(dg.GetDependees("A1").Contains("A2"));
        Assert.IsTrue(dg.GetDependees("A1").Contains("A3"));
        Assert.IsTrue(dg.GetDependees("A2").Contains("A3"));
        Assert.IsTrue(dg.GetDependees("A2").Contains("A4"));
    }
    
    // --- DependencyGraph Constructor Tests---
    
    /// <summary>
    ///   This test also confirms that the constructor starts with no ordered pairs
    ///   as the size is 0 and thus empty
    /// </summary>
    [TestMethod]
    public void DependencyGraph_InitialState_Size0NoPairs()
    {
        DependencyGraph dg = new();
        Assert.IsTrue(dg.Size == 0);
    }
    
    // --- HasDependents Tests ---
    
    // --- HasDependees Tests ---
    
    // --- GetDependents Tests ---
    
    // --- GetDependees Tests ---
    
    // --- AddDependency Tests ---
    
    // --- RemoveDependency Tests ---
    
    // --- ReplaceDependents Tests ---
    
    // --- ReplaceDependees Tests ---

    [TestMethod]
    public void AddDependency_DependencyAdded_AddedCorrectly()
    {
        DependencyGraph dg = new();
        dg.AddDependency("A", "B");
        Assert.IsTrue(dg.HasDependents("A"));
        Assert.IsTrue(dg.HasDependees("B"));
    }

    [TestMethod]
    public void AddDependency_ClassDefinitionExample_AddedCorrectly()
    {
        DependencyGraph dg = new();
        dg.AddDependency("a", "b");
        dg.AddDependency("a", "c");
        dg.AddDependency("b", "d");
        dg.AddDependency("d", "d");
        Assert.IsTrue(dg.HasDependents("a"));
        Assert.IsFalse(dg.HasDependees("a"));
        Assert.IsTrue(dg.HasDependents("b"));
        Assert.IsTrue(dg.HasDependees("b"));
        Assert.IsTrue(dg.HasDependents("d"));
        Assert.IsTrue(dg.HasDependees("d"));
    }

    [TestMethod]
    public void RemoveDependency_DependencyRemoved_RemovedCorrectly()
    {
        DependencyGraph dg = new();
        dg.AddDependency("A", "B");
        Assert.IsTrue(dg.HasDependents("A"));
        Assert.IsTrue(dg.HasDependees("B"));
        Assert.IsTrue(dg.GetDependees("B").Contains("A"));
        Assert.IsTrue(dg.GetDependents("A").Contains("B"));
        dg.RemoveDependency("A", "B");
        Assert.IsFalse(dg.GetDependees("B").Contains("A"));
        Assert.IsFalse(dg.GetDependents("A").Contains("B"));
    }
}