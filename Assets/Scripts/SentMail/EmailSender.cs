using UnityEngine;
using System.ComponentModel;
using System.Net.Mail;
using TMPro;
using System.Text.RegularExpressions;
using System;
using VRInnocent.Utils;
using UnityEngine.Events;

namespace VRInnocent.Email
{
    public class EmailSender : MonoBehaviour
    {
        public string senderEmail = "snaptive.studio@gmail.com";
        public string senderPassword = "fhqpqxhiacysrcoy";
        public string SenderName;

        [Space]
        public string subject;
        [TextArea]
        public string body;
        [Space]
        public string SendTo;
        public GameObject[] buttons;

        [Space]
        [Header("UI Components")]
        public GameObject panelContainer;
        private CanvasGroup canvasGroup;

        [Space]
        public UnityEvent OnSuccess;

        void Start()
        {
            canvasGroup = panelContainer.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = panelContainer.AddComponent<CanvasGroup>();
            }

            panelContainer.transform.localPosition = new Vector3(-1084, 0, 0);
            canvasGroup.alpha = 0;
        }

        private bool IsValidEmail(string email)
        {
            // Ekspresi reguler untuk memeriksa email
            string emailPattern = @"^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$";
            Regex regex = new Regex(emailPattern);

            return regex.IsMatch(email);
        }

        public void SendEmail()
        {
            string recipientEmail = SendTo;

            if (string.IsNullOrEmpty(recipientEmail) && !IsValidEmail(recipientEmail))
            {
                Debug.Log("Invalid email address!");
                return;
            }

            foreach (var item in buttons)
            {
                item.SetActive(false);
            }

            try
            {
                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                client.Credentials = new System.Net.NetworkCredential(
                    senderEmail,
                    senderPassword);
                client.EnableSsl = true;

                MailAddress from = new MailAddress(
                    senderEmail,
                    SenderName,
                    System.Text.Encoding.UTF8);
                // Set destinations for the email message.
                MailAddress to = new MailAddress(recipientEmail);

                // btn_sendEmail.SetActive(false);

                // Specify the message content.
                MailMessage message = new MailMessage(from, to);
                message.Subject = subject;
                message.SubjectEncoding = System.Text.Encoding.UTF8;
                message.Body = body;
                message.BodyEncoding = System.Text.Encoding.UTF8;
                // string attachmentPath = $"";
                // Attachment attachment = new Attachment(attachmentPath);
                // message.Attachments.Add(attachment);

                client.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);
                string userState = "test message1";
                client.SendAsync(message, userState);
            }
            catch (Exception ex)
            {
                Debug.Log("Error sending email: " + ex.Message);
                // invalidEmailUI.SetActive(true);
            }
        }

        private void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            string token = (string)e.UserState;

            if (e.Cancelled)
            {
                Debug.Log("Send canceled " + token);
            }
            if (e.Error != null)
            {
                Debug.Log("[ " + token + " ] " + " " + e.Error.ToString());
            }
            else
            {
                Debug.Log("Message sent.");

                foreach (var item in buttons)
                {
                    item.SetActive(true);
                }

                OnSuccess?.Invoke();
            }
        }

        public void ShowPanel() =>
            UIAnimator.SlideHorizontalWithFade(panelContainer, canvasGroup, 0, 1);

        public void HidePanel()
        {
            UIAnimator.SlideHorizontalWithFade(panelContainer, canvasGroup, -1084, 0);
        }
    }
}