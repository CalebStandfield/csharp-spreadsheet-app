// Skeleton implementation written by Joe Zachary for CS 3500, September 2013
// Version 1.1 - Joe Zachary
//   (Fixed error in comment for RemoveDependency)
// Version 1.2 - Daniel Kopta Fall 2018
//   (Clarified meaning of dependent and dependee)
//   (Clarified names in solution/project structure)
// Version 1.3 - H. James de St. Germain Fall 2024

namespace CS3500.DependencyGraph;

/// <summary>
///   <para>
///     (s1,t1) is an ordered pair of strings, meaning t1 depends on s1.
///     (in other words: s1 must be evaluated before t1.)
///   </para>
///   <para>
///     A DependencyGraph can be modeled as a set of ordered pairs of strings.
///     Two ordered pairs (s1,t1) and (s2,t2) are considered equal if and only
///     if s1 equals s2 and t1 equals t2.
///   </para>
///   <remarks>
///     Recall that sets never contain duplicates.
///     If an attempt is made to add an element to a set, and the element is already
///     in the set, the set remains unchanged.
///   </remarks>
///   <para>
///     Given a DependencyGraph DG:
///   </para>
///   <list type="number">
///     <item>
///       If s is a string, the set of all strings t such that (s,t) is in DG is called dependents(s).
///       (The set of things that depend on s.)
///     </item>
///     <item>
///       If s is a string, the set of all strings t such that (t,s) is in DG is called dependees(s).
///       (The set of things that s depends on.)
///     </item>
///   </list>
///   <para>
///      For example, suppose DG = {("a", "b"), ("a", "c"), ("b", "d"), ("d", "d")}.
///   </para>
///   <code>
///     dependents("a") = {"b", "c"}
///     dependents("b") = {"d"}
///     dependents("c") = {}
///     dependents("d") = {"d"}
///     dependees("a")  = {}
///     dependees("b")  = {"a"}
///     dependees("c")  = {"a"}
///     dependees("d")  = {"b", "d"}
///   </code>
/// </summary>
public class DependencyGraph
{
    // A dictionary representing string dependeees and their dependents
    private readonly Dictionary<string, HashSet<string>> _dependents;

    // A dictionary representing string dependents and their dependees
    private readonly Dictionary<string, HashSet<string>> _dependees;

    // Size of the dependencyGraph
    private int _size;

    /// <summary>
    ///   Initializes a new instance of the <see cref="DependencyGraph"/> class.
    ///   The initial DependencyGraph is empty.
    /// </summary>
    public DependencyGraph()
    {
        _dependents = new Dictionary<string, HashSet<string>>();
        _dependees = new Dictionary<string, HashSet<string>>();
        _size = 0;
    }

    /// <summary>
    /// The number of ordered pairs in the DependencyGraph.
    /// </summary>
    public int Size
    {
        get { return _size; }
    }

    /// <summary>
    ///   Reports whether the given node has dependents (i.e., other nodes depend on it).
    /// </summary>
    /// <param name="nodeName"> The name of the node.</param>
    /// <returns> true if the node has dependents. </returns>
    public bool HasDependents(string nodeName)
    {
        // Check if key exists
        if (_dependents.TryGetValue(nodeName, out var hashSet))
        {
            return hashSet.Count > 0; // Check and return true if any elements are in the HashSet
        }

        return false;
    }

    /// <summary>
    ///   Reports whether the given node has dependees (i.e., depends on one or more other nodes).
    /// </summary>
    /// <returns> true if the node has dependees.</returns>
    /// <param name="nodeName">The name of the node.</param>
    public bool HasDependees(string nodeName)
    {
        // Check if key exists
        if (_dependees.TryGetValue(nodeName, out var hashSet))
        {
            // Check and return true if any elements are in the HashSet
            return hashSet.Count > 0;
        }

        return false;
    }

    /// <summary>
    ///   <para>
    ///     Returns the dependents of the node with the given name.
    ///   </para>
    /// </summary>
    /// <param name="nodeName"> The node we are looking at.</param>
    /// <returns> The dependents of nodeName. </returns>
    public IEnumerable<string> GetDependents(string nodeName)
    {
        // Will return the HashSet of dependents or a default empty HashSet
        return _dependents.GetValueOrDefault(nodeName, []);
    }

    /// <summary>
    ///   <para>
    ///     Returns the dependees of the node with the given name.
    ///   </para>
    /// </summary>
    /// <param name="nodeName"> The node we are looking at.</param>
    /// <returns> The dependees of nodeName. </returns>
    public IEnumerable<string> GetDependees(string nodeName)
    {
        // Will return the HashSet of dependees or a default empty HashSet
        return _dependees.GetValueOrDefault(nodeName, []);
    }

    /// <summary>
    /// <para>Adds the ordered pair (dependee, dependent), if it doesn't exist.</para>
    ///
    /// <para>
    ///   This can be thought of as: dependee must be evaluated before dependent
    /// </para>
    /// </summary>
    /// <param name="dependee"> the name of the node that must be evaluated first</param>
    /// <param name="dependent"> the name of the node that cannot be evaluated until after dependee</param>
    public void AddDependency(string dependee, string dependent)
    {
        // Forwards and backwards addition
        // Both methods will work or neither will
        AddDependent(dependee, dependent);
        AddDependee(dependent, dependee);
    }

