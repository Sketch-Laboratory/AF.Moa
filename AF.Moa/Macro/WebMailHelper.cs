using System.Windows.Navigation;

namespace AF.Moa.Macro
{
    class WebMailHelper : IMacro
    { 
        public void OnPageLoaded(IEController controller, NavigationEventArgs e)
        {
            var webMailFrontPageUrl = "http://cloud16.hq.af.mil/servlet/crinity?ptype=sso&paction=login";
            if (e.Uri.ToString() == webMailFrontPageUrl)
            {
                controller.RunScript("loadMsg();");
            }
        }
    }
}
