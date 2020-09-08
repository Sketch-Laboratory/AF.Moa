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
            this.MinWidth = Navigator.Width * 2;
            FitBrowserWidth();
            Controller = new IEController(Browser);
            Controller.AddOnLoadCompleted(new ScriptInvoker());
            Controller.AddOnLoadCompleted(new PostHeaderRemover());
            Controller.Navigate(pages.HomePage);

            InitializeNavigator(pages.Pages);
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
            /**
             * TODO: Navigator 안에서 컨텐츠 스크롤이 되어야 함
             */
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            FitBrowserWidth();
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if(this.WindowState == WindowState.Maximized)
            {
                /**
                 * TODO: 최대화 시 화면 크기 에서 Navigator의 Width를 뺀 값으로 설정
                 * 화면 크기 구해오는 방법 알아내야 함.
                 */
            }
            else  FitBrowserWidth();
            
        }

        private void FitBrowserWidth()
        {
            Browser.Width = this.Width - Navigator.Width;
        }
    }
}
