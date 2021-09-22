# Footnote Behaviour Trees

Footnote behaviour tree is a relatively simple implementation of the behaviour tree pattern for designing game AI.

Read more about behaviour trees in general [here](https://lmgtfy.app/?q=behaviour+tree).

## Usage

Footnote behaviour trees are entirely code driven. To create a behaviour tree, create a new class that extends `Brain`. You will need to implement one method `GetTree` that will create a behaviour tree and return the root node.

Brain is a MonoBehaviour. Attach your extending class to a GameObject and the tree will automatically run when the gameobject is active.

Nodes are initialised using their constructor. Some nodes require children to be specified in their constructor. Composite nodes that have multiple children implement the builder pattern:

```
new Sequence().Builder()
	.Add(new ChildNode())
	.Add(new ChildNode())
.Build()
```

Example nodes have been provided, including the node `RunAction`, which will run any arbitrary no argument function.

Each Brain has a Blackboard that can be used to store information that should exist outside of nodes (typically it represents what the AI knows about the state of the world).

## Implementation Notes

Footnote Behaviour Trees is event driven. This means that unlike many implements of behaviour trees, the tree is not ticked regularly. The tree only evaluates when a particular child node returns a signal. Children that run over multiple frames may be responsible for ticking themselves. For example, a NavTo node will tick every frame to check whether the AI reached its destination.

This means that writing your own nodes requires some discipline.

To write your own leaf node, implement the `Node` interface. To write a parent node, implement the `Node` and `ParentNode` interface.

Nodes can be MonoBehaviours - which means you can set it up in the inspector. Useful for things like referencing prefabs for nodes that spawn other GameObjects (ShootProjectile would be an example).

Parent Nodes must implement `HandleChildComplete`, `HandleChildFailed` and `HandleChildInterrupt`. This is the mechanism by which children report back signals to their parent.

Child Nodes must implement `Run` which takes a parent node as an argument.

**Always remember that child nodes must report a signal to their parent when their action is finished.** Due to the current implementation, the compiler will not tell you when you forgot to do this, and your tree will become stuck.

If your node does not run and complete or fail instantly, you may need to cache the parent so it can be called at a later time.

`HandleChildComplete` is to be called when the child succeeds its action.

`HandleChildFailed` is to be called when the child fails its action.

`HandleChildInterrupt` is a special case that allows children to flag to their parent that the action is not complete (or was previously completed), but nevertheless the parent should re-evaluate running children.

At the moment, `HandleChildInterrupt` is only implemented for `Selector`, and is called only by `ConditionalRun`. It is for the scenario where a selector has moved past a failed node, but the failed node reports that it should be tried again.

## Samples

// Sequence. Waits for the activation trigger, then moves to the main tree.
// --- Nice and clean, and easy to think about. It's the preferred option!

Node tree = new Sequence().Builder()
    .Add(new WaitForCondition(this, 0.1f, () => ActivationTrigger()))
    .Add(new RunAction(() => OnActivation()))
    .Add(GetMainTree())
    .Build();
        
// Selector. Activation Trigger fails, so the conditional run fails, and the tree moves to infinite wait.
// When the activation triggers becomes true, the conditional run will interrupt the wait and run the main tree.
// --- Much harder to reason about. Not preferred! But can be extended for interesting behaviour!

Node tree = new Selector().Builder()
	.Add(ConditionalRun.WithReevaluate(this, () => fleeTrigger(), GetFleeTree()))
	.Add(new Wait(this))
	.Build();