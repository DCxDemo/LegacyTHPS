using LegacyThps.QScript.Helpers;
using System;
using System.Collections.Generic;

namespace LegacyThps.QScript.Nodes
{
    public class Node
    {
        public Vector3f Position = Vector3f.Zero;
        public Vector3f Angles = Vector3f.Zero;
        public string Name = "";
        public bool CreatedAtStart = false;
        public TerrainType terrainType = TerrainType.None;

        public NodeClass nodeClass = NodeClass.Empty;
        public RailType railType = RailType.None;
        public RestartType restartType = RestartType.None;
        public string RestartName = "";

        public bool TrickObject = false;
        public string Cluster = "";

        public string SpawnObjScript = "";
        public string TriggerScript = "";
        public List<int> Links = new List<int>();

        public bool NetEnabled = false;
        public bool Permanent = false;
        public bool AbsentInNetGames = false;

        public int Zone_Multiplier = 0;

        public Node()
        {
            Position = new Vector3f(200, 500, 4000);
            Angles = new Vector3f(0, 3.14f, 0);
            Name = "test_node";
        }

        public override string ToString()
        {
            string res = "";

            res += "{ ";
            if (Position != Vector3f.Zero) res += String.Format("Position = {0} ", Position.ToString());
            if (Angles != Vector3f.Zero) res += String.Format("Angles = {0} ", Angles.ToString());
            if (Name != "") res += "Name = " + Name + " ";

            if (terrainType != TerrainType.None) res += "TerrainType = " + terrainType.ToString() + " ";

            if (nodeClass != NodeClass.Empty)
            {
                res += "NodeClass = " + nodeClass.ToString() + " ";

                switch (nodeClass)
                {
                    case NodeClass.RailNode: res += "RailType = " + railType.ToString() + " "; break;
                }
            }


            if (TrickObject) res += "TrickObject ";
            if (Cluster != "") res += "Cluster = " + Cluster + " ";

            if (TriggerScript != "") res += "TriggerScript = " + TriggerScript + " ";
            if (SpawnObjScript != "") res += "SpawnObjScript = " + SpawnObjScript + " ";

            if (Links.Count > 0) res += "\tLinks = [ ]";

            if (CreatedAtStart) res += "CreatedAtStart ";
            if (NetEnabled) res += "NetEnabled ";
            if (Permanent) res += "Permanent ";
            if (AbsentInNetGames) res += "AbsentInNetGames ";

            if (Links.Count > 0)
            {
                res += "Links = [ ";
                foreach (int i in Links) res += i + " ";
                res += "] ";
            }

            res += "}\r\n";

            return res;
        }


        public string ToCSV()
        {
            string res = "";

            res += String.Format("{0},{1},{2},", Position.X, Position.Y, Position.Z);
            res += String.Format("{0},{1},{2},", Angles.X, Angles.Y, Angles.Z);
            res += String.Format("{0},", Name);
            res += String.Format("{0},", terrainType.ToString());
            res += String.Format("{0},", nodeClass.ToString());
            res += String.Format("{0},", railType.ToString());
            res += String.Format("{0},", TrickObject);
            res += String.Format("{0},", Cluster);
            res += String.Format("{0},", TriggerScript);
            res += String.Format("{0},", SpawnObjScript);
            res += String.Format("{0},", CreatedAtStart);
            res += String.Format("{0},", NetEnabled);
            res += String.Format("{0},", Permanent);
            res += String.Format("{0},", AbsentInNetGames);

            return res + "\r\n";
        }

    }
}