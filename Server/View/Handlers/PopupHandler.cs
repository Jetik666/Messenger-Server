using System;
using System.Collections.Generic;
using System.Windows.Controls.Primitives;

namespace Server.View.Handlers
{
    public class PopupHandler
    {
        public List<Popup> Popups { get; }

        public PopupHandler() => Popups = new List<Popup>();

        public Popup? OpenedPopup
        {
            get
            {
                return Popups.Find(popup => popup.IsOpen == true);
            }
            set
            {
                Popup? openedPopup = Popups.Find(popup => popup.IsOpen == true);
                if (openedPopup != null)
                {
                    openedPopup.IsOpen = false;
                }
                Popup? newOpenedPopup = Popups.Find(popup => popup.Name == value?.Name);
                if (newOpenedPopup != null)
                {
                    newOpenedPopup.IsOpen = true;
                }
            }
        }

        public void RegisterPopups(Func<List<Popup>> GetPopups)
        {
            List<Popup> popups = GetPopups();
            foreach (Popup popup in popups)
            {
                Popups.Add(popup);
            }
        }
        public void UnregisterPopup(Popup popup) => Popups.Remove(popup);
        public void UnregisterAllPopups() => Popups.Clear();
        public void DeactivatePopups()
        {
            foreach (Popup popup in Popups)
            {
                popup.IsOpen = false;
            }
        }

        public void ClosePopup()
        {
            Popup? popup = OpenedPopup;
            if (popup != null)
            {
                popup.IsOpen = false;
            }
        }
        public void ClosePopup(Popup openedPopup)
        {
            Popup? popup = Popups.Find(popup => popup.Name == openedPopup.Name);
            if (popup != null)
            {
                popup.IsOpen = false;
            }
        }
    }
}
