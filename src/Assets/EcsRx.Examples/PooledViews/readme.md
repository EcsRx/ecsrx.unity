# Pooled Views Example Scene

This example shows you how to make use of the built in pooled scene views, it allows you to pre allocate as many instances as you want
and then keep reusing the same gameobjects in the pool for the entity `ViewComponent` instances.

It shows the basics of:

- Using the `ViewPool` and `DefaultPooledViewResolverSystem`

You can use your own pooling system by implementing your own verison of `PooledViewResolverSystem`, the `DefaultPooledViewResolverSystem`
just uses the built in simple pooler, however you can easily plug in any game object pooling system.