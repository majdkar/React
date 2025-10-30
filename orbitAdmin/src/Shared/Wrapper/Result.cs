using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolV01.Shared.Wrapper
{
    public class Result : IResult
    {
        public Result()
        {
        }

        public List<string> Messages { get; set; } = [];

        public bool Succeeded { get; set; }

        public static IResult Fail() => new Result { Succeeded = false };
        public static IResult Fail(string message) => new Result { Succeeded = false, Messages = [message] };
        public static IResult Fail(List<string> messages) => new Result { Succeeded = false, Messages = messages };
        public static Task<IResult> FailAsync() => Task.FromResult(Fail());
        public static Task<IResult> FailAsync(string message) => Task.FromResult(Fail(message));
        public static Task<IResult> FailAsync(List<string> messages) => Task.FromResult(Fail(messages));

        public static IResult Success() => new Result { Succeeded = true };
        public static IResult Success(string message) => new Result { Succeeded = true, Messages = [message] };

        public static Task<IResult> SuccessAsync()
        {
            return Task.FromResult(Success());
        }

        public static Task<IResult> SuccessAsync(string message)
        {
            return Task.FromResult(Success(message));
        }

    }

    public class Result<T> : Result, IResult<T>
    {
        protected internal Result(T data) =>Data = data;
        public Result()
        {
        }

        public T Data { get; set; }

        public static new Result<T> Fail() => new() { Succeeded = false };

        public static new Result<T> Fail(string message) => new() { Succeeded = false, Messages = [message] };

        public static new Result<T> Fail(List<string> messages) => new() { Succeeded = false, Messages = messages };

        public static new Task<Result<T>> FailAsync() => Task.FromResult(Fail());

        public static new Task<Result<T>> FailAsync(string message) => Task.FromResult(Fail(message));

        public static new Task<Result<T>> FailAsync(List<string> messages) => Task.FromResult(Fail(messages));

        public static new Result<T> Success() => new() { Succeeded = true };
        public static new Result<T> Success(string message) => new() { Succeeded = true, Messages = [message] };
        public static Result<T> Success(T data) => new() { Succeeded = true, Data = data };
        public static Result<T> Success(T data, string message) => new() { Succeeded = true, Data = data, Messages = [message] };
        public static Result<T> Success(T data, List<string> messages) => new() { Succeeded = true, Data = data, Messages = messages };
        public static new Task<Result<T>> SuccessAsync() => Task.FromResult(Success());
        public static new Task<Result<T>> SuccessAsync(string message) => Task.FromResult(Success(message));
        public static Task<Result<T>> SuccessAsync(T data) => Task.FromResult(Success(data));
        public static Task<Result<T>> SuccessAsync(T data, string message) => Task.FromResult(Success(data, message));


        public static Result<T> Create(T date) => date is not null ? Success(date) : Fail();

        public static implicit operator Result<T>(T data) => Create(data);
    }
}