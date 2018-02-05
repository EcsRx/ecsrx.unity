using System;
using UniRx;

namespace BindingsRx.Filters
{
    public class DynamicSampleFilter<T> : IFilter<T>, IDisposable
    {
        private readonly IObservable<long> _sampleRateObservable;
        public ReactiveProperty<TimeSpan> SampleRate { get; set; }
        public ReactiveProperty<bool> Result { get; set; }
        public DynamicSampleFilter(TimeSpan sampleRate)
        {
            Result = new ReactiveProperty<bool>(true);
            SampleRate = new ReactiveProperty<TimeSpan>(sampleRate);
            _sampleRateObservable = SampleRate.Select(Observable.Interval).Switch();
        }

        public IObservable<T> InputFilter(IObservable<T> inputStream)
        { return inputStream.Sample(_sampleRateObservable); }

        public IObservable<T> OutputFilter(IObservable<T> outputStream)
        { return outputStream.Sample(_sampleRateObservable); }

        public void Dispose()
        {
            SampleRate.Dispose();
            Result.Dispose();
        }
    }
}