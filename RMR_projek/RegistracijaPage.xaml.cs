namespace RMR_projek;

public partial class RegistracijaPage : ContentPage
{
	public RegistracijaPage()
	{
		InitializeComponent();
        BindingContext = new ViewModels.RegistracijaViewModel(Navigation, UporabniskoImeEntry, GesloEntry);

    }
}