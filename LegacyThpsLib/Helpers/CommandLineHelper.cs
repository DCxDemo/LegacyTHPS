using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LegacyThps.Helpers
{
    // some day i will check System.CommandLine, but not today

    // cmd line parsing outcomes
    public enum CmdParserStatus
    {
        // no input at all
        Empty,
        // no valid params found
        Unknown,
        // both params and invalid input 
        Mixed,
        // only params without invalid input
        Valid,
        // if no values are given, when expected
        MissingValue
    }

    public class CmdParamDesc
    {
        public string ShortName;
        public string FullName;
        public string Description;

        public bool Present = false;
        public bool ExpectsValues = false;

        public List<string> Values = new List<string>();

        public bool Matches(string arg)
        {
            var f = FullName.ToUpper();
            var s = ShortName.ToUpper();
            var a = arg.ToUpper();

            // basically look for any match: param PARAM p P /p -p --param
            return a == f || a == s || a == $"-{s}" || a == $"/{s}" || a == $"--{f}";
        }

        public override string ToString()
        {
            return $"-{ShortName}, --{FullName}:\t{Description}";
        }
    }

    public class CmdParser
    {
        public CmdParserStatus Status = CmdParserStatus.Empty;

        public List<CmdParamDesc> Parameters = new List<CmdParamDesc>();

        public CmdParser(string[] args, List<CmdParamDesc> desc) 
        {
            Read(args, desc);
        }

        public CmdParamDesc Get(string name)
        {
            foreach (var param in Parameters)
                if (param.Matches(name))
                    return param;

            return null;
        }

        public int NumPresentParams()
        {
            int i = 0;

            foreach (var param in Parameters)
                if (param.Present)
                    i++;

            return i;
        }

        public static CmdParser Parse(string[] args, List<CmdParamDesc> desc) => new CmdParser(args, desc);

        public void Read(string[] args, List<CmdParamDesc> desc)
        {
            Parameters = desc;

            var empty = new CmdParamDesc() { 
                ShortName = null, 
                FullName = null, 
                Description = "A storage for invalid parameters." 
            };

            Parameters.Add(empty);

            var curParam = empty;

            foreach (var arg in args)
            {
                bool isParam = false;

                foreach (var d in desc)
                {
                    // is it a command?
                    if (d.Matches(arg))
                    {
                        curParam = d;
                        curParam.Present = true;

                        isParam = true;

                        break;
                    }
                }

                if (!isParam)
                {
                    // it's a value!
                    curParam.Values.Add(arg);
                    curParam.Present = true;
                }
            }

            Status = Validate();
        }

        public CmdParserStatus Validate()
        {
            // case empty
            if (NumPresentParams() == 0)
            {
                //Console.WriteLine($"Tool launched without any parameter, refer to documentation for more info.");
                return CmdParserStatus.Empty;
            }

            // case only unknown
            if (NumPresentParams() == 1 && Get(null).Present)
            {
                //Console.WriteLine($"No known parameters found.");
                return CmdParserStatus.Unknown;
            }

            // case any unknowns
            foreach (var arg in Parameters)
            {
                if (!arg.Present)
                    continue;

                if (arg.ShortName == null)
                {
                    //Console.WriteLine($"Warning, mixed command line parameters.");
                    return CmdParserStatus.Mixed;
                }

                if (arg.ExpectsValues)
                    if (arg.Values.Count == 0)
                    {
                        //Console.WriteLine($"Missing value for param: {arg.FullName}");
                        return CmdParserStatus.MissingValue;
                    }

            }

            // we only have known params
            return CmdParserStatus.Valid;
        }

        public void Documentation()
        {
            Console.WriteLine("Usage:");

            foreach (var cmd in Parameters)
                if (cmd.ShortName != null)
                    Console.WriteLine($"\t{cmd}");
        }
    }
}