using System.Collections.Generic;
using System.Xml.Serialization;

namespace Game
{
    [System.Serializable]
    public class ABMD5
    {
        [XmlElement("MD5List")]
        public List<BaseMD5> MD5List { get; set; }
    }
}
