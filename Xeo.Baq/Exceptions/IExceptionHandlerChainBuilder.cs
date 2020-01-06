using System;
using Xeo.Baq.Extensions;

namespace Xeo.Baq.Exceptions
{
    public interface IExceptionHandlerChainBuilder
    {
        void SendSuccess();
        void SendException(Exception ex);
        IExceptionHandlerChainBuilder ContinueOnSuccess(Action action);
        IExceptionHandlerChainBuilder ContinueOnException(Action<Exception> action);
    }

    public class ExceptionHandlerChainBuilder : IExceptionHandlerChainBuilder
    {
        private readonly IExceptionHandler _exceptionHandler;
        private Exception _lastException;

        public ExceptionHandlerChainBuilder(IExceptionHandler exceptionHandler)
            => _exceptionHandler = exceptionHandler;

        public void SendSuccess()
            => _lastException = null;

        public void SendException(Exception ex)
            => _lastException = ex;

        public IExceptionHandlerChainBuilder ContinueOnSuccess(Action action)
            => this.ReturnThis(() =>
            {
                if (_lastException == null)
                {
                    _exceptionHandler.Handle(action);
                }
            });

        public IExceptionHandlerChainBuilder ContinueOnException(Action<Exception> action)
            => this.ReturnThis(() =>
            {
                if (_lastException != null)
                {
                    _exceptionHandler.Handle(action, _lastException);
                }
            });
    }
}