using AF.Moa.Config;
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
        private AutoLoginParser accounts = new AutoLoginParser("./Config/AutoLogin.txt");

        public MainWindow()
        {
            InitializeComponent();
            Controller = new IEController(Browser);
            Controller.LoadCompleted.Add(AutoLogin);
            Controller.Navigate(pages.HomePage);
        }

        private void AutoLogin(NavigationEventArgs e)
        {
            foreach (var account in accounts.List)
            {
                if (e.Uri.ToString() == account.Url)
                {
                    Controller.SetValue(account.IDFormId, account.ID);
                    Controller.SetValue(account.PasswordFormId, account.Password);
                    Controller.Click(account.ConfirmButtonId);
                }
            }
        }
    }
}
