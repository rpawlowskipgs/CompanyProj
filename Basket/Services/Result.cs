using System;
using Basket.Models;

namespace Basket.Services
{
    public class Result
    {
        public Status Status { get; }
        public Exception BasketError { get; private set; }
        public bool IsFailure => Status != Status.Ok;

        protected Result(Status status, Exception basketError)
        {
            Status = status;
            BasketError = basketError;
        }

        public static Result Fail(Status status, Exception message = null)
        {
            return new Result(status, message);
        }

        public static Result<T> Fail<T>(Status status, Exception message = null)
        {
            return new Result<T>(default(T), status, message);
        }

        public static Result Ok()
        {
            return new Result(Status.Ok, null);
        }

        public static Result<T> Ok<T>(T value)
        {
            return new Result<T>(value, Status.Ok, null);
        }
    }

    public class Result<T> : Result
    {
        private readonly T _value;

        public T Value
        {
            get
            {
                if(IsFailure)
                    throw new InvalidOperationException();
                return _value;
            }
        }

        protected internal Result(T value, Status status, Exception basketError) : base(status, basketError)
        {
            _value = value;
        }
    }
}
