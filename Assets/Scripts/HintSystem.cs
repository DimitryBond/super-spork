using System.Collections;
using TMPro;
using UnityEngine;

namespace DefaultNamespace
{
    public class HintSystem : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private TextMeshProUGUI hintText;
        [SerializeField] private float fadeDuration = 0.5f;
        [SerializeField] private float typingSpeed = 0.05f;
        [SerializeField] private AudioSource audioSource;

        private string[] currentHints;
        private int currentHintIndex;
        private Coroutine typingCoroutine;

        public void ShowHints(string[] hints)
        {
            currentHints = hints;
            currentHintIndex = 0;

            if (currentHints.Length > 0)
            {
                if (typingCoroutine != null)
                    StopCoroutine(typingCoroutine);

                StartCoroutine(FadeCanvas(0f, 1f, fadeDuration, () => {
                    typingCoroutine = StartCoroutine(TypeText(currentHints[currentHintIndex]));
                }));
            }
        }

        public void HideHints()
        {
            if (typingCoroutine != null)
                StopCoroutine(typingCoroutine);

            StartCoroutine(FadeCanvas(1f, 0f, fadeDuration));
        }

        private IEnumerator TypeText(string message)
        {
            hintText.text = "";

            foreach (char c in message)
            {
                hintText.text += c;
                yield return new WaitForSeconds(typingSpeed);
            }

            typingCoroutine = null;
        }

        private IEnumerator FadeCanvas(float from, float to, float duration, System.Action onComplete = null)
        {
            float elapsed = 0f;
            canvasGroup.alpha = from;
            canvasGroup.blocksRaycasts = to > 0f;
            canvasGroup.interactable = to > 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(from, to, elapsed / duration);
                yield return null;
            }

            canvasGroup.alpha = to;

            canvasGroup.blocksRaycasts = to > 0f;
            canvasGroup.interactable = to > 0f;

            onComplete?.Invoke();
        }
    }
}
