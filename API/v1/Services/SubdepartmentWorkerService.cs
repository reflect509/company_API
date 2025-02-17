using API.v1.Models;
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
                        SELECT subdepartment_id, subdepartment_name, description, parent_id,
			                    ARRAY[subdepartment_id] AS path
                        FROM subdepartments
                        WHERE parent_id IS NULL

                        UNION ALL

                        SELECT s.subdepartment_id, s.subdepartment_name, s.description, s.parent_id,
			                    rs.path || s.subdepartment_id AS path
                        FROM subdepartments s
                        INNER JOIN recursive_subdepartments rs ON s.parent_id = rs.subdepartment_id
	                    WHERE NOT s.subdepartment_id = ANY (rs.path)
                        )
                        SELECT *
                        FROM recursive_subdepartments
                        ORDER BY subdepartment_id";
            return await context.Subdepartments
                .FromSqlRaw(query)
                .ToListAsync();
        }
    }
}
