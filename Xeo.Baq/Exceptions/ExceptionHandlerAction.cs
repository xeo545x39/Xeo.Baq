using System;
using Xeo.Baq.Extensions;

namespace Xeo.Baq.Exceptions
{
    public interface IExceptionHandlerAction
    {
        IExceptionHandlerAction Describe(string description);
        IExceptionHandlerAction Successable();
        IExceptionHandlerAction NotSuccessable();
    }

    public class ExceptionHandlerAction : IExceptionHandlerAction
    {
        public ExceptionHandlerAction()
            => ShouldSendSuccess = true;

        public string Description { get; private set; }
        public bool ShouldSendSuccess { get; private set; }

        public IExceptionHandlerAction Describe(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                throw new ArgumentException("Invalid action description.", nameof(description));
            }

            Description = description;

            return this;
        }

        public IExceptionHandlerAction Successable()
            => this.ReturnThis(() => ShouldSendSuccess = true);

        public IExceptionHandlerAction NotSuccessable()
            => this.ReturnThis(() => ShouldSendSuccess = false);
    }
}