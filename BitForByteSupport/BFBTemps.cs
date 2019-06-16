using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitForByteSupport
{
    public class BFBTemps
    {
        private int index;
        int oldTemperature;
        int newTemperature;

        public BFBTemps(int index, int temperature)
        {
            Index = index;
            OldTemperature = temperature;
        }

        public int Index
        {
            get => index;
            set => index = value;
        }
        public int OldTemperature
        {
            get => oldTemperature;
            set => oldTemperature = value;
        }
        public int NewTemperature
        {
            get => newTemperature;
            set => newTemperature = value;
        }
    }
}
