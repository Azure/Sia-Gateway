﻿namespace Sia.Shared.Exceptions
{
    public class ConflictException : BaseException
    {
        public ConflictException(string message) : base(message)
        {
        }

        public override int StatusCode => 409;
    }
}
