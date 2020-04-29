using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;


namespace Vkloud
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FileBrowserPage : Page
    {
        public FileBrowserPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var vkClient = e.Parameter as VkApi.Client;
            var model = new Model.FileSystemModel(vkClient);
            //FileView.ItemsSource = docs;
        }
    }
}
