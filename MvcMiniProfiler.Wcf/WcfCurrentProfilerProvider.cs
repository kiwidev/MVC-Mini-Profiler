using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvcMiniProfiler.Wcf.Helpers;

namespace MvcMiniProfiler.Wcf
{
    public class WcfCurrentProfilerProvider : ICurrentProfilerProvider
    {
        private const string WcfCacheKey = ":mini-profiler:";

        public MiniProfiler GetCurrentProfiler()
        {
            var context = WcfInstanceContext.GetCurrentWithoutInstantiating();
            if (context == null) return null;

            return context.Items[WcfCacheKey] as MiniProfiler;
        }

        public void SetCurrentProfiler(MiniProfiler profiler)
        {
            var context = WcfInstanceContext.Current;
            if (context == null) return;

            context.Items[WcfCacheKey] = profiler;
        }
    }
}
