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
        PageListParser pages = new PageListParser("./Config/PageList.txt");
        AutoLoginParser accounts = new AutoLoginParser("./Config/AutoLogin.txt");

        public MainWindow()
        {
            InitializeComponent();
            Browser.Navigate(pages.HomePage);
            Browser.LoadCompleted += Browser_LoadCompleted;
        }

        private void Browser_LoadCompleted(object sender, NavigationEventArgs e)
        {
            var document = (IHTMLDocument3)Browser.Document;
            #region AutoLogin
            foreach (var account in accounts.List)
            {
                if (e.Uri.ToString() == account.Url)
                {
                    ((IHTMLInputElement)document.getElementById(account.IDFormId)).value = account.ID;
                    ((IHTMLInputElement)document.getElementById(account.PasswordFormId)).value = account.Password;
                    document.getElementById(account.ConfirmButtonId).click();
                }
            }
            #endregion
            #region 게시판 최적화 함수

            #endregion
        }
    }
}
