using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Mainmenu
{
    public class GameplayOption : MonoBehaviour
    {
        //public bool isStory;

        public Sprite checklist;
        public Sprite defaultSprite; // Tambahkan ini untuk sprite default

        public Button storyButton;
        public Button freePlayButton;

        public enum PlayMode
        {
            blankMode,freePlay,stroyPlay
        }

        public PlayMode playMode = PlayMode.blankMode;

        private void Awake()
        {
            playMode = PlayMode.blankMode;
        }

        // Memulai FreePlayMode
        public void FreePlayMode()
        {
            //isStory = false;
            playMode = PlayMode.freePlay;
            ResetStoryMode(); // Reset UI story mode
            SetupFreeMode(); // Setup UI free mode
        }

        // Memulai StoryMode
        public void StoryyMode()
        {
            //isStory = true;
            playMode = PlayMode.stroyPlay;
            ResetFreeMode(); // Reset UI free mode
            SetupStoryMode(); // Setup UI story mode
        }

        // Menyiapkan FreeMode dengan mengganti sprite
        private void SetupFreeMode()
        {
            // Pastikan freePlayButton memiliki setidaknya 3 anak
            if (freePlayButton.transform.childCount >= 3)
            {
                // Mendapatkan anak ke-3 yang bernama "sprite"
                Transform spriteTransform = freePlayButton.transform.GetChild(2);

                // Memastikan "sprite" memiliki anak
                if (spriteTransform.childCount > 0)
                {
                    // Mendapatkan anak dari "sprite"
                    GameObject childObject = spriteTransform.GetChild(0).gameObject;
                    Image image = childObject.GetComponent<Image>();

                    // Pastikan komponen Image ditemukan
                    if (image != null)
                    {
                        image.sprite = checklist;
                    }
                    else
                    {
                        
                    }
                }
                else
                {
                    
                }
            }
            else
            {
                
            }
        }

        // Menyiapkan StoryMode dengan mengganti sprite
        private void SetupStoryMode()
        {
            // Pastikan storyButton memiliki setidaknya 3 anak
            if (storyButton.transform.childCount >= 3)
            {
                // Mendapatkan anak ke-3 yang bernama "sprite"
                Transform spriteTransform = storyButton.transform.GetChild(2);

                // Memastikan "sprite" memiliki anak
                if (spriteTransform.childCount > 0)
                {
                    // Mendapatkan anak dari "sprite"
                    GameObject childObject = spriteTransform.GetChild(0).gameObject;
                    Image image = childObject.GetComponent<Image>();

                    // Pastikan komponen Image ditemukan
                    if (image != null)
                    {
                        image.sprite = checklist;
                    }
                    else
                    {
                        Debug.LogWarning("No Image component found on the child of 'sprite'.");
                    }
                }
                else
                {
                    Debug.LogWarning("'sprite' does not have any children.");
                }
            }
            else
            {
                Debug.LogWarning("storyButton does not have enough children.");
            }
        }

        // Mengatur ulang UI Story Mode ke kondisi semula
        private void ResetStoryMode()
        {
            // Pastikan storyButton memiliki setidaknya 3 anak
            if (storyButton.transform.childCount >= 3)
            {
                // Mendapatkan anak ke-3 yang bernama "sprite"
                Transform spriteTransform = storyButton.transform.GetChild(2);

                // Memastikan "sprite" memiliki anak
                if (spriteTransform.childCount > 0)
                {
                    // Mendapatkan anak dari "sprite"
                    GameObject childObject = spriteTransform.GetChild(0).gameObject;
                    Image image = childObject.GetComponent<Image>();

                    // Pastikan komponen Image ditemukan
                    if (image != null)
                    {
                        image.sprite = defaultSprite; // Setel ke sprite default
                    }
                    else
                    {
                        Debug.LogWarning("No Image component found on the child of 'sprite'.");
                    }
                }
                else
                {
                    Debug.LogWarning("'sprite' does not have any children.");
                }
            }
            else
            {
                Debug.LogWarning("storyButton does not have enough children.");
            }
        }

        // Mengatur ulang UI Free Mode ke kondisi semula
        private void ResetFreeMode()
        {
            // Pastikan freePlayButton memiliki setidaknya 3 anak
            if (freePlayButton.transform.childCount >= 3)
            {
                // Mendapatkan anak ke-3 yang bernama "sprite"
                Transform spriteTransform = freePlayButton.transform.GetChild(2);

                // Memastikan "sprite" memiliki anak
                if (spriteTransform.childCount > 0)
                {
                    // Mendapatkan anak dari "sprite"
                    GameObject childObject = spriteTransform.GetChild(0).gameObject;
                    Image image = childObject.GetComponent<Image>();

                    // Pastikan komponen Image ditemukan
                    if (image != null)
                    {
                        image.sprite = defaultSprite; // Setel ke sprite default
                    }
                    else
                    {
                        Debug.LogWarning("No Image component found on the child of 'sprite'.");
                    }
                }
                else
                {
                    Debug.LogWarning("'sprite' does not have any children.");
                }
            }
            else
            {
                Debug.LogWarning("freePlayButton does not have enough children.");
            }
        }
    }
}
