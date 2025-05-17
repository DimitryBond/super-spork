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

        private void Update()
        {
            if (!isShowingMessages) return;

            if (Input.GetMouseButtonDown(0))
            {
                ShowNextMessage();
            }
        }

        public void ShowDialogue(string[] messages)
        {
            currentMessages = messages;
            currentMessageIndex = 0;
            isShowingMessages = true;

            ShowMessage(currentMessages[currentMessageIndex]);
        }

        private void ShowMessage(string message)
        {
            introText.text = message;
            canvasGroup.alpha = 1;
            canvasGroup.blocksRaycasts = true;
            canvasGroup.interactable = true;
        }

        private void HideMessage()
        {
            canvasGroup.alpha = 0;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;
        }

        private void ShowNextMessage()
        {
            currentMessageIndex++;

            if (currentMessageIndex < currentMessages.Length)
            {
                ShowMessage(currentMessages[currentMessageIndex]);
            }
            else
            {
                isShowingMessages = false;
                HideMessage();
            }
        }
    }
}