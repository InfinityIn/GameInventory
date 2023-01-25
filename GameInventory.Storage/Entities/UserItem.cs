using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameInventory.Storage.Entities
{
    public class UserItem
    {
        public long Id { get; set; }
        public Guid UserId { get; set; }
        public long ItemId { get; set; }

        public int Amount { get; set; }
    }
}
