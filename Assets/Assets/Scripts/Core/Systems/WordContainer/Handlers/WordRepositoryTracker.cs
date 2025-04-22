using System;
using System.Collections.Generic;
using System.Linq;
using Core.Services.Models;
using UniRx;

namespace Core.Systems.WordContainer
{
    public class WordRepositoryTracker
    {
        private readonly Dictionary<int, ValidatedWordData> _validatedWords = new();
        private readonly CompositeDisposable _disposable = new();

        public IReadOnlyList<ValidatedWordData> GetOrderedWords() =>
            _validatedWords.OrderBy(kvp => kvp.Value.filledOrder).Select(kvp => kvp.Value).ToList();

        private int _currentOrder;
        
        public void Track(IWordContainerStatus container, Func<int, ValidatedWordData> snapshotProvider)
        {
            container.OnFullyFilled
                .Subscribe(_ =>
                {
                    var snapshot = snapshotProvider(container.Index);
                    snapshot.filledOrder = _currentOrder++;
                    _validatedWords[container.Index] = snapshot;
                })
                .AddTo(_disposable);

            container.OnBecameIncomplete
                .Subscribe(_ =>
                {
                    if (_validatedWords.ContainsKey(container.Index))
                    {
                        _validatedWords.Remove(container.Index);
                        RecalculateOrder();
                    }
                })
                .AddTo(_disposable);
        }

        private void RecalculateOrder()
        {
            var ordered = _validatedWords
                .OrderBy(kvp => kvp.Value.filledOrder)
                .ToList();

            _currentOrder = 0;
            foreach (var entry in ordered)
            {
                entry.Value.filledOrder = _currentOrder++;
            }
        }
    }
}