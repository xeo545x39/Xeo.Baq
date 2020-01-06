namespace Xeo.Baq.Exceptions
{
    public interface IExceptionHandlerAction
    {
        void Describe(string description);
    }

    public class ExceptionHandlerAction : IExceptionHandlerAction
    {
        public string Description { get; set; }

        public void Describe(string description)
        {
            Description = description;
        }
    }
}