using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Motakim.EngineTools
{
    public class ProjectManager
    {
        public ProjectBuilder Builder { get; set; }
        public Project CurrentProject { get; set; }

        public string FilePath { get; set; }
        public string ProjectPath => Path.GetDirectoryName(FilePath);

        public void LoadProject()
        {
            if (CurrentProject != null) UnloadProject();

            var projfile = File.OpenRead(FilePath);
            var serializer = new XmlSerializer(typeof(Project));
            CurrentProject = (Project)serializer.Deserialize(projfile);
            projfile.Close();
        }
        public void UnloadProject()
        {
            CurrentProject = null;
            FilePath = "";
        }
        public void SaveProjetc()
        {
            var projfile = File.OpenWrite(FilePath);
            var serializer = new XmlSerializer(typeof(Project));
            var writer = XmlWriter.Create(projfile, new XmlWriterSettings()
            {
                OmitXmlDeclaration = true,
                CloseOutput = true
            });
            serializer.Serialize(projfile, CurrentProject);
            writer.Close();
        }
    }
}

