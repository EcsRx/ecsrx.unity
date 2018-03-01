# Group Accessor Filtering Example

This is an experimental feature, and provides you a way to filter on a group accessor.

The example shows how you can have a vast amount of entities in a pool and via a group accessor filter and sort
them in any way you want, such as only showing the top 5 entities based upon score or distance etc. 

It shows the basics of:

- Creating normal filters
- Creating cacheable filters

The cacheable filter in this example is extremely simple and just caches results every second, but this 
simple use case can be extended upon further.