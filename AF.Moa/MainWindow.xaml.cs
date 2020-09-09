using AF.Moa.Config;
using AF.Moa.Macro;
using AF.Moa.Navigator;
using mshtml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace AF.Moa
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        IEController Controller = null;

        private AttributeParser wConfig = new AttributeParser("./Config/WindowConfig.txt");
        private PageListParser pages = new PageListParser("./Config/PageList.txt");

        public MainWindow()
        {
            InitializeComponent();
            InitializeWindowConfig();

            this.MinWidth = Navigator.Width * 2;
            Controller = new IEController(Browser);
            Controller.AddOnLoadCompleted(new ScriptInvoker());
            Controller.AddOnLoadCompleted(new PostHeaderRemover());
            Controller.Navigate(pages.HomePage);

            InitializeNavigator(pages.Pages);
        }

        private void InitializeWindowConfig()
        {
            var width = wConfig.GetAttribute("Width");
            if (width != null)  this.Width = int.Parse(width);

            var height = wConfig.GetAttribute("Height");
            if (height != null) this.Height = int.Parse(height);

            var wState = wConfig.GetAttribute("WindowState");
            if(wState != null)
                this.WindowState = wState == "Maximized" ? WindowState.Maximized : WindowState.Normal;

            this.Closing += delegate (object sender, System.ComponentModel.CancelEventArgs e)
            {
                wConfig.SetAttribute("Width", this.Width.ToString());
                wConfig.SetAttribute("Height", this.Height.ToString());
                wConfig.SetAttribute("WindowState", this.WindowState.ToString());
                wConfig.Write();
            };
        }

        private void InitializeNavigator(List<Node> pages)
        {
            foreach (var page in pages)
            {
                var view = new NavigatorView(page);
                view.Navigate += delegate (string url)
                {
                    Controller.Navigate(url);
                };
                Navigator.Children.Add(view);
            }
        }
    }
}
