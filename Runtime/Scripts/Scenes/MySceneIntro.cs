﻿using GameToFunLab.Scenes;

namespace GameToFunLab.Runtime.Scripts.Scenes
{
    public class MySceneIntro : DefaultScene
    {
        public void OnClickGameStart()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Loading");
        }
    }
}