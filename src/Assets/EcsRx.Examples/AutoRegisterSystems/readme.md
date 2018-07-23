# Auto System Registration Example

This example shows you how you can automatically load systems up based upon containing namespaces. 
This allows you to get your systems within the containing namespaces bound for free without needing to 
explicitly inject them anywhere and manually bind them, so you can just call `RegisterAllBoundSystems` in the application.

It shows the basics of:

- Automatically registering and setting up systems within specific namespaces

Setup systems are always registered first then any other systems, there is not much else happening in this scene other than a cube that moves every second.