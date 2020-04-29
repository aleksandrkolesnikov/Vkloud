using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using VkApi;


namespace Vkloud
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var client = new Client(LoginField.Text, PasswordField.Password);

            Frame.Navigate(typeof(FileBrowserPage), client);
        }
    }
}
