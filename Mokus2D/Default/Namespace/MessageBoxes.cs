using System;

using Mokus2D.GamerServices;

namespace Default.Namespace
{
    public static class MessageBoxes
    {
        public static void ShowNotSignedToXBoxError(AsyncCallback callback)
        {
            ShowOKWindow(callback, "ACCOUNT_ERROR", "NOT_SIGNED_TO_XBOX");
        }

        public static void ShowInternetError(AsyncCallback callback)
        {
            ShowOKWindow(callback, " ", "NO_INTERNET");
        }

        private static void ShowOKWindow(AsyncCallback callback, string title, string message)
        {
            if (!Guide.IsVisible)
            {
                _ = Guide.BeginShowMessageBox(title.Localize(), message.Localize(), ["OK".Localize()], 0, MessageBoxIcon.None, callback, null);
            }
        }

        public static void ShowInternetError()
        {
            ShowInternetError(null);
        }
    }
}
