using System;
using System.Collections.Generic;
using NLog;

namespace Xeo.Baq.Exceptions
{
    public interface IGenericExceptionHandler : IExceptionHandler
    {
        IExceptionHandlerChainBuilder Handle(Action<IExceptionHandlerAction> handledAction, string actionDescription);

        IExceptionHandlerChainBuilder Handle(Action<IExceptionHandlerAction, Exception> handledAction,
            string actionDescription,
            Exception exception);
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

        public IExceptionHandlerChainBuilder Handle(Action<IExceptionHandlerAction> handledAction, string actionDescription)
        {
            var action = new ExceptionHandlerAction();

            try
            {
                handledAction(action);

                _exceptionHandlerChainBuilder.SendSuccess();
            }
            catch (Exception ex)
            {
                Catch(ex, action);
            }

            return _exceptionHandlerChainBuilder;
        }

        public IExceptionHandlerChainBuilder Handle(Action<IExceptionHandlerAction, Exception> handledAction,
            string actionDescription,
            Exception exception)
        {
            var action = new ExceptionHandlerAction();

            try
            {
                handledAction(action, exception);

                _exceptionHandlerChainBuilder.SendSuccess();
            }
            catch (Exception ex)
            {
                Catch(ex, action);
            }

            return _exceptionHandlerChainBuilder;
        }

        IExceptionHandlerChainBuilder IExceptionHandler.Handle(Action<IExceptionHandlerAction> handledAction)
            => Handle(handledAction, "Generic action");

        public IExceptionHandlerChainBuilder Handle(Action<IExceptionHandlerAction, Exception> handledAction, Exception exception)
            => Handle(handledAction, "Generic action", exception);

        public IEnumerable<Exception> GetCatchedExceptions()
            => _catchedList;

        private void Catch(Exception ex, ExceptionHandlerAction action)
        {
            _catchedList.Add(ex);
            _logger.Error(ex, $"Error occured during the \"{action.Description}\" operation. Exception: {ex}");

            _exceptionHandlerChainBuilder.SendException(ex);
        }
    }
}