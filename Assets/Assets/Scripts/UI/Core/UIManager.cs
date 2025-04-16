using UnityEngine;
using Zenject;

namespace Assets.Scripts.UI.Core
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