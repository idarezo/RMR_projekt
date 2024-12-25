using Firebase.Auth.Providers;
using Firebase.Auth;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RMR_projek.Models;

namespace RMR_projek.ViewModels
{
    internal class VpisViewModel
    {
        public string webApiKey = "AIzaSyBqhp1SpzEHiLxqk-VD8bfpy3nMYJRJmEw";

        private INavigation _navigation;

        public Command RegistracijaBtn { get; }
        public Command PrijavaBtn { get; }
        public Command preveriGmail { get; }

        public Uporabnik trenutniUporabnik { get; set; }


        public string email { get; set; }
        public string geslo { get; set; }
        public string userId { get; set; }
        public string userEmail { get; set; }
        public string displayName { get; set; }

        


        private Entry _eTxEmail;
        private Entry _eTxpass;

        public VpisViewModel(INavigation navigation, Entry eTxEmail, Entry eTxPass)
        {
            _navigation = navigation;
            RegistracijaBtn = new Command(RegistracijaBtnTappedAsync);
            PrijavaBtn = new Command(PrijavaBtnTappedAsync);
            preveriGmail = new Command(preveriBtn);

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
                var user = userCredential.User;
                 userId = user.Uid; 
                 userEmail = user.Info.Email; 
                 displayName = user.Info.DisplayName; 

                Preferences.Set("SvezToken", serializiranaVsebina);

                // Check if user exists in the Firebase Realtime Database
                var databaseUrl = "https://rmr-projektnovo-default-rtdb.europe-west1.firebasedatabase.app/";
                var userRef = $"users";  
                var response = await new HttpClient().GetStringAsync(databaseUrl + userRef + ".json");

                if (!string.IsNullOrEmpty(response) && response != "null")
                {

                    var retrievedUsers = JsonConvert.DeserializeObject<Dictionary<string, Uporabnik>>(response);
                    if (retrievedUsers != null)
                    {
                        // Find the user based on email
                        var iskaniUporabnik = retrievedUsers.Values.FirstOrDefault(user => user.email == userEmail);

                        if (iskaniUporabnik != null)
                        {
                            trenutniUporabnik = new Uporabnik(iskaniUporabnik.naslov, iskaniUporabnik.email, iskaniUporabnik.id, iskaniUporabnik.ime, iskaniUporabnik.telefon, iskaniUporabnik.geslo);
                        }
                    }
                }


                if (email =="admin@gmail.com")
                {
                    await _navigation.PushAsync(new NajemodajalecPage());
                }
                else
                {
                    await _navigation.PushAsync(new PodnajemnikPage(trenutniUporabnik));

                }


            }
            catch (FirebaseAuthException firebaseEx)
            {
                if (firebaseEx.Reason.ToString() == "Unknown")
                {
                    await Application.Current.MainPage.DisplayAlert("Opozorilo", "Vnesli ste narobne prijavne podatke. Zmotili ste se pri vnosu uporabniskega imena in/ali gesla.", "OK");
                }

            }

        }

        private async void preveriBtn(object obj)
        {
            HttpClient httpclient = new HttpClient();
            var response = await httpclient.GetStringAsync("https://rmr-projektnovo-default-rtdb.europe-west1.firebasedatabase.app/");
            var uporabnikiList = JsonConvert.DeserializeObject<List<Uporabnik>>(response);

            if (!string.IsNullOrEmpty(response) && response != "null") // Preveri, če je odgovor veljaven
            {

                bool emailObstaja = uporabnikiList.Any(uporabnik => uporabnik.email == "admin@gmail.com");
                if (emailObstaja)
                {
                    await _navigation.PushAsync(new NajemodajalecPage());

                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Obvestilo", "E-pošta ni v bazi.", "OK");
                }
            }


        }
    }
}
