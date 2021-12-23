using System;
using System.IO;
using System.Xml.Serialization;

namespace Motakim.EngineTools
{
    public sealed class Project
    {
        [XmlAttribute]
        public int Version;
    }
}

