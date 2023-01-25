using GameInventory.Dto;
using MediatR;

namespace GameInventory.Queries
{
    public class GetItemListQuery
    {
        public class Request : IRequest<Response>
        {
            public Guid UserId { get; set; }
        }

        public class Response
        {
            public List<InventoryItem> InventoryItems { get; set; }
        }
    }
}
