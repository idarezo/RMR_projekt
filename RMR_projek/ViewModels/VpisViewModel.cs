using Firebase.Auth.Providers;
using Firebase.Auth;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMR_projek.ViewModels
{
    internal class VpisViewModel
    {
        public string webApiKey = "AIzaSyBqhp1SpzEHiLxqk-VD8bfpy3nMYJRJmEw";

        private INavigation _navigation;

        public Command RegistracijaBtn { get; }
        public Command PrijavaBtn { get; }

        public string email { get; set; }
        public string geslo { get; set; }

        private Entry _eTxEmail;
        private Entry _eTxpass;

        public VpisViewModel(INavigation navigation, Entry eTxEmail, Entry eTxPass)
        {
            _navigation = navigation;
            RegistracijaBtn = new Command(RegistracijaBtnTappedAsync);
            PrijavaBtn = new Command(PrijavaBtnTappedAsync);

            _eTxEmail = eTxEmail;
            _eTxpass = eTxPass;
        }

        private async void RegistracijaBtnTappedAsync(object obj)
        {
            await _navigation.PushAsync(new NajemodajalecPage());
        }

        private async void PrijavaBtnTappedAsync(object obj)
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
                var userCredential = await client.SignInWithEmailAndPasswordAsync(email, geslo);
                var serializiranaVsebina = JsonConvert.SerializeObject(userCredential.AuthCredential);

                Preferences.Set("SvezToken", serializiranaVsebina);
                await _navigation.PushAsync(new NajemodajalecPage());

            }
            catch (FirebaseAuthException firebaseEx)
            {
                if (firebaseEx.Reason.ToString() == "Unknown")
                {
                    await Application.Current.MainPage.DisplayAlert("Opozorilo", "Vnesli ste narobne prijavne podatke. Zmotili ste se pri vnosu uporabniskega imena in/ali gesla.", "OK");
                }

            }



        }
    }
}
