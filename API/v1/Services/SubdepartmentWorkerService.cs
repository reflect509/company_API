using API.v1.Models;
using API.v1.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace API.v1.Services
{
    public class SubdepartmentWorkerService : ISubdepartmentWorkerService
    {
        private readonly ApiDbContext context;

        public SubdepartmentWorkerService(ApiDbContext context)
        {
            this.context = context;
        }

        public async Task<List<Subdepartment>> GetSubdepartmentsHierarchyAsync()
        {
            var query = @"WITH RECURSIVE recursive_subdepartments AS (
                        SELECT subdepartment_id, subdepartment_name, description, parent_id
			                    
                        FROM subdepartments
                        WHERE parent_id IS NULL

                        UNION ALL

                        SELECT s.subdepartment_id, s.subdepartment_name, s.description, s.parent_id
                        FROM subdepartments s
                        INNER JOIN recursive_subdepartments rs ON s.parent_id = rs.subdepartment_id
                        )
                        SELECT *
                        FROM recursive_subdepartments
                        ORDER BY subdepartment_id";

            return await context.Subdepartments
                .FromSqlRaw(query)
                .Include(s => s.Workers)
                .ToListAsync();
        }
    }
}
