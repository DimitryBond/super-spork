using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace DefaultNamespace
{
    public class DialogSystem : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private TextMeshProUGUI introText;

        private string[] currentMessages;
        private int currentMessageIndex = 0;
        private bool isShowingMessages = false;
        private Coroutine typingCoroutine;

        private bool canTouch;

        [SerializeField] private float typingSpeed = 0.05f; // Скорость печати текста (сек на символ)
        [SerializeField] private float fadeDuration = 0.5f; // Время плавного появления/исчезновения

        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip[] randomSounds;
        [SerializeField] private Animator animator;

        private void Update()
        {
            if (!isShowingMessages) return;

            if (Input.GetMouseButtonDown(0))
            {
                if (!canTouch) return;
                if (typingCoroutine != null)
                {
                    StopCoroutine(typingCoroutine);
                    introText.text = currentMessages[currentMessageIndex];
                    typingCoroutine = null;
                }
                else
                {
                    ShowNextMessage();
                }
            }
        }

        public void ShowDialogue(string[] messages)
        {
            canTouch = true;
            introText.text = "";
            currentMessages = messages;
            currentMessageIndex = 0;
            isShowingMessages = true;

            GameManager.Instance.IsTaskActive = false;

            audioSource.Stop();

            StartCoroutine(ShowDialogueSequence());
        }

        private IEnumerator ShowDialogueSequence()
        {
            yield return StartCoroutine(FadeCanvas(0f, 1f, fadeDuration, null));

            ShowMessage(currentMessages[currentMessageIndex]);
        }


        private void ShowMessage(string message)
        {
            var randomIndex = UnityEngine.Random.Range(0, randomSounds.Length);
            audioSource.clip = randomSounds[randomIndex];
            audioSource.Play();

            animator.SetBool("Talk", true);
            if (typingCoroutine != null)
            {
                audioSource.Stop();
                StopCoroutine(typingCoroutine);
            }

            typingCoroutine = StartCoroutine(TypeText(message));
        }

        private IEnumerator TypeText(string message)
        {
            introText.text = "";
            foreach (char c in message)
            {
                introText.text += c;
                yield return new WaitForSeconds(typingSpeed);
            }

            typingCoroutine = null;

            if (audioSource.isPlaying) audioSource.Stop();
            animator.SetBool("Talk", false);
        }

        private void HideMessage()
        {
            canTouch = false;
            StartCoroutine(FadeCanvas(1f, 0f, fadeDuration, () => { isShowingMessages = false; }));
        }

        private IEnumerator FadeCanvas(float startAlpha, float endAlpha, float duration, System.Action onComplete)
        {
            float elapsed = 0f;
            canvasGroup.alpha = startAlpha;
            canvasGroup.blocksRaycasts = true;
            canvasGroup.interactable = true;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
                yield return null;
            }

            canvasGroup.alpha = endAlpha;

            bool visible = endAlpha > 0f;
            canvasGroup.blocksRaycasts = visible;
            canvasGroup.interactable = visible;

            onComplete?.Invoke();
        }

        public event Action OnDialogFinished;

        private void ShowNextMessage()
        {
            currentMessageIndex++;

            if (currentMessageIndex < currentMessages.Length)
            {
                audioSource.Stop();
                animator.SetBool("Talk", false);
                ShowMessage(currentMessages[currentMessageIndex]);
            }
            else
            {
                HideMessage();
                OnDialogFinished?.Invoke();
            }
        }
    }
}