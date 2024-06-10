
using UnityEngine;
using VRInnocent.RestAPI;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    [SerializeField]
    public Authentication.RootLogin.Response displayedResponses;

    public string userId
    {
        get => displayedResponses.userId;
        private set => userId = value;
    }

    private string m_userEmail = "shantaufiq021@gmail.com";
    public string userEmail { get => m_userEmail; set => m_userEmail = value; }

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
}
