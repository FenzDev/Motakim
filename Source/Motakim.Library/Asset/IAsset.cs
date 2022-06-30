using System;
using System.IO;

namespace Motakim
{
    public interface IAsset : IDisposable
    {
        string Name { get; }
        
        void Load(Stream stream);
    }
}