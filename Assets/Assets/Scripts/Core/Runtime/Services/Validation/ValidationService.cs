using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UI.Services;
using UniRx;
using Zenject;

namespace Core.Services.Validation
{
    public sealed class ValidationService : IValidationService
    {
        [Inject] private readonly IGameDataRepository _gameDataRepository;
        [Inject] private readonly IWordContainersService _containersService;

        private readonly ReactiveProperty<bool> _validationStatus = new(false);
        private readonly List<string> _remainingWords = new();

        public IReadOnlyReactiveProperty<bool> ValidationStatus => _validationStatus;

        // Runs synchronously (no UniTask.RunOnThreadPool): WebGL has no real ThreadPool, so
        // SwitchToThreadPool() there registers a callback nothing ever services, and the
        // await hangs forever. The comparison below is cheap in-memory work anyway.
        public UniTask<bool> Validate()
        {
            _validationStatus.Value = AreAllWordsAssembled();

            return UniTask.FromResult(_validationStatus.Value);
        }

        public void Clear() => _validationStatus.Value = false;

        /// <summary>
        /// Compares what each row currently spells against the level's words, as a multiset.
        /// Reading the assembled letters is the only source of truth that cannot drift: it is
        /// derived from the slot indices the clusters actually occupy, so it stays correct no
        /// matter how many times a piece was moved between rows, pulled back to the pool, or
        /// how many times the player pressed "check" along the way. The previous implementation
        /// matched per-cluster bookkeeping (orderInWord / wordGroupIndex) that gameplay rewrites
        /// on the level's own answer data, and bailed out of the whole check on the first
        /// candidate word that did not line up instead of trying the next one.
        /// </summary>
        private bool AreAllWordsAssembled()
        {
            var level = _gameDataRepository.CurrentLevel;

            if (level?.words == null || level.words.Length == 0)
                return false;

            var containers = _containersService.ContainerPresenters;

            if (containers == null || containers.Length != level.words.Length)
                return false;

            // Each expected word may only be matched once, so consume from a working copy.
            _remainingWords.Clear();

            for (var i = 0; i < level.words.Length; i++)
                _remainingWords.Add(Normalize(level.words[i].word));

            for (var i = 0; i < containers.Length; i++)
            {
                var assembled = Normalize(containers[i].GetAssembledWord());

                if (assembled.Length == 0)
                    return false;

                var matchIndex = _remainingWords.IndexOf(assembled);

                if (matchIndex < 0)
                    return false;

                _remainingWords.RemoveAt(matchIndex);
            }

            return _remainingWords.Count == 0;
        }

        private static string Normalize(string value) =>
            string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpperInvariant();
    }
}
