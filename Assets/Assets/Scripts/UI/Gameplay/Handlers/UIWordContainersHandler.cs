using System.Collections.Generic;
using UI.Gameplay.Elements;
using UnityEngine;

namespace UI.Gameplay
{
    public class UIWordContainersHandler : MonoBehaviour
    {
        [SerializeField] private UIWordContainerView _containerPrefab;
        [SerializeField] private Transform _containersParent;
    
        private List<UIWordContainerView> _containers = new(); 

        public void Initialize(int wordCount, int lettersPerWord)
        {
            ClearContainers();
        
            for (var i = 0; i < wordCount; i++)
            {
                var container = Instantiate(_containerPrefab, _containersParent);
                container.Initialize(lettersPerWord);
                _containers.Add(container);
            }
        }

        private void ClearContainers()
        {
            foreach (var container in _containers)
                Destroy(container.gameObject);
            _containers.Clear();
        }
    }
}