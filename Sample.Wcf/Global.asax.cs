using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using MvcMiniProfiler;
using MvcMiniProfiler.Wcf.Storage;
using System.IO;
using SampleWcf.Helpers;
using Dapper;
using MvcMiniProfiler.Wcf;

namespace Sample.Wcf
{
    public class Global : System.Web.HttpApplication
    {

        /// <summary>
        /// Customize aspects of the MiniProfiler.
        /// </summary>
        private void InitProfilerSettings()
        {
            WcfMiniProfiler.InitialiseWcfMiniProfiler();
            // a powerful feature of the MiniProfiler is the ability to share links to results with other developers.
            // by default, however, long-term result caching is done in HttpRuntime.Cache, which is very volatile.
            // 
            // let's rig up serialization of our profiler results to a database, so they survive app restarts.

            // At the moment we're just echoing results back out with every request - we don't need to store these
            //MiniProfiler.Settings.Storage = new WcfRequestInstanceStorage();

            // different RDBMS have different ways of declaring sql parameters - SQLite can understand inline sql parameters just fine
            // by default, sql parameters won't be displayed
            MiniProfiler.Settings.SqlFormatter = new MvcMiniProfiler.SqlFormatters.InlineFormatter();

            // Ignore the following as we're not displaying anything through this currently
            //// these settings are optional and all have defaults, any matching setting specified in .RenderIncludes() will
            //// override the application-wide defaults specified here, for example if you had both:
            ////    MiniProfiler.Settings.PopupRenderPosition = RenderPosition.Right;
            ////    and in the page:
            ////    @MiniProfiler.RenderIncludes(position: RenderPosition.Left)
            //// then the position would be on the left that that page, and on the right (the app default) for anywhere that doesn't
            //// specified position in the .RenderIncludes() call.
            //MiniProfiler.Settings.PopupRenderPosition = RenderPosition.Right; //defaults to left
            //MiniProfiler.Settings.PopupMaxTracesToShow = 10;                  //defaults to 15
            //MiniProfiler.Settings.RouteBasePath = "~/profiler";               //e.g. /profiler/mini-profiler-includes.js

            // optional settings to control the stack trace output in the details pane
            // the exclude methods are not thread safe, so be sure to only call these once per appdomain

            MiniProfiler.Settings.ExcludeType("SessionFactory"); // Ignore any class with the name of SessionFactory
            MiniProfiler.Settings.ExcludeAssembly("NHibernate"); // Ignore any assembly named NHibernate
            MiniProfiler.Settings.ExcludeMethod("Flush");        // Ignore any method with the name of Flush
            MiniProfiler.Settings.StackMaxLength = 256;          // default is 120 characters

            // because profiler results can contain sensitive data (e.g. sql queries with parameter values displayed), we
            // can define a function that will authorize clients to see the json or full page results.
            // we use it on http://stackoverflow.com to check that the request cookies belong to a valid developer.
            MiniProfiler.Settings.Results_Authorize = (request, profiler) =>
            {
                //// we'll use it here to check that a specific page had a query parameter
                //if ("Home/ResultsAuthorization".Equals(profiler.Name, StringComparison.OrdinalIgnoreCase))
                //{
                //    // root Timing's Name will always be the profiled request's full url
                //    return profiler.Root.Name.ToLower().Contains("isauthorized");
                //}
                return true; // all other requests are kosher
            };
        }

        protected void Application_Start(object sender, EventArgs e)
        {

            InitProfilerSettings();

            var dbFile = HttpContext.Current.Server.MapPath("~/App_Data/TestMiniProfiler.sqlite");
            if (System.IO.File.Exists(dbFile))
            {
                File.Delete(dbFile);
            }

            using (var cnn = new System.Data.SQLite.SQLiteConnection(WcfCommon.ConnectionString))
            {
                cnn.Open();
                cnn.Execute("create table RouteHits(RouteName,HitCount)");
                // we need some tiny mods to allow sqlite support 
                foreach (var sql in SqliteMiniProfilerStorage.TableCreationSQL)
                {
                    cnn.Execute(sql);
                }
            }
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}