using AF.Moa.Config;
using AF.Moa.Macro;
using AF.Moa.Navigator;
using CefSharp;
using CefSharp.Wpf;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AF.Moa {
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window {
        IEController Controller = null;

        private PageListParser pages = new PageListParser("./Config/PageList.txt");
        private AttributeParser wConfig = new AttributeParser("./Config/WindowConfig.txt");
        private AttributeParser foldState = new AttributeParser("./Config/FoldState");

        public MainWindow() {
            InitializeChromium();
            InitializeComponent();
            InitializeWindowConfig();

            this.MinWidth = Navigator.Width * 2;
            Controller = new IEController(Browser);
            Controller.AddOnLoadCompleted(new ScriptInvoker());
            Controller.AddOnLoadCompleted(new PostHeaderRemover());
            Browser.IsBrowserInitializedChanged += delegate (object sender, DependencyPropertyChangedEventArgs e) {
                if(Browser.IsBrowserInitialized) Controller.Navigate(pages.HomePage);
            };

            InitializeNavigator(pages.Pages);
        }

        private void InitializeChromium() {
            var settings = new CefSettings {
                BrowserSubprocessPath = System.IO.Path.GetFullPath("./x86/CefSharp.BrowserSubprocess.exe")
            };

            Cef.Initialize(settings, performDependencyCheck: false, browserProcessHandler: null);
        }

        private void InitializeWindowConfig() {
            var width = wConfig.GetAttribute("Width");
            if (width != null) this.Width = float.Parse(width);

            var height = wConfig.GetAttribute("Height");
            if (height != null) this.Height = float.Parse(height);

            var wState = wConfig.GetAttribute("WindowState");
            if (wState != null)
                this.WindowState = wState == "Maximized" ? WindowState.Maximized : WindowState.Normal;

            this.Closing += delegate (object sender, System.ComponentModel.CancelEventArgs e) {
                wConfig.SetAttribute("Width", this.Width.ToString());
                wConfig.SetAttribute("Height", this.Height.ToString());
                wConfig.SetAttribute("WindowState", this.WindowState.ToString());
                wConfig.Write();

                SaveFoldState();
            };
        }

        private void InitializeNavigator(List<Node> pages) {
            var brushes = new SolidColorBrush[] {
                new SolidColorBrush(Color.FromRgb(250, 250, 250)),
                new SolidColorBrush(Color.FromRgb(251, 192, 45)),
                new SolidColorBrush(Color.FromRgb(126, 87 , 194)),
                new SolidColorBrush(Color.FromRgb(33, 150, 243)),
                new SolidColorBrush(Color.FromRgb(244, 81, 30))
            };

            for (int i = 0; i < pages.Count; i++) {
                var page = pages[i];
                // 홈페이지 처리
                if (page.PageName.ToLower() == "homepage") {
                    Logo.MouseDown += delegate {
                        Controller.Navigate(page.PageUrl);
                    };
                    continue;
                }

                var view = new NavigatorView(page);
                view.Navigate += delegate (string url) {
                    Controller.Navigate(url);
                };
                view.Margin = new Thickness(5, 5, 5, 0);
                view.SetBrush(brushes[i % brushes.Length]);
                Navigator.Children.Add(view);
            }

            LoadFoldState();
        }

        private void Browser_LostFocus(object sender, RoutedEventArgs e) {
            ((Control)sender).Focus();
        }

        private void LoadFoldState() {
            foreach (var attr in foldState.Attributes) {
                foreach (var nav in Navigator.Children) {
                    if (nav is NavigatorView) SetCollapseStyle((NavigatorView)nav, attr);
                }
            }
        }

        private void SaveFoldState() {
            foreach (var nav in Navigator.Children) {
                if (nav is NavigatorView) GetcollapseStyle((NavigatorView)nav, foldState);
            }
            foldState.Write();
        }

        private void SetCollapseStyle(NavigatorView nav, AttributeParser.Attribute attr, int deep = 0) {
            var prefix = "";
            for (int i = 0; i < deep; i++) { prefix += '\t'; }
            if (prefix + nav.Name.Content.ToString() == attr.Name) {
                var visibility = attr.Value == true.ToString() ? Visibility.Visible : Visibility.Collapsed;
                nav.SubPagesContainer.Visibility = visibility;
            }
            else foreach (var subNav in nav.SubPagesContainer.Children) {
                    if (subNav is NavigatorView) SetCollapseStyle((NavigatorView)subNav, attr, deep + 1);
                }
        }

        private void GetcollapseStyle(NavigatorView nav, AttributeParser foldState, int deep = 0) {
            var prefix = "";
            for (int i = 0; i < deep; i++) { prefix += '\t'; }
            var expended = nav.SubPagesContainer.Visibility == Visibility.Visible;
            foldState.SetAttribute(prefix + nav.Name.Content.ToString(), expended.ToString());
            foreach (var subNav in nav.SubPagesContainer.Children) {
                if (subNav is NavigatorView) GetcollapseStyle((NavigatorView)subNav, foldState, deep + 1);
            }
        }
    }
}
