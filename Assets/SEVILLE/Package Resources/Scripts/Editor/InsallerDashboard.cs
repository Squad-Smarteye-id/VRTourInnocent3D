#if UNITY_EDITOR
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
#endif

using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Seville
{
#if UNITY_EDITOR
    public class InsallerDashboard : EditorWindow
    {
        private static List<string> dependenciesList = new List<string>() { "com.unity.xr.interaction.toolkit" };
        private static List<string> buttonList = new List<string>() { "", "" };

        private ListRequest listRequest;
        private AddRequest addRequest;

        static bool isChecking = false;
        private bool isInstalling = false;
        private bool isButtonUpdate = false;

        float packagesDownloadedCount = 0;

        // Tags and Layers Settings
        static List<string> tagsList = new List<string>();
        static List<string> layersList = new List<string>();



        [MenuItem("Seville/Installer Dashboard")]
        public static void OpenInstallerDashboard()
        {
            GetWindow<InsallerDashboard>("Installer Basic Package Dashboard");

            isChecking = true;
        }

        private void OnGUI()
        {
            if (isChecking)
            {
                CheckDepedenciesPackage();
            }

            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("Installer Progress : ", EditorStyles.boldLabel);

            InstallerProgress(packagesDownloadedCount, dependenciesList.Count, "Install Progress");

            UpdateButtons();

            if (packagesDownloadedCount < dependenciesList.Count)
                EditorGUILayout.HelpBox("You have to install all basic package before use the Seville framework", MessageType.Error);

            if (!isButtonUpdate) EditorGUILayout.LabelField("Checking...", EditorStyles.boldLabel);

            if (isInstalling)
                EditorGUILayout.LabelField("Installing...", EditorStyles.boldLabel);
        }

        void UpdateButtons()
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("XR Integration ToolKit :", EditorStyles.boldLabel);

            string nameBtn1 = buttonList[0] != "" ? buttonList[0] : "wait...";

            if (GUILayout.Button(nameBtn1))
            {
                InstallPackage(0, "2.4.3");
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space(10);

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Impor Tags and Layers from seville package :", EditorStyles.boldLabel);

            string nameBtn2 = "Load Tags";

            if (GUILayout.Button(nameBtn2))
            {
                importTags();
            }

            string nameBtn3 = "Load Layers";

            if (GUILayout.Button(nameBtn3))
            {
                importLayers();
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space(10);
        }

        void InstallerProgress(float value, float maxValue, string label)
        {
            float realValue = 1f / maxValue;
            float progressValue = value * realValue;

            Rect rect = GUILayoutUtility.GetRect(18, 18, "TextField");
            EditorGUI.ProgressBar(rect, progressValue, label);
            EditorGUILayout.Space(10);
        }

        private void CheckDepedenciesPackage()
        {
            packagesDownloadedCount = 0;
            isButtonUpdate = false;
            // Membuat permintaan untuk mendapatkan daftar paket yang ada dalam proyek
            listRequest = Client.List();

            // Menjalankan proses instalasi setelah daftar paket tersedia
            EditorApplication.update += ResetDependenciesState;

            isChecking = false;
        }

        private void ResetDependenciesState()
        {
            if (listRequest.IsCompleted)
            {
                if (listRequest.Status == StatusCode.Success)
                {
                    Debug.Log($"Processing....");
                    for (int i = 0; i < dependenciesList.Count; i++)
                    {
                        bool notAvailable = listRequest.Result.Any((x) => x.name == dependenciesList[i]);
                        if (!notAvailable)
                        {
                            Debug.Log($"{dependenciesList[i]} is not available in package manager");
                            buttonList[i] = "Install";
                        }
                        else
                        {
                            Debug.Log($"{dependenciesList[i]} is available");
                            buttonList[i] = "Reinstall";

                            packagesDownloadedCount += 1;
                        }
                    }

                    isButtonUpdate = true;
                }
                else
                {
                    Debug.LogError("Gagal mendapatkan daftar paket.");
                }
                // Hentikan pemantauan pembaruan
                EditorApplication.update -= ResetDependenciesState;
            }
        }

        private void InstallPackage(int _index, string _version)
        {
            isInstalling = true;

            // Create the request to add the package
            AddRequest request = Client.Add($"{dependenciesList[_index]}@{_version}");

            // Check the status of the request in the Update loop
            EditorApplication.update += OnUpdate;

            void OnUpdate()
            {
                if (request.IsCompleted)
                {
                    isInstalling = false;
                    if (request.Status == StatusCode.Success)
                    {
                        Debug.Log($"Package {dependenciesList[_index]}@{_version} installed successfully!");

                        isChecking = true;
                    }
                    else
                    {
                        Debug.LogError($"Failed to install package {dependenciesList[_index]}@{_version}: {request.Error.message}");

                        isChecking = true;
                    }

                    // Remove the update callback
                    EditorApplication.update -= OnUpdate;
                }
            }
        }

        // [MenuItem("Seville/Taggler/Backup Tags")]
        static void writeTagsBackup()
        {
            //check if folders exist
            string path = "Assets/SEVILLE/Package Resources/Tags and Layers/";

            // if (!File.Exists(path)) Directory.CreateDirectory(path);
            // path += "Taggler/";

            if (!File.Exists(path)) Directory.CreateDirectory(path);
            path += "Tags Backup.txt";

            File.WriteAllText(path, String.Empty);

            StreamWriter writer = new StreamWriter(path, true);

            //cache, loop and write the tags into stream
            int max = UnityEditorInternal.InternalEditorUtility.tags.Length;
            for (int i = 0; i < max; i++)
            {
                writer.WriteLine(UnityEditorInternal.InternalEditorUtility.tags[i]);
            }

            writer.Close();

            //Re-import the file to update the reference in the editor
            AssetDatabase.ImportAsset(path);
            TextAsset asset = (TextAsset)Resources.Load("Tags Backup");

            Debug.Log("Backup completed!");
            Debug.Log("You can find it inside SEVILLE/Package Resources/Tags and Layers/Tags Backup.txt");
        }

        // [MenuItem("Seville/Taggler/Backup Layers")]
        static void writeLayersBackup()
        {
            //check if folders exist
            string path = "Assets/SEVILLE/Package Resources/Tags and Layers/";

            // if (!File.Exists(path)) Directory.CreateDirectory(path);
            // path += "Taggler/";

            if (!File.Exists(path)) Directory.CreateDirectory(path);
            path += "Layers Backup.txt";

            File.WriteAllText(path, String.Empty);

            StreamWriter writer = new StreamWriter(path, true);

            for (int i = 0; i <= 31; i++)
            {
                var layerN = LayerMask.LayerToName(i);
                if (layerN.Length > 0) writer.WriteLine(layerN);
            }

            writer.Close();

            AssetDatabase.ImportAsset(path);
            TextAsset asset = (TextAsset)Resources.Load("Layers Backup");

            Debug.Log("Backup completed!");
            Debug.Log("You can find it inside Assets/SEVILLE/Package Resources/Tags and Layers/Layers Backup.txt");
        }

        void importLayers()
        {
            string path = chooseLayersBackupFilePath();

            using (StreamReader reader = new StreamReader(path))
            {
                while (reader.Peek() >= 0)
                {
                    layersList.Add(reader.ReadLine());
                }
            }

            addLayers(layersList);
        }

        void importTags()
        {
            string path = chooseTagsBackupFilePath();

            using (StreamReader reader = new StreamReader(path))
            {
                while (reader.Peek() >= 0)
                {
                    tagsList.Add(reader.ReadLine());
                }
            }

            addTags(tagsList);
        }

        static void addTags(List<string> tags)
        {
            // Open tag manager
            SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
            SerializedProperty tagsProp = tagManager.FindProperty("tags");

            // Adding a Tag
            foreach (var item in tags)
            {
                // First check if it is not already present
                bool found = false;
                for (int i = 0; i < tagsProp.arraySize; i++)
                {
                    SerializedProperty t = tagsProp.GetArrayElementAtIndex(i);
                    if (t.stringValue.Equals(item)) { found = true; break; }
                }

                // if not found, add it
                if (!found)
                {
                    tagsProp.InsertArrayElementAtIndex(0);
                    SerializedProperty n = tagsProp.GetArrayElementAtIndex(0);
                    n.stringValue = item;
                }
            }

            tagManager.ApplyModifiedProperties();
            Debug.Log("Import completed!");
        }

        static void addLayers(List<string> layers)
        {
            SerializedObject manager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
            SerializedProperty layersProp = manager.FindProperty("layers");

            int index = 0;
            foreach (var item in layers)
            {
                bool found = false;
                for (int i = 0; i < layersProp.arraySize; i++)
                {
                    SerializedProperty sp = layersProp.GetArrayElementAtIndex(i);
                    if (sp.stringValue.Equals(item)) { found = true; index = i; break; }
                }

                if (!found)
                {
                    layersProp.InsertArrayElementAtIndex(index);
                    SerializedProperty n = layersProp.GetArrayElementAtIndex(index);
                    n.stringValue = item;
                }
            }

            manager.ApplyModifiedProperties();
            Debug.Log("Import completed!");
        }

        static string chooseTagsBackupFilePath()
        {
            string path = EditorUtility.OpenFilePanel("Tags Backup.txt File", "Assets/SEVILLE/Package Resources/Tags and Layers/", "txt");
            return path;
        }

        static string chooseLayersBackupFilePath()
        {
            string path = EditorUtility.OpenFilePanel("Layers Backup.txt File", "Assets/SEVILLE/Package Resources/Tags and Layers/", "txt");
            return path;
        }
    }
#endif
}