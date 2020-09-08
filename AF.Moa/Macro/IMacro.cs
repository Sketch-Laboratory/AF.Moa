using System.Windows.Navigation;

namespace AF.Moa.Macro
{
    interface IMacro
    {
        void OnPageLoaded(IEController controller, NavigationEventArgs e);
    }
}
