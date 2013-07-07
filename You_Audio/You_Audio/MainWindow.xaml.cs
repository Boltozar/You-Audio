using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace You_Audio
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Pages.InitializePages();
            SetPage(Pages.SearchPage);
            Download.SetupFFMPEG();
        }

        public void SetPage(UserControl page)
        {
            this.Content = page;
        }
    }

    //"container class" to access our pages
    public static class Pages
    {
        private static MainWindow _mainWindow = (MainWindow)Application.Current.MainWindow;
        private static SearchPage _searchPage;
        private static ResultsPage _resultsPage;

        public static void InitializePages()
        {
            _searchPage = new SearchPage();
            _resultsPage = new ResultsPage();
        }

        public static MainWindow MainWindow
        {
            get
            {
                return _mainWindow;
            }
        }

        public static SearchPage SearchPage
        {
            get
            {
                return _searchPage;
            }
        }

        public static ResultsPage ResultsPage
        {
            get
            {
                return _resultsPage;
            }
        }
    }
}

