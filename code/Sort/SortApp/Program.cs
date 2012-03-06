using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SortApp
{
    public interface ISort
    {
        void Sort(string[] array);
    }

    public class ArraySorter:ISort
    {
        public void  Sort(string[] array)
        {
 	        Array.Sort(array);
        }
    }

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

        private IFileWrapper filewrapper;
        private TextWriter writer;
        private ISort sorter;
        public Program(IFileWrapper fw, ISort sorter, TextWriter ow)
        {
            filewrapper = fw;
            this.sorter = sorter;
            writer = ow;
        }

        public int DoMain(string[] args)
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
            sorter.Sort(lines);
            for (int i = 0; i < lines.Length; i++)
            {
                writer.WriteLine(lines[i]);
            }
            return 0;

        }

        public static int Main(string[] args)
        {
            Program instance = new Program(new FileWrapper(),
                                        new ArraySorter(),
                                        Console.Out);
            return instance.DoMain(args);
        }
    }
}
