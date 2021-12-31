using System;
using System.Diagnostics;
using System.IO;

namespace Motakim.EngineTools
{
    public abstract class ProjectBuilder
    {
        protected ProjectBuilder(ProjectManager manager)
        {
            Manager = manager;
        }

        protected ProjectManager Manager { get; set; }
        protected string GeneratePath => Path.Combine(Manager.ProjectPath, ".M7K.Generated");

        public virtual void Generate()
        {
            var genprojdir = Path.Combine(GeneratePath, "_src");
            Directory.CreateDirectory(genprojdir);

            var codefile = File.Open(Path.Combine(genprojdir, "_GameCode.cs"), FileMode.OpenOrCreate);
            GenerateCode(new StreamWriter(codefile));

            var csprojfile = File.Open(Path.Combine(genprojdir, "_Game.csproj"), FileMode.OpenOrCreate);
            GenerateCode(new StreamWriter(csprojfile));
        }
        protected abstract void GenerateCode(StreamWriter writer);
        protected abstract void GenerateProjectFile(StreamWriter writer);
        protected void MSBuild(string options)
        {
            Process.Start("dotnet", $"dotnet build {options}").WaitForExit();
        }
    }
}


