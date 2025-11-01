using System;
using System.Collections.Generic;
using System.IO;
using LegacyThps.Shared;

namespace LegacyThps.Thps2.Triggers
{
    public class GetName
    {
        public static string NodeType(short x)
        {
            string res = "";

            switch (x)
            {
                case 1: res = "Baddy"; break;
                case 2: res = "Crate"; break;
                case 3: res = "Point"; break;
                case 4: res = "Autoexec"; break;
                case 5: res = "PowerUp"; break;
                case 6: res = "Command"; break;
                case 7: res = "SeedableBaddy"; break;
                case 8: res = "Restart"; break;
                case 9: res = "Barrel"; break;
                case 10: res = "RailDef"; break;
                case 11: res = "RailPoint"; break;
                case 12: res = "TrickOb"; break;
                case 13: res = "CamPt"; break;
                case 14: res = "GoalOb"; break;
                case 15: res = "AutoExec2"; break;
                case 16: res = "Mysterious 16"; break; // MHPB node type
                case 17: res = "TrickObCluster"; break; // MHPB node type
                case 255: res = "Terminator"; break;
                case 500: res = "Light"; break;
                case 501: res = "OffLight"; break;
                case 1000: res = "ScriptPoint"; break;
                case 1001: res = "CameraPath"; break;
                default: res = "Unknown node type [" + x.ToString() + "] "; break;
            }
            return res;
        }

        //returns baddy type name
        public static string BaddyType(short x)
        {
            string res = "";

            switch (x)
            {
                case 402: res = "Platform [402] "; break;
                case 203: res = "ScriptOnlyBaddy [203] "; break;
                case 213: res = "Car_Taxi [213] "; break;
                case 214: res = "Car_Police [214] "; break;
                case 215: res = "Car_Bus [215] "; break;
                case 216: res = "Car_Cable [216] "; break;
                case 217: res = "Car_Kart [217] "; break;
                case 218: res = "Car_Mar [218] "; break;
                case 219: res = "Car_Bul [219] "; break;
                default: res = "Unknown type [" + x.ToString() + "] "; break;
            }
            return res;
        }

        // returns power up name
        public static string PowerUp(short x)
        {
            string r = "";

            switch (x)
            {
                // combo letters (thps4)
                case 1: r = "C"; break;
                case 2: r = "O"; break;
                case 3: r = "M"; break;
                case 7: r = "B"; break;

                //skate letters
                case 4: r = "K"; break;
                case 5: r = "S"; break;
                case 6: r = "A"; break;
                case 10: r = "E"; break;
                case 15: r = "T"; break;

                case 16: r = "Tape"; break;
                case 17: r = "ProSpecificGoal"; break; // thps4
                case 18: r = "GotATape"; break; //probably not in levels

                //points in thps1, repurposed as Goal Tokens in THPS4
                case 21: r = "100pt"; break;
                case 22: r = "200pt"; break;
                case 23: r = "500pt"; break;

                //bucks in thps2
                case 25: r = "$50"; break; // repurposed as CD in THPS4
                case 26: r = "$100"; break; // repurposed as StatPoint in THPS3/4
                case 24: r = "$250"; break; // repurposed as deck in THPS3/4

                // you will doubtfully face these
                case 27: r = "WheelsPickup"; break;
                case 28: r = "HeadPickup"; break;
                case 29: r = "ShoesPickup"; break;
                case 30: r = "TorsoPickup"; break;
                case 31: r = "PantsPickup"; break;
                case 32: r = "BoardPickup"; break;

                // this is specific to level, pilot wings in hangar, spray cans in venice, etc
                case 33: r = "LevSpec"; break;

                // you won't open trg from apocalypse, will you?
                default: r = "Unknown! (" + x.ToString() + ")"; break;
            }

            return r;
        }

        // returns string for the drop state
        // used by powerup, defines whether powerup will drop or not
        public static string DropTime(short x)
        {
            string r = "";

            switch (x)
            {
                case 0: r = "Drop"; break;
                case 1: r = "DontDrop"; break;
                default: r = $"unknown drop type {x}!"; break;
            }
            return r;
        }

        // returns creation state, used by baddy and powerup. baddy has 3 of them.
        public static string CreationTime(short x)
        {
            string r = "";

            switch (x)
            {
                case 0: r = "NotCreatedAtStart"; break;
                case 1: r = "CreatedAtStart"; break;
                case 2: r = "ActiveWhenCreated"; break;
                case 3: r = "SuspendedWhenCreated"; break;
                case 4: r = "ManualSuspendControl"; break;
                case 5: r = "RadialSuspendControl"; break;
                default: r = $"unknown creation time type {x}!"; break;
            }
            return r;
        }

        //returns bounce type
        public static string Bouncy(short x)
        {
            string r = "";

            switch (x)
            {
                case 1: r = "Barrier"; break;
                case 2: r = "Cone"; break;
                case 3: r = "Trashcan"; break;
                case 4: r = "Plastic"; break;
                case 5: r = "WoodenBox"; break;
                default: r = $"unknown bouncy type {x}!"; break;
            }
            return r;
        }


