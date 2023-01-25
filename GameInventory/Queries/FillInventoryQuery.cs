using GameInventory.Dto;
using MediatR;

namespace GameInventory.Queries
{
    public class FillInventoryQuery
    {
        public class Request : IRequest<Response>
        {
            public Guid UserId { get; set; }
            public int Count { get; set; }            
        }

        public class Response
        {           
        }
    }
}
