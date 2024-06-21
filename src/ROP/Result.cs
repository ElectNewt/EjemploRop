﻿using System.Net;
using System.Threading.Tasks;

namespace ROP
{
    public static partial class Result
    {
        /// <summary>
        /// Object to avoid using void
        /// </summary>
        public static readonly Unit Unit = Unit.Value;
        
        /// <summary>
        /// chains an object into the Result Structure
        /// </summary>
        public static Result<T> Success<T>(this T value) => new Result<T>(value, HttpStatusCode.OK);
        
        /// <summary>
        /// chains an object into the Result Structure
        /// </summary>
        public static Result<T> Success<T>(this T value, HttpStatusCode httpStatusCode) => new Result<T>(value, httpStatusCode);
        
        /// <summary>
        /// chains an Result.Unit into the Result Structure
        /// </summary>
        public static Result<Unit> Success() => new Result<Unit>(Unit, HttpStatusCode.OK);
        
        /// <summary>
        /// Converts a synchronous Result structure into async
        /// </summary>
        public static Task<Result<T>> Async<T>(this Result<T> r) => Task.FromResult(r);


    }
}
