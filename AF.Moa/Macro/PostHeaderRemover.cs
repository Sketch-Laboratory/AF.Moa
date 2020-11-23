using mshtml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

/**
 * 머릿말 제거 코드
 */
namespace AF.Moa.Macro
{
    class PostHeaderRemover : IMacro
    {
        private string[] Headers;

        public PostHeaderRemover()
        {
            Headers = File.ReadAllLines("./Macro/PostHeaders.txt");
        }
        public void OnPageLoaded(IEController controller, string url)
        {
            if (!url.StartsWith("http://www.af.mil/user/boardList.action?command=view")) return;
            try
            {
                foreach (var line in Headers)
                {
                    if (string.IsNullOrEmpty(line)) continue;
                    var s = $"document.getElementsById('divView').outerHTML = document.getElementsById('divView').outerHTML.replaceAll('{line.Trim()}', '');";
                    controller.RunScript(s);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"document 조작에러: {ex.ToString()}");
            }
        }
    }
}