    /// <summary>
    /// <para>
    ///   Adds the ordered pair (dependee, dependent), if it doesn't exist.
    ///   Adds the pair into the _dependents member variable.
    ///   This method will change the size of the dependencyGraph if successful.
    /// </para>
    /// </summary>
    /// <param name="dependee"> the name of the node that must be evaluated first</param>
    /// <param name="dependent"> the name of the node that cannot be evaluated until after dependee</param>
    private void AddDependent(string dependee, string dependent)
    {
        // Key exists. Try to add dependent to HashSet
        if (_dependents.TryGetValue(dependee, out var dependeesHashSet))
        {
            if (dependeesHashSet.Add(dependent))
            {
                _size++;
            }
        }
        // Key did not exist. Create new (K, V) pair
        else
        {
            _dependents.Add(dependee, [dependent]);
            _size++;
        }
    }

    /// <summary>
    /// <para>
    ///   Adds the ordered pair (dependent, dependee) if it doesn't exist.
    ///   Adds the pair into the _dependees member variable.
    ///   This method executes backwards addition into the dependencyGraph for easy deletion later.
    ///   Will not change the size of the dependencyGraph relies on AddDependent for size change.
    /// </para>
    /// </summary>
    /// <param name="dependent"> the name of the node that cannot be evaluated until after dependee</param>
    /// <param name="dependee"> the name of the node that must be evaluated first</param>
    private void AddDependee(string dependent, string dependee)
    {
        // Key exists. Try to add dependee to HashSet
        if (_dependees.TryGetValue(dependent, out var dependentsHashSet))
        {
            dependentsHashSet.Add(dependee);
        }
        // Key did not exist. Create new (K, V) pair
        else
        {
            _dependees.Add(dependent, [dependee]);
        }
    }

    /// <summary>
    ///   <para>
    ///     Removes the ordered pair (dependee, dependent), if it exists.
    ///   </para>
    /// </summary>
    /// <param name="dependee"> The name of the node that must be evaluated first</param>
    /// <param name="dependent"> The name of the node that cannot be evaluated until after dependee</param>
    public void RemoveDependency(string dependee, string dependent)
    {
        // Forward and backwards removal
        // Both methods will work or neither will
        RemoveDependent(dependee, dependent);
        RemoveDependee(dependent, dependee);
    }

    /// <summary>
    ///   <para>
    ///     Removes the ordered pair (dependee, dependent), if it exists.
    ///     Removes the pair from the _dependents member variable.
    ///     Will return a bool and use that value to appropriately affect the size of the dependencyGraph
    ///   </para>
    /// </summary>
    /// <param name="dependee"> The name of the node that must be evaluated first</param>
    /// <param name="dependent"> The name of the node that cannot be evaluated until after dependee</param>
    /// <returns>True if deleted, false if it wasn't in the dependencyGraph</returns>
    private void RemoveDependent(string dependee, string dependent)
    {
        // Check if key exists, if not return
        if (!_dependents.TryGetValue(dependee, out var dependeesHashSet)) return;
        if (dependeesHashSet.Remove(dependent))
        {
            // Key existed and value was removed
            _size--;
        }
    }

    /// <summary>
    ///   <para>
    ///     Removes the ordered pair (dependee, dependent), if it exists.
    ///     Removes the pair from _dependees member variable.
    ///   </para>
    /// </summary>
    /// <param name="dependee"> The name of the node that must be evaluated first</param>
    /// <param name="dependent"> The name of the node that cannot be evaluated until after dependee</param>
    private void RemoveDependee(string dependent, string dependee)
    {
        if (_dependees.TryGetValue(dependent, out var dependentsHashSet))
        {
            dependentsHashSet.Remove(dependee);
        }
    }

    /// <summary>
    ///   Removes all existing ordered pairs of the form (nodeName, *).  Then, for each
    ///   t in newDependents, adds the ordered pair (nodeName, t).
    /// </summary>
    /// <param name="nodeName"> The name of the node whose dependents are being replaced </param>
    /// <param name="newDependents"> The new dependents for nodeName</param>
    public void ReplaceDependents(string nodeName, IEnumerable<string> newDependents)
    {
        foreach (var key in _dependees.Keys)
        {
            // Handles both forwards and backwards deletion
            RemoveDependency(nodeName, key);
        }

        foreach (var dependent in newDependents)
        {
            AddDependency(nodeName, dependent);
        }
    }

    /// <summary>
    ///   <para>
    ///     Removes all existing ordered pairs of the form (*, nodeName).  Then, for each
    ///     t in newDependees, adds the ordered pair (t, nodeName).
    ///   </para>
    /// </summary>
    /// <param name="nodeName"> The name of the node who's dependees are being replaced</param>
    /// <param name="newDependees"> The new dependees for nodeName</param>
    public void ReplaceDependees(string nodeName, IEnumerable<string> newDependees)
    {
        foreach (var key in _dependees.Keys)
        {
            // Handles both forwards and backwards deletion
            RemoveDependency(nodeName, key);
        }

        foreach (var dependent in newDependees)
        {
            AddDependency(nodeName, dependent);
        }
    }
}