using System.Collections.Generic;
using System.Linq;
using DataBase.Models;
using UI.Gameplay.WordContainers;
using UI.Services;
using UniRx;

namespace Core.Systems.WordContainer
{
    public sealed class WordRepositoryTracker : IWordRepositoryTracker
    {
        private readonly Dictionary<int, ValidatedWordData> _validatedWords = new();
        private readonly CompositeDisposable _disposable = new();

        private int _currentOrder;

        public IReadOnlyList<ValidatedWordData> GetOrderedWords() =>
            _validatedWords
                .OrderBy(kvp => kvp.Value.filledOrder)
                .Select(kvp => kvp.Value)
                .ToList();

        public void Track(UIWordContainerPresenter container)
        {
            container.OnFullyFilled
                .Subscribe(_ =>
                {
                    var data = BuildValidatedWordData(container);
                    data.filledOrder = _currentOrder++;
                    _validatedWords[container.Index] = data;
                })
                .AddTo(_disposable);

            container.OnBecameIncomplete
                .Subscribe(_ =>
                {
                    if (_validatedWords.Remove(container.Index))
                        RecalculateOrder();
                })
                .AddTo(_disposable);
        }

        public void Clear()
        {
            _currentOrder = 0;
            _disposable?.Clear();
            _validatedWords.Clear();
        }

        private void RecalculateOrder()
        {
            var ordered = _validatedWords
                .OrderBy(kvp => kvp.Value.filledOrder)
                .ToList();

            _currentOrder = 0;
            foreach (var entry in ordered)
                entry.Value.filledOrder = _currentOrder++;
        }

        private ValidatedWordData BuildValidatedWordData(UIWordContainerPresenter container)
        {
            return new ValidatedWordData
            {
                text = new string(container.GetPlacedClusters()
                    .OrderBy(kvp => kvp.Value)
                    .SelectMany(kvp => kvp.Key.GetData().value)
                    .ToArray())
            };
        }
    }
}
