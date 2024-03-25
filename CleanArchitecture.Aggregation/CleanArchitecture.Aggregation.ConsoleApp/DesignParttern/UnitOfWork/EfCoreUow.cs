using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Aggregation.ConsoleApp.DesignParttern.UnitOfWork
{
    /// <summary>
    /// EF Core đã tích hợp sẵn Unit of Work Pattern thông qua DbContext và các phương thức SaveChanges(), 
    /// SaveChangesAsync() để quản lý các transaction và thực hiện các thao tác CRUD trên cơ sở dữ liệu
    /// </summary>
    internal class EfCoreUow
    {
    }
}
