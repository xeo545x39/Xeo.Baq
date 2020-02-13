using System;
using Xeo.Baq.Extensions;

namespace Xeo.Baq.Exceptions
{
    public interface IExceptionHandlerChainBuilder
    {
        void SendSuccess();
        void SendException(Exception ex);
        IExceptionHandler Finish();
        IExceptionHandlerChainBuilder ContinueOnSuccess(Action<IExceptionHandlerAction> action);
        IExceptionHandlerChainBuilder ContinueOnSuccessWhen(Func<bool> predicate, Action<IExceptionHandlerAction> action);
        IExceptionHandlerChainBuilder ContinueOnException(Action<IExceptionHandlerAction, Exception> action);
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

        public IExceptionHandler Finish()
            => _exceptionHandler;

        public IExceptionHandlerChainBuilder ContinueOnSuccess(Action<IExceptionHandlerAction> action)
            => this.ReturnThis(() =>
            {
                if (_lastException == null)
                {
                    _exceptionHandler.Handle(action);
                }
            });

        public IExceptionHandlerChainBuilder ContinueOnSuccessWhen(Func<bool> predicate, Action<IExceptionHandlerAction> action)
            => this.ReturnThis(() =>
            {
                if (_lastException == null && predicate())
                {
                    _exceptionHandler.Handle(action);
                }
            });

        public IExceptionHandlerChainBuilder ContinueOnException(Action<IExceptionHandlerAction, Exception> action)
            => this.ReturnThis(() =>
            {
                if (_lastException != null)
                {
                    _exceptionHandler.Handle(action, _lastException);
                }
            });
    }
}