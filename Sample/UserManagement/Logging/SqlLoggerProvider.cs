﻿using System;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace UserManagement.Logging
{
    public class SqlLoggerProvider : ILoggerProvider
    {
        private static readonly string[] _whitelist = new string[]
        {
                "Microsoft.Data.Entity.Update.BatchExecutor",
                "Microsoft.Data.Entity.Query.QueryContextFactory"
        };

        public ILogger CreateLogger(string name)
        {
            if(_whitelist.Contains(name))
            {
                return new SqlLogger(); 
            }
            //return new SqlLogger();
            return NullLogger.Instance;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
