using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;
using UnityEngine.Events;

public class LoadAddressable : MonoBehaviour
{

    [SerializeField] AssetLabelReference assetLabelReference;
    [SerializeField] Text uiText;
    [SerializeField] List<GameObject> _instanceObject;
    [SerializeField] Slider progressBar;
    [SerializeField] UnityEvent AfterLoad;

    private List<GameObject> loadedInstances = new List<GameObject>();

    private void Start()
    {
        LoadAssets();
        Debug.Log("load");
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.T))
        //{
        //    LoadAssets();
        //}

        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    ReleaseLoadedInstances();
        //}
    }

    public void DoAddressable()
    {
        LoadAssets();
    }

    public void RealeaseAddressable()
    {
        ReleaseLoadedInstances();
    }

    private async void LoadAssets()
    {
        AsyncOperationHandle<IList<GameObject>> asyncOperationHandle = Addressables.LoadAssetsAsync<GameObject>(assetLabelReference, null);

        while (!asyncOperationHandle.IsDone)
        {
            // Cek jika progres bar sudah diinisialisasi
            if (progressBar != null)
            {
                // Update nilai progres bar dengan persentase loading
                progressBar.value = asyncOperationHandle.PercentComplete;
            }

           

            // Yield control back to Unity to update UI
            await Task.Yield();
        }

        try
        {
            // Tunggu sampai proses load selesai
            await asyncOperationHandle.Task;
        }
        catch (Exception e)
        {
            Debug.LogError($"Error loading assets: {e}");
            UpdateUIText("Failed to load assets.");
            return;
        }

        if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
        {
            IList<GameObject> loadedObjects = asyncOperationHandle.Result;

            foreach (GameObject obj in loadedObjects)
            {
                GameObject instance = Instantiate(obj);
                loadedInstances.Add(instance);
                _instanceObject.Add(instance);
            }

            AfterLoad.Invoke();

            UpdateUIText($"Loaded {loadedObjects.Count} assets successfully!");
        }
        else
        {
            UpdateUIText("Failed to load assets.");
        }
    }



    private void ReleaseLoadedInstances()
    {
        // Melepaskan semua instance yang dimuat
        foreach (GameObject instance in loadedInstances)
        {
            Destroy(instance); // Menghancurkan instance GameObject
        }

        // Membersihkan daftar instance
        loadedInstances.Clear();

        UpdateUIText("Released all loaded instances.");
    }

    private void UpdateUIText(string message)
    {
        if (uiText != null)
        {
            uiText.text = message;
        }
        else
        {
            Debug.LogWarning("UI Text reference is not set. Please assign the UI Text component in the Inspector.");
        }
    }

    private void OnDestroy()
    {
        // Memastikan semua instance GameObject dihancurkan saat objek dihancurkan
        ReleaseLoadedInstances();
    }
}
