using Firebase.Auth.Providers;
using Firebase.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Query;

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
        private Entry _ime;
        private Entry _priimek;
        private Entry _telefon;
        private Entry _id;
        private Entry _naslov;



        public RegistracijaViewModel(INavigation navigation, Entry eTxEmail, Entry eTxPass,Entry ime,Entry priimek, Entry telefon,Entry idEntry,Entry naslov)
        {
            _navigation = navigation;
            _eTxEmail = eTxEmail;
            _eTxpass = eTxPass;
            _ime = ime;
            _priimek = priimek;
            _telefon = telefon;
            _id = idEntry;  
            _naslov = naslov;

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

                    var userId = userCredential.User.Uid;
                    var displayName = userCredential.User.Info.DisplayName;
                    var userEmail = userCredential.User.Info.Email;

                    var user = new
                    {
                        id = _id.Text,
                        email = userEmail,
                        ime = _ime.Text,
                        priimek= _priimek.Text ,
                        telefon = _telefon.Text,
                        naslov=_naslov.Text

                    };

                    var firebase = new FirebaseClient("https://rmr-projektnovo.firebaseio.com/");
                    await firebase
                     .Child("users")
                     .PostAsync(user);

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
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Napaka",
                   $"Unhandled Exception: {firebaseEx.Message}", "OK");
                }

            }
            catch (Exception ex)
            {
          
                await Application.Current.MainPage.DisplayAlert("Napaka",
                    $"Unhandled Exception: {ex.Message}\nStackTrace: {ex.StackTrace}", "OK");
            }


        }


    }
}
