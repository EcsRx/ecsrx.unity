# Plugins Example

This example shows how to make use of the plugin interface to create your own plugins to share with others, these plugins can register dependencies for
various classes or overwrite framework components, as well as allowing you to automatically register systems.

It shows the basics of:

- Creating a plugin using the `IEcsRxPlugin` interface
- Registering dependencies with the plugin
- Registering systems with the plugin
- Registering the plugin and using the plugin components in the `AppContainer`

You are free to do what you want within plugins, and this is just a simple example of how they can be used.