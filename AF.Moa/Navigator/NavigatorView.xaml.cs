﻿using System;
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
        SolidColorBrush defaultBackgroundBrush = new SolidColorBrush(Color.FromArgb(30, 255, 255, 255));
        SolidColorBrush hoverBackgroundBrush = new SolidColorBrush(Color.FromArgb(60, 255, 255, 255));

        public event Action<string> Navigate = null;

        public NavigatorView(Config.Node page)
        {
            InitializeComponent();
            this.Name.Content = page.PageName;
            if (page.PageUrl != null)
            {
                Body.MouseEnter += Body_MouseEnter;
                Body.MouseLeave += Body_MouseLeave;
                Body.MouseDown += delegate (object sender, MouseButtonEventArgs e)
                {
                    Navigate?.Invoke(page.PageUrl);
                    e.Handled = true;
                };
            }
            Body.Background = defaultBackgroundBrush;
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
            FoldButton.Visibility = SubPagesContainer.Children.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        private void Body_MouseEnter(object sender, MouseEventArgs e)
        {
            Body.Background = hoverBackgroundBrush;
        }

        private void Body_MouseLeave(object sender, MouseEventArgs e)
        {
            Body.Background = defaultBackgroundBrush;
        }

        private void FoldButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SubPagesContainer.Visibility = SubPagesContainer.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            var b = new BitmapImage();
            b.BeginInit();
            var uriBase = $"/{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name};component/Resources/";
            b.UriSource = SubPagesContainer.Visibility == Visibility.Visible ?
                new Uri(uriBase + "expend.png", UriKind.Relative) :
                new Uri(uriBase + "fold.png", UriKind.Relative);
            b.EndInit();
            FoldButtonImage.Source = b;
            e.Handled = true;
        }

        /**
         * 메뉴 앞 띄지 색상 변경
         */
        public void SetBrush(SolidColorBrush brush)
        {
            this.LeftBorder.Background = brush;
            foreach (var child in this.SubPagesContainer.Children)
            {
                if(child is NavigatorView)
                {
                    ((NavigatorView)child).SetBrush(brush);
                }
            }
        }
    }
}
