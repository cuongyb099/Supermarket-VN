using System;
using UnityEngine;

namespace Core.Interact
{
    public interface IIndicatable
    {
        public Action<Color> OnEnableIndicator { get; set; }
        public Action OnDisableIndicator { get; set; }
        public void EnableIndicator(Color color);
        public void DisableIndicator();
    }
}
