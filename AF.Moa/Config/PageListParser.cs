using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AF.Moa.Config
{
    class PageListParser
    {
        public List<Tuple<string, string>> List { get; private set; } = new List<Tuple<string, string>>();
        public string HomePage { get { return List.Find(x => x.Item1.ToLower() == "homepage").Item2; } }

        public PageListParser(string path)
        {
            foreach (var line in File.ReadAllLines(path))
            {
                if (!line.Contains('=')) continue;
                var attrs = StringFunction.splitWithFirst(line, "=");
                List.Add(new Tuple<string, string>(attrs[0].Trim(), attrs[1].Trim()));
            }
        }
    }
}
