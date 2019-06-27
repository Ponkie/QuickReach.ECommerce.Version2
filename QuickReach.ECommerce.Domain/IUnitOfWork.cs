using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace QuickReach.ECommerce.Domain
{
    public interface IUnitOfWork
    {
        void Save();

    }
}