        public static string BSFunc(short x)
        {
            string r = "";

            switch (x)
            {
                // technically this should stop the script, but there are some nodes with codes after C_DONE
                case 0x4100: r = "C_DONE"; break;

                case 0x4102: r = "C_GOTO_BREAK"; break;

                case 0x429C: r = "C_FLASH_SCREEN"; break;

                // label (n) = no labels in th3? i guess used with IFs mostly, so can translate into IF
                case 0x4104: r = "C_LABEL"; break;
                case 0x4105: r = "C_READ_LABELS"; break;
                case 0x4106: r = "C_SET_SCRIPT_NODE"; break;
                case 0x4107: r = "C_BREAK"; break;

                case 0x4110: r = "C_ADD"; break;

                case 0x4112: r = "C_IF_GT"; break;
                case 0x4113: r = "C_IF_LT"; break;
                case 0x4114: r = "C_IF_EQ"; break;

                case 0x4115: r = "C_IF_FLAG_SET"; break;
                case 0x4116: r = "C_IF_FLAG_CLEAR"; break;

                case 0x4120: r = "C_ENDIF"; break;

                case 0x4203: r = "C_DISPLAY_ON"; break;
                case 0x4204: r = "C_DISPLAY_OFF"; break;
                case 0x4205: r = "C_DIE_QUIETLY"; break;

                case 0x4221: r = "C_MOVE_TO_NODE"; break;

                //wait (n) = wait n frames?
                case 0x4280: r = "C_WAIT"; break;
                case 0x4281: r = "C_WAIT_FOR_SIGNAL"; break;                     
                //play (n) = PlaySound
                case 0x4290: r = "C_PLAY_SFX"; break;
                //play_positional_sfx (n) = PlaySound, positioning is defined at loadsound
                case 0x4291: r = "C_PLAY_POSITIONAL_SFX"; break;

                case 0x42B0: r = "C_TEXT_MESSAGE"; break;

                case 0x4507: r = "C_PLAY_LOOPING_SFX"; break;   
                case 0x4509: r = "C_STOP_LOOPING_SFX"; break;   

                //creates spark at the node position. have no idea how we'll simulate this
                case 0x4292: r = "C_SPARK"; break;
                //C_SHAKE_CAMERA( n ) shakes the cmera. (n = 0:SMALL, 1:MEDIUM, 2:BIG).
                case 0x4298: r = "C_SHAKE_CAMERA"; break;

                case 0x4299: r = "C_SET_FMV_TRACK"; break;
                case 0x4293: r = "C_SET_FMV_CHECKSUM"; break;
                case 0x4294: r = "C_START_FMV"; break;

                    

                case 0x42C0: r = "C_GOALCOUNTER"; break;

                //sets water level? we have no water lel
                case 0x429E: r = "C_SET_WATER_LEVEL"; break;

                case 0x42B1: r = "C_SEND_SIGNAL_TO_LINKS"; break;
                case 0x42B2: r = "C_SEND_PULSE_TO_LINKS"; break;

                case 0x42B4: r = "C_SEND_PULSE_TO_NODE"; break;

                case 0x4301: r = "C_SHATTER"; break;

                case 0x4304: r = "C_SCALE"; break;

                case 0x4306: r = "C_SCALE_X"; break;
                case 0x4307: r = "C_SCALE_Y"; break;
                case 0x4308: r = "C_SCALE_Z"; break;

                //makes object bouncy = bouncy node in th3
                case 0x4309: r = "C_IS_BOUNCY"; break;

                case 0x4508: r = "C_PLAY_LOOPING_POSITIONAL_SFX"; break;





                case 0x2120: r = "V_REGISTER"; break;
                case 0x2125: r = "V_ATTRIBUTE"; break;

                //Model(n)
                case 0x212F: r = "V_MODEL_CHECKSUM"; break;

                case 0x2127: r = "V_ANGULAR_VELOCITY"; break;
                case 0x2128: r = "V_ANGULAR_ACCELERATION"; break;

                case 0x2129: r = "V_RANDOM"; break;
                case 0x212A: r = "V_MY_NODE"; break;
                case 0x212B: r = "V_LINKED_NODE"; break;

                case 0x212C: r = "V_INPUT_SIGNAL"; break;

                case 0x2132: r = "V_BRUCE_XZ_DIST"; break;
                case 0x2134: r = "V_VELOCITY"; break;


                case 0x2138: r = "V_SENT_DEATH_PULSE"; break;


                default: r = "0x" + x.ToString("X"); break;
            }

            return r+" ";
        }


        //kinda weird aint it
        public static bool isKnownCommand(short x)
        {
            return !(Command(x) == "Unknown command: 0x" + x.ToString("X") + " ");
        }


