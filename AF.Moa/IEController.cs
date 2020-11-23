using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Threading;
using AF.Moa.Macro;
using CefSharp;
using CefSharp.Wpf;

namespace AF.Moa {
    class IEController
    {
        private ChromiumWebBrowser Browser { get; } = null;


        public IEController(ChromiumWebBrowser browser)
        {
            Browser = browser;
            Browser.LoadingStateChanged += this.Browser_LoadingStateChanged; ;
        }

        private void Browser_LoadingStateChanged(object sender, LoadingStateChangedEventArgs e) 
        {
            if (!e.IsLoading) {
                Browser.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => {
                    foreach (var macro in Macros) macro.OnPageLoaded(this, Browser.Address);
                }));
            };
        }

        #region LoadCompleted Event 관련
        private readonly List<IMacro> Macros = new List<IMacro>();

        internal void AddOnLoadCompleted(IMacro macro)
        {
            this.Macros.Add(macro);
        }
        #endregion

        #region 상호작용 Interface

        public void Navigate(string url) {
            if (url == null) return;
            Browser.Load(url);
        }

        public void RunScript(string script)
        {
            this.Browser.ExecuteScriptAsync(script);
        }

        public async Task GetDocument(Action<string> callback) {
            var source = await Browser.GetBrowser().MainFrame.GetSourceAsync();
            callback(source);
        }

        #endregion
    }
}
