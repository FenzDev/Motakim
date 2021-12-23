using System;
using System.Resources;
using System.Reflection;
using Microsoft.Xna.Framework;

namespace Motakim.Editor
{
    public class EditorProgram
    {
        public static void Start(params string[] args)
        {
            var msg = "Starting 'Motakim Engine Editor'...";
            Console.WriteLine(msg);
            using (var mg = new MGEditor()) mg.Run();
            
        } 
    }
}
