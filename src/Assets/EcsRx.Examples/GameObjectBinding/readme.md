# GameObject Binding Scene

This example shows the basics of how to create entities with the notion of views, as well as being able to tell a gameobject in the scene
to register and enitty and assign itself to the view using the `RegisterAsEntity` MonoBehaviour.

It shows the basics of:

- Using the ViewComponent and ViewResolver conventions
- Creating entities to back existing GameObjects

You do not have to use these conventions and can roll your own, however given a large part of unity is GameObject and MonoBehaviour based
there will come a point where you will need a sane way to access entities from GOs and access GOs from entities within the
ECS world. So these are meant to try and streamline the most common scenarios of trying to access and deal with entities which
are also going to be gameobjects.