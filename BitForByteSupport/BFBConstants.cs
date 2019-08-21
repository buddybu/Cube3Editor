using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitForByteSupport
{
    static public class BFBConstants
    {
        public const string CUBE3_MODEL = "CUBE3";
        public const string EKO_MODEL = "EKOCYCLE";
        public const string CUBEPRO_MODEL = "CUBEPRO";

        public const string CUBE3_VERSION = "V1.14B";
        public const string EKO_VERSION = "V1.05";
        public const string CUBEPRO_VERSION = "V1.87";

        public const string FIRMWARE = "^Firmware:";
        public const string MINFIRMWARE = "^Minfirmware:";
        public const string PRINTERMODEL = "^PrinterModel:";
        public const string MATERIALCODEE1 = "^MaterialCodeE1:";
        public const string MATERIALCODEE2 = "^MaterialCodeE2:";
        public const string MATERIALCODEE3 = "^MaterialCodeE3";
        public const string RAFT = "^Raft:";
        public const string SIDEWALKS = "^Sidewalks:";
        public const string SUPPORTS = "^Supports:";

        public const string LEFT_TEMP = "M104";
        public const string RIGHT_TEMP = "M204";
        public const string MID_TEMP = "M304";
        public const string RETRACT_START = "M227";
        public const string RETRACT_STOP = "M228";

        public const string EXTRUDER_PRESSURE = "M108";

        public const int CUBE3_INFINITY_RINSE = 500;
        public const int CUBEPRO_INFINITY_RINSE = 501;

        public const int ABS_TEAL = 76;
        public const int ABS_PURPLE = 77;
        public const int ABS_BROWN = 78;
        public const int ABS_SILVER = 79;
        public const int ABS_GITDG = 80;
        public const int ABS_GITDB = 81;
        public const int ABS_FOREST_GREEN = 105;
        public const int ABS_NAVY_BLUE = 106;
        public const int ABS_CORAL = 107;
        public const int ABS_DARK_GREY = 108;
        public const int ABS_PALE_YELLOW = 109;
        public const int ABS_GOLD = 110;
        public const int ABS_BRONZE = 111;
        public const int ABS_RED = 132;
        public const int ABS_GREEN = 133;
        public const int ABS_BLUE = 134;
        public const int ABS_YELLOW = 135;
        public const int ABS_WHITE = 136;
        public const int ABS_BLACK = 137;
        public const int ABS_TAN = 138;
        public const int ABS_MAGENTA = 139;
        public const int ABS_ORANGE = 140;
        public const int ABS_NEON_GREEN = 141;

        public const int PLA_TEAL = 92;
        public const int PLA_PURPLE = 93;
        public const int PLA_BROWN = 94;
        public const int PLA_SILVER = 95;
        public const int PLA_GITDG = 96;
        public const int PLA_GITDB = 97;
        public const int PLA_FOREST_GREEN = 98;
        public const int PLA_NAVY_BLUE = 99;
        public const int PLA_CORAL = 100;
        public const int PLA_DARK_GREY = 101;
        public const int PLA_PALE_YELLOW = 102;
        public const int PLA_GOLD = 103;
        public const int PLA_BRONZE = 104;
        public const int PLA_RED = 82;
        public const int PLA_GREEN = 83;
        public const int PLA_BLUE = 84;
        public const int PLA_YELLOW = 85;
        public const int PLA_WHITE = 86;
        public const int PLA_BLACK = 87;
        public const int PLA_TAN = 88;
        public const int PLA_MAGENTA = 89;
        public const int PLA_ORANGE = 90;
        public const int PLA_NEON_GREEN = 91;
        public const int PLA_NATURAL = 112;

        public const int CP_ABS_TEAL = 260;
        public const int CP_ABS_PURPLE = 262;
        public const int CP_ABS_BROWN = 261;
        public const int CP_ABS_SILVER = 263;
        public const int CP_ABS_GITDG = 264;
        public const int CP_ABS_GITDB = 265;
        public const int CP_ABS_FOREST_GREEN = 267;
        public const int CP_ABS_NAVY_BLUE = 268;
        public const int CP_ABS_CORAL = 269;
        public const int CP_ABS_DARK_GREY = 270;
        public const int CP_ABS_PALE_YELLOW = 271;
        public const int CP_ABS_GOLD = 272;
        public const int CP_ABS_BRONZE = 273;
        public const int CP_ABS_RED = 252;
        public const int CP_ABS_GREEN = 257;
        public const int CP_ABS_BLUE = 258;
        public const int CP_ABS_YELLOW = 255;
        public const int CP_ABS_WHITE = 250;
        public const int CP_ABS_BLACK = 259;
        public const int CP_ABS_TAN = 254;
        public const int CP_ABS_MAGENTA = 251;
        public const int CP_ABS_ORANGE = 253;
        public const int CP_ABS_NEON_GREEN = 256;
        public const int CP_ABS_INDUST_GREY = 266;

        public const int CP_PLA_TEAL = 210;
        public const int CP_PLA_PURPLE = 212;
        public const int CP_PLA_BROWN = 211;
        public const int CP_PLA_SILVER = 213;
        public const int CP_PLA_GITDG = 216;
        public const int CP_PLA_GITDB = 217;
        public const int CP_PLA_FOREST_GREEN = 218;
        public const int CP_PLA_NAVY_BLUE = 219;
        public const int CP_PLA_CORAL = 220;
        public const int CP_PLA_DARK_GREY = 221;
        public const int CP_PLA_PALE_YELLOW = 222;
        public const int CP_PLA_GOLD = 223;
        public const int CP_PLA_BRONZE = 224;
        public const int CP_PLA_RED = 202;
        public const int CP_PLA_GREEN = 207;
        public const int CP_PLA_BLUE = 208;
        public const int CP_PLA_YELLOW = 205;
        public const int CP_PLA_WHITE = 200;
        public const int CP_PLA_BLACK = 209;
        public const int CP_PLA_TAN = 204;
        public const int CP_PLA_MAGENTA = 201;
        public const int CP_PLA_ORANGE = 203;
        public const int CP_PLA_NEON_GREEN = 206;
        public const int CP_PLA_NATURAL = 214;
        public const int CP_PLA_INDUST_GREY = 215;

        public const int EKO_RED = 300;
        public const int EKO_WHITE = 301;
        public const int EKO_BLACK = 302;
        public const int EKO_NATURAL = 303;
        public const int EKO_GREY = 304;

        public const string PLA = "PLA";
        public const string ABS = "ABS";
        public const string CP_PLA = "CP_PLA";
        public const string CP_ABS = "CP_ABS";
        public const string EKO = "EKO";
        public const string INFRINSE = "INFRINSE";
        public const string CP_INFRINSE = "CP_INFRINSE";
        public const string EMPTY = "EMPTY";

        public const string BLACK = "Black";
        public const string BLUE = "Blue";
        public const string BRONZE = "Bronze";
        public const string BROWN = "Brown";
        public const string CORAL = "Coral";
        public const string DARK_GREY = "Dark Grey";
        public const string FOREST_GREEN = "Forest Green";
        public const string GITDB = "GITDB";
        public const string GITDG = "GITDG";
        public const string GOLD = "Gold";
        public const string GREEN = "Green";
        public const string INDUST_GREY = "Grey";
        public const string MAGENTA = "Magenta";
        public const string NATURAL = "Natural";
        public const string NAVY_BLUE = "Navy Blue";
        public const string NEON_GREEN = "Neon Green";
        public const string ORANGE = "Orange";
        public const string PALE_YELLOW = "Pale Yellow";
        public const string PURPLE = "Purple";
        public const string RED = "Red";
        public const string SILVER = "Silver";
        public const string TAN = "Tan";
        public const string TEAL = "Teal";
        public const string WHITE = "White";
        public const string YELLOW = "Yellow";
        public const string INFINITY_RINSE = "InfRinse";

        public static List<String> cube3PLAColors = new List<String>(new String[]
        {
            BLACK, BLUE, BRONZE, CORAL, DARK_GREY, FOREST_GREEN, GITDB,
            GITDG, GOLD, GREEN, MAGENTA, NATURAL, NAVY_BLUE,
            NEON_GREEN, ORANGE, PALE_YELLOW, PURPLE, RED, SILVER, TAN,
              TEAL, WHITE, YELLOW
        });

        public static List<String> cubeProPLAColors = new List<String>(new String[]
{
            BLACK, BLUE, BRONZE, CORAL, DARK_GREY, FOREST_GREEN, GITDB,
            GITDG, GOLD, GREEN, INDUST_GREY, MAGENTA, NATURAL, NAVY_BLUE,
            NEON_GREEN, ORANGE, PALE_YELLOW, PURPLE, RED, SILVER, TAN,
              TEAL, WHITE, YELLOW
});

        public static List<String> cube3ABSColors = new List<String>(new String[]
        {
            BLACK, BLUE, BRONZE, CORAL, DARK_GREY, FOREST_GREEN, GITDB,
            GITDG, GOLD, GREEN, MAGENTA, NAVY_BLUE,
            NEON_GREEN, ORANGE, PALE_YELLOW, PURPLE, RED, SILVER, TAN,
              TEAL, WHITE, YELLOW
        });

        public static List<String> cubeProABSColors = new List<String>(new String[]
{
            BLACK, BLUE, BRONZE, CORAL, DARK_GREY, FOREST_GREEN, GITDB,
            GITDG, GOLD, GREEN, INDUST_GREY, MAGENTA, NAVY_BLUE,
            NEON_GREEN, ORANGE, PALE_YELLOW, PURPLE, RED, SILVER, TAN,
              TEAL, WHITE, YELLOW
});

        public static List<String> ekoCycleColors = new List<String>(new String[]
        {
            BLACK, INDUST_GREY, NATURAL, RED, WHITE
        });

        static public string GetMaterialType(int material)
        {
            switch (material)
            {
                case -1:
                    return EMPTY;

                case ABS_BLACK:
                case ABS_BLUE:
                case ABS_BRONZE:
                case ABS_BROWN:
                case ABS_CORAL:
                case ABS_DARK_GREY:
                case ABS_FOREST_GREEN:
                case ABS_GITDB:
                case ABS_GITDG:
                case ABS_GOLD:
                case ABS_GREEN:
                case ABS_MAGENTA:
                case ABS_NAVY_BLUE:
                case ABS_NEON_GREEN:
                case ABS_ORANGE:
                case ABS_PALE_YELLOW:
                case ABS_PURPLE:
                case ABS_RED:
                case ABS_SILVER:
                case ABS_TAN:
                case ABS_TEAL:
                case ABS_WHITE:
                case ABS_YELLOW:
                    return ABS;

                case CP_ABS_BLACK:
                case CP_ABS_BLUE:
                case CP_ABS_BRONZE:
                case CP_ABS_BROWN:
                case CP_ABS_CORAL:
                case CP_ABS_DARK_GREY:
                case CP_ABS_FOREST_GREEN:
                case CP_ABS_GITDB:
                case CP_ABS_GITDG:
                case CP_ABS_GOLD:
                case CP_ABS_GREEN:
                case CP_ABS_INDUST_GREY:
                case CP_ABS_MAGENTA:
                case CP_ABS_NAVY_BLUE:
                case CP_ABS_NEON_GREEN:
                case CP_ABS_ORANGE:
                case CP_ABS_PALE_YELLOW:
                case CP_ABS_PURPLE:
                case CP_ABS_RED:
                case CP_ABS_SILVER:
                case CP_ABS_TAN:
                case CP_ABS_TEAL:
                case CP_ABS_WHITE:
                case CP_ABS_YELLOW:
                    return CP_ABS;

                case PLA_BLACK:
                case PLA_BLUE:
                case PLA_BRONZE:
                case PLA_BROWN:
                case PLA_CORAL:
                case PLA_DARK_GREY:
                case PLA_FOREST_GREEN:
                case PLA_GITDB:
                case PLA_GITDG:
                case PLA_GOLD:
                case PLA_GREEN:
                case PLA_MAGENTA:
                case PLA_NATURAL:
                case PLA_NAVY_BLUE:
                case PLA_NEON_GREEN:
                case PLA_ORANGE:
                case PLA_PALE_YELLOW:
                case PLA_PURPLE:
                case PLA_RED:
                case PLA_SILVER:
                case PLA_TAN:
                case PLA_TEAL:
                case PLA_WHITE:
                case PLA_YELLOW:
                    return PLA;

                case CP_PLA_BLACK:
                case CP_PLA_BLUE:
                case CP_PLA_BRONZE:
                case CP_PLA_BROWN:
                case CP_PLA_CORAL:
                case CP_PLA_DARK_GREY:
                case CP_PLA_FOREST_GREEN:
                case CP_PLA_GITDB:
                case CP_PLA_GITDG:
                case CP_PLA_GOLD:
                case CP_PLA_GREEN:
                case CP_PLA_INDUST_GREY:
                case CP_PLA_MAGENTA:
                case CP_PLA_NATURAL:
                case CP_PLA_NAVY_BLUE:
                case CP_PLA_NEON_GREEN:
                case CP_PLA_ORANGE:
                case CP_PLA_PALE_YELLOW:
                case CP_PLA_PURPLE:
                case CP_PLA_RED:
                case CP_PLA_SILVER:
                case CP_PLA_TAN:
                case CP_PLA_TEAL:
                case CP_PLA_WHITE:
                case CP_PLA_YELLOW:
                    return CP_PLA;

                case EKO_GREY:
                case EKO_RED:
                case EKO_WHITE:
                case EKO_BLACK:
                case EKO_NATURAL:
                    return EKO;

                case CUBE3_INFINITY_RINSE:
                    return INFRINSE;

                case CUBEPRO_INFINITY_RINSE:
                    return CP_INFRINSE;

                default:
                    return "";
            }
        }


        static public string GetMaterialColor(int material)
        {
            switch (material)
            {
                case -1:
                    return "";

                case PLA_BLACK:
                case ABS_BLACK:
                case CP_PLA_BLACK:
                case CP_ABS_BLACK:
                case EKO_BLACK:
                    return BLACK;

                case PLA_BLUE:
                case ABS_BLUE:
                case CP_PLA_BLUE:
                case CP_ABS_BLUE:
                    return BLUE;

                case PLA_BRONZE:
                case ABS_BRONZE:
                case CP_PLA_BRONZE:
                case CP_ABS_BRONZE:
                    return BRONZE;

                case PLA_BROWN:
                case ABS_BROWN:
                case CP_PLA_BROWN:
                case CP_ABS_BROWN:
                    return BROWN;

                case PLA_CORAL:
                case ABS_CORAL:
                case CP_PLA_CORAL:
                case CP_ABS_CORAL:
                    return CORAL;

                case PLA_DARK_GREY:
                case ABS_DARK_GREY:
                case CP_PLA_DARK_GREY:
                case CP_ABS_DARK_GREY:
                    return DARK_GREY;

                case PLA_FOREST_GREEN:
                case ABS_FOREST_GREEN:
                case CP_PLA_FOREST_GREEN:
                case CP_ABS_FOREST_GREEN:
                    return FOREST_GREEN;

                case PLA_GITDB:
                case ABS_GITDB:
                case CP_PLA_GITDB:
                case CP_ABS_GITDB:
                    return GITDB;

                case PLA_GITDG:
                case ABS_GITDG:
                case CP_PLA_GITDG:
                case CP_ABS_GITDG:
                    return GITDG;

                case PLA_GOLD:
                case ABS_GOLD:
                case CP_PLA_GOLD:
                case CP_ABS_GOLD:
                    return GOLD;

                case PLA_GREEN:
                case ABS_GREEN:
                case CP_PLA_GREEN:
                case CP_ABS_GREEN:
                    return GREEN;

                case CP_PLA_INDUST_GREY:
                case CP_ABS_INDUST_GREY:
                case EKO_GREY:
                    return INDUST_GREY;

                case PLA_MAGENTA:
                case ABS_MAGENTA:
                case CP_PLA_MAGENTA:
                case CP_ABS_MAGENTA:
                    return MAGENTA;

                case PLA_NATURAL:
                case CP_PLA_NATURAL:
                case EKO_NATURAL:
                    return NATURAL;

                case PLA_NAVY_BLUE:
                case ABS_NAVY_BLUE:
                case CP_PLA_NAVY_BLUE:
                case CP_ABS_NAVY_BLUE:
                    return NAVY_BLUE;

                case PLA_NEON_GREEN:
                case ABS_NEON_GREEN:
                case CP_PLA_NEON_GREEN:
                case CP_ABS_NEON_GREEN:
                    return NEON_GREEN;

                case PLA_ORANGE:
                case ABS_ORANGE:
                case CP_PLA_ORANGE:
                case CP_ABS_ORANGE:
                    return ORANGE;

                case PLA_PALE_YELLOW:
                case ABS_PALE_YELLOW:
                case CP_PLA_PALE_YELLOW:
                case CP_ABS_PALE_YELLOW:
                    return PALE_YELLOW;

                case PLA_PURPLE:
                case ABS_PURPLE:
                case CP_PLA_PURPLE:
                case CP_ABS_PURPLE:
                    return PURPLE;

                case PLA_RED:
                case ABS_RED:
                case CP_PLA_RED:
                case CP_ABS_RED:
                case EKO_RED:
                    return RED;

                case PLA_SILVER:
                case ABS_SILVER:
                case CP_PLA_SILVER:
                case CP_ABS_SILVER:
                    return SILVER;

                case PLA_TAN:
                case ABS_TAN:
                case CP_PLA_TAN:
                case CP_ABS_TAN:
                    return TAN;

                case PLA_TEAL:
                case ABS_TEAL:
                case CP_PLA_TEAL:
                case CP_ABS_TEAL:
                    return TEAL;

                case PLA_WHITE:
                case ABS_WHITE:
                case CP_PLA_WHITE:
                case CP_ABS_WHITE:
                case EKO_WHITE:
                    return WHITE;

                case PLA_YELLOW:
                case ABS_YELLOW:
                case CP_PLA_YELLOW:
                case CP_ABS_YELLOW:
                    return YELLOW;

                case CUBE3_INFINITY_RINSE:
                case CUBEPRO_INFINITY_RINSE:
                    return INFINITY_RINSE;

                default:
                    return "";
            }
        }

        static public int getCUBE3Code(string type, string color)
        {
            int cube3Code = -1;
            if (type.Equals(PLA))
            {
                if (color.Equals(BLACK))
                    cube3Code = PLA_BLACK;
                else if (color.Equals(BLUE))
                    cube3Code = PLA_BLUE;
                else if (color.Equals(BRONZE))
                    cube3Code = PLA_BRONZE;
                else if (color.Equals(BROWN))
                    cube3Code = PLA_BROWN;
                else if (color.Equals(CORAL))
                    cube3Code = PLA_CORAL;
                else if (color.Equals(DARK_GREY))
                    cube3Code = PLA_DARK_GREY;
                else if (color.Equals(FOREST_GREEN))
                    cube3Code = PLA_FOREST_GREEN;
                else if (color.Equals(GITDB))
                    cube3Code = PLA_GITDB;
                else if (color.Equals(GITDG))
                    cube3Code = PLA_GITDG;
                else if (color.Equals(GOLD))
                    cube3Code = PLA_GOLD;
                else if (color.Equals(GREEN))
                    cube3Code = PLA_GREEN;
                else if (color.Equals(MAGENTA))
                    cube3Code = PLA_MAGENTA;
                else if (color.Equals(NATURAL))
                    cube3Code = PLA_NATURAL;
                else if (color.Equals(NAVY_BLUE))
                    cube3Code = PLA_NAVY_BLUE;
                else if (color.Equals(NEON_GREEN))
                    cube3Code = PLA_NEON_GREEN;
                else if (color.Equals(ORANGE))
                    cube3Code = PLA_ORANGE;
                else if (color.Equals(PALE_YELLOW))
                    cube3Code = PLA_PALE_YELLOW;
                else if (color.Equals(PURPLE))
                    cube3Code = PLA_PURPLE;
                else if (color.Equals(RED))
                    cube3Code = PLA_RED;
                else if (color.Equals(SILVER))
                    cube3Code = PLA_SILVER;
                else if (color.Equals(TAN))
                    cube3Code = PLA_TAN;
                else if (color.Equals(TEAL))
                    cube3Code = PLA_TEAL;
                else if (color.Equals(WHITE))
                    cube3Code = PLA_WHITE;
                else if (color.Equals(YELLOW))
                    cube3Code = PLA_YELLOW;
            }
            else if (type.Equals(ABS))
            {
                if (color.Equals(BLACK))
                    cube3Code = ABS_BLACK;
                else if (color.Equals(BLUE))
                    cube3Code = ABS_BLUE;
                else if (color.Equals(BRONZE))
                    cube3Code = ABS_BRONZE;
                else if (color.Equals(BROWN))
                    cube3Code = ABS_BROWN;
                else if (color.Equals(CORAL))
                    cube3Code = ABS_CORAL;
                else if (color.Equals(DARK_GREY))
                    cube3Code = ABS_DARK_GREY;
                else if (color.Equals(FOREST_GREEN))
                    cube3Code = ABS_FOREST_GREEN;
                else if (color.Equals(GITDB))
                    cube3Code = ABS_GITDB;
                else if (color.Equals(GITDG))
                    cube3Code = ABS_GITDG;
                else if (color.Equals(GOLD))
                    cube3Code = ABS_GOLD;
                else if (color.Equals(GREEN))
                    cube3Code = ABS_GREEN;
                else if (color.Equals(MAGENTA))
                    cube3Code = ABS_MAGENTA;
                else if (color.Equals(NAVY_BLUE))
                    cube3Code = ABS_NAVY_BLUE;
                else if (color.Equals(NEON_GREEN))
                    cube3Code = ABS_NEON_GREEN;
                else if (color.Equals(ORANGE))
                    cube3Code = ABS_ORANGE;
                else if (color.Equals(PALE_YELLOW))
                    cube3Code = ABS_PALE_YELLOW;
                else if (color.Equals(PURPLE))
                    cube3Code = ABS_PURPLE;
                else if (color.Equals(RED))
                    cube3Code = ABS_RED;
                else if (color.Equals(SILVER))
                    cube3Code = ABS_SILVER;
                else if (color.Equals(TAN))
                    cube3Code = ABS_TAN;
                else if (color.Equals(TEAL))
                    cube3Code = ABS_TEAL;
                else if (color.Equals(WHITE))
                    cube3Code = ABS_WHITE;
                else if (color.Equals(YELLOW))
                    cube3Code = ABS_YELLOW;
            }
            else if (type.Equals(CP_ABS))
            {
                if (color.Equals(BLACK))
                    cube3Code = CP_ABS_BLACK;
                else if (color.Equals(BLUE))
                    cube3Code = CP_ABS_BLUE;
                else if (color.Equals(BRONZE))
                    cube3Code = CP_ABS_BRONZE;
                else if (color.Equals(BROWN))
                    cube3Code = CP_ABS_BROWN;
                else if (color.Equals(CORAL))
                    cube3Code = CP_ABS_CORAL;
                else if (color.Equals(DARK_GREY))
                    cube3Code = CP_ABS_DARK_GREY;
                else if (color.Equals(FOREST_GREEN))
                    cube3Code = CP_ABS_FOREST_GREEN;
                else if (color.Equals(GITDB))
                    cube3Code = CP_ABS_GITDB;
                else if (color.Equals(GITDG))
                    cube3Code = CP_ABS_GITDG;
                else if (color.Equals(GOLD))
                    cube3Code = CP_ABS_GOLD;
                else if (color.Equals(GREEN))
                    cube3Code = CP_ABS_GREEN;
                else if (color.Equals(INDUST_GREY))
                    cube3Code = CP_ABS_INDUST_GREY;
                else if (color.Equals(MAGENTA))
                    cube3Code = CP_ABS_MAGENTA;
                else if (color.Equals(NAVY_BLUE))
                    cube3Code = CP_ABS_NAVY_BLUE;
                else if (color.Equals(NEON_GREEN))
                    cube3Code = CP_ABS_NEON_GREEN;
                else if (color.Equals(ORANGE))
                    cube3Code = CP_ABS_ORANGE;
                else if (color.Equals(PALE_YELLOW))
                    cube3Code = CP_ABS_PALE_YELLOW;
                else if (color.Equals(PURPLE))
                    cube3Code = CP_ABS_PURPLE;
                else if (color.Equals(RED))
                    cube3Code = CP_ABS_RED;
                else if (color.Equals(SILVER))
                    cube3Code = CP_ABS_SILVER;
                else if (color.Equals(TAN))
                    cube3Code = CP_ABS_TAN;
                else if (color.Equals(TEAL))
                    cube3Code = CP_ABS_TEAL;
                else if (color.Equals(WHITE))
                    cube3Code = CP_ABS_WHITE;
                else if (color.Equals(YELLOW))
                    cube3Code = CP_ABS_YELLOW;
            }   
            else if (type.Equals(CP_PLA))
            {
                if (color.Equals(BLACK))
                    cube3Code = CP_PLA_BLACK;
                else if (color.Equals(BLUE))
                    cube3Code = CP_PLA_BLUE;
                else if (color.Equals(BRONZE))
                    cube3Code = CP_PLA_BRONZE;
                else if (color.Equals(BROWN))
                    cube3Code = CP_PLA_BROWN;
                else if (color.Equals(CORAL))
                    cube3Code = CP_PLA_CORAL;
                else if (color.Equals(DARK_GREY))
                    cube3Code = CP_PLA_DARK_GREY;
                else if (color.Equals(FOREST_GREEN))
                    cube3Code = CP_PLA_FOREST_GREEN;
                else if (color.Equals(GITDB))
                    cube3Code = CP_PLA_GITDB;
                else if (color.Equals(GITDG))
                    cube3Code = CP_PLA_GITDG;
                else if (color.Equals(GOLD))
                    cube3Code = CP_PLA_GOLD;
                else if (color.Equals(GREEN))
                    cube3Code = CP_PLA_GREEN;
                else if (color.Equals(INDUST_GREY))
                    cube3Code = CP_PLA_INDUST_GREY;
                else if (color.Equals(MAGENTA))
                    cube3Code = CP_PLA_MAGENTA;
                else if (color.Equals(NATURAL))
                    cube3Code = CP_PLA_NATURAL;
                else if (color.Equals(NAVY_BLUE))
                    cube3Code = CP_PLA_NAVY_BLUE;
                else if (color.Equals(NEON_GREEN))
                    cube3Code = CP_PLA_NEON_GREEN;
                else if (color.Equals(ORANGE))
                    cube3Code = CP_PLA_ORANGE;
                else if (color.Equals(PALE_YELLOW))
                    cube3Code = CP_PLA_PALE_YELLOW;
                else if (color.Equals(PURPLE))
                    cube3Code = CP_PLA_PURPLE;
                else if (color.Equals(RED))
                    cube3Code = CP_PLA_RED;
                else if (color.Equals(SILVER))
                    cube3Code = CP_PLA_SILVER;
                else if (color.Equals(TAN))
                    cube3Code = CP_PLA_TAN;
                else if (color.Equals(TEAL))
                    cube3Code = CP_PLA_TEAL;
                else if (color.Equals(WHITE))
                    cube3Code = CP_PLA_WHITE;
                else if (color.Equals(YELLOW))
                    cube3Code = CP_PLA_YELLOW;
            }
            else if (type.Equals(EKO))
            {
                if (color.Equals(RED))
                    cube3Code = EKO_RED;
                else if (color.Equals(BLACK))
                    cube3Code = EKO_BLACK;
                else if (color.Equals(INDUST_GREY))
                    cube3Code = EKO_GREY;
                else if (color.Equals(WHITE))
                    cube3Code = EKO_WHITE;
                else if (color.Equals(NATURAL))
                    cube3Code = EKO_NATURAL;
            }
            else if (type.Equals(INFRINSE))
            {
                cube3Code = CUBE3_INFINITY_RINSE;
            }

            else if (type.Equals(CP_INFRINSE))
            {
                cube3Code = CUBEPRO_INFINITY_RINSE;
            }

            return cube3Code;
        }
    }
}



