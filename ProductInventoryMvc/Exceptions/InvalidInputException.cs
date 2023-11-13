using ProductInventoryMvc.Models;

namespace ProductInventoryMvc.Exceptions
{
    [Serializable]
    public class InvalidInputException : Exception
    {
        public string ExcMessage { get; }

        public InvalidInputException(CommandResultModel commandResult)
        {
            ExcMessage = commandResult.Message;
        }

        //public InvalidInputException(string? message) : base(message)
        //{
        //}

        //public InvalidInputException(string? message, Exception? innerException) : base(message, innerException)
        //{
        //}
        //public InvalidInputException(string? message, CommandResultModel commandResult): base(message)
        //{
            
        //}
    }
}
