using System;

namespace BitForByteSupport
{
    public  class Retraction
    {
        private int p;
        private int s;
        private int g;
        private int f;
        private int index;

        public Retraction(string v, int index)
        {
            string[] splitRetraction = v.Split(' ');
            int entries = splitRetraction.Length;

            Index = index;

            switch (entries)
            {
                case 0:
                case 1:
                    P = S = G = F = -1;
                    break;

                case 2:
                    P = Convert.ToInt32(new string(splitRetraction[1].ToCharArray(1, splitRetraction[1].Length - 1)));
                    S = G = F = -1;
                    break;

                case 3:
                    P = Convert.ToInt32(new string(splitRetraction[1].ToCharArray(1, splitRetraction[1].Length - 1)));
                    S = Convert.ToInt32(new string(splitRetraction[2].ToCharArray(1, splitRetraction[2].Length - 1)));
                    G = F = -1;
                    break;

                case 4:
                    P = Convert.ToInt32(new string(splitRetraction[1].ToCharArray(1, splitRetraction[1].Length - 1)));
                    S = Convert.ToInt32(new string(splitRetraction[2].ToCharArray(1, splitRetraction[2].Length - 1)));
                    G = Convert.ToInt32(new string(splitRetraction[3].ToCharArray(1, splitRetraction[3].Length - 1)));
                    F = -1;
                    break;

                case 5:
                default:
                    P = Convert.ToInt32(new string(splitRetraction[1].ToCharArray(1, splitRetraction[1].Length - 1)));
                    S = Convert.ToInt32(new string(splitRetraction[2].ToCharArray(1, splitRetraction[2].Length - 1)));
                    G = Convert.ToInt32(new string(splitRetraction[3].ToCharArray(1, splitRetraction[3].Length - 1)));
                    F = Convert.ToInt32(new string(splitRetraction[4].ToCharArray(1, splitRetraction[4].Length - 1)));
                    break;
            }
        }

        public int P
        {
            get => p;
            set => p = value;
        }
        public int S
        {
            get => s;
            set => s = value;
        }
        public int G
        {
            get => g;
            set => g = value;
        }
        public int F
        {
            get => f;
            set => f = value;
        }
        public int Index
        {
            get => index;
            set => index = value;
        }
    }
}