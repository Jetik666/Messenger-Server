using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Controls.Primitives;

namespace Server.View.Page_Handlers
{
    public class PopupHandler : IDisposable
    {
        public List<Popup> Popups { get; }
        private bool _disposed = false;

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

                if (Debugger.IsAttached)
                {
                    Debug.WriteLine($"Closed popup: {openedPopup?.Name ?? "Unnamed"}\n" +
                        $"Opened popup: {newOpenedPopup?.Name ?? "Unnamed"}");
                    foreach (Popup popup in Popups)
                    {
                        Debug.WriteLine($"{popup.Name} {popup.IsOpen}");
                    }
                }
            }
        }

        public void RegisterPopups(Func<List<Popup>> GetPopups)
        {
            List<Popup> popups = GetPopups();
            foreach (Popup popup in popups)
            {
                Popups.Add(popup);
                if (Debugger.IsAttached)
                {
                    Debug.WriteLine($"Popup {popup.Name ?? "Unnamed"}");
                }
            }
        }
        public void UnregisterPopup(Popup popup) => Popups.Remove(popup);
        public void UnregisterAllPopups() => Popups.Clear();
        public void DeactivatePopup()
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

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Dispose managed resources here.
                    Popups.Clear();
                }
                // Dispose unmanaged resources here, if any.
                _disposed = true;
            }
        }
    }
}
