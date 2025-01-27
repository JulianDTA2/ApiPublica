using SPJApiPublica.Views;

namespace SPJApiPublica
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

          
            MainPage = new NavigationPage(new CarPage());
        
        }
    }
}
