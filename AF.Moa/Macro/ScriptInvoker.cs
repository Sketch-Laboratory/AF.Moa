using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace AF.Moa.Macro
{
    class ScriptInvoker : IMacro
    {
        private List<Tuple<string, string>> ScriptIndexes = new List<Tuple<string, string>>();

        public ScriptInvoker()
        {
            foreach (var line in File.ReadAllLines("./Macro/ScriptIndex.txt"))
            {
                if (!line.Contains("=")) continue;
                var attributes = line.Split('=');
                ScriptIndexes.Add(new Tuple<string, string>(attributes[0], attributes[1]));
            }
        }

        public void OnPageLoaded(IEController controller, NavigationEventArgs e)
        {
            foreach (var scriptIndex in ScriptIndexes)
            {
                if(e.Uri.ToString().StartsWith(scriptIndex.Item2))
                {
                    var filePath = $"./Macro/Scripts/{scriptIndex.Item1}";
                    if (!File.Exists(filePath))
                    {
                        Console.WriteLine($"스크립트 파일이 없습니다: {filePath}");
                        continue;
                    }
                    var js = File.ReadAllText(filePath).Replace("\n", "").Trim();
                    controller.RunScript(js);
                }
            }
        }
    }
}
