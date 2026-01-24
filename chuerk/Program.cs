using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace chuerk
{



    class Program
    {






        private static List<string> GetSeparated(string cadena) { 
            
            List<string> result = new List<string>();
            string directory = "";
            string files = "";

            for (int i = cadena.Length - 1; i > 0; i--) { 
                if (cadena[i] == '\\'){
                    directory = cadena.Substring(0, i);
                    files = cadena.Substring(i + 1);
                }
            }

            result.Add(directory);
            result.Add(files);

            return result;

        }



        private static void Main(string[] args)
        {


            ConsoleColor prevcolor = Console.ForegroundColor;

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("   ________                    __");
            Console.WriteLine("  / ____/ /_  __  _____  _____/ /__");
            Console.WriteLine(" / /   / __ \\/ / / / _ \\/ ___/ //_/");
            Console.WriteLine("/ /___/ / / / /_/ /  __/ /  / ,<");
            Console.WriteLine("\\____/_/ /_/\\__,_/\\___/_/  /_/|_|\n");

            Console.ForegroundColor = prevcolor;

            Console.WriteLine("CHUERK v2.3.0  ---  A simple file shredder for Windows by CaptainLainist");


            if (args.Length > 0 && args.Length < 3)
            {

                //recursive delete
                if (args[0].Equals("-r"))
                {

                    try
                    {

                        RecursiveDelete(new DirectoryInfo(args[1]));

                        
                    }
                    catch (System.IO.IOException)
                    {

                        Console.WriteLine("{0} --- directory is being used by another process", args[1]);
                    }
                    
                }
                else if (args[0].Equals("-h"))
                {

                    Console.Write("Chuerk help manual\n chuerk (-h/-r) <folder or file> \n -r <folder>: Recursively shred folders and files \n -h show help file\n");

                }
                //non recursive delete
                else
                {

                    try
                    {
                        string path = Directory.GetCurrentDirectory();
                        string[] files = Directory.GetFiles(path, args[0]);

                        if (files.Length == 0)
                        {
                            Console.WriteLine("No files found");
                        }
                        foreach (string f in files)
                        {
                            Shredder(f);

                        }
                    }
                    catch (System.IO.IOException)
                    {

                        Console.WriteLine("{0} --- ERROR: file is being used by another process", args[0]);
                    }
                    //if it's an absolute route
                    catch (System.ArgumentException) {

                        //if c:/fdfs/*

                        try
                        {

                            List<string> separated = GetSeparated(args[0]);
                            string file = separated[1];

                            if (file.Contains('*'))
                            {


                                string directory = separated[0];
                                string[] files = Directory.GetFiles(directory, file);
                                foreach (string f in files)
                                {

                                    Shredder(f);

                                }
                            }
                            else
                            {
                                //if C:/gfgfs/aaa.txt
                                Shredder(args[0]);
                            }

                        }
                        catch (System.IO.IOException) {
                            Console.WriteLine("{0} --- ERROR: directory sintax is not correct", args[0]);
                        }

                        
                        
                    }
                   
                }
            } else
            {
                Console.Write("Chuerk help manual\n chuerk (-h/-r) <folder or file> \n -r <folder>: Recursively shred folders and files \n -h show help file\n");
            }
        }



        //shredding the file
        private static void Shredder(string file) {
            if (File.Exists(file))
            {
                FileInfo fi = new FileInfo(file);

                int KB = 1024;
                int MB = KB * KB;
                int MB8 = 8 * MB;


                byte[] bytes = new byte[MB8];


                for (int i = -1; i <= 1; i++)
                {

                    using FileStream fs = new FileStream(
                        file,
                        FileMode.Open,
                        FileAccess.Write,
                        FileShare.None,
                        MB,
                        FileOptions.SequentialScan
                    );


                    fs.Seek(0, SeekOrigin.Begin);
                    long remaining = fi.Length;

                    while (remaining > 0)
                    {

                        int toWrite = (int)Math.Min(MB8, remaining);
                        if (i == 1)
                        {
                            RandomNumberGenerator.Fill(bytes.AsSpan(0, toWrite));
                        }
                        else {
                            byte v = i == -1 ? (byte)0xFF : (byte)0x00;
                            bytes.AsSpan(0, toWrite).Fill(v);
                        }
                            
                        fs.Write(bytes, 0, toWrite);
                        remaining -= toWrite;

                    }

                }

                File.Delete(file);
                Console.WriteLine("{0} --- Done", file);


            }
            else {
                Console.WriteLine("{0} --- ERROR: File does not exist", file);
            }
        }

        private  static bool IsDirectoryEmpty(string path)
        {
            return !Directory.EnumerateFileSystemEntries(path).Any();
        }

        //recursive delete
        private static void RecursiveDelete(DirectoryInfo baseDir)
         {

            if (!baseDir.Exists)
            {
                Console.WriteLine("{0} --- ERROR: Folder does not exist", baseDir.FullName);
                return;
            }

            

            foreach (FileInfo file in baseDir.GetFiles()) {

                    
               Shredder(file.FullName);
                
                
            }



            foreach (var dir in baseDir.EnumerateDirectories())
            {
                
                RecursiveDelete(dir);
            }
            if (IsDirectoryEmpty(baseDir.FullName))
            {
                baseDir.Delete(true);
            }
         }
    }
}
