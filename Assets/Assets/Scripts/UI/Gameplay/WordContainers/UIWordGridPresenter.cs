using Core.Factories;
using UI.Abstract;
using Zenject;

namespace UI.Gameplay
{
    public class UIWordGridPresenter : UIPresenter<UIWordGridView>
    {
        private IWordContainerFactory _wordContainerFactory;

        public UIWordGridPresenter(UIWordGridView view) : base(view)
        {
        }

        [Inject]
        public void Construct(IWordContainerFactory wordContainerFactory)
        {
            _wordContainerFactory = wordContainerFactory;
        }

        public void UpdateData(int wordCount, int lettersPerWord)
        {
            var containerPrefab = _view.ContainerPrefab;
            var containerParent = _view.ContainersParent;
            
            _wordContainerFactory.CreateWordContainers(containerPrefab, containerParent, lettersPerWord, wordCount);
        }
    }
}