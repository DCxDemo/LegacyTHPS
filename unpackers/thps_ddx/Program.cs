using System;
using System.Collections.Generic;
using LegacyThps.Thps2;
using LegacyThps.Helpers;

namespace thps_ddx
{
    internal class Program
    {
        public static void Intro()
        {
            var lines = new string[] {
                "THPS2x DDX tool",
                "Supports DDX files found in THPS2x (Xbox).\r\n",
                "2024, dcxdemo.\r\n",
            };

            DebugLog.Print(lines);
        }

        static void Main(string[] args)
        {
            Intro();

            var paramDesc = new List<CmdParamDesc>()
            {
                new CmdParamDesc() { ShortName = "b", FullName = "build", Description = "Converts DDS files in a folder to a DDX file." },
                new CmdParamDesc() { ShortName = "x", FullName = "extract", Description = "Extracts DDS textures from DDX file." },
                new CmdParamDesc() { ShortName = "i", FullName = "input", Description = "Input paths (DDX file or folder of DDS files)", ExpectsValues = true },
                new CmdParamDesc() { ShortName = "o", FullName = "output", Description = "Output paths (DDX filename or output folder name).", ExpectsValues = true },
                new CmdParamDesc() { ShortName = "v", FullName = "verbose", Description = "Enables detailed output." },
                new CmdParamDesc() { ShortName = "h", FullName = "help", Description = "Available commands listing" },
            };

            var cmd = CmdParser.Parse(args, paramDesc);

            // if user requested help
            if (cmd.Get("help").Present)
            {
                cmd.Documentation();
                return;
            }

            switch (cmd.Status)
            {
                case CmdParserStatus.Mixed:
                case CmdParserStatus.Valid:
                    // normal parsing
                    {
                        if (cmd.Status == CmdParserStatus.Mixed)
                            Console.WriteLine("Warning: your command line contains invalid entries, only valid entries will be processed.");

                        var input = cmd.Get("input").Values;
                        var output = cmd.Get("output").Present ? cmd.Get("output").Values[0] : null;

                        DebugLog.Verbose = cmd.Get("verbose").Present;

                        if (cmd.Get("build").Present)
                        {
                            XboxDdx.Build(input[0], output);
                        }
                        else if(cmd.Get("extract").Present)
                        {
                            foreach (var path in input)
                                XboxDdx.Extract(path, output);
                        }
                        else
                        {
                            cmd.Documentation();
                            return;
                        }

                        Console.WriteLine("Done.");
                        return;
                    }
                case CmdParserStatus.Unknown:
                    {
                        var input = cmd.Get(null).Values;

                        foreach (var path in input)
                            XboxDdx.Extract(path);

                        Console.WriteLine("Done.");

                        return;
                    }

                default:
                case CmdParserStatus.Empty:
                    {
                        cmd.Documentation();
                        return;
                    }
                case CmdParserStatus.MissingValue:
                    {
                        Console.WriteLine("Please check your command line parameters, cannot process.");
                        return;
                    }
            }
        }
    }
}