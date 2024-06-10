using UnityEngine;
using System.Collections.Generic;
using System;

namespace VRInnocent.RestAPI
{
    [CreateAssetMenu]
    public class TargetAPIConfig : ScriptableObject
    {
        public string url;
        public List<endpointTarget> endpoints;

        [Serializable]
        public struct endpointTarget
        {
            public string title;
            public string targetEndpoint;
        }

        public string GetEndpoint(string _title)
        {
            var target = endpoints.Find((x) => x.title == _title);
            return target.targetEndpoint;
        }
    }
}