        public static string Command(short x)
        {
            string r = "";

            switch (x)
            {
                //this ends the command list, better check if we actually hit the end of node with it
                case -1: r = ""; break;

                //we normally shouldnt meet 0x0 anywhere in command list as a command.
                //however, my wild zero check for command assumes that if we got 
                //word1 and word2 = 0 then checksum is 0 and no wildzero.
                //but in fact there can be zero, and it will pass to the command list
                //so here it is the check for zero. even if wildzero check improved,
                //this should stay here to notify if anything else gone wrong
                case 0: r = "0x0"; break;

                //my vision of pulse system is like when you trigger something,
                //it sends pulses to linked nodes and activates the trigger
                //in case it's command, command is performed, if it's baddy, baddy script is performed
                //we probably should use qb LOOP to wrap multiple pulses, might be tricky to generate 
                //or i am just plain wrong
                //we might want to investigate the command links deeper 

                case 2: r = "SetCheatRestarts"; break;      //defines available cheat restarts from this restart. we dont need that.
                case 3: r = "SendPulse"; break;             //activate all linked triggers (either commands or baddy)
                case 4: r = "SendActivate"; break;          //??? allow trigger?  idk
                case 5: r = "SendSuspend"; break;           //??? disallow trigger? idk
                case 10: r = "SendSignal"; break;           //not sure how this and pulse are different, but signal is used by baddyscript like C_WAIT_FOR_SIGNAL
                case 11: r = "SendKill"; break;             //kill object i guess
                case 12: r = "SendKillLoudly"; break;       //kill object loud? what might that mean?
                case 13: r = "SendVisible"; break;          //it takes 0/1 as param, pretty much like Visible/Invisible

                    // MHPB
                case 119: r = "SomeDebugString_119?"; break;

                case 104: r = "SetFoggingParams"; break;    //fogging params for th2, we dont need this      
                case 126: r = "SpoolIn"; break;             //i guess this adds psx file to memory pool
                case 127: r = "SpoolOut"; break;            //probably removes psx file, didnt find this one anywhere
                case 128: r = "SpoolEnv"; break;            //sets level file
                case 131: r = "BackgroundOn"; break;        //have no idea what this does, maybe turn sky on? seems so
                case 132: r = "BackgroundOff"; break;       //same off?
                case 134: r = "SendInitialPulses"; break;   //sets number of times to activate the trigger?
                case 140: r = "SetRestart"; break;          //restart for player 1
                case 142: r = "SetObjFile"; break;          //sets the object file (SkSmth_O.PSX)
                case 147: r = "SetGameLevel"; break;        //it's always setgamelevel (1). probably turns game mode on or smth like that.

                case 151: r = "SetDualBufferSize"; break;

                case 171: r = "BackgroundCreate"; break;    //Makes sky, contains sky checksum. i told ya it's sky checksum!
                case 178: r = "SetRestart2"; break;         //restart for player 2
                case 141: r = "SetVisibilityInBox"; break;  //i guess works as kill/create for everything in the box, shouldnt use as th3 can handle it
                case 149: r = "EndIf"; break;               //end if, lol
                case 152: r = "KillBruce"; break;           //kill skater, no params, sound might depend on terrain sound
                case 157: r = "SetReverbType"; break;       //reverb unfortunately doesn't work properly on th3
                case 158: r = "EndLevel"; break;            //this forces end level on mall and jam. should teleport to restart from autoexec instead.
                case 166: r = "SetOTPushback"; break;       //have no idea
                case 169: r = "SetOTPushback2"; break;      //have no idea for 2 players
                case 200: r = "SetFadeColor"; break;        //fog color? not sure. dont need this anyway
                case 201: r = "GapPolyHit"; break;          //StartGap/EndGap, we have it already, should check if editor gap reading is correct.
                case 202: r = "SetSkyColor"; break;         //skycolor, which is likely background color, as online in th3. we may use it, but probably better just ignore 

                // this is GapPolyStart
                // and GapPolyEnd in MHPB
                case 203: r = "SetCareerFlag"; break;       //some stuff used in career mode. we aint using it, ay?
                case 204: r = "IfCareerFlag"; break;        //checks if flag set by previous func is correc t and calls career stuff 


                // thps4 functions, maybe career related?
                // like spawn all goals maybe
                // 300 - no params
                // 301 - 1 short
                // 302 - no params

                default: r = "Unknown command: 0x" + x.ToString("X"); break;
            }

            return r+" ";
        }



        public static string Param(int x)
        {
            return "(" + x.ToString() + ")";
        }

        public static string Param(string x)
        {
            return "(" + x + ")";
        }

        public static string Param(int x, int y)
        {
            return "(" + x.ToString() + ", " + y.ToString() + ")";
        }

        public static bool isBaddyFunc(short x) => (x & 0xF000) == 0x4000; 
        public static bool isBaddyVar(short x) => (x & 0xF000) == 0x2000;
    } 
}
