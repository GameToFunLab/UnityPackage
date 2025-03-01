using GameToFunLab.Scenes;

namespace Scripts.Scenes
{
    public class MySceneIntro : DefaultScene
    {
        public void OnClickGameStart()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Loading");
        }
    }
}