using SPJApiPublica.ViewModel;

namespace SPJApiPublica.Views;

public partial class CarPage : ContentPage
{
	public CarPage()
	{
		InitializeComponent();
        BindingContext = new CarViewModel();
    }
}