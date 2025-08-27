using System;
using UI.Abstract;
using UI.Factories;
using UI.Services;
using UI.Victory.Grid;
using UniRx;
using Zenject;

namespace UI.Victory
{
    public sealed class UIVictoryPresenter : UIPresenter<UIVictoryView>
    {
        [Inject] private IWordRepositoryTracker _repositoryTracker;
        [Inject] private IUIWordContainerFactory _wordContainerFactory;
        private UIFinallyWordPresenter[] _finallyWords;

        public IObservable<Unit> OnMenuBtnClick => _view.MenuBtn.OnClick.AsObservable();
        public IObservable<Unit> OnContinueBtnClick => _view.ContinueBtn.OnClick.AsObservable();
        
        public UIVictoryPresenter(UIVictoryView view) : base(view)
        {
        }

        public override void Initialize() 
        {
            Hide();
        }

        protected override void BeforeShow()
        {
            var data = _repositoryTracker.GetOrderedWords();

            var gridElementPrefab = _view.ElementPrefab;
            var gridParentLayer = _view.GridTransform;

            _finallyWords = _wordContainerFactory.CreateFinallyWords(gridElementPrefab, gridParentLayer, data);
        }

        protected override void BeforeHide() => Clear();
        
        private void Clear()
        {
            if(_finallyWords == null)
                return;
            
            for (var iterator = 0; iterator < _finallyWords.Length; iterator++)
                _finallyWords[iterator].Destroy();
        }
    }
}