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

namespace AF.Moa.Navigator
{
    /// <summary>
    /// NativatorView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class NavigatorView : StackPanel
    {
        SolidColorBrush defaultBackgroundBrush = new SolidColorBrush(Color.FromArgb(10, 0, 0, 0));
        SolidColorBrush hoverBackgroundBrush = new SolidColorBrush(Color.FromArgb(80, 0, 0, 0));

        public event Action<string> Navigate = null;

        public NavigatorView(Config.Node page)
        {
            InitializeComponent();
            this.Name.Content = page.PageName;
            if(page.PageUrl != null) this.MouseDown += delegate
            {
                Navigate?.Invoke(page.PageUrl);
            };
            this.Background = defaultBackgroundBrush;
            InflateSubPages(page.SubPages);
        }

        private void InflateSubPages(List<Config.Node> subPages)
        {
            foreach (var page in subPages)
            {
                var view = new NavigatorView(page);
                view.Navigate += delegate (string url)
                {
                    Navigate?.Invoke(url);
                };
                SubPagesContainer.Children.Add(view);
            }
        }

        private void UserControl_MouseEnter(object sender, MouseEventArgs e)
        {
            this.Background = hoverBackgroundBrush;
        }

        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        {
            this.Background = defaultBackgroundBrush;
        }
    }
}
