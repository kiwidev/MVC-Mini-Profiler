using System.Web;

namespace MvcMiniProfiler
{
    /// <summary>
    /// Default ICurrentProfilerProvider.  This retrieves the current profiler from <see cref="HttpContext.Current"/>
    /// or null if <see cref="HttpContext.Current"/> is null.
    /// </summary>
    public class HttpContextProfilerProvider : ICurrentProfilerProvider
    {
        private const string CacheKey = ":mini-profiler:";

        /// <summary>
        /// Gets the current profiler from the HttpContext
        /// </summary>
        /// <returns></returns>
        public MiniProfiler GetCurrentProfiler()
        {
            var context = HttpContext.Current;
            if (context == null) return null;

            return context.Items[CacheKey] as MiniProfiler;
        }

        /// <summary>
        /// Sets the profiler on the HttpContext
        /// </summary>
        /// <param name="profiler"></param>
        public void SetCurrentProfiler(MiniProfiler profiler)
        {
            var context = HttpContext.Current;
            if (context == null) return;

            context.Items[CacheKey] = profiler;
        }
    }
}
