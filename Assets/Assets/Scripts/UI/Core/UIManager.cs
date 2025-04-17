using UI.Abstract;
using UnityEngine;
using Zenject;

namespace UI.Core
{
    public class UIManager : MonoBehaviour
    {
        [Inject] private DiContainer _container;
    
        private void Awake()
        {
            foreach (Transform child in transform)
            {
                var view = child.GetComponent<IUIView>();
                if (view != null)
                {
                    _container.InjectGameObject(child.gameObject);
                }
            }
        }
    }
}