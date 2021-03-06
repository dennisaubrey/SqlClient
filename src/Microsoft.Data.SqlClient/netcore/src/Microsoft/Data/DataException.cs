// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;
using Microsoft.Data.SqlClient;

namespace Microsoft.Data
{
    // These functions are major point of localization.
    // We need to have a rules to enforce consistency there.
    // The dangerous point there are the string arguments of the exported (internal) methods.
    // This string can be argument, table or constraint name but never text of exception itself.
    // Make an invariant that all texts of exceptions coming from resources only.

    internal static class ExceptionBuilder
    {
        // The class defines the exceptions that are specific to the DataSet.
        // The class contains functions that take the proper informational variables and then construct
        // the appropriate exception with an error string obtained from the resource Data.txt.
        // The exception is then returned to the caller, so that the caller may then throw from its
        // location so that the catcher of the exception will have the appropriate call stack.
        // This class is used so that there will be compile time checking of error messages.
        // The resource Data.txt will ensure proper string text based on the appropriate
        // locale.

        static private void TraceException(
           string trace, Exception e)
        {
            Debug.Assert(null != e, "TraceException: null Exception");
            if (null != e)
            {
                SqlClientEventSource.Log.TryAdvancedTraceEvent(trace, e.Message);
                SqlClientEventSource.Log.TryAdvancedTraceEvent("<comm.ADP.TraceException|ERR|ADV> Environment StackTrace = '{0}'", Environment.StackTrace);
            }
        }

        internal static void TraceExceptionAsReturnValue(Exception e)
        {
            TraceException("<comm.ADP.TraceException|ERR|THROW> Message='{0}'", e);
        }

        //
        // COM+ exceptions
        //
        internal static ArgumentException _Argument(string error)
        {
            ArgumentException e = new ArgumentException(error);
            ExceptionBuilder.TraceExceptionAsReturnValue(e);
            return e;
        }
        public static Exception InvalidOffsetLength()
        {
            return _Argument(StringsHelper.GetString(Strings.Data_InvalidOffsetLength));
        }
    }// ExceptionBuilder
}
