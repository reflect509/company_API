using Desktop_app.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desktop_app.Services
{
    public interface IApiService
    {
        Task<IEnumerable<Node>> GetSubdepartmentsAsync();
        Task<bool> UpdateWorker(Worker worker);
    }
}
