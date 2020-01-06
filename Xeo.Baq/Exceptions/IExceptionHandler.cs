using System;
using System.Collections.Generic;

namespace Xeo.Baq.Exceptions
{
    public interface IExceptionHandler
    {
        IExceptionHandlerChainBuilder Handle(Action<IExceptionHandlerAction> handledAction);
        IExceptionHandlerChainBuilder Handle(Action<IExceptionHandlerAction, Exception> handledAction, Exception exception);
        IEnumerable<Exception> GetCatchedExceptions();
    }
}