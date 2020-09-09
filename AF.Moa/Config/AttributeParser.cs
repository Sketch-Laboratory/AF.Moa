using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AF.Moa.Config
{
    class AttributeParser
    {
        public List<Attribute> Attributes { get; private set; } = new List<Attribute>();
        public string FilePath { get; }

        public AttributeParser(string path)
        {
            this.FilePath = path;
            foreach (var line in File.ReadAllLines(path))
            {
                if (!line.Contains('=')) continue;
                var attrs = line.Split('=');
                Attributes.Add(new Attribute(attrs[0], attrs[1]));
            }
        }

        public void Write()
        {
            var stream = string.Empty;
            foreach (var item in Attributes) stream += $"{item.ToString()}\n";
            File.WriteAllText(FilePath, stream);
        }

        public class Attribute
        {
            public string Name { get; set; }
            public string Value { get; set; }

            public Attribute(string name, string value)
            {
                this.Name = name;
                this.Value = value;
            }

            public override string ToString()
            {
                return $"{Name}={Value}";
            }
        }

        internal void SetAttribute(string attrName, string newValue)
        {
            Attributes.RemoveAll(x => { return x.Name == attrName; });
            Attributes.Add(new Attribute(attrName, newValue));
        }

        internal string GetAttribute(string attrName)
        {
            return Attributes.Find(it => { return it.Name == attrName; })?.Value;
        }
    }
}
