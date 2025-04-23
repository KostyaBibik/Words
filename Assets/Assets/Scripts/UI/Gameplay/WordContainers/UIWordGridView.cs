using UI.Abstract;
using UI.Gameplay.Elements;
using UnityEngine;

namespace UI.Gameplay
{
    public sealed class UIWordGridView : UIView
    {
        [SerializeField] private UIWordContainerView _containerPrefab;
        [SerializeField] private Transform _containersParent;
        
        public UIWordContainerView ContainerPrefab => _containerPrefab;
        public Transform ContainersParent => _containersParent;
        public UIWordGridPresenter Presenter { get; private set; }


        public void Initialize(UIWordGridPresenter presenter) =>
            Presenter = presenter;
    }
}