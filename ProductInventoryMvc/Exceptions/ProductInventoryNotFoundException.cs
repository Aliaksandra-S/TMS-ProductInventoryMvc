using ProductInventoryMvc.Models;

namespace ProductInventoryMvc.Exceptions
{
    [Serializable]
    public class ProductInventoryNotFoundException : Exception
    {
        public string ExcMessage { get; }

        public ProductInventoryNotFoundException(CommandResultModel commandResult)
        {
            ExcMessage = commandResult.Message;
        }
    }
}
