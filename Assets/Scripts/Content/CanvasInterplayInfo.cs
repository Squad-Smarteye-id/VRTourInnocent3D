using UnityEngine;
using VRInnocent.Email;

namespace VRInnocent.Content
{
    public class CanvasInterplayInfo : MonoBehaviour
    {
        public EmailSender playbookPanel;
        public FeedbackController feedbackController;
        public SuccessPopup popupPrefab;

        [Space]
        [Header("UI Components")]
        public Transform canvasRight;

        public enum PanelType
        {
            NONE = 0, PLAYBOOK = 1, FEEDBACK = 2
        }

        private PanelType m_currentPanelActive = PanelType.NONE;

        private void Start()
        {
            m_currentPanelActive = PanelType.NONE;
        }

        private void OpenPanel(PanelType _panelType)
        {
            switch (_panelType)
            {
                case PanelType.NONE:
                    break;
                case PanelType.PLAYBOOK:
                    if (m_currentPanelActive == PanelType.NONE) playbookPanel.ShowPanel();
                    else
                    {
                        playbookPanel.ShowPanel();
                        feedbackController.HidePanel();
                    }
                    break;
                case PanelType.FEEDBACK:
                    if (m_currentPanelActive == PanelType.NONE) feedbackController.ShowPanel();
                    else
                    {
                        feedbackController.ShowPanel();
                        playbookPanel.HidePanel();
                    }
                    break;
            }

            m_currentPanelActive = _panelType;
        }

        public void OpenPlayBook()
        {
            OpenPanel(PanelType.PLAYBOOK);

        }
        public void OpenFeedback()
        {
            OpenPanel(PanelType.FEEDBACK);
        }
        public void NonePanelType() => OpenPanel(PanelType.NONE);

        public void ShowPopupSeuccess(string popupName)
        {
            var popup = Instantiate(popupPrefab, canvasRight);
            popup.ShowPopupAnimate(popupName);
        }
    }
}