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
        public List<Node> Pages { get; private set; } = new List<Node>();
        public string HomePage { get { return Pages.Find(x => x.PageName.ToLower() == "homepage")?.PageUrl; } }

        public PageListParser(string path)
        {
            foreach (var line in File.ReadAllLines(path))
            {
                if (!line.Contains('=')) continue;
                AddIntoSubNode(Pages, line);
            }
        }

        private void AddIntoSubNode(List<Node> tree, string line)
        {
            if (line[0] == '\t')
            {
                // 탭이 들어가있는 경우 가장 최근에 추가된 노드의 하위 노드로 추가하도록 재귀
                AddIntoSubNode(tree[tree.Count - 1].SubPages, line.Substring(1));
            }
            else tree.Add(MakeNode(line));
        }

        private Node MakeNode(string line)
        {
            var attrs = StringFunction.splitWithFirst(line, "=");
            return new Node(attrs[0].Trim(), attrs[1].Trim());
        }
    }
    public class Node
    {
        public string PageName;
        public string PageUrl;
        public List<Node> SubPages = new List<Node>();

        public Node(string name, string url)
        {
            this.PageName = name;
            this.PageUrl = url.ToLower() == "none" ? null : url;
        }
    }
}
