using System;

namespace Amos.Abp.Extensions
{
    public static class LambdaExtension
    {
        public static Func<TResult> CreateFuncForVar<TResult>(Func<TResult> func) => func;

        public static Func<TIn, TResult> CreateFuncForVar<TIn, TResult>(Func<TIn, TResult> func) => func;
    }
}
