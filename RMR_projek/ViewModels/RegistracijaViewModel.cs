using Firebase.Auth.Providers;
using Firebase.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMR_projek.ViewModels
{
    internal class RegistracijaViewModel
    {

        public string webApiKey = "AIzaSyBqhp1SpzEHiLxqk-VD8bfpy3nMYJRJmEw";

        private INavigation _navigation;

        public Command RegistracijaUporabnika { get; }

        public string email { get; set; }
        public string geslo { get; set; }

        private Entry _eTxEmail;
        private Entry _eTxpass;

        public RegistracijaViewModel(INavigation navigation, Entry eTxEmail, Entry eTxPass)
        {
            _navigation = navigation;
            _eTxEmail = eTxEmail;
            _eTxpass = eTxPass;

            RegistracijaUporabnika = new Command(RegistracijaUporabnikaTappendAsync);
        }

        private async void RegistracijaUporabnikaTappendAsync(object obj)
        {

            email = _eTxEmail.Text;
            geslo = _eTxpass.Text;

            try
            {
                var config = new FirebaseAuthConfig
                {
                    ApiKey = webApiKey,
                    AuthDomain = "rmr-projektnovo.firebaseapp.com",
                    Providers = new FirebaseAuthProvider[]
                    {
                        new GoogleProvider().AddScopes("email"),
                        new EmailProvider()
                    }
                };

                var client = new FirebaseAuthClient(config);
                var userCredential = await client.CreateUserWithEmailAndPasswordAsync(email, geslo, "Prikazno ime");

                if (userCredential != null)
                {
                    await Application.Current.MainPage.DisplayAlert("Obvestilo", "Uporabnik uspesno registriran", "OK");
                    _navigation.PopAsync();
                }
            }

            catch (FirebaseAuthException firebaseEx)
            {
                if (firebaseEx.Reason.ToString() == "EmailExists")
                {
                    await Application.Current.MainPage.DisplayAlert("Opozorilo", "Email je ze v uporabi. Vnesite drugo uporabnisko ime.", "OK");
                }

            }


        }


    }
}
