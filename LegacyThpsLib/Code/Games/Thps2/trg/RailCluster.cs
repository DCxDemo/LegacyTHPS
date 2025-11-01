using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LegacyThps.Shared;

namespace LegacyThps.Thps2.Triggers
{
    [Serializable]
    public class RailCluster
    {
        public string Name;
        public List<TNode> Entries = new List<TNode>();

        public static RailCluster TraceRailNode(TNode node, TriggerFile trg)
        {
            if (trg == null)
                throw new Exception("TraceRailNode(): null trig file.");

            if (node == null)
                throw new Exception("TraceRailNode(): null node");

            if (!node.IsRail)
                throw new Exception("TraceRailNode(): not a valid rail node");

            //if (node.LinkedNodes.Count != 0)
            //    throw new Exception($"TraceRailNode(): I expect no links here. but got {node.LinkedNodes.Count} links.");

            // it's a rail

            var cluster = new RailCluster();
            var next = node;

            do
            {
                if (next.InRailCluster)
                {
                    MessageBox.Show("cmon");
                    break;
                }    

                cluster.Entries.Add(next);
                next.InRailCluster = true;
                next = next.GetParentRail(trg.Nodes);
            }
            while (next != null);


            cluster.Entries.Reverse();

            cluster.AutoName();

            return cluster;
        }

        public void AutoName()
        {
            Name = $"From node {Entries[0].Number} chain {Entries.Count}";
        }

        public override string ToString() => String.IsNullOrEmpty(Name) ? "empty" : Name;

        public string ToStringLong()
        {
            var sb = new StringBuilder();

            foreach (var node in Entries)
                sb.AppendLine(node.ToStringLong());

            return sb.ToString();
        }
    }
}