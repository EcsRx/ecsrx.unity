using System;
using UniRx;
using UnityEngine;

namespace BindingsRx.Filters
{
    public class SampleFilter<T> : IFilter<T>, IDisposable
    {
        public TimeSpan SampleRate { get; set; }
        public ReactiveProperty<bool> Result { get; set; }
        public SampleFilter(TimeSpan sampleRate)
        {
            Result = new ReactiveProperty<bool>(true);
            SampleRate = sampleRate;
        }

        public IObservable<T> InputFilter(IObservable<T> inputStream)
        { return inputStream.Sample(SampleRate); }

        public IObservable<T> OutputFilter(IObservable<T> outputStream)
        { return outputStream.Sample(SampleRate); }


        public void Dispose()
        {
            Result.Dispose();
        }
    }
}