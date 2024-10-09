using System.Collections;
using TMPro;
using UnityEngine;

namespace GameToFunLab.UI
{
    public class UITextFadeInOut : MonoBehaviour
    {
        private TextMeshProUGUI text; // TextMeshPro UGUI 컴포넌트를 할당
        public float fadeDuration = 1.0f; // 페이드 인/아웃 지속 시간
        public bool repeat; // 반복 여부 설정
        public float startAlpha; // 시작 알파값
        public float endAlpha = 1f; // 종료 알파값

        private void Awake() {
            text = GetComponent<TextMeshProUGUI>();
        }
        private void Start()
        {
            StartCoroutine(FadeText());
        }

        private IEnumerator FadeText()
        {
            while (true)
            {
                // 페이드 인
                yield return StartCoroutine(Fade(startAlpha, endAlpha));

                if (!repeat)
                    yield break;

                // 페이드 아웃
                yield return StartCoroutine(Fade(endAlpha, startAlpha));
            }
        }

        private IEnumerator Fade(float fromAlpha, float toAlpha)
        {
            float elapsedTime = 0;
            Color color = text.color;
            color.a = fromAlpha;
            text.color = color;

            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                color.a = Mathf.Lerp(fromAlpha, toAlpha, elapsedTime / fadeDuration);
                text.color = color;
                yield return null;
            }

            color.a = toAlpha;
            text.color = color;
        }
    }
}
