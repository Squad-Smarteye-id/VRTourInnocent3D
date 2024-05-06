
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using Tproject.AudioManager;
using System.Linq;
using System;

namespace VirtualInnocent.Content
{
    public class ContentLiberary : MonoBehaviour
    {
        public static ContentLiberary Instance { get; private set; }
        [SerializeField] private List<MappingContent> contents;

        public T GetContent<T>(string _interplayName, int index) where T : class
        {
            try
            {
                if (!contents.Exists(x => x.interplayName == _interplayName))
                {
                    Debug.LogError($"Content {_interplayName} is null.");
                    return null;
                }

                var content = contents.FirstOrDefault(c => c.interplayName == _interplayName);

                // Check if content is not the default value, indicating it was found
                if (!content.Equals(default(MappingContent)) && content != null)
                {
                    if (typeof(T) == typeof(MappingContent.Info) && content.informations != null && index >= 0 && index < content.informations.Count)
                    {
                        return content.informations[index] as T;
                    }
                    else if (typeof(T) == typeof(MappingContent.Vid) && content.videos != null && index >= 0 && index < content.videos.Count)
                    {
                        return content.videos[index] as T;
                    }

                    Debug.LogError($"Content found, but no valid entry at index {index} or list is null for {_interplayName}.");
                }
                else
                {
                    Debug.LogError($"Interplay name '{_interplayName}' does not exist or content is uninitialized.");
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"An error occurred while retrieving content: {ex.Message}");
            }

            return null;
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            // ex:
            // var info = GetContent<MappingContent.Info>("smarteye", 0);
            // Debug.Log(info.text);
        }
    }

    [System.Serializable]
    public class MappingContent
    {
        public string interplayName;
        public List<Info> informations;

        [System.Serializable]
        public class Info
        {
            public Sprite sprite;
            [TextArea]
            public string title;
            [TextArea]
            public string text;
        }

        public List<Vid> videos;
        [System.Serializable]
        public class Vid
        {
            public Sprite thumbnail;
            public VideoClip videoClip;
        }
        public Sound[] voiceOver;
    }
}