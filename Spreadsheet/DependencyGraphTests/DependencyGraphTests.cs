using System.Reflection;

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
    ///   <para>
    ///   This simulates a stress test that ensures that the dg can complete all the adding and removing within 2 seconds.
    ///   </para>
    ///   <para>
    ///     <list type="number">
    ///       <item>This stress test creates "size" amount of strings to use for the tests.</item>
    ///       <item>Creates two Hashsets that will act as controls to simulate what the dependencyGraph methods should simulate.</item>
    ///       <item>Starting with adding a bunch of the strings into the dg and HashSets.</item>
    ///       <item>Then removing a bunch of strings from the dg and Hashsets.</item>
    ///       <item>Adding some back. Then removing more.</item>
    ///       <item>Finally ensuring that the Hashsets and the internal dictionaries of dg are equal.</item>
    ///     </list>
    /// </para>
    /// </summary>
    [TestMethod]
    [Timeout(2000)] // 2 second run time limit
    public void StressTest()
    {
        DependencyGraph dg = new();

        // A bunch of strings to use
        const int size = 200;
        var letters = new string[size];
        for (var i = 0; i < size; i++)
        {
            letters[i] = string.Empty + ((char)('a' + i));
        }

        // The correct answers
        var dependents = new HashSet<string>[size];
        var dependees = new HashSet<string>[size];
        for (var i = 0; i < size; i++)
        {
            dependents[i] = [];
            dependees[i] = [];
        }

        // Add a bunch of dependencies
        for (var i = 0; i < size; i++)
        {
            for (var j = i + 1; j < size; j++)
            {
                dg.AddDependency(letters[i], letters[j]);
                dependents[i].Add(letters[j]);
                dependees[j].Add(letters[i]);
            }
        }

        // Remove a bunch of dependencies
        for (var i = 0; i < size; i++)
        {
            for (var j = i + 4; j < size; j += 4)
            {
                dg.RemoveDependency(letters[i], letters[j]);
                dependents[i].Remove(letters[j]);
                dependees[j].Remove(letters[i]);
            }
        }

        // Add some back
        for (var i = 0; i < size; i++)
        {
            for (var j = i + 1; j < size; j += 2)
            {
                dg.AddDependency(letters[i], letters[j]);
                dependents[i].Add(letters[j]);
                dependees[j].Add(letters[i]);
            }
        }

        // Remove some more
        for (var i = 0; i < size; i += 2)
        {
            for (var j = i + 3; j < size; j += 3)
            {
                dg.RemoveDependency(letters[i], letters[j]);
                dependents[i].Remove(letters[j]);
                dependees[j].Remove(letters[i]);
            }
        }

        // Make sure everything is right
        for (var i = 0; i < size; i++)
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

    [TestMethod]
    public void DependencyGraphTest_DependencyGraphXMlCommentExampleRecreation_IsAccurate()
    {
        DependencyGraph dg = new();
        dg.AddDependency("a", "b");
        dg.AddDependency("a", "c");
        dg.AddDependency("b", "d");
        dg.AddDependency("d", "d");

        // --- HasDependents ---
        // IsTrue ---
        Assert.IsTrue(dg.HasDependents("a"));
        Assert.IsTrue(dg.HasDependents("b"));
        Assert.IsTrue(dg.HasDependents("d"));
        // IsFalse ---
        Assert.IsFalse(dg.HasDependents("c"));

        // --- Has Dependees ---
        // IsTrue 
        Assert.IsTrue(dg.HasDependees("b"));
        Assert.IsTrue(dg.HasDependees("d"));
        Assert.IsTrue(dg.HasDependees("c"));
        // IsFalse 
        Assert.IsFalse(dg.HasDependees("a"));

        // --- GetDependents ---
        Assert.IsTrue(dg.GetDependents("a").Contains("b"));
        Assert.IsTrue(dg.GetDependents("a").Contains("c"));
        Assert.IsTrue(dg.GetDependents("b").Contains("d"));
        Assert.IsTrue(dg.GetDependents("d").Contains("d"));

        // --- GetDependees ---
        Assert.IsTrue(dg.GetDependees("b").Contains("a"));
        Assert.IsTrue(dg.GetDependees("c").Contains("a"));
        Assert.IsTrue(dg.GetDependees("d").Contains("b"));
        Assert.IsTrue(dg.GetDependees("d").Contains("d"));
    }

    // --- DependencyGraph Constructor Tests---

    /// <summary>
    ///   This test also confirms that the constructor starts with no ordered pairs
    ///   as the size is 0 and thus empty
    /// </summary>
    [TestMethod]
    public void DependencyGraph_InitialState_Size0()
    {
        DependencyGraph dg = new();
        Assert.IsTrue(dg.Size == 0);
    }

    // --- HasDependents Tests ---

    [TestMethod]
    public void HasDependents_AHasDependents_True()
    {
        DependencyGraph dg = new();
        dg.AddDependency("A", "B");
        Assert.IsTrue(dg.HasDependents("A"));
    }

    [TestMethod]
    public void HasDependents_AHasMultipleDependents_True()
    {
        DependencyGraph dg = new();
        dg.AddDependency("A", "B");
        dg.AddDependency("A", "C");
        Assert.IsTrue(dg.HasDependents("A"));
        Assert.IsTrue(dg.GetDependents("A").Contains("B"));
        Assert.IsTrue(dg.GetDependents("A").Contains("C"));
    }

    [TestMethod]
    public void HasDependents_ADoesNotHaveAfterRemoval_True()
    {
        DependencyGraph dg = new();
        dg.AddDependency("A", "B");
        Assert.IsTrue(dg.HasDependents("A"));
        dg.RemoveDependency("A", "B");
        Assert.IsFalse(dg.HasDependents("A"));
    }

    [TestMethod]
    public void HasDependents_DependeeNotAddedToDG_False()
    {
        DependencyGraph dg = new();
        dg.AddDependency("A", "B");
        Assert.IsFalse(dg.HasDependents("C"));
    }

    // --- HasDependees Tests ---

    [TestMethod]
    public void HasDependees_BHasDependees_True()
    {
        DependencyGraph dg = new();
        dg.AddDependency("A", "B");
        Assert.IsTrue(dg.HasDependees("B"));
    }

    [TestMethod]
    public void HasDependees_BHasMultipleDependees_True()
    {
        DependencyGraph dg = new();
        dg.AddDependency("A", "B");
        dg.AddDependency("C", "B");
        Assert.IsTrue(dg.HasDependees("B"));
        Assert.IsTrue(dg.GetDependees("B").Contains("A"));
        Assert.IsTrue(dg.GetDependees("B").Contains("C"));
    }

    [TestMethod]
    public void HasDependees_BDoesNotHaveAfterRemoval_True()
    {
        DependencyGraph dg = new();
        dg.AddDependency("A", "B");
        Assert.IsTrue(dg.HasDependees("B"));
        dg.RemoveDependency("A", "B");
        Assert.IsFalse(dg.HasDependees("B"));
    }

    [TestMethod]
    public void HasDependees_DependeesNotAddedToDG_False()
    {
        DependencyGraph dg = new();
        dg.AddDependency("A", "B");
        Assert.IsFalse(dg.HasDependees("C"));
    }

    // --- GetDependents Tests ---

    [TestMethod]
    public void GetDependents_AHasDependentB_True()
    {
        DependencyGraph dg = new();
        dg.AddDependency("A", "B");
        Assert.IsTrue(dg.GetDependents("A").Contains("B"));
    }

    [TestMethod]
    public void GetDependents_AHasMultipleDependents_True()
    {
        DependencyGraph dg = new();
        dg.AddDependency("A", "B");
        dg.AddDependency("A", "C");
        dg.AddDependency("A", "D");
        Assert.IsTrue(dg.GetDependents("A").Contains("B"));
        Assert.IsTrue(dg.GetDependents("A").Contains("C"));
        Assert.IsTrue(dg.GetDependents("A").Contains("D"));
    }

    [TestMethod]
    public void GetDependents_MultipleDependeesHaveDependents_True()
    {
        DependencyGraph dg = new();
        dg.AddDependency("A", "B");
        dg.AddDependency("C", "D");
        Assert.IsTrue(dg.GetDependents("A").Contains("B"));
        Assert.IsTrue(dg.GetDependents("C").Contains("D"));
    }

    [TestMethod]
    public void GetDependents_ADoesNotHaveAfterRemove_True()
    {
        DependencyGraph dg = new();
        dg.AddDependency("A", "B");
        Assert.IsTrue(dg.GetDependents("A").Contains("B"));
        dg.RemoveDependency("A", "B");
        Assert.IsFalse(dg.GetDependents("A").Contains("B"));
    }

    // --- GetDependees Tests ---

    [TestMethod]
    public void GetDependees_BHasDependeeA_True()
    {
        DependencyGraph dg = new();
        dg.AddDependency("A", "B");
        Assert.IsTrue(dg.GetDependees("B").Contains("A"));
    }

    [TestMethod]
    public void GetDependees_BHasMultipleDependees_True()
    {
        DependencyGraph dg = new();
        dg.AddDependency("A", "B");
        dg.AddDependency("C", "B");
        dg.AddDependency("D", "B");
        Assert.IsTrue(dg.GetDependees("B").Contains("A"));
        Assert.IsTrue(dg.GetDependees("B").Contains("C"));
        Assert.IsTrue(dg.GetDependees("B").Contains("D"));
    }

    [TestMethod]
    public void GetDependees_MultipleDependeesHaveDependents_True()
    {
        DependencyGraph dg = new();
        dg.AddDependency("A", "B");
        dg.AddDependency("C", "D");
        Assert.IsTrue(dg.GetDependees("B").Contains("A"));
        Assert.IsTrue(dg.GetDependees("D").Contains("C"));
    }

    [TestMethod]
    public void GetDependees_BDoesNotHaveAfterRemove_True()
    {
        DependencyGraph dg = new();
        dg.AddDependency("A", "B");
        Assert.IsTrue(dg.GetDependees("B").Contains("A"));
        dg.RemoveDependency("A", "B");
        Assert.IsFalse(dg.GetDependees("B").Contains("A"));
    }

    // --- AddDependency Tests ---

    [TestMethod]
    public void AddDependency_DependentAdded_AddedCorrectly()
    {
        DependencyGraph dg = new();
        dg.AddDependency("A", "B");
        Assert.IsTrue(dg.HasDependents("A"));
    }

    [TestMethod]
    public void AddDependency_DependentAdded_Size1()
    {
        DependencyGraph dg = new();
        dg.AddDependency("A", "B");
        Assert.IsTrue(dg.Size == 1);
    }

    [TestMethod]
    public void AddDependency_DependeeAdded_AddedCorrectly()
    {
        DependencyGraph dg = new();
        dg.AddDependency("A", "B");
        Assert.IsTrue(dg.HasDependees("B"));
    }

    /// <summary>
    ///   Also Ensures that forwards and backward adding of ordered pairs works.
    ///   Where (s, t) and (t, s) exist in dg.
    /// </summary>
    [TestMethod]
    public void AddDependency_DictionariesContainCorrectNode_AddedCorrectly()
    {
        DependencyGraph dg = new();
        dg.AddDependency("A", "B");
        Assert.IsTrue(dg.GetDependents("A").Contains("B"));
        Assert.IsTrue(dg.GetDependees("B").Contains("A"));
    }

    [TestMethod]
    public void AddDependency_MultipleDependentsAddedSize5_AddedCorrectlyCorrectSize()
    {
        DependencyGraph dg = new();
        dg.AddDependency("A", "B");
        dg.AddDependency("A", "C");
        dg.AddDependency("A", "D");
        dg.AddDependency("A", "E");
        dg.AddDependency("A", "F");
        Assert.IsTrue(dg.GetDependents("A").Contains("B"));
        Assert.IsTrue(dg.GetDependents("A").Contains("C"));
        Assert.IsTrue(dg.GetDependents("A").Contains("D"));
        Assert.IsTrue(dg.GetDependents("A").Contains("E"));
        Assert.IsTrue(dg.GetDependents("A").Contains("F"));
        Assert.IsTrue(dg.Size == 5);
    }

    [TestMethod]
    public void AddDependency_MultipleDependeesAddedSize5_AddedCorrectlyCorrectSize()
    {
        DependencyGraph dg = new();
        dg.AddDependency("A", "B");
        dg.AddDependency("C", "B");
        dg.AddDependency("D", "B");
        dg.AddDependency("E", "B");
        dg.AddDependency("F", "B");
        Assert.IsTrue(dg.GetDependees("B").Contains("A"));
        Assert.IsTrue(dg.GetDependees("B").Contains("C"));
        Assert.IsTrue(dg.GetDependees("B").Contains("D"));
        Assert.IsTrue(dg.GetDependees("B").Contains("E"));
        Assert.IsTrue(dg.GetDependees("B").Contains("F"));
        Assert.IsTrue(dg.Size == 5);
    }

    [TestMethod]
    public void AddDependency_SameOrderedPairNotAddedTwice_AddedCorrectly()
    {
        DependencyGraph dg = new();
        dg.AddDependency("A", "B");
        dg.AddDependency("A", "B");
        Assert.IsTrue(dg.Size == 1);
    }

    [TestMethod]
    public void AddDependency_SizeChangesAfterEachAdd_Size10()
    {
        DependencyGraph dg = new();
        for (var i = 0; i < 10; i++)
        {
            dg.AddDependency(i.ToString(), (i + 1).ToString());
            Assert.IsTrue(dg.Size == i + 1);
        }
    }

    // --- RemoveDependency Tests ---

    [TestMethod]
    public void RemoveDependency_DependentRemoved_RemovedCorrectly()
    {
        DependencyGraph dg = new();
        dg.AddDependency("A", "B");
        Assert.IsTrue(dg.HasDependents("A"));
        dg.RemoveDependency("A", "B");
        Assert.IsFalse(dg.HasDependents("A"));
    }

    [TestMethod]
    public void RemoveDependency_DependencyRemoved_Size0()
    {
        DependencyGraph dg = new();
        dg.AddDependency("A", "B");
        Assert.IsTrue(dg.Size == 1);
        dg.RemoveDependency("A", "B");
        Assert.IsTrue(dg.Size == 0);
    }

    [TestMethod]
    public void RemoveDependency_DependeeRemoved_RemovedCorrectly()
    {
        DependencyGraph dg = new();
        dg.AddDependency("A", "B");
        Assert.IsTrue(dg.HasDependees("B"));
        dg.RemoveDependency("A", "B");
        Assert.IsFalse(dg.HasDependees("B"));
    }

    /// <summary>
    ///   Also Ensures that forwards and backward deletion of ordered pairs works.
    ///   Where (s, t) and (t, s) are both removed from dg.
    /// </summary>
    [TestMethod]
    public void RemoveDependency_DictionariesDoNotContainAfterRemove_RemovedCorrectly()
    {
        DependencyGraph dg = new();
        dg.AddDependency("A", "B");
        Assert.IsTrue(dg.GetDependents("A").Contains("B"));
        Assert.IsTrue(dg.GetDependees("B").Contains("A"));
        dg.RemoveDependency("A", "B");
        Assert.IsFalse(dg.GetDependents("A").Contains("B"));
        Assert.IsFalse(dg.GetDependees("B").Contains("A"));
    }

    [TestMethod]
    public void RemoveDependency_MultipleDependentsRemovedSize0_RemovedCorrectlyCorrectSize()
    {
        DependencyGraph dg = new();
        dg.AddDependency("A", "B");
        dg.AddDependency("A", "C");
        dg.AddDependency("A", "D");
        dg.AddDependency("A", "E");
        dg.AddDependency("A", "F");
        Assert.IsTrue(dg.GetDependents("A").Contains("B"));
        Assert.IsTrue(dg.GetDependents("A").Contains("C"));
        Assert.IsTrue(dg.GetDependents("A").Contains("D"));
        Assert.IsTrue(dg.GetDependents("A").Contains("E"));
        Assert.IsTrue(dg.GetDependents("A").Contains("F"));
        Assert.IsTrue(dg.Size == 5);
        dg.RemoveDependency("A", "B");
        dg.RemoveDependency("A", "C");
        dg.RemoveDependency("A", "D");
        dg.RemoveDependency("A", "E");
        dg.RemoveDependency("A", "F");
        Assert.IsFalse(dg.GetDependents("A").Contains("B"));
        Assert.IsFalse(dg.GetDependents("A").Contains("C"));
        Assert.IsFalse(dg.GetDependents("A").Contains("D"));
        Assert.IsFalse(dg.GetDependents("A").Contains("E"));
        Assert.IsFalse(dg.GetDependents("A").Contains("F"));
        Assert.IsTrue(dg.Size == 0);
    }

    [TestMethod]
    public void RemoveDependency_MultipleDependeesRemovedSize0_RemovedCorrectlyCorrectSize()
    {
        DependencyGraph dg = new();
        dg.AddDependency("A", "B");
        dg.AddDependency("C", "B");
        dg.AddDependency("D", "B");
        dg.AddDependency("E", "B");
        dg.AddDependency("F", "B");
        Assert.IsTrue(dg.GetDependees("B").Contains("A"));
        Assert.IsTrue(dg.GetDependees("B").Contains("C"));
        Assert.IsTrue(dg.GetDependees("B").Contains("D"));
        Assert.IsTrue(dg.GetDependees("B").Contains("E"));
        Assert.IsTrue(dg.GetDependees("B").Contains("F"));
        Assert.IsTrue(dg.Size == 5);
        dg.RemoveDependency("A", "B");
        dg.RemoveDependency("C", "B");
        dg.RemoveDependency("D", "B");
        dg.RemoveDependency("E", "B");
        dg.RemoveDependency("F", "B");
        Assert.IsFalse(dg.GetDependees("B").Contains("A"));
        Assert.IsFalse(dg.GetDependees("B").Contains("C"));
        Assert.IsFalse(dg.GetDependees("B").Contains("D"));
        Assert.IsFalse(dg.GetDependees("B").Contains("E"));
        Assert.IsFalse(dg.GetDependees("B").Contains("F"));
        Assert.IsTrue(dg.Size == 0);
    }

    [TestMethod]
    public void RemoveDependency_SamePairRemovedTwiceSizeChangedOnce_Size1()
    {
        DependencyGraph dg = new();
        dg.AddDependency("A", "B");
        dg.AddDependency("A", "C");
        Assert.IsTrue(dg.Size == 2);
        dg.RemoveDependency("A", "B");
        dg.RemoveDependency("A", "B");
        Assert.IsTrue(dg.Size == 1);
    }

    [TestMethod]
    public void RemoveDependency_SizeChangesAfterEachRemove_Size10to0()
    {
        DependencyGraph dg = new();
        dg.AddDependency("A", "B");
        dg.AddDependency("A", "C");
        dg.AddDependency("A", "D");
        dg.AddDependency("A", "E");
        dg.AddDependency("A", "F");
        Assert.IsTrue(dg.Size == 5);
        dg.RemoveDependency("A", "B");
        Assert.IsTrue(dg.Size == 4);
        dg.RemoveDependency("A", "C");
        Assert.IsTrue(dg.Size == 3);
        dg.RemoveDependency("A", "D");
        Assert.IsTrue(dg.Size == 2);
        dg.RemoveDependency("A", "E");
        Assert.IsTrue(dg.Size == 1);
        dg.RemoveDependency("A", "F");
        Assert.IsTrue(dg.Size == 0);
    }

    [TestMethod]
    public void RemoveDependency_RemovedFakePairSizeDoesNotDecreaseOrGoNegative_Size0()
    {
        DependencyGraph dg = new();
        dg.RemoveDependency("A", "B");
        Assert.IsTrue(dg.Size == 0);
    }

    // --- ReplaceDependents Tests ---

    [TestMethod]
    public void ReplaceDependents_ReplaceWithNewDG_CorrectlyReplaces()
    {
        DependencyGraph dg = new();
        dg.ReplaceDependents("A", new HashSet<string> { "B" });
        Assert.IsTrue(dg.GetDependents("A").Contains("B"));
        Assert.IsTrue(dg.GetDependees("B").Contains("A"));
    }

    [TestMethod]
    public void ReplaceDependents_ABtoAC_CorrectlyReplaces()
    {
        DependencyGraph dg = new();
        dg.AddDependency("A", "B");
        Assert.IsTrue(dg.GetDependents("A").Contains("B"));
        Assert.IsTrue(dg.GetDependees("B").Contains("A"));
        dg.ReplaceDependents("A", new HashSet<string> { "C" });
        Assert.IsTrue(dg.GetDependents("A").Contains("C"));
        Assert.IsTrue(dg.GetDependees("C").Contains("A"));
    }

    [TestMethod]
    public void ReplaceDependents_FromSinglePairToFivePairs_CorrectlyReplaces()
    {
        DependencyGraph dg = new();
        dg.AddDependency("A", "B");
        Assert.IsTrue(dg.GetDependents("A").Contains("B"));
        Assert.IsTrue(dg.GetDependees("B").Contains("A"));
        dg.ReplaceDependents("A", new HashSet<string> { "C", "D", "E", "F", "G" });
        Assert.IsTrue(dg.GetDependents("A").Contains("C"));
        Assert.IsTrue(dg.GetDependents("A").Contains("D"));
        Assert.IsTrue(dg.GetDependents("A").Contains("E"));
        Assert.IsTrue(dg.GetDependents("A").Contains("F"));
        Assert.IsTrue(dg.GetDependents("A").Contains("G"));

        Assert.IsTrue(dg.GetDependees("C").Contains("A"));
        Assert.IsTrue(dg.GetDependees("D").Contains("A"));
        Assert.IsTrue(dg.GetDependees("E").Contains("A"));
        Assert.IsTrue(dg.GetDependees("F").Contains("A"));
        Assert.IsTrue(dg.GetDependees("G").Contains("A"));
    }

    [TestMethod]
    public void ReplaceDependents_ReplaceOneForOneKeepsSameSize_Size1()
    {
        DependencyGraph dg = new();
        dg.AddDependency("A", "B");
        dg.ReplaceDependents("A", new HashSet<string> { "C" });
        Assert.IsTrue(dg.Size == 1);
    }

    [TestMethod]
    public void ReplaceDependents_ReplacedDependentNoLongerExists_NoLongerExists()
    {
        DependencyGraph dg = new();
        dg.AddDependency("A", "B");
        Assert.IsTrue(dg.GetDependents("A").Contains("B"));
        Assert.IsTrue(dg.GetDependees("B").Contains("A"));
        dg.ReplaceDependents("A", new HashSet<string> { "C" });
        Assert.IsFalse(dg.GetDependents("A").Contains("B"));
        Assert.IsFalse(dg.HasDependees("B"));
    }

    /// <summary>
    ///   This also deletes (A, B) and therefore tests that the size is not 6 but 5.
    /// </summary>
    [TestMethod]
    public void ReplaceDependents_SizeGrowsAfterReplace_Size1To5()
    {
        DependencyGraph dg = new();
        dg.AddDependency("A", "B");
        Assert.IsTrue(dg.Size == 1);
        dg.ReplaceDependents("A", new HashSet<string> { "C", "D", "E", "F", "G" });
        Assert.IsTrue(dg.Size == 5);
    }

    [TestMethod]
    public void ReplaceDependents_SizeShrinksAfterReplace_Size5to1()
    {
        DependencyGraph dg = new();
        dg.AddDependency("A", "B");
        dg.AddDependency("A", "C");
        dg.AddDependency("A", "D");
        dg.AddDependency("A", "E");
        dg.AddDependency("A", "F");
        Assert.IsTrue(dg.Size == 5);
        dg.ReplaceDependents("A", new HashSet<string> { "S" });
        Assert.IsTrue(dg.Size == 1);
    }


    // --- ReplaceDependees Tests ---
}