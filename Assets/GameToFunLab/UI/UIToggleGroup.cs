﻿using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace GameToFunLab.UI
{
    /// <summary>
    /// Toogle 버튼의 이름을 Unum 으로 사용한다 
    /// </summary>
    public class UIToggleGroup : MonoBehaviour
    {
        private ToggleGroup toggleGroup;

        private void Start()
        {
            toggleGroup = GetComponent<ToggleGroup>();
        }
        
        public int GetToggleUnum()
        {
            if (toggleGroup == null) return -1;
            Toggle selectedToggle = toggleGroup.ActiveToggles().FirstOrDefault();
            if (selectedToggle == null) return -1;
            return int.Parse(selectedToggle.name);
        }
    }
}