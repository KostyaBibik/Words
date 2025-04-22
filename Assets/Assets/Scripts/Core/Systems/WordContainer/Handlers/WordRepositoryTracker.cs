using System;
using System.Collections.Generic;
using System.Linq;
using Core.Services.Models;
using UniRx;

namespace Core.Systems.WordContainer
{
    public class WordRepositoryTracker
    {
        private readonly Dictionary<int, ClusterData> _collectedWords = new();
        private readonly CompositeDisposable _disposable = new();

        public IReadOnlyList<ClusterData> GetOrderedWords() =>
            _collectedWords.OrderBy(kvp => kvp.Value.orderInWord).Select(kvp => kvp.Value).ToList();

        private int _currentOrder = 0;

        public void Track(IWordContainerStatus container, Func<int, ClusterData> snapshotProvider)
        {
            container.OnFullyFilled
                .Subscribe(_ =>
                {
                    var snapshot = snapshotProvider(container.Index);
                    snapshot.orderInWord = _currentOrder++;
                    _collectedWords[container.Index] = snapshot;
                })
                .AddTo(_disposable);

            container.OnBecameIncomplete
                .Subscribe(_ =>
                {
                    if (_collectedWords.ContainsKey(container.Index))
                    {
                        _collectedWords.Remove(container.Index);
                        RecalculateOrder();
                    }
                })
                .AddTo(_disposable);
        }

        private void RecalculateOrder()
        {
            var ordered = _collectedWords
                .OrderBy(kvp => kvp.Value.orderInWord)
                .ToList();

            _currentOrder = 0;
            foreach (var entry in ordered)
            {
                entry.Value.orderInWord = _currentOrder++;
            }
        }
    }
}