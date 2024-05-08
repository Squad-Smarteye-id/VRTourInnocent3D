
using UnityEditor;
namespace VirtualInnocent.Content
{
#if UNITY_EDITOR
    [CustomEditor(typeof(ContentHandler))]
    public class ContentHandlerEditor : Editor
    {
        SerializedProperty interplayNameProp;
        SerializedProperty contentTypeProp;
        SerializedProperty contentIdProp;
        SerializedProperty thumbnailImageProp;
        SerializedProperty videoControllerProp;
        SerializedProperty titleTextProp;
        SerializedProperty contentTextProp;
        SerializedProperty contentImgProp;
        SerializedProperty isGetOnStartProp;

        private void OnEnable()
        {
            interplayNameProp = serializedObject.FindProperty("interplayName");
            contentTypeProp = serializedObject.FindProperty("contentType");
            contentIdProp = serializedObject.FindProperty("contentId");
            thumbnailImageProp = serializedObject.FindProperty("thumbnailImage");
            videoControllerProp = serializedObject.FindProperty("videoController");
            titleTextProp = serializedObject.FindProperty("titleText");
            contentTextProp = serializedObject.FindProperty("contentText");
            contentImgProp = serializedObject.FindProperty("contentImg");
            isGetOnStartProp = serializedObject.FindProperty("isGetOnStart");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(interplayNameProp);
            EditorGUILayout.PropertyField(contentTypeProp);
            EditorGUILayout.PropertyField(contentIdProp);

            ContentHandler.ctType type = (ContentHandler.ctType)contentTypeProp.enumValueIndex;

            switch (type)
            {
                case ContentHandler.ctType.video:
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("Component Content Video", EditorStyles.boldLabel);
                    EditorGUILayout.PropertyField(thumbnailImageProp);
                    EditorGUILayout.PropertyField(videoControllerProp);
                    break;
                case ContentHandler.ctType.information:
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("Component Content Information", EditorStyles.boldLabel);
                    EditorGUILayout.PropertyField(titleTextProp);
                    EditorGUILayout.PropertyField(contentTextProp);
                    EditorGUILayout.PropertyField(contentImgProp);
                    break;
            }

            EditorGUILayout.PropertyField(isGetOnStartProp);

            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}