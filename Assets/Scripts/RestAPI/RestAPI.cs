using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using System;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace VRInnocent.RestAPI
{
    public class RestAPI : MonoBehaviour
    {
        public TargetAPIConfig targetAPIConfig;
        private string endpointTitle = "";

        string tempDataJson;

        #region RestAPI
        public void PostAction(Dictionary<string, string> _data, Action<JObject> success, Action<JObject> err, Action<JObject> dataErr, string _endpointTitle, string uniqValue = "")
        {
            endpointTitle = _endpointTitle;
            var jsonDataToSend = JsonConvert.SerializeObject(_data);

            tempDataJson = jsonDataToSend;

            object[] callbacks = new object[4] { success, err, dataErr, uniqValue };

            StartCoroutine(nameof(Post), callbacks);
        }

        IEnumerator Get()
        {
            using (UnityWebRequest request = UnityWebRequest.Get(targetAPIConfig.url + targetAPIConfig.GetEndpoint(endpointTitle)))
            {
                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.ConnectionError)
                    Debug.LogError(request.error);
                else
                {
                    Debug.Log("berhasil");
                    string json = request.downloadHandler.text;
                    SimpleJSON.JSONNode stats = SimpleJSON.JSON.Parse(json);
                }
            }
        }

        IEnumerator Post(object[] callback)
        {
            Action<JObject> successCallback = (Action<JObject>)callback[0];
            Action<JObject> errCallback = (Action<JObject>)callback[1];
            Action<JObject> dataErrCallback = (Action<JObject>)callback[2];
            string uniqVal = (string)callback[3];

            string uri = targetAPIConfig.url + targetAPIConfig.GetEndpoint(endpointTitle) + uniqVal;
            Debug.Log($"Start post request to: {uri}");

            using UnityWebRequest webRequest = new UnityWebRequest(uri, "POST");
            webRequest.SetRequestHeader("Content-Type", "application/json");
            byte[] rowData = Encoding.UTF8.GetBytes(tempDataJson);
            webRequest.uploadHandler = new UploadHandlerRaw(rowData);
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            yield return webRequest.SendWebRequest();

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.InProgress:
                    break;
                case UnityWebRequest.Result.Success:
                    string responseSuccess = webRequest.downloadHandler.text;
                    JObject dataSuccess = JObject.Parse(responseSuccess);

                    // RowData returnData = dataSuccess.ToObject<RowData>();
                    // Debug.Log($"{returnData.message}. player token: {returnData.token}");

                    successCallback?.Invoke(dataSuccess);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    string responseProtocolError = webRequest.downloadHandler.text;
                    JObject dataResponseProtocolError = JObject.Parse(responseProtocolError);
                    // Debug.LogError(dataResponseProtocolError);

                    errCallback?.Invoke(dataResponseProtocolError);
                    break;
                case UnityWebRequest.Result.DataProcessingError:
                    string responseDataProcessingError = webRequest.downloadHandler.text;
                    JObject dataDataProcessingError = JObject.Parse(responseDataProcessingError);
                    // Debug.LogError(dataDataProcessingError);

                    dataErrCallback?.Invoke(dataDataProcessingError);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        #endregion
    }
}