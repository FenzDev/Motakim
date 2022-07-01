using System;
using System.IO;

namespace Motakim
{
    public interface IAsset : IDisposable
    {
        public string Name { get; set; }
        
        public void Load(Stream stream);
    }
}