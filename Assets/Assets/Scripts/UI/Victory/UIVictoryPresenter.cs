using System;
using Core.Factories;
using Core.Systems.WordContainer;
using UI.Abstract;
using UniRx;
using Zenject;

namespace UI.Victory
{
    public class UIVictoryPresenter : UIPresenter<UIVictoryView>
    {
        private WordRepositoryTracker _repositoryTracker;
        private IUIWordContainerFactory _wordContainerFactory;

        public IObservable<Unit> OnMenuBtnClick => _view.MenuBtn.OnClickAsObservable();
        public IObservable<Unit> OnContinueBtnClick => _view.ContinueBtn.OnClickAsObservable();
        
        public UIVictoryPresenter(UIVictoryView view) : base(view)
        {
        }

        public override void Initialize()
        {
            Hide();
        }

        [Inject]
        public void Construct(
            WordRepositoryTracker repositoryTracker,
            IUIWordContainerFactory wordContainerFactory
        )
        {
            _repositoryTracker = repositoryTracker;
            _wordContainerFactory = wordContainerFactory;
        }
        
        protected override void BeforeShow()
        {
            var data = _repositoryTracker.GetOrderedWords();

            var gridElementPrefab = _view.ElementPrefab;
            var gridParentLayer = _view.GridTransform;

            var finallyWords = _wordContainerFactory.CreateFinallyWords(gridElementPrefab, gridParentLayer, data);

            //_view.UpdateView(data);
        }
    }
}