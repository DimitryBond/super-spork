using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "DialogueData", menuName = "Game/DialogueData")]
    public class DialogueData : ScriptableObject
    {
        [SerializeField] private List<DialogueEntry> entries;

        private Dictionary<string, string[]> dialogueMap;

        public void Initialize()
        {
            dialogueMap = new Dictionary<string, string[]>();
            foreach (var entry in entries)
            {
                if (!dialogueMap.ContainsKey(entry.Id))
                    dialogueMap.Add(entry.Id, entry.Messages);
            }
        }

        public string[] GetDialogue(string id)
        {
            if (dialogueMap == null) Initialize();

            if (dialogueMap.TryGetValue(id, out var result))
            {
                return result;
            }

            Debug.LogWarning($"Диалог с ID '{id}' не найден.");
            return new[] { "..." };
        }

        public string[] GetHints(string id)
        {
            if (dialogueMap == null) Initialize();

            foreach (var entry in entries)
            {
                if (entry.Id == id)
                    return entry.Hints ?? Array.Empty<string>();
            }

            Debug.LogWarning($"Подсказки для диалога с ID '{id}' не найдены.");
            return Array.Empty<string>();
        }
    }

    [Serializable]
    public class DialogueEntry
    {
        public string Id;
        public string[] Messages;
        public string[] Hints; // <-- Новое поле
    }
}