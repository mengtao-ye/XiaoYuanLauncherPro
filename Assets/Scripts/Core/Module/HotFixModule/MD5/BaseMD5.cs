using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Game
{
    [System.Serializable]
    public class BaseMD5
    {
        [XmlAttribute("Name")]
        public string Name { get; set; }
        [XmlAttribute("MD5")]
        public string MD5 { get; set; }
        [XmlAttribute("Size")]
        public float Size { get; set; }
    }
}
