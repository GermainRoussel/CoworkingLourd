using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoworkingLourd.Class
{
    public static class ConnectedUser
    {
        #region Vérification connexion utilisateur
        public static void CheckLoggedInUser()
        {

            string loggedInUsername;

            // Vérifier si un utilisateur est connecté
            if (!SessionManager.IsUserLoggedIn())
            {
                // Rediriger vers le formulaire de connexion
                FormConnection loginForm = new FormConnection();
                loginForm.ShowDialog();

                // Vérifier à nouveau l'état de connexion après la fermeture du formulaire de connexion
                CheckLoggedInUser();
            }
            else
            {
                loggedInUsername = SessionManager.GetLoggedInUser();
            }
        }
        #endregion

        #region Afficher utilisateur connecté
        public static string GetUserIdConnected()
        {
            CheckLoggedInUser();
            string loggedInUsername = SessionManager.GetLoggedInUser();
            string userIdConnected = "Connecté en tant que : " + loggedInUsername;
            return (userIdConnected);
        }
        #endregion
    }
}
