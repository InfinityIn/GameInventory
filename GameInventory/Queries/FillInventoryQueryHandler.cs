using GameInventory.Dto;
using GameInventory.Storage;
using GameInventory.Storage.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace GameInventory.Queries
{
    public class FillInventoryQueryHandler : IRequestHandler<FillInventoryQuery.Request, FillInventoryQuery.Response>
    {
        readonly StorageContext _db;
        public FillInventoryQueryHandler(StorageContext db) => _db = db;
        public async Task<FillInventoryQuery.Response> Handle(FillInventoryQuery.Request request, CancellationToken cancellationToken)
        {
            var lines = File.ReadAllLines(@"test_items_ids.txt");

            var query = from userItem in _db.UserItems
                        join item in _db.Items on userItem.ItemId equals item.Id
                        where userItem.UserId == request.UserId
                        select new InventoryItem() { ItemName = item.Name, Amount = userItem.Amount };

            var items = await query.ToDictionaryAsync(x => x.ItemName, y => y.Amount);

            var itemsToAdd = new Dictionary<string, int>();
            foreach(var line in lines)
            {
                var amount = items.TryGetValue(line, out int value) ? value : 0;
                itemsToAdd.Add(line, amount);
            }

            var maxValue = itemsToAdd.Max(x => x.Value);
            if (maxValue == 0) maxValue = 1;
            for (int i = 1; i <= request.Count; i++)
            {
                if (!itemsToAdd.Any(x => x.Value < maxValue))
                    maxValue++;
                AddRandomItem(ref itemsToAdd, ref items, maxValue);
            }
            await UpdateDataBase(items, request.UserId);

            return new FillInventoryQuery.Response() { };
        }

        private void AddRandomItem(ref Dictionary<string, int> itemsToAdd, ref Dictionary<string, int> items, int maxValue)
        {
            var random = new Random();

            var canAddList = itemsToAdd.Where(x => x.Value < maxValue).ToDictionary(x => x.Key, y => y.Value);
            var keysCanAddList = canAddList.Keys.ToArray();
            var randomNumber = random.Next(0, keysCanAddList.Length);
            var randomKey = keysCanAddList[randomNumber];
            var randomValue = canAddList[randomKey];
            if (randomValue > 0)
                items[randomKey] += 1;
            else
                items.Add(randomKey, randomValue + 1);
            itemsToAdd[randomKey] += 1;
        }

        public async Task UpdateDataBase(Dictionary<string, int> items, Guid userID)
        {
            foreach(var item in items)
            {
                var dbItem = await _db.Items
                    .SingleOrDefaultAsync(x => x.Name == item.Key);
                if (dbItem == null)
                {
                    dbItem = new Item()
                    {
                        Name = item.Key
                    };
                    _db.Add(dbItem);
                    await _db.SaveChangesAsync();
                    var dbUserItem = new UserItem()
                    {
                        ItemId = dbItem.Id,
                        UserId = userID,
                        Amount = item.Value,
                    };
                    _db.Add(dbUserItem);
                }
                else
                {
                    var dbUserItem = await _db.UserItems
                        .SingleOrDefaultAsync(x => x.ItemId == dbItem.Id && x.UserId == userID);
                    if (dbUserItem != null)
                    {
                        dbUserItem.Amount = item.Value;
                        _db.Update(dbUserItem);
                    }
                    else
                    {
                        dbUserItem = new UserItem()
                        {
                            ItemId = dbItem.Id,
                            UserId = userID,
                            Amount = item.Value,
                        };
                        _db.Add(dbUserItem);
                    }
                }
                
            }
            await _db.SaveChangesAsync();
        }
    }
}
