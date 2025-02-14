using API.v1.Models;

namespace API.v1.Services
{
    public interface ISubdepartmentWorkerService
    {
        public Task<List<Subdepartment>> GetSubdepartmentsHierarchyAsync();
    }
}
