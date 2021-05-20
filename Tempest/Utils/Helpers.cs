using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Tempest.Utils
{
    class Helpers
    {
        public static bool InDebugMode()
        {
            VerifyInDebugMode();
            return DebugMode;
        }

        private static bool DebugMode;
        
        [Conditional("DEBUG")]
        private static void VerifyInDebugMode() { DebugMode = true; }
    }
}
