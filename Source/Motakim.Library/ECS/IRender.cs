using System;
using System.IO;

namespace Motakim
{
    public interface IRender : IComponent
    {
        void Render(RenderHelper render);
    }
}