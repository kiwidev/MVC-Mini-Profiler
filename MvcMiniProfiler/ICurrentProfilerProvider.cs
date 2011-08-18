
namespace MvcMiniProfiler
{
    /// <summary>
    /// Provider used by <see cref="MiniProfiler.Current"/> to get the current profiler
    /// Use this when not using a HttpContext based profiler mechanism
    /// </summary>
    public interface ICurrentProfilerProvider
    {
        /// <summary>
        /// Gets the current profiler, or null if there is no current profiler
        /// </summary>
        /// <returns>The current profiler, or null if there is no current profiler</returns>
        MiniProfiler GetCurrentProfiler();

        /// <summary>
        /// Sets the current profiler.  Can be null.
        /// </summary>
        /// <param name="profiler">The current profiler.  Can be null</param>
        void SetCurrentProfiler(MiniProfiler profiler);
    }
}
