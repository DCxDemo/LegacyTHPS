using LegacyThps.Containers;
using System;
using System.IO;
using System.Windows.Forms;

namespace kat2wav
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
            }
            else
            {
                foreach (var arg in args)
                {
                    try
                    {
                        Kat kat = Kat.FromFile(arg);
                        kat.Extract(Path.Combine(
                            Path.GetDirectoryName(arg),
                            Path.GetFileNameWithoutExtension(arg)
                            ));
                    }
                    catch
                    {
                        Console.WriteLine("Error: {arg}");
                    }
                }
            }
        }
    }
}