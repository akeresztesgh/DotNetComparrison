using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    public interface ISchoolService
    {
        Task<int> GetEnrollmentCount();
        Task<List<Enrollment>> GetEnrollments(int from, int count);
    }

    public class SchoolService : ISchoolService
    {
        private readonly SchoolContext db;

        public SchoolService(SchoolContext db)
        {
            this.db = db;
        }

        public async Task<int> GetEnrollmentCount()
        {
            return await db.Enrollments.CountAsync();
        }

        public async Task<List<Enrollment>> GetEnrollments(int from, int count)
        {
            var result = await db.Enrollments.Include(x => x.Course)
                .AsNoTracking()
                .Include(x => x.Student)
                .OrderBy(x => x.EnrollmentID)
                .Skip(from)
                .Take(count)
                .ToListAsync();
            return result;
        }

    }
}
