using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Unit of Work Pattern là một design pattern quan trọng được sử dụng để quản lý các transaction và tập hợp các hoạt động CRUD (Create, Read, Update, Delete) trên các đối tượng trong cơ sở dữ liệu. 
/// Unit of Work Pattern giúp đảm bảo tính nhất quán và quản lý tốt cho các thao tác dữ liệu.
/// </summary>
namespace CleanArchitecture.Aggregation.ConsoleApp.DesignParttern.UnitOfWork
{
    /// <summary>
    /// Trong cách triển khai này, bạn có thể tạo một lớp UnitOfWork chứa các phương thức để quản lý các hoạt động CRUD trên các đối tượng của cơ sở dữ liệu.
    /// UnitOfWork thường kết hợp với Repository Pattern để quản lý việc truy cập dữ liệu và thực hiện các thao tác dữ liệu cần thiết.
    /// </summary>
    internal class BasicUow
    {
    }
}
