using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using LegacyThps.Shared;

namespace LegacyThps.Thps2.Triggers
{
    [Serializable]
    public class TriggerFile
    {
        public static ParsingMode ParsingMode = ParsingMode.THPS2;

        // generic trg params
        public const int Magic = 0x4752545f; // magic word "_TRG"
        public short Version;         // trg version
        public short Project;         // project version

        public string Name;

        public List<TNode> Nodes = new List<TNode>();
        public List<RailCluster> RailClusters = new List<RailCluster>();

        public TriggerFile() { }

        // trg constructor
        public TriggerFile(BinaryReader br) => Read(br);

        public static TriggerFile FromFile(string filename)
        {
            using (var br = new BinaryReader(File.Open(filename, FileMode.Open)))
            {
                return new TriggerFile(br) {
                    Name = Path.GetFileNameWithoutExtension(filename)
                };
            }
        }

        public void Read(BinaryReader br)
        {
            int magic = br.ReadInt32();

            if (magic != Magic)
                throw new Exception("Not a valid THPS trigger file!");

            Version = br.ReadInt16(); // always 2
            Project = br.ReadInt16(); // thps - 0, mhpb - 1
            int numNodes = br.ReadInt32();

            for (int i = 0; i < numNodes; i++)
                Nodes.Add(new TNode() { 
                    Offset = br.ReadInt32() 
                });

            for (int i = 0; i < numNodes - 1; i++)
            {
                Nodes[i].Size = Nodes[i + 1].Offset - Nodes[i].Offset;

                if (Nodes[i].Size < 0)
                    throw new Exception("wtf - negative node size, offsets are not in order!");

                Nodes[i].Number = i;
                Nodes[i].ReadNode(br);
            }

            Nodes[numNodes-1].Size = 2;
            // must be terminator!
            Nodes[numNodes-1].ReadNode(br);

            var sb = new StringBuilder();

            // at this point we have parsed all nodes, lets populate actual list of links, so we dont recalculate that every time
            foreach (var node in Nodes)
            {
                foreach (int x in node.Links)
                {
                    if (x >= Nodes.Count)
                    {
                        // there is a bugged node in MHPB
                        MessageBox.Show("bugged node: " + node.Name + "\r\n" + node.Offset.ToString("X8"));
                        break;
                    }

                    if (Nodes[x] == node)
                    {
                        sb.AppendLine(node.Name);
                        break;
                    }

                    node.LinkedNodes.Add(Nodes[x]);

                    if (node.IsRestart)
                        if (Nodes[x].IsCommand)
                            Nodes[x].Name += "_SpawnCommand";

                    if (node.IsCommand)
                        if (Nodes[x].IsCrate)
                            node.Name += "_CrateKiller";
                }
            }

            if (sb.Length > 0)
                MessageBox.Show("self links in this file:\r\n" + sb.ToString());
            

            // create rail clusters
            // basically find nodes without links and trace back to root rail.
            //foreach (var node in Nodes)
            //    if (node.IsRail)
            //        if (!node.GotChildrenRails())
            //           RailClusters.Add(RailCluster.TraceRailNode(node, this));

            // then we need a second pass
            // and a second pass for circular rails. like in shipyard
            //foreach (var node in Nodes)
            //    if (node.IsRail && !node.InRailCluster)
            //        RailClusters.Add(RailCluster.TraceRailNode(node, this));
        }


        public List<TNode> GetAllOfType(NodeType type) => Nodes.Where(x => x.Type == type).ToList();

        public List<TNode> GetAllParents(TNode node)
        {
            var parents = new List<TNode>();

            foreach (var parent in Nodes)
                if (parent.LinkedNodes.Contains(node))
                    parents.Add(parent);

            return parents;
        }

        public override string ToString()
        {
            string res = "";

            res = Name + "\r\n"
                + Magic.ToString() + " : just \"_TRG\"\r\n"
                + Version.ToString() + " : version is always 2\r\n" 
                + Nodes.Count.ToString() + " : node count\r\n";

            foreach (var node in Nodes)
                res += node.ToStringLong();

            return res;
        }

        public string Decompile()
        {
            var sb = new StringBuilder();

            foreach (var node in Nodes)
                sb.AppendLine(node.Decompile());

            return sb.ToString();
        }
    }
}