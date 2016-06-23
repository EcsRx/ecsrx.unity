# Manual Systems

Manual systems provide you an alternative way to run logic, you are notified when the system starts and stops, then within there you can
either kick off since actions, or create observables for your own purposes, the benefit here is that you can create your own eco system
within the manual system, but you also need to clean up after yourself and do more work if you were to do much entity interaction.

It shows the basics of:

- Using `IManualSystem` implementations to run isolated logic

This is generally a niche use case where you want to run logic maybe not against entities, or have some sort of high level orchestration
within a system or timing based things which are not tied to a single entity type etc. There are many use cases but most common conventions
are already represented via the other systems.