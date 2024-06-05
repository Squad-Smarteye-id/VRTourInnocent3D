using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using System;

public abstract class RestAPIHandler : MonoBehaviour
{
    public RestAPI restAPI;
    public abstract void OnSuccessResult(JObject result);
    public abstract void OnProtocolErr(JObject result);
    public abstract void DataProcessingErr(JObject result);
}

[Serializable]
public class RowData
{
    public Dictionary<string, string> baseData;

    public RowData(Dictionary<string, string> _baseData)
    {
        baseData = _baseData;
    }
}