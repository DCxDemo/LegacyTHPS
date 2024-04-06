using LegacyThps.QScript;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ThpsQScriptEd
{
    static class Program
    {
        [DllImport("kernel32")]
        static extern bool AllocConsole();

        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                AllocConsole();
                Console.WriteLine("ThpsQScriptEd, dcxdemo");

                if (!File.Exists(args[0]))
                {
                    Console.WriteLine("Bad file.");
                    return;
                }

                SymbolCache.Create();
                QBuilder.Init();

                switch (Path.GetExtension(args[0].ToUpper()))
                {
                    case ".Q":
                        QBuilder.Compile(File.ReadAllText(args[0]));
                        QBuilder.SaveChunks(Path.ChangeExtension(args[0], ".qb"));
                        break;

                    case ".QB":
                        QBuilder.LoadCompiledScript(args[0]);
                        File.WriteAllText(Path.ChangeExtension(args[0], ".q"), QBuilder.GetSource(false));
                        break;

                    default:
                        break;
                }
            }
            else
            {
                //else launch GUI
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
            }
        }
    }
}