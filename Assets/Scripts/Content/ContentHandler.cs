using UnityEngine.UI;
using UnityEngine;
using TProject;
using TMPro;

namespace VRInnocent.Content
{
    public class ContentHandler : MonoBehaviour
    {
        public string interplayName;
        public enum ctType
        {
            information,
            video
        }
        public ctType contentType;
        public int contentId;

        [Space]

        public Image thumbnailImage;
        public VideoPlayerController videoController;

        public TextMeshProUGUI titleText;
        public TextMeshProUGUI contentText;
        public Image contentImg;

        [Space]
        [SerializeField] private bool isGetOnStart;

        private void Start()
        {
            if (isGetOnStart && IsComponentsValid())
            {
                switch (contentType)
                {
                    case ContentHandler.ctType.video:
                        var vid = ContentLiberary.Instance.GetContent<MappingContent.Vid>(interplayName, contentId);
                        thumbnailImage.sprite = vid.thumbnail;
                        videoController.videoClip = vid.videoClip;
                        break;

                    case ContentHandler.ctType.information:
                        var info = ContentLiberary.Instance.GetContent<MappingContent.Info>(interplayName, contentId);
                        titleText.text = info.title;
                        contentText.text = info.text;
                        contentImg.sprite = info.sprite;
                        break;
                }
            }
        }

        public bool IsComponentsValid()
        {
            switch (contentType)
            {
                case ctType.video:
                    CheckComponent(thumbnailImage, "Thumbnail image is missing.");
                    CheckComponent(videoController, "Video controller is missing.");
                    return thumbnailImage != null && videoController != null;

                case ctType.information:
                    CheckComponent(titleText, "Title text is missing.");
                    CheckComponent(contentText, "Content text is missing.");
                    CheckComponent(contentImg, "Content image is missing.");
                    return titleText != null && contentText != null && contentImg != null;

                default:
                    return false;
            }
        }

        private void CheckComponent(Object component, string errorMessage)
        {
            if (component == null)
            {
                Debug.LogWarning(errorMessage);
            }
        }
    }
}