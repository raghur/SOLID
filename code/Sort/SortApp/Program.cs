using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SortApp
{
    public interface IFileWrapper
    {
        bool Exists(String filename);
        String[] ReadAllLines(string filename);
    }

    public class FileWrapper : IFileWrapper
    {

        public bool Exists(string filename)
        {
            return File.Exists(filename);
        }

        public string[] ReadAllLines(string filename)
        {
            return File.ReadAllLines(filename);
        }
    }

    public class Program
    {
        public static IFileWrapper filewrapper = new FileWrapper();
        public static TextWriter writer = Console.Out;
        public static int Main(string[] args)
        {
            if (args.Length == 0)
            {
                writer.WriteLine(" Please provide filename to sort");
                return 1;
            }

            String filename = args[0];
            if (!filewrapper.Exists(filename))
            {
                writer.WriteLine("File does not exist: " + filename);
                return 2;
            }
            string[] lines = filewrapper.ReadAllLines(filename);
            Array.Sort(lines);
            for (int i = 0; i < lines.Length; i++)
            {
                writer.WriteLine(lines[i]);
            }
            return 0;
        }
    }
}
