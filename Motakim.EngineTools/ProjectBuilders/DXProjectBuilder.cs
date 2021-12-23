using System;
using System.IO;

namespace Motakim.EngineTools.ProjectBuilders
{
    public class DXProjectBuilder : ProjectBuilder
    {
        public DXProjectBuilder(ProjectManager manager) : base(manager)
        {
            
        }

        protected override void GenerateCode(StreamWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
