using System.Collections.Generic;
using DataBase.Models;
using UniRx;

namespace Core.Services
{
    public interface IWordProgressService
    {
        IReadOnlyReactiveProperty<int> SolvedCount { get; }
        int TotalCount { get; }

        /// <summary>Binds to the containers currently created for the level.</summary>
        void Initialize(ProcessedLevelData level);

        /// <summary>Words from the level that are not currently assembled in any container.</summary>
        IReadOnlyList<WordEntry> GetUnsolvedWords();

        void Clear();
    }
}
