using GameInventory.Dto;
using GameInventory.Storage;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GameInventory.Queries
{
    public class GetItemListQueryHandler : IRequestHandler<GetItemListQuery.Request, GetItemListQuery.Response>
    {
        readonly StorageContext _db;
        public GetItemListQueryHandler(StorageContext db) => _db = db;
        public async Task<GetItemListQuery.Response> Handle(GetItemListQuery.Request request, CancellationToken cancellationToken)
        {
            var query = from userItem in _db.UserItems
                          join item in _db.Items on userItem.ItemId equals item.Id
                          where userItem.UserId == request.UserId
                          select new InventoryItem() { ItemName = item.Name, Amount = userItem.Amount};

            var items = await query.ToListAsync();            

            if (items.Any())
                return new GetItemListQuery.Response() { InventoryItems = items };

            return new GetItemListQuery.Response() { InventoryItems = new List<InventoryItem>() };
        }
    }
}
