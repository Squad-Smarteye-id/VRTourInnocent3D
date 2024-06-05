using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interplay
{
    public class InterplayHandler : MonoBehaviour
    {
        [Header("Handler Settings")][Space(5)]
        public int interplayId;
        public string interplayName;

        public bool isVisited;

        [Header("Video and Photo Settings")][Space(5)]
        public List<VidioProgress> vidioProgresses;
        public List<PhotoProgress> photoProgresses;

        public void AddVidioValue(int _index)
        {
            if (_index >= 0 && _index < vidioProgresses.Count)
            {
                // Mengambil elemen struct dari list
                VidioProgress vidioProgress = vidioProgresses[_index];

                // Mengubah nilai boolean
                vidioProgress.vidioState = true;

                // Mengembalikan struct yang telah dimodifikasi ke dalam list
                vidioProgresses[_index] = vidioProgress;

                // Mengambil vidioId
                int vidioId = vidioProgress.vidioId;

                Debug.Log("Video with ID: " + vidioId + " has been marked as seen.");
            }
            else
            {
                Debug.LogError("Index out of range: " + _index);
            }

            CheckVisited();
        }

        public void AddPhotoValue(int _index)
        {
            if (_index >= 0 && _index < photoProgresses.Count)
            {
                // Mengambil elemen struct dari list
                PhotoProgress photoProgress = photoProgresses[_index];

                // Mengubah nilai boolean
                photoProgress.photoState = true;

                // Mengembalikan struct yang telah dimodifikasi ke dalam list
                photoProgresses[_index] = photoProgress;

                // Mengambil photoId
                int photoId = photoProgress.photoId;

                Debug.Log("Photo with ID: " + photoId + " has been marked as seen.");
            }
            else
            {
                Debug.LogError("Index out of range: " + _index);
            }

            CheckVisited();
        }

        public bool CheckVisited()
        {
            // Menghitung jumlah vidio yang telah dilihat
            int vidioSeenCount = 0;
            foreach (var vidio in vidioProgresses)
            {
                if (vidio.vidioState)
                {
                    vidioSeenCount++;
                }
            }

            // Menghitung jumlah foto yang telah dilihat
            int photoSeenCount = 0;
            foreach (var photo in photoProgresses)
            {
                if (photo.photoState)
                {
                    photoSeenCount++;
                }
            }

            // Mengatur kondisi boolean untuk vidio dan foto
            bool vidioState = vidioSeenCount == vidioProgresses.Count;
            bool photoState = photoSeenCount == photoProgresses.Count;

            // Mengatur isVisited dan mengembalikan nilai
            isVisited = vidioState && photoState;
            return isVisited;
        }
    }

    [System.Serializable]
    public struct VidioProgress
    {
        public int vidioId;
        public bool vidioState;
    }

    [System.Serializable]
    public struct PhotoProgress
    {
        public int photoId;
        public bool photoState;
    }
}
