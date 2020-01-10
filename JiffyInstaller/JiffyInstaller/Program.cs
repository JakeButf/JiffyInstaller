using IWshRuntimeLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace JiffyInstaller
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("//////////////////////////////////////////////////////////////////////////////////////////");
            Console.WriteLine("//////////////////////////////////////////////////////////////////////////////////////////");
            Console.WriteLine("///                                                                                    ///");
            Console.WriteLine("///                                                                                    ///");
            Console.WriteLine("///                                 --JIFFY INSTALLER--                                ///");
            Console.WriteLine("///                            PRESS ANY BUTTON TO CONTINUE                            ///");
            Console.WriteLine("///                                                                                    ///");
            Console.WriteLine("///BY JAKE BUTFILOSKI                                                                  ///");
            Console.WriteLine("//////////////////////////////////////////////////////////////////////////////////////////");
            Console.WriteLine("//////////////////////////////////////////////////////////////////////////////////////////");
            Console.ReadKey();
            //Start installation
            Console.WriteLine("Use custom installation directory? (y/n)");
            string a = Console.ReadLine();
            if (a == "y")
            {
                string temp = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
                string source = temp.Substring(6);
                Console.WriteLine(@"Type directory (ex C:\Program Files (x86)\Untitled Folder\:");
                string target = Console.ReadLine();
                try
                {
                    DirectoryCopy(source, target, true);
                } catch(Exception ex)
                {
                    Console.WriteLine(ex);
                    Main(args);
                }

            }
            else
            {
                string temp = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
                string source = temp.Substring(6);
                string target = @"C:\Program Files (x86)\Jiffy Software\";
                Console.WriteLine("Installing...");
                DirectoryCopy(source, target, true);
                Console.WriteLine("Done! It is recommended you make a shortcut of the exe file.");
                Console.ReadKey();
                System.Diagnostics.Process.Start("explorer.exe", target);

            }
        }
        public static void CreateShortcut(string shortcutName, string shortcutPath, string targetFileLocation)
        {
            string shortcutLocation = System.IO.Path.Combine(shortcutPath, shortcutName + ".lnk");
            WshShell shell = new WshShell();
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutLocation);

            shortcut.Description = "My shortcut description";   // The description of the shortcut
            shortcut.IconLocation = @"c:\myicon.ico";           // The icon of the shortcut
            shortcut.TargetPath = targetFileLocation;                 // The path of the file that will launch when the shortcut is run
            shortcut.Save();                                    // Save the shortcut
        }
        private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, false);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }
    }
}
