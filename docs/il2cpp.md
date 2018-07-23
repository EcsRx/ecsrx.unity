# IL2CPP

There are a few things to keep in mind when using IL2CPP.

## Link.xml

You can create a link.xml file and place it in the assets folder to avoid any code stripping issues with the framework. This prevents all code from being stripped. It's possible there is a more specific version that would still work.

```
<linker>
    <assembly fullname="EcsRx">
        <type fullname="EcsRx.*" preserve="all"/>
    </assembly>
</linker>
```

## IReactToDataSystems

If you create an IReactToDataSystem for a value type, it will get stripped and cause issues. You can work around this by wrapping the value type in a class, or just don't make IReactToDataSystems for value types.
