using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegacyThps.Thps2.Triggers
{


    public class GenericNode : INode
    {
        // tha file we're in
        public TriggerFile Context;

        // tha name
        public string Name { get; set; }

        // tha node links
        public List<GenericNode> Links = new List<GenericNode>();

        public GenericNode(TriggerFile context)
        {
            Context = context;
        }

        /// <summary>
        /// Converts node text to class data.
        /// </summary>
        /// <param name="text"></param>
        /// <exception cref="NotImplementedException"></exception>
        public virtual void Compile(string text)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Converts class data to text.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public virtual string Decompile()
        {
            var sb = new StringBuilder();

            sb.AppendLine($"@{Name}");
            sb.AppendLine($"; not implemented");

            return sb.ToString();
        }

        /// <summary>
        /// Parses class data from compiled binary.
        /// </summary>
        /// <param name="br"></param>
        /// <exception cref="NotImplementedException"></exception>
        public virtual void Read(BinaryReader br)
        {
            Console.WriteLine("generic node read, implement me!");
        }

        /// <summary>
        /// Generates binary from class data.
        /// </summary>
        /// <param name="bw"></param>
        /// <exception cref="NotImplementedException"></exception>
        public virtual void Write(BinaryWriter bw)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Reads a list of node indices.
        /// </summary>
        /// <param name="br"></param>
        /// <returns></returns>
        public List<int> ReadLinks(BinaryReader br)
        {
            int numLinks = br.ReadInt16();
            var links = new List<int>();

            for (int i = 0; i < numLinks; i++)
                links.Add(br.ReadInt16());

            return links;
        }
    }
}