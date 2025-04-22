﻿using Core.Services;
using UI.Abstract;
using Zenject;

namespace UI.Gameplay
{
    public sealed class UIWordGridPresenter : UIPresenter<UIWordGridView>
    {
        private IWordContainersService _containersService;
        
        public UIWordGridPresenter(UIWordGridView view) : base(view)
        {
        }

        [Inject]
        public void Construct(IWordContainersService containersService)
        {
            _containersService = containersService;
        }

        public void UpdateData(int wordCount, int lettersPerWord)
        {
            var containerPrefab = _view.ContainerPrefab;
            var containerParent = _view.ContainersParent;
            
            _containersService.UpdateClusters(containerPrefab, containerParent, wordCount, lettersPerWord);
        }
    }
}