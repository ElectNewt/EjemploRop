using System;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;

namespace ROP
{
    public static class Result_ThenCombine
    {
        public class Combiner<T, TR>
        {
            public readonly T Identifier;
            public readonly TR Value;

            public Combiner(T identifier, TR value)
            {
                Identifier = identifier;
                Value = value;
            }
        }


        public static async Task<Result<TR>> GetCombined<T, TR>(this Task<Result<Combiner<T, TR>>> result)
        {
            Result<Combiner<T, TR>> t = await result;
            return !t.Success ? t.Errors : t.Value.Value.Success();
        }
        
        

        public static async Task<Result<Combiner<T, TR1>>> ThenCombine<T, TR1>(this Task<Result<T>> result, Func<T, Task<Result<TR1>>> method)
        {
            try
            {
                Result<T> t = await result;
                if (!t.Success)
                    return t.Errors;

                Result<TR1> r2 = await method(t.Value);


                if (t.Success && r2.Success)
                    return Result.Success(new Combiner<T, TR1>(t.Value, r2.Value));

                if (!r2.Success)
                    return r2.Errors;

                return Result.Failure<Combiner<T, TR1>>(t.Errors.Concat(r2.Errors).ToImmutableArray());
            }
            catch (Exception e)
            {
                ExceptionDispatchInfo.Capture(e).Throw();
                throw;
            }
        }

        public static async Task<Result<Combiner<T, (TR1, TR2)>>> ThenCombine<T, TR1, TR2>(this Task<Result<Combiner<T, TR1>>> result, Func<T, Task<Result<TR2>>> method)
        {
            try
            {
                Result<Combiner<T, TR1>> t = await result;
                if (!t.Success)
                    return t.Errors;

                Result<TR2> r2 = await method(t.Value.Identifier);


                if (t.Success && r2.Success)
                    return Result.Success(new Combiner<T, (TR1, TR2)>(t.Value.Identifier, (t.Value.Value, r2.Value)));

                if (!r2.Success)
                    return r2.Errors;

                return Result.Failure<Combiner<T, (TR1, TR2)>>(t.Errors.Concat(r2.Errors).ToImmutableArray());
            }
            catch (Exception e)
            {
                ExceptionDispatchInfo.Capture(e).Throw();
                throw;
            }
        }

        public static async Task<Result<Combiner<T, (TR1, TR2, TR3)>>> ThenCombine<T, TR1, TR2, TR3>(this Task<Result<Combiner<T, (TR1, TR2)>>> result, Func<T, Task<Result<TR3>>> method)
        {
            try
            {
                Result<Combiner<T, (TR1, TR2)>> t = await result;
                if (!t.Success)
                    return t.Errors;

                Result<TR3> r3 = await method(t.Value.Identifier);


                if (t.Success && r3.Success)
                    return Result.Success(new Combiner<T, (TR1, TR2, TR3)>(t.Value.Identifier, (t.Value.Value.Item1, t.Value.Value.Item2, r3.Value)));

                if (!r3.Success)
                    return r3.Errors;

                return Result.Failure<Combiner<T, (TR1, TR2, TR3)>>(t.Errors.Concat(r3.Errors).ToImmutableArray());
            }
            catch (Exception e)
            {
                ExceptionDispatchInfo.Capture(e).Throw();
                throw;
            }
        }
        
        public static async Task<Result<Combiner<T, (TR1, TR2, TR3, TR4)>>> ThenCombine<T, TR1, TR2, TR3, TR4>(this Task<Result<Combiner<T, (TR1, TR2, TR3)>>> result, Func<T, Task<Result<TR4>>> method)
        {
            try
            {
                Result<Combiner<T, (TR1, TR2, TR3)>> t = await result;
                if (!t.Success)
                    return t.Errors;

                Result<TR4> r4 = await method(t.Value.Identifier);


                if (t.Success && r4.Success)
                    return Result.Success(new Combiner<T, (TR1, TR2, TR3, TR4)>(t.Value.Identifier, (t.Value.Value.Item1, t.Value.Value.Item2, t.Value.Value.Item3, r4.Value)));

                if (!r4.Success)
                    return r4.Errors;

                return Result.Failure<Combiner<T, (TR1, TR2, TR3, TR4)>>(t.Errors.Concat(r4.Errors).ToImmutableArray());
            }
            catch (Exception e)
            {
                ExceptionDispatchInfo.Capture(e).Throw();
                throw;
            }
        }
    }
}