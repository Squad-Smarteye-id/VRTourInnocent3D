using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace VRInnocent.RestAPI
{
    public class Authentication : RestAPIHandler
    {
        public bool isLoginSection = true;

        [Header("Sign In Components")]
        public TMP_InputField signInUsername;
        public TMP_InputField signInPassword;

        [Header("Sign Up Components")]
        public TMP_InputField signUpFirstName;
        public TMP_InputField signUpLastName;
        public TMP_InputField signUpUsername;
        public TMP_InputField signUpEmail;
        public TMP_InputField signUpPassword;
        public TMP_InputField signUpPasswordComfirmation;
        public TMP_InputField signUpPhone;
        public string signUpGender;

        public GameObject successPopUp;
        public GameObject errPopUp;

        public void OnClickRegister()
        {
            if (string.IsNullOrEmpty(signUpPassword.text) || string.IsNullOrEmpty(signUpPasswordComfirmation.text)
            || string.IsNullOrEmpty(signUpUsername.text) || string.IsNullOrEmpty(signUpEmail.text))
            {
                Debug.LogWarning("please fill the blank");
                return;
            }

            if (signUpPassword.text != signUpPasswordComfirmation.text)
            {
                Debug.LogWarning("comfirm your password");
                return;
            }

            Dictionary<string, string> newPlayer = new Dictionary<string, string>
        {
            {"first_name", signUpFirstName.text},
            {"last_name", signUpLastName.text},
            {"username", signUpUsername.text},
            {"email", signUpEmail.text},
            {"password", signUpPassword.text},
            {"confirm_password", signUpPasswordComfirmation.text},
            {"gender", signUpGender},
            {"phone", signUpPhone.text}
        };

            RowData dataObject = new RowData(newPlayer);
            restAPI.PostAction(dataObject.baseData, OnSuccessResult, OnProtocolErr, DataProcessingErr, "register");
        }

        public void OnClickLogin()
        {
            // if (string.IsNullOrEmpty(signInUsername.text) || string.IsNullOrEmpty(signInPassword.text))
            // {
            //     Debug.LogWarning("please fill the blank");
            //     return;
            // }

            // List<string> c = new List<string>()
            // {
            //     signInUsername.text, signInPassword.text
            // };

            Dictionary<string, string> newPlayer = new Dictionary<string, string>
            {
                // {"username", signInUsername.text},
                // {"password", signInPassword.text}
                {"email", "smarteye.id@gmail.com"},
                {"password", "smarteye2024"}
            };

            RowData dataObject = new RowData(newPlayer);
            var test = JsonConvert.SerializeObject(dataObject);
            Debug.Log("JSON: " + dataObject.baseData);

            restAPI.PostAction(dataObject.baseData, OnSuccessResult, OnProtocolErr, DataProcessingErr, "login");
        }

        private void Start()
        {
            OnClickLogin();
        }

        private void Update()
        {
            // if (!isLoginSection)
            // {
            //     if (!string.IsNullOrEmpty(signUpPassword.text))
            //     {
            //         signUpPasswordComfirmation.interactable = true;
            //     }
            //     else
            //     {
            //         signUpPasswordComfirmation.interactable = false;
            //         signUpPasswordComfirmation.text = "";
            //     }
            // }
            // else
            // {
            //     successPopUp.SetActive(false);
            //     errPopUp.SetActive(false);
            // }
        }

        public void OnClickGenderSelection(string str)
        {
            signUpGender = str;
            Debug.Log($"player is {str}");
        }

        public void OnClickChangePanel(bool state)
        {
            isLoginSection = state;
        }

        public override void OnSuccessResult(JObject result)
        {
            if (isLoginSection)
            {
                RootLogin.Root returnData = result.ToObject<RootLogin.Root>();
                // RootLogin.loginResponses.Add(returnData.response);  // Add to static list
                Debug.Log($"{returnData.response.userName}. player token: {returnData.response.access_token}");

                PlayerManager.Instance.displayedResponses = returnData.response;
            }
            else
            {
                // successPopUp.SetActive(true);
                // errPopUp.SetActive(false);

                RootRegister returnData = result.ToObject<RootRegister>();
                Debug.Log($"Message: {returnData.message}.");
            }
        }

        public override void OnProtocolErr(JObject result)
        {
            if (isLoginSection)
            {
                // RootLogin.Root returnData = result.ToObject<RootLogin.Root>();
                // RootLogin.loginResponses.Add(returnData.response);  // Add to static list
            }
            else
            {
                // errPopUp.SetActive(true);
                // successPopUp.SetActive(false);

                // RootRegister returnData = result.ToObject<RootRegister>();
                // Debug.LogError($"Message: {returnData.message}.");
            }
        }

        public override void DataProcessingErr(JObject result)
        {
            // if (isLoginSection)
            // {
            //     RootLogin returnData = result.ToObject<RootLogin>();
            //     Debug.LogError($"{returnData.message}");
            // }
            // else
            // {
            //     RootRegister returnData = result.ToObject<RootRegister>();
            //     Debug.LogError($"Message: {returnData.message}.");
            // }
        }

        public class RootRegister
        {
            public string status { get; set; }
            public string message { get; set; }
        }

        [System.Serializable]
        public class RootLogin
        {
            [System.Serializable]
            public class Response
            {
                public string userId;
                public string userName;
                public string access_token;
            }

            [System.Serializable]
            public class Root
            {
                public Response response;
            }

            // Static list to hold responses
            // public static List<Response> loginResponses = new List<Response>();
        }
    }
}