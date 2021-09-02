using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginDrownSolutions
{
    public static class MeuEnum
    {
        public enum Stage
        {
            Prevalidation = 10,
            PreOperation = 20,
            PostOperation = 40,
        }

        public enum Mode
        {
            Asynchronous = 1,
            Synchronous = 0
        }

    }
}
