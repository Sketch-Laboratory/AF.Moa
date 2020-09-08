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

        private PageListParser pages = new PageListParser("./Config/PageList.txt");

        public MainWindow()
        {
            InitializeComponent();
            Controller = new IEController(Browser);
            Controller.AddOnLoadCompleted(new ScriptInvoker());
            Controller.AddOnLoadCompleted(new PostHeaderRemover());
            Controller.Navigate(pages.HomePage);

            InitializeNavigator(pages.List);
        }

        private void InitializeNavigator(List<Tuple<string, string>> pages)
        {
            foreach (var page in pages)
            {
                var view = new NavigatorView();
                view.Name.Content = page.Item1;
                view.MouseDown += delegate
                {
                    Controller.Navigate(page.Item2);
                };
                this.Navigator.Children.Add(view);
            }
        }
    }
}
