using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Navigation;
using AF.Moa.Config;
using mshtml;

namespace AF.Moa
{
    class IEController
    {
        private WebBrowser Browser { get; } = null;
        private IHTMLDocument3 document { get; set; } = null;
        public List<Action<NavigationEventArgs>> LoadCompleted { get; internal set; } = new List<Action<NavigationEventArgs>>();

        public IEController(WebBrowser browser)
        {
            Browser = browser;
            Browser.LoadCompleted += Browser_LoadCompleted;
        }

        private void Browser_LoadCompleted(object sender, NavigationEventArgs e)
        {
            document = (IHTMLDocument3)Browser.Document;
            foreach (var @event in LoadCompleted) @event(e);
        }

        public void Navigate(string url)
        {
            document = null;
            Browser.Navigate(url);
        }
        
        private void Handle(Action @delegate)
        {
            try
            {
                @delegate();
            }
            catch (NullReferenceException ne)
            {
                Console.WriteLine($"NullReferenceException: ID 값이 일치하는 객체를 찾을 수 없었습니다.");
                throw ne;
            }
        }

        public void SetValue(string elementId, string value)
        {
            Handle(delegate
            {
                var element = document.getElementById(elementId);
                if(element is IHTMLInputElement)
                {
                    ((IHTMLInputElement)element).value = value;
                }
            });
        }

        public void Click(string elementId)
        {
            Handle(delegate
            {
                var button = document.getElementById(elementId);
                button.click();
            });
        }

        public void RunScript(string js)
        {

        }
    }
}
