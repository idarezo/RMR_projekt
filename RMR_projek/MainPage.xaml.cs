namespace RMR_projek;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
        BindingContext = new ViewModels.VpisViewModel(Navigation, ImeEntry, PriimekEntry);

    }
    private async void OnPojdiNaUcniNacrtClicked(object sender, EventArgs e)

    {

        await Navigation.PushAsync(new RegistracijaPage());

    }
    private void prijavaClick(object sender, EventArgs e)
    {

        string ime = ImeEntry.Text;
        string priimek = PriimekEntry.Text;

        if (ime != "ime" || priimek != "priimek")
        {

            DisplayAlert("Napacno uporabnisko ime ali geslo", "Prosimo, poskusite", "V redu");
        }
        else
        {
            ImeEntry.Text = "";
            PriimekEntry.Text = "";
            OnPojdiNaUcniNacrtClicked(sender, e);

        }

        


    }

    private async void registracijaClick(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new RegistracijaPage());

    }
}

