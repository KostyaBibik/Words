using Core.Services.Models;
using TMPro;
using UnityEngine;

namespace UI.Gameplay
{
    public class UIClusterElementView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;
        
        public void Setup(ClusterData clusterData)
        {
            _text.text = clusterData.value;
        }
    }
}