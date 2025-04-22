using System;
using UniRx;

namespace Core.Systems.WordContainer
{
    public interface IWordContainerStatus
    {
        public int Index { get; }
        public IObservable<Unit> OnFullyFilled { get; }
        public IObservable<Unit> OnBecameIncomplete { get; }
        public bool IsFullyFilled { get; }
    }
}