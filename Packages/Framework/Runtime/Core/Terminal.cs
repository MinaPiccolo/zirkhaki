using System.Collections.Generic;

namespace Revy.Framework
{
    public sealed partial class  MFramework
    {
        private static class Terminal
        {
            public static void IndexObject(ITerminalIndex obj)
            {
                FTerminal.IndexObject(obj);
            }
        }
    }

}