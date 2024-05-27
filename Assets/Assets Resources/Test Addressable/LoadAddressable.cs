using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Addressable
{
    public class LoadAddressable : MonoBehaviour
    {
        [SerializeField] List<AssetReference> instantiateObjects;
        private Dictionary<AssetReference, AsyncOperationHandle<GameObject>> handles = new Dictionary<AssetReference, AsyncOperationHandle<GameObject>>();
        private List<GameObject> instantiatedObjects = new List<GameObject>();

        private void Start()
        {
            LoadAssets();
        }

        #region LoadAsset
        // Load assets from cloud
        public void LoadAssets()
        {
            foreach (var asset in instantiateObjects)
            {
                var asyncOperationHandle = asset.LoadAssetAsync<GameObject>();
                asyncOperationHandle.Completed += OnAssetLoaded;
                handles[asset] = asyncOperationHandle;
            }
        }

        // Callback when asset is loaded
        private void OnAssetLoaded(AsyncOperationHandle<GameObject> obj)
        {
            if (obj.Status == AsyncOperationStatus.Succeeded)
            {
                GameObject asset = obj.Result;
                InstantiateAsset(asset);
            }
        }

        // Instantiate
        private void InstantiateAsset(GameObject asset)
        {
            GameObject instance = Instantiate(asset);
            instantiatedObjects.Add(instance);
        }

        // Release
        public void ReleaseAssets()
        {
            foreach (var obj in instantiatedObjects)
            {
                Addressables.ReleaseInstance(obj);
            }
            instantiatedObjects.Clear();

            foreach (var handle in handles.Values)
            {
                if (handle.IsValid())
                {
                    Addressables.Release(handle);
                }
            }
            handles.Clear();
        }

        #endregion
    }
}
