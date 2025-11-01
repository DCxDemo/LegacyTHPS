using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace LegacyThps.Thps2.Triggers
{
    public class GenericNode3D : GenericNode
    {
        public Vector3 Position = Vector3.Zero;
        public Vector3 Angles = Vector3.Zero;

        public GenericNode3D(TriggerFile context) : base(context)
        {
            Context = context;
        }
    }

    public class PowerUpNode : GenericNode3D
    {
        public PowerUpNode(TriggerFile context) : base(context)
        {
            Context = context;
        }
    }

    public class GenericObjectNode : GenericNode3D
    {
        uint Checksum;

        public GenericObjectNode(TriggerFile context) : base(context)
        {
            Context = context;
        }
    }


    public class TrickObNode : GenericObjectNode
    {
        public TrickObNode(TriggerFile context) : base(context)
        {
            Context = context;
        }
    }

    public class GoalObNode : GenericObjectNode
    {
        public GoalObNode(TriggerFile context) : base(context)
        {
            Context = context;
        }
    }

}
