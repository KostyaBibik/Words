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
    }
}