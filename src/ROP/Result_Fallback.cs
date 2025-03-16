using System;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;

namespace ROP
{
    /// <summary>
    /// Provides extension methods for handling fallback logic in result chains.
    /// </summary>
    public static class Result_Fallback
    {
        /// <summary>
        /// The method gets executed IF the chain is in Error state and the criteria of de Predicate is met,
        /// the previous information will be lost
        /// </summary>
        /// <returns>
        /// The original successful <see cref="Result{T}"/> if it is successful; 
        /// otherwise, the result of executing <paramref name="method"/> if the condition is met; 
        /// otherwise, the original unsuccessful <see cref="Result{T}"/>.
        /// </returns>
        public static Result<T> Fallback<T>(this Result<T> r, Predicate<Result<T>> condition, Func<T, Result<T>> method)
        {
            try
            {
                if (r.Success) return r.Value;

                if (!r.Success && condition(r)) return method(r.Value);

                return Result.Failure<T>(r.Errors, r.HttpStatusCode);
            }
            catch (Exception e)
            {
                ExceptionDispatchInfo.Capture(e).Throw();
                throw;
            }
        }

        /// <summary>
        /// The method gets executed IF the chain is in Error state and the criteria of de Predicate is met,
        /// the previous information will be lost
        /// </summary>
        /// <returns>
        /// The original successful <see cref="Result{T}"/> if it is successful; 
        /// otherwise, the result of executing <paramref name="method"/> if the condition is met; 
        /// otherwise, the original unsuccessful <see cref="Result{T}"/>.
        /// </returns>
        public static async Task<Result<T>> Fallback<T>(this Task<Result<T>> r, Predicate<Result<T>> condition, Func<T, Task<Result<T>>> method)
        {
            try
            {
                var result = await r;
                if (result.Success) return result.Value;

                if (!result.Success && condition(result)) return await method(result.Value);

                return Result.Failure<T>(result.Errors, result.HttpStatusCode);
            }
            catch (Exception e)
            {
                ExceptionDispatchInfo.Capture(e).Throw();
                throw;
            }
        }

        /// <summary>
        /// The method gets executed IF the chain is in Error state and the criteria of de Predicate is met,
        /// the previous information will be lost
        /// </summary>
        /// <returns>
        /// The original successful <see cref="Result{T}"/> if it is successful; 
        /// otherwise, the result of executing <paramref name="method"/> if the condition is met; 
        /// otherwise, the original unsuccessful <see cref="Result{T}"/>.
        /// </returns>
        public static async Task<Result<T>> Fallback<T>(this Task<Result<T>> r, Predicate<Result<T>> condition, Func<T, Result<T>> method)
        {
            try
            {
                var result = await r;
                if (result.Success) return result.Value;

                if (!result.Success && condition(result)) return method(result.Value);

                return Result.Failure<T>(result.Errors, result.HttpStatusCode);
            }
            catch (Exception e)
            {
                ExceptionDispatchInfo.Capture(e).Throw();
                throw;
            }
        }

        /// <summary>
        /// The method gets executed IF the chain is in Error state,
        /// the previous information will be lost
        /// </summary>
        /// <returns>The original result if successful; otherwise, the result of the fallback method.</returns>
        public static Result<T> Fallback<T>(this Result<T> r, Func<T, Result<T>> method)
        {
            try
            {
                return r.Fallback(_ => true, method);
            }
            catch (Exception e)
            {
                ExceptionDispatchInfo.Capture(e).Throw();
                throw;
            }
        }

        /// <summary>
        /// The method gets executed IF the chain is in Error state,
        /// the previous information will be lost
        /// </summary>
        /// <returns>The original result if successful; otherwise, the result of the fallback method.</returns>
        public static async Task<Result<T>> Fallback<T>(this Task<Result<T>> r, Func<T, Task<Result<T>>> method)
        {
            try
            {
                return await r.Fallback(_ => true, method);
            }
            catch (Exception e)
            {
                ExceptionDispatchInfo.Capture(e).Throw();
                throw;
            }
        }

        /// <summary>
        /// The method gets executed IF the chain is in Error state,
        /// the previous information will be lost
        /// </summary>
        /// <returns>The original result if successful; otherwise, the result of the fallback method.</returns>
        public static async Task<Result<T>> Fallback<T>(this Task<Result<T>> r, Func<T, Result<T>> method)
        {
            try
            {
                return await r.Fallback(_ => true, method);
            }
            catch (Exception e)
            {
                ExceptionDispatchInfo.Capture(e).Throw();
                throw;
            }
        }
    }
}
