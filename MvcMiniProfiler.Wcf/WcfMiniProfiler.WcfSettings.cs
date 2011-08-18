using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvcMiniProfiler.Wcf.Storage;

namespace MvcMiniProfiler.Wcf
{
    public static class WcfSettings
    {
        public static IWcfUserProvider UserProvider { get; set; }

        internal static void EnsureStorageStrategy()
        {
            if (MvcMiniProfiler.MiniProfiler.Settings.Storage == null)
            {
                MvcMiniProfiler.MiniProfiler.Settings.Storage = new WcfRequestInstanceStorage();
            }
        }
    }
}
