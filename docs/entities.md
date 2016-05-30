# Entities

Entities are basically things that exist in your world with components on them, just like lots of little databases. Each entity has a unique ID and a list of components, these can be added and removed as you wish.

## Creating entities

So entities are created via pools (see the docs on pools for more info) so you don't need to do much here other than get the pool you wish to create your entity in and call the `CreateEntity` method which will return you a method to play with.