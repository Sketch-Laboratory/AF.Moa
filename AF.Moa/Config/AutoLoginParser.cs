using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AF.Moa.Config
{
    class AutoLoginParser
    {
        public List<AutoLoginInfo> List { get; private set; } = new List<AutoLoginInfo>();
        public AutoLoginParser(string path)
        {
            AutoLoginInfo block = null;
            foreach (var line in File.ReadAllLines(path))
            {
                var l = line.Trim();
                if(l.StartsWith("[") && l.EndsWith("]"))
                {
                    if (block != null) List.Add(block); // 이전 블록을 리스트에 저장
                    block = new AutoLoginInfo(); // 새 블록 전개
                }
                else if(l.Contains('='))
                {
                    var attrs = StringFunction.splitWithFirst(line, "=");
                    block.Set(attrs[0].Trim(), attrs[1].Trim());
                }
            }
            if (block != null) List.Add(block); // 마지막 블록을 리스트에 저장
        }

        public class AutoLoginInfo
        {
            public string Url { get; internal set; }
            public string IDFormId { get; internal set; }
            public string PasswordFormId { get; internal set; }
            public string ID { get; internal set; }
            public string Password { get; internal set; }
            public string ConfirmButtonId { get; internal set; }

            public void Set(string propertyName, string value)
            {
                switch(propertyName.ToLower())
                {
                    case "url":
                        Url = value;
                        break;
                    case "idform":
                        IDFormId = value;
                        break;
                    case "pwform":
                        PasswordFormId = value;
                        break;
                    case "confirmbtn":
                        ConfirmButtonId = value;
                        break;
                    case "id":
                        ID = value;
                        break;
                    case "pw":
                        Password = value;
                        break;
                }
            }
        }
    }
}
