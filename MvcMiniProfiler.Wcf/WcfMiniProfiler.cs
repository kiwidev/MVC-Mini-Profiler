using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvcMiniProfiler.Wcf.Helpers;
using System.ServiceModel;
using System.Runtime.Serialization;
using MvcMiniProfiler.Wcf.Storage;

namespace MvcMiniProfiler.Wcf
{
    [DataContract]
    public class WcfMiniProfiler : MiniProfiler
    {
        /// <summary>
        /// Initialise the default settings for the 
        /// </summary>
        public static void InitialiseWcfMiniProfiler()
        {
            
            MiniProfiler.Settings.Storage = new WcfRequestInstanceStorage();
        }


        private WcfMiniProfiler(string methodName, ProfileLevel level = ProfileLevel.Info)
            :base(methodName, level)
        {
        
        }


        /// <summary>
        /// Starts a new MiniProfiler for the current Request. This new profiler can be accessed by
        /// <see cref="MiniProfiler.Current"/>
        /// </summary>
        public static new WcfMiniProfiler Start(ProfileLevel level = ProfileLevel.Info)
        {
            var context = WcfInstanceContext.Current;
            if (context == null) return null;

            var operationContext = OperationContext.Current;
            if (operationContext == null) return null;

            var instanceContext = operationContext.InstanceContext;
            if (instanceContext == null) return null;

            string serviceName = instanceContext.Host.Description.Name;// .BaseAddresses.FirstOrDefault();

            //var url = context.Request.Url;
            //var path = context.Request.AppRelativeCurrentExecutionFilePath.Substring(1);

            //// don't profile /content or /scripts, either - happens in web.dev
            //foreach (var ignored in Settings.IgnoredPaths ?? new string[0])
            //{
            //    if (path.ToUpperInvariant().Contains((ignored ?? "").ToUpperInvariant()))
            //        return null;
            //}

            CurrentProfilerProvider = new WcfCurrentProfilerProvider();

            var result = new WcfMiniProfiler(/*0url.OriginalString*/ serviceName, level);

            Current = result;

            // don't really want to pass in the context to MiniProfler's constructor or access it statically in there, either
            result.User = (WcfSettings.UserProvider ?? new EmptyUserProvider()).GetUser(/*context.Request*/);

            result.IsActive = true;

            return result;
        }

        /// <summary>
        /// Ends the current profiling session, if one exists.
        /// </summary>
        /// <param name="discardResults">
        /// When true, clears the <see cref="MiniProfiler.Current"/> for this HttpContext, allowing profiling to 
        /// be prematurely stopped and discarded. Useful for when a specific route does not need to be profiled.
        /// </param>
        public static new void Stop(bool discardResults = false)
        {
            StopAndReturnCurrent(discardResults);
        }

        /// <summary>
        /// Stops the profiler and returns the current profiler (if any)
        /// so it can be used in the next step
        /// </summary>
        /// <returns></returns>
        public static MiniProfiler StopAndReturnCurrent(bool discardResults = false)
        {
            var current = MiniProfiler.Current as WcfMiniProfiler;
            if (current == null)
                return null;

            // stop our timings - when this is false, we've already called .Stop before on this session
            if (!current.StopImpl())
                return null;

            if (discardResults)
            {
                MiniProfiler.Current = null;
                return null;
            }



            //var request = context.Request;
            //var response = context.Response;

            // set the profiler name to Service/Method
            EnsureServiceName(current/*, request*/);

            // because we fetch profiler results after the page loads, we have to put them somewhere in the meantime
            WcfSettings.EnsureStorageStrategy();
            Settings.Storage.Save(current);
            CurrentProfilerProvider = null;

            return current;
        }

        /// <summary>
        /// Makes sure 'profiler' has a Name, pulling it from route data or url.
        /// </summary>
        private static void EnsureServiceName(WcfMiniProfiler profiler/*, HttpRequest request*/)
        {
            // also set the profiler name to Controller/Action or /url
            if (string.IsNullOrWhiteSpace(profiler.Name))
            {
                profiler.Name = "UnknownName";
                //var rc = request.RequestContext;
                //RouteValueDictionary values;

                //if (rc != null && rc.RouteData != null && (values = rc.RouteData.Values).Count > 0)
                //{
                //    var controller = values["Controller"];
                //    var action = values["Action"];

                //    if (controller != null && action != null)
                //        profiler.Name = controller.ToString() + "/" + action.ToString();
                //}

                //if (string.IsNullOrWhiteSpace(profiler.Name))
                //{
                //    profiler.Name = request.Url.AbsolutePath ?? "";
                //    if (profiler.Name.Length > 50)
                //        profiler.Name = profiler.Name.Remove(50);
                //}
            }
        }
    }
}
