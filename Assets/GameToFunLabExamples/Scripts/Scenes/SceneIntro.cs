using GameToFunLab.Core;
using GameToFunLab.Scenes;

namespace Scripts.Scenes
{
    public class SceneIntro : DefaultScene
    {
        public void OnClickGameStart()
        {
            SceneManager.LoadSceneByName("Loading");
        }
    }
}