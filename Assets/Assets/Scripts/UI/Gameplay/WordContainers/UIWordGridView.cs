using UI.Abstract;
using UI.Gameplay.Elements;
using UI.Juice;
using UnityEngine;

namespace UI.Gameplay
{
    public sealed class UIWordGridView : UIView
    {
        [SerializeField] private UIWordContainerView _containerPrefab;
        [SerializeField] private Transform _containersParent;
        [Tooltip("Optional stagger played once the word rows have been spawned.")]
        [SerializeField] private UIStaggerReveal _containersReveal;
        [Tooltip("Shrinks the grid so many words never overflow onto the buttons below.")]
        [SerializeField] private UIGridFitter _gridFitter;

        public UIWordContainerView ContainerPrefab => _containerPrefab;
        public Transform ContainersParent => _containersParent;
        public UIWordGridPresenter Presenter { get; private set; }


        public void Initialize(UIWordGridPresenter presenter) =>
            Presenter = presenter;

        public void PlayIntro()
        {
            // Fit before revealing: the stagger reads the settled layout.
            if (_gridFitter != null)
                _gridFitter.Fit();

            if (_containersReveal != null)
                _containersReveal.Play();
        }
    }
}