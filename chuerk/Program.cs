using System;
using System.IO;

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
                        File.Delete(f);
                        Console.WriteLine("{0} --- Done", f);
                    }
                }
            } else
            {
                Console.Write("Chuerk help manual\n chuerk <args> <folder or file> \n -r <folder>: Recursively shred folders and files \n -h show help file\n");
            }
        }

        //shredding the file
        private static void Shredder(string file) {
            if (File.Exists(file)) {
                FileInfo fi = new FileInfo(file);
                string chars = "qwertyuiopasdfghjklzxcvbnm1234567890QWERTYUIOPASDFGHJKLZXCVBNM";
                Random rand = new Random();
   
                string characters = "";
                for (int i = 0; i < fi.Length; i++) {
                        
                    characters += chars[rand.Next(chars.Length)].ToString();
                        
                }
                File.WriteAllText(file, characters);

            }
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
                Console.WriteLine("{0} --- Done", file.Name);
            }

            foreach (var dir in baseDir.EnumerateDirectories())
            {
                
                RecursiveDelete(dir);
            }
            baseDir.Delete(true);
         }
    }
}
