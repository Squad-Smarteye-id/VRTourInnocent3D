using UnityEngine;
using UnityEditor;

namespace TProject
{
#if UNITY_EDITOR
    [CustomEditor(typeof(VideoPlayerController))]
    public class VideoPlayerControllerEditor : Editor
    {
        SerializedProperty videoClipProp;
        SerializedProperty onVideoFinishedProp;
        SerializedProperty _hideScreenControlTime;

        private void OnEnable()
        {
            // Mengambil properti yang akan ditampilkan
            videoClipProp = serializedObject.FindProperty("videoClip");
            onVideoFinishedProp = serializedObject.FindProperty("OnVideoFinished");
            _hideScreenControlTime = serializedObject.FindProperty("hideScreenControlTime");
        }

        public override void OnInspectorGUI()
        {
            // Pastikan untuk memulai dan mengakhiri dengan ini agar perubahan yang dilakukan oleh user disimpan
            serializedObject.Update();

            // Menampilkan hanya properti yang diinginkan di Inspector
            EditorGUILayout.PropertyField(videoClipProp);
            EditorGUILayout.PropertyField(onVideoFinishedProp);
            EditorGUILayout.PropertyField(_hideScreenControlTime);

            // Apply changes to the serializedProperty - always do this at the end of OnInspectorGUI.
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}