# PowerCraft: a Minecraft clone in Unity3D

## Personal project notes

### Optimization & architectural ideas

- `Block` inherits from `BaseBlock` as a very crude way to optimize memory usage during terrain generation. `Block` and `Item`, however, share a lot of functionality. Logically, they also stand similar. `Block` and `Item` should inherit from a common base class.
- As of right now, each block is represented by its own class. That's perfectly fine since each block implements its own custom behaviour. Runtime instantiation, however, is very expensive. A lot of the instantiation is done just to recuperate block & item basic properties, thus bringing to the following new potential structure.

Basic `BaseObject` information (`Block` and `Item`) should be stored inside JSON files. At load time, every JSON file gets parsed and cached cutting info access time to its bone, given each parsed instance would be stored in a hashtable.
Since `BaseObject`'s are now available, the `BaseObject` hash could be used during terrain generation.
