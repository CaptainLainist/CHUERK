using System;
using System.IO;
using System.Linq;

namespace chuerk
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("CHUERK  ---  A simple file shredder for Windows");

            if (args.Length > 0 && args.Length < 3)
            {

                //recursive delete
                if (args[0].Equals("-r"))
                {
                    RecursiveDelete(new DirectoryInfo(args[1]));
                }
                else if (args[0].Equals("-h"))
                {

                    Console.Write("Chuerk help manual\n chuerk <args> <folder or file> \n -r <folder>: Recursively shred folders and files \n -h show help file\n");

                }
                //non recursive delete
                else
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
            } else
            {
                Console.Write("Chuerk help manual\n chuerk <args> <folder or file> \n -r <folder>: Recursively shred folders and files \n -h show help file\n");
            }
        }

        //shredding the file
        private static void Shredder(string file) {
            if (File.Exists(file))
            {
                FileInfo fi = new FileInfo(file);
                if (fi.Length <= 536870912) //512 MB
                {

                    File.WriteAllText(file, String.Concat(Enumerable.Repeat("c", (int)fi.Length)));
                    File.Delete(file);
                    Console.WriteLine("{0} --- Done", file);
                }
                else {
                    Console.WriteLine("{0} --- File's too big, it has to be lower or equal to 512MB", file);
                }

            }
            else {
                Console.WriteLine("{0} --- File doesn't exist", file);
            }
        }

        public static bool IsDirectoryEmpty(string path)
        {
            return !Directory.EnumerateFileSystemEntries(path).Any();
        }

        //recursive delete
        private static void RecursiveDelete(DirectoryInfo baseDir)
         {
            if (!baseDir.Exists)
            {
                Console.WriteLine("Folder does not exist");
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
