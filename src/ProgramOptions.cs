using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarframeUnity
{
    public static class ProgramOptions
    {
        public static bool ShowAll = true;
        public static int MinCredits = 2000;
        public static Stream Sound = Properties.Resources.Ringtone_Alarm;
        public static bool PlaySound = true;
    }

}
