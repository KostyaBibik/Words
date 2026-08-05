using System.Collections.Generic;
using DataBase.Models;
using UI.Gameplay.WordContainers;
using UI.Services;
using UniRx;
using Zenject;

namespace Core.Services.Progress
{
    /// <summary>
    /// Tracks how many level words are currently assembled correctly, without waiting for
    /// the explicit "Validate" press. Drives both the progress counter and the per-row
    /// solved highlight, so the player gets feedback the moment a word clicks into place.
    /// </summary>
    public sealed class WordProgressService : IWordProgressService
    {
        private readonly ReactiveProperty<int> _solvedCount = new(0);
        private readonly CompositeDisposable _disposable = new();
        private readonly List<string> _expectedWords = new();
        private readonly List<string> _remainingBuffer = new();
        private readonly List<WordEntry> _unsolvedBuffer = new();

        [Inject] private readonly IWordContainersService _containersService;

        private WordEntry[] _wordEntries = System.Array.Empty<WordEntry>();
        private UIWordContainerPresenter[] _containers;

        public IReadOnlyReactiveProperty<int> SolvedCount => _solvedCount;
        public int TotalCount => _expectedWords.Count;

        public void Initialize(ProcessedLevelData level)
        {
            Clear();

            _containers = _containersService.ContainerPresenters;
            _wordEntries = level.words;

            for (var i = 0; i < level.words.Length; i++)
                _expectedWords.Add(Normalize(level.words[i].word));

            if (_containers == null)
                return;

            for (var i = 0; i < _containers.Length; i++)
            {
                _containers[i].FilledState
                    .Subscribe(_ => Recalculate())
                    .AddTo(_disposable);
            }

            Recalculate();
        }

        public IReadOnlyList<WordEntry> GetUnsolvedWords()
        {
            Recalculate();

            return _unsolvedBuffer;
        }

        public void Clear()
        {
            _disposable.Clear();
            _expectedWords.Clear();
            _unsolvedBuffer.Clear();
            _remainingBuffer.Clear();
            _wordEntries = System.Array.Empty<WordEntry>();
            _containers = null;
            _solvedCount.Value = 0;
        }

        private void Recalculate()
        {
            if (_containers == null)
                return;

            // Each expected word may only be matched once, so consume from a working copy.
            _remainingBuffer.Clear();
            _remainingBuffer.AddRange(_expectedWords);

            var solved = 0;

            for (var i = 0; i < _containers.Length; i++)
            {
                var container = _containers[i];
                var isSolved = false;

                if (container.IsFullyFilled)
                {
                    var assembled = Normalize(container.GetAssembledWord());
                    var matchIndex = _remainingBuffer.IndexOf(assembled);

                    if (matchIndex >= 0)
                    {
                        _remainingBuffer.RemoveAt(matchIndex);
                        isSolved = true;
                        solved++;
                    }
                }

                container.SetSolved(isSolved);
            }

            RebuildUnsolvedBuffer();

            _solvedCount.Value = solved;
        }

        /// <summary>_remainingBuffer holds the words nobody matched; map them back to their entries.</summary>
        private void RebuildUnsolvedBuffer()
        {
            _unsolvedBuffer.Clear();

            var taken = new bool[_wordEntries.Length];

            for (var i = 0; i < _remainingBuffer.Count; i++)
            {
                for (var j = 0; j < _wordEntries.Length; j++)
                {
                    if (taken[j] || Normalize(_wordEntries[j].word) != _remainingBuffer[i])
                        continue;

                    taken[j] = true;
                    _unsolvedBuffer.Add(_wordEntries[j]);
                    break;
                }
            }
        }

        private static string Normalize(string value) =>
            string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpperInvariant();
    }
}
