using System.Collections;
using UnityEngine;

namespace GameToFunLab.UI
{
    public class UIFloatingText : MonoBehaviour
    {
        public float moveDuration = 1.0f; // Time to move up
        public float fadeDuration = 0.5f; // Time to fade out
        public int moveY = 20;

        private Vector3 initialPosition;
        private Color initialColor;

        private void Awake()
        {
        }

        void Start()
        {
            StartCoroutine(AnimateText());
        }

        public void SetFontSize(float size)
        {
        }
        public void SetColor(Color color)
        {
        }
        public void SetText(string text)
        {
        }

        private IEnumerator AnimateText()
        {
            float elapsedTime = 0f;

            // Move text upwards
            while (elapsedTime < moveDuration)
            {
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            elapsedTime = 0f;

            // Fade out text
            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Optionally, destroy the GameObject after animation
            Destroy(gameObject);
        }
    }
}