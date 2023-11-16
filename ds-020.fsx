(**
<!-- header: '**F# Data Structures**' -->

# Immutable Data Structures

---

# Why?
- mutation is common source of bugs
- immutable data structures are easier to reason about
  - when you pass a value to a function, you know it won't be changed
- immutable data structures are thread-safe

---

# How?
- MYTH: to create new immutable value, you need to copy the whole thing
- you can share parts of the structure between old and new value

*)