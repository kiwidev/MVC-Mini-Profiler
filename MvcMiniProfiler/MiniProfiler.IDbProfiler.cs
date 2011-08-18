using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvcMiniProfiler.Data;

namespace MvcMiniProfiler
{
    partial class MiniProfiler : IDbProfiler
    {

        void IDbProfiler.ExecuteStart(System.Data.Common.DbCommand profiledDbCommand, ExecuteType executeType)
        {
            SqlProfiler.ExecuteStart(profiledDbCommand, executeType);
        }

        void IDbProfiler.ExecuteFinish(System.Data.Common.DbCommand profiledDbCommand, ExecuteType executeType, System.Data.Common.DbDataReader reader)
        {
            SqlProfiler.ExecuteFinish(profiledDbCommand, executeType, reader);
        }

        void IDbProfiler.ExecuteFinish(System.Data.Common.DbCommand profiledDbCommand, ExecuteType executeType)
        {
            SqlProfiler.ExecuteFinish(profiledDbCommand, executeType);
        }

        void IDbProfiler.ReaderFinish(System.Data.Common.DbDataReader reader)
        {
            SqlProfiler.ReaderFinish(reader);
        }

        bool _isActive;
        bool IDbProfiler.IsActive { get { return _isActive; } }

        /// <summary>
        /// Flags stating whether the DbProfiler is active.  Set this to true once the
        /// profiler has been initialised.
        /// This is set to false as part of <see cref="StopImpl"/>.
        /// </summary>
        protected bool IsActive { set { _isActive = value; } }
    }
}
