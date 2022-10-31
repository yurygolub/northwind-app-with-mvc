﻿using System;
using System.Runtime.Serialization;

namespace Northwind.Services.EntityFrameworkCore.Blogging.Context
{
    [Serializable]
    public class EnvironmentVariableException : Exception
    {
        public EnvironmentVariableException()
        {
        }

        public EnvironmentVariableException(string message)
            : base(message)
        {
        }

        public EnvironmentVariableException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected EnvironmentVariableException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
