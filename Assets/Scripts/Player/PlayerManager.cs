using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    [SerializeField]
    public List<Authentication.RootLogin.Response> displayedResponses;

    public string AccessToken { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Make this object persistent
        }
        else if (Instance != this)
        {
            Destroy(gameObject);  // Ensures that there is only one instance
        }
    }

    public void SetAccessToken(string token)
    {
        AccessToken = token;
    }

    public string GetAccessToken()
    {
        return AccessToken;
    }
}
