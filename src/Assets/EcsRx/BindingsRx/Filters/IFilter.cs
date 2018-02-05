using System;
using UniRx;

namespace BindingsRx.Filters
{
    public interface IFilter<T>
    {
        ReactiveProperty<bool> Result { get; set; }
        IObservable<T> InputFilter(IObservable<T> inputStream);
        IObservable<T> OutputFilter(IObservable<T> outputStream);
    }
}