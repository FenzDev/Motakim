using System;
using System.IO;

namespace Motakim.EngineTools
{
    public abstract class ProjectBuilder
    {
        public ProjectBuilder(ProjectManager manager)
        {
            Manager = manager;
        }

        protected ProjectManager Manager { get; set; }
        protected string GeneratePath => Path.Combine(Manager.ProjectPath, ".M7K.Generated");

        public virtual void Generate()
        {
            var genprojdir = Path.Combine(GeneratePath, "csproj");
            Directory.CreateDirectory(genprojdir);

            var codefile = File.Create(Path.Combine(genprojdir, "GameCode.cs"));
            GenerateCode(new StreamWriter(codefile));
        }
        protected abstract void GenerateCode(StreamWriter writer);
        protected virtual void MSBuild(string options)
        {
            
        }
    }
}


