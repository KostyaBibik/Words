using System.Collections.Generic;
using DataBase.Models;
using UI.Gameplay.WordContainers;

namespace UI.Services
{
    public interface IWordRepositoryTracker
    {
        public void Track(UIWordContainerPresenter container);
        public IReadOnlyList<ValidatedWordData> GetOrderedWords();
        public void Clear();
    }
}