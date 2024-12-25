using Firebase.Database;
using RMR_projek.Models;

namespace RMR_projek;

public partial class MojProfilPage : ContentPage
{
    Uporabnik uporabnik { get; set; }
	public MojProfilPage(Uporabnik uporabnik)
	{
		InitializeComponent();
        this.uporabnik = uporabnik;
        imeEntry.Text = uporabnik.ime ?? string.Empty; 
        priimekEntry.Text = uporabnik.priimek ?? string.Empty;
        naslovEntry.Text = uporabnik.naslov ?? string.Empty;
        emailEntry.Text = uporabnik.email ?? string.Empty;
        telefonEntry.Text = uporabnik.telefon ?? string.Empty;
        gesloEntry.Text = uporabnik.geslo ?? string.Empty;
    }


    async public void OnSaveClicked()
    {
        var ime = imeEntry.Text;
        var priimek = priimekEntry.Text;
        var naslov = naslovEntry.Text;
        var email = emailEntry.Text;
        var telefon = telefonEntry.Text;
        var geslo = gesloEntry.Text;

      
        var uporabnik = new Uporabnik(naslov, email, 0, ime, telefon, geslo);

        DisplayAlert("Uspešno!", "Podatki so bili shranjeni.", "OK");

        imeEntry.Text = string.Empty;
        priimekEntry.Text = string.Empty;
        naslovEntry.Text = string.Empty;
        emailEntry.Text = string.Empty;
        telefonEntry.Text = string.Empty;
        gesloEntry.Text = string.Empty;

        var userId = uporabnik.email;  

      
        var updatedUser = new Uporabnik(naslov, email, uporabnik.id, ime, telefon, geslo);
        await UpdateUserInFirebase(userId, updatedUser);



    }

    private async Task UpdateUserInFirebase(string userId, Uporabnik updatedUser)
    {
        var firebase = new FirebaseClient("https://your-firebase-url.firebaseio.com/");
        


        Console.WriteLine("User data updated successfully.");

    }
    }