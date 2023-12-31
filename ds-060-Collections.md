<!-- header: '**F# Data Structures**' -->

# Comparison with C# collections

<style scoped>
table {
  font-size: 30px;
}
</style>

Collection | F# | C#
--- | --- | ---
Linked list | `list<'T>` | `LinkedList<T>`
Resizeable array | `ResizeArray<'T>` | `List<T>`
Array | `array<'T>`, `'T[]` | `T[]`
Map (immutable dictionary) | `Map<'K, 'V>` | `ImmutableDictionary<K, V>`
Set (immutable set) | `Set<'T>` | `ImmutableHashSet<T>`
Dictionary (mutable) | - | `Dictionary<K, V>`
HashSet (mutable) | - | `HashSet<T>`
Enumerable | `seq<'T>` | `IEnumerable<T>`

---

# Other useful C# collections

* `Queue<T>`
* `PriorityQueue<T>`
* `ConcurrentDictionary<K, V>`

---
