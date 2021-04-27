﻿using System.Collections.Immutable;

namespace ROP
{
    public class ResultDto<T>
    {
        public T Value { get; set; }
        public ImmutableArray<Error> Errors { get; set; }
        public bool Success => Errors.Length == 0;

        public ResultDto()
        {
            Value = default;
            Errors = ImmutableArray<Error>.Empty;
        }

        public ResultDto(T value)
        {
            Value = value;
            Errors = ImmutableArray<Error>.Empty;
        }

        public ResultDto(ImmutableArray<Error> errors)
        {
            Value = default;
            Errors = errors;
        }

    }
}
