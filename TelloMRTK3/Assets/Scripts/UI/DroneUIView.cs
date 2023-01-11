using System;
using Microsoft.MixedReality.Toolkit.UX;
using UnityEngine;


namespace Hololux.Tello.UI
{
    public class DroneUIView : MonoBehaviour
    {
        [SerializeField] private PressableButton videoToggleButton;
        
        public EventHandler<bool> OnVideoToggleButtonClicked;
        
        private void OnEnable()
        {
            videoToggleButton.OnClicked.AddListener(OnVideoToggleButtonClick);
        }
        
        private void OnDisable()
        {
            videoToggleButton.OnClicked.RemoveListener(OnVideoToggleButtonClick);
        }
        
        private void OnVideoToggleButtonClick()
        {
            OnVideoToggleButtonClicked.Invoke(this, videoToggleButton.IsToggled.Active);
        }
    }
}
