﻿using System;
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
        bool Exists();
        String[] ReadAllLines();
    }
    public class FileWrapper : IFileWrapper
    {
        string thePath;
        public FileWrapper(string path)
        {
            thePath = path;
        }
        public bool Exists()
        {
            return File.Exists(thePath);
        }

        public string[] ReadAllLines()
        {
            return File.ReadAllLines(thePath);
        }
    }

    public class StreamWrapper : IFileWrapper
    {
        StreamReader sr;
        bool exists;
        public StreamWrapper(string path)
        {
            exists = true;
            Stream theStream;
            try
            {
                theStream = new FileStream(path, FileMode.Open);
            }
            catch (FileNotFoundException)
            {
                exists = false;
                return;
            }
            sr = new StreamReader(theStream);
        }
        public bool Exists()
        {
            return exists;
        }

        public string[] ReadAllLines()
        {
            List<string> lines = new List<string>();
            while (!sr.EndOfStream)
            {
                lines.Add(sr.ReadLine());
            }
            return lines.ToArray();
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

        public int DoMain()
        {
            if (!filewrapper.Exists())
            {
                writer.WriteLine("File does not exist");
                return 2;
            }

            string[] lines = filewrapper.ReadAllLines();
            sorter.Sort(lines);
            
            for (int i = 0; i < lines.Length; i++)
            {
                writer.WriteLine(lines[i]);
            }
            return 0;
        }

        public static TextWriter outwriter = Console.Out;
        public static int Main(string[] args)
        {
            if (args.Length == 0)
            {
                outwriter.WriteLine(" Please provide filename to sort");
                return 1;
            }
            Program instance = new Program(new StreamWrapper(args[0]),
                                        new ArraySorter(),
                                        outwriter);
            return instance.DoMain();
        }
    }
}
