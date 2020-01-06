using System;
using System.Collections.Generic;

namespace Xeo.Baq.Exceptions
{
    public interface IExceptionHandler
    {
        IExceptionHandlerChainBuilder Handle(Action handledAction);
        IExceptionHandlerChainBuilder Handle(Action<Exception> handledAction, Exception exception);
        IEnumerable<Exception> GetCatchedExceptions();
    }
}