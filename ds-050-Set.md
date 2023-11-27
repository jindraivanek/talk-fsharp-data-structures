<!-- header: '**F# Data Structures**' -->

# Structural sharing

---

# F# Set

Like `Map`, but without values

---

## When to use Set instead of List?

- generally its faster to search for item with `Set`
- but for small sizes `List.constains` is faster