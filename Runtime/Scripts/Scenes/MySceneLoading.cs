using System;
using GameToFunLab.Scenes;
using TMPro;

namespace GameToFunLab.Runtime.Scripts.Scenes
{
    public class MySceneLoading : DefaultScene
    {
        public TextMeshProUGUI textLoadingPercent;

        private void Start() {
            if (textLoadingPercent != null)
            {
                textLoadingPercent.text = "0";
            }
        }
        public void SetTextLoadingPercent(float progress) {
            if (textLoadingPercent != null)
            {
                textLoadingPercent.text = $"데이터 불러오는 중...{Math.Floor(progress)}%";
            }
        }
    }
}