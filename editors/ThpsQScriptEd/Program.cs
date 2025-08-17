using System;
using System.Windows.Forms;

namespace ThpsQScriptEd
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm(args.Length > 0 ? args[0] : ""));
        }
    }
}

// cmd 

/* 
        [DllImport("kernel32")]
    static extern bool AllocConsole();


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

 */