using GameToFunLab.Core;
using GameToFunLab.Scenes;

namespace Scripts.Scenes
{
    public class MySceneIntro : DefaultScene
    {
        public void OnClickGameStart()
        {
            SceneManager.LoadSceneByName("Loading");
        }
    }
}