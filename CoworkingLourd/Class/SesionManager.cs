using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoworkingLourd
{
    public static class SessionManager
    {
        private static string loggedInUser;

        #region Vérification connexion utilisateur
        public static void SetLoggedInUser(string username)
        {
            loggedInUser = username;
        }

        public static bool IsUserLoggedIn()
        {

            return !string.IsNullOrEmpty(loggedInUser);

        }

        public static string GetLoggedInUser()
        {
            return loggedInUser;
        }
        #endregion
    }
}
