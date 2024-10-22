using UnityEngine;

namespace GameToFunLab.Scenes
{
    public class SceneManager : MonoBehaviour
    {
        public static void LoadSceneByName(string sceneName)
        {
            // 지정된 이름의 씬을 로드합니다.
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        }
    }
}