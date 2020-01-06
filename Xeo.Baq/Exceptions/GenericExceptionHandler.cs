using System;
using System.Collections.Generic;
using NLog;

namespace Xeo.Baq.Exceptions
{
    public interface IGenericExceptionHandler : IExceptionHandler
    {
        IExceptionHandlerChainBuilder Handle(Action handledAction, string actionDescription);
        IExceptionHandlerChainBuilder Handle(Action<Exception> handledAction, string actionDescription, Exception exception);
    }

    public class GenericExceptionHandler : IGenericExceptionHandler
    {
        private readonly ICollection<Exception> _catchedList;
        private readonly IExceptionHandlerChainBuilder _exceptionHandlerChainBuilder;
        private readonly ILogger _logger;

        public GenericExceptionHandler(ILogger logger,
            Func<IExceptionHandler, IExceptionHandlerChainBuilder> exceptionHandlerChainBuilderFactory)
        {
            _logger = logger;
            _catchedList = new List<Exception>();
            _exceptionHandlerChainBuilder = exceptionHandlerChainBuilderFactory(this);
        }

        public IExceptionHandlerChainBuilder Handle(Action handledAction, string actionDescription)
        {
            try
            {
                handledAction();

                _exceptionHandlerChainBuilder.SendSuccess();
            }
            catch (Exception ex)
            {
                _catchedList.Add(ex);
                _logger.Error(ex, $"Error occured during the '{actionDescription}' operation.");

                _exceptionHandlerChainBuilder.SendException(ex);
            }

            return _exceptionHandlerChainBuilder;
        }

        public IExceptionHandlerChainBuilder Handle(Action<Exception> handledAction, string actionDescription, Exception exception)
        {
            try
            {
                handledAction(exception);

                _exceptionHandlerChainBuilder.SendSuccess();
            }
            catch (Exception ex)
            {
                _catchedList.Add(ex);
                _logger.Error(ex, $"Error occured during the '{actionDescription}' operation.");

                _exceptionHandlerChainBuilder.SendException(ex);
            }

            return _exceptionHandlerChainBuilder;
        }


        IExceptionHandlerChainBuilder IExceptionHandler.Handle(Action handledAction)
            => Handle(handledAction, "Generic action");

        public IExceptionHandlerChainBuilder Handle(Action<Exception> handledAction, Exception exception)
            => Handle(handledAction, "Generic action", exception);

        public IEnumerable<Exception> GetCatchedExceptions()
            => _catchedList;
    }
}