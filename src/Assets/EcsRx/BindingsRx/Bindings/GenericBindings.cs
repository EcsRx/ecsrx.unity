using System;
using BindingsRx.Convertors;
using BindingsRx.Exceptions;
using BindingsRx.Extensions;
using BindingsRx.Filters;
using UniRx;

namespace BindingsRx.Bindings
{
    public static class GenericBindings
    {
        public static IDisposable Bind<T>(IReactiveProperty<T> propertyA , IReactiveProperty<T> propertyB, BindingTypes bindingTypes = BindingTypes.Default, params IFilter<T>[] filters)
        {
            var propertyBBinding = propertyB
                .ApplyInputFilters(filters)
                .DistinctUntilChanged()
                .Subscribe(x => propertyA.Value = x);

            if (bindingTypes == BindingTypes.OneWay)
            { return propertyBBinding; }

            var propertyABinding = propertyA
                .ApplyOutputFilters(filters)
                .DistinctUntilChanged()
                .Subscribe(x => propertyB.Value = x);

            return new CompositeDisposable(propertyABinding, propertyBBinding);
        }
        
        public static IDisposable Bind<T>(Func<T> propertyAGetter, Action<T> propertyASetter, IReactiveProperty<T> propertyB, BindingTypes bindingTypes = BindingTypes.Default, params IFilter<T>[] filters)
        {
            var propertyBBinding = propertyB
                .ApplyInputFilters(filters)
                .DistinctUntilChanged()
                .Subscribe(propertyASetter);

            if (bindingTypes == BindingTypes.OneWay)
            { return propertyBBinding; }

            var propertyABinding = Observable.EveryUpdate()
                .Select(x => propertyAGetter())
                .ApplyOutputFilters(filters)
                .DistinctUntilChanged()
                .Subscribe(x => propertyB.Value = propertyAGetter());

            return new CompositeDisposable(propertyABinding, propertyBBinding);
        }

        public static IDisposable Bind<T>(Func<T> propertyAGetter, Action<T> propertyASetter, Func<T> propertyBGetter, Action<T> propertyBSetter, BindingTypes bindingTypes = BindingTypes.Default, params IFilter<T>[] filters)
        {
            var propertyBBinding = Observable.EveryUpdate()
                .Select(x => propertyBGetter())
                .ApplyInputFilters(filters)
                .DistinctUntilChanged()
                .Subscribe(propertyASetter);

            if (bindingTypes == BindingTypes.OneWay)
            { return propertyBBinding; }

            if (propertyBSetter == null)
            { throw new SetterNotProvidedException(); }

            var propertyABinding = Observable.EveryUpdate()
                .Select(x => propertyAGetter())
                .ApplyOutputFilters(filters)
                .DistinctUntilChanged()
                .Subscribe(propertyBSetter);

            return new CompositeDisposable(propertyABinding, propertyBBinding);
        }

        public static IDisposable Bind<T1, T2>(IReactiveProperty<T1> propertyA, IReactiveProperty<T2> propertyB, IConvertor<T1, T2> convertor, BindingTypes bindingTypes = BindingTypes.Default, params IFilter<T1>[] filters)
        {
            var propertyBBinding = propertyB
                .Select(convertor.From)
                .ApplyInputFilters(filters)
                .DistinctUntilChanged()
                .Subscribe(x => propertyA.Value = x);

            if (bindingTypes == BindingTypes.OneWay)
            { return propertyBBinding; }

            var propertyABinding = propertyA
                .ApplyOutputFilters(filters)
                .DistinctUntilChanged()
                .Select(convertor.From)
                .Subscribe(x => propertyB.Value = x);

            return new CompositeDisposable(propertyABinding, propertyBBinding);
        }

        public static IDisposable Bind<T1, T2>(Func<T1> propertyAGetter, Action<T1> propertyASetter, IReactiveProperty<T2> propertyB, IConvertor<T1, T2> convertor, BindingTypes bindingTypes = BindingTypes.Default, params IFilter<T1>[] filters)
        {
            var propertyBBinding = propertyB
                .Select(convertor.From)
                .ApplyInputFilters(filters)
                .DistinctUntilChanged()
                .Subscribe(propertyASetter);

            if (bindingTypes == BindingTypes.OneWay)
            { return propertyBBinding; }

            var propertyABinding = Observable.EveryUpdate()
                .Select(x => propertyAGetter())
                .ApplyOutputFilters(filters)
                .DistinctUntilChanged()
                .Subscribe(x => propertyB.Value = convertor.From(propertyAGetter()));

            return new CompositeDisposable(propertyABinding, propertyBBinding);
        }

        public static IDisposable Bind<T1, T2>(Func<T1> propertyAGetter, Action<T1> propertyASetter, Func<T2> propertyBGetter, Action<T2> propertyBSetter, IConvertor<T1, T2> convertor, BindingTypes bindingTypes = BindingTypes.Default, params IFilter<T1>[] filters)
        {
            var propertyBBinding = Observable.EveryUpdate()
                .Select(x => convertor.From(propertyBGetter()))
                .ApplyInputFilters(filters)
                .DistinctUntilChanged()
                .Subscribe(propertyASetter);

            if (bindingTypes == BindingTypes.OneWay)
            { return propertyBBinding; }

            if (propertyBSetter == null)
            { throw new SetterNotProvidedException(); }

            var propertyABinding = Observable.EveryUpdate()
                .Select(x => propertyAGetter())
                .ApplyOutputFilters(filters)
                .DistinctUntilChanged()
                .Select(convertor.From)
                .Subscribe(propertyBSetter);

            return new CompositeDisposable(propertyABinding, propertyBBinding);
        }
    }
}