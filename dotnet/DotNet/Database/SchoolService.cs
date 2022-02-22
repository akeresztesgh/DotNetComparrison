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
        Task CallFuncOnEnrollment(Func<Enrollment, Task> func);
        Task<List<Enrollment>> GetEnrollments(List<long> enrollmentIds);
        Task<List<Enrollment>> GetEnrollmentsSplit(List<long> enrollmentIds);
    }

    public class SchoolService : ISchoolService
    {
        private readonly SchoolContext db;
        private readonly Func<SchoolContext> dbFactory;

        public SchoolService(SchoolContext db, Func<SchoolContext> dbFactory)
        {
            this.db = db;
            this.dbFactory = dbFactory;
        }

        public async Task<int> GetEnrollmentCount()
        {
            return await db.Enrollments.CountAsync();
        }

        // Never do this with large datasets!  (think more than 1k items)
        public async Task<List<Enrollment>> GetAllEnrollments()
        {
            var result = await db.Enrollments.Include(x => x.Course)
                .AsNoTracking()
                .Include(x => x.Student)
                .OrderBy(x => x.EnrollmentID)
                .ToListAsync();
            return result;
        }

        public async Task<List<Enrollment>> GetEnrollments(int start, int count)
        {
            var result = await db.Enrollments.AsNoTracking()
                .Include(x => x.Course)
                .Include(x => x.Student)
                .OrderBy(x => x.EnrollmentID)
                .Skip(start)
                .Take(count)
                .ToListAsync();
            return result;
        }

        public async Task CallFuncOnEnrollment(Func<Enrollment, Task> func)
        {
            foreach (var enrollment in db.Enrollments.AsNoTracking()
                .Include(x => x.Course)
                .Include(x => x.Student)
                .OrderBy(x => x.EnrollmentID))
            {
                await func(enrollment);
            }
        }

        public async Task<List<Enrollment>> GetEnrollments(List<long> enrollmentIds)
        {
            var result = await db.Enrollments.AsNoTracking()
                .Include(x => x.Course)
                .Include(x => x.Student)
                .Where(x => enrollmentIds.Contains(x.EnrollmentID))
                .OrderBy(x => x.EnrollmentID)
                .ToListAsync();
            return result;
        }

    public async Task<List<Enrollment>> GetEnrollmentsSplit(List<long> enrollmentIds)
    {
        var tasks = new List<Task<List<Enrollment>>>();
        var tsk = (Func<List<long>, Task<List<Enrollment>>>)(async (enrollmentIdsSplit) =>
        {
            using (var dbc = dbFactory())
            {
                var results = await dbc.Enrollments.AsNoTracking()
                .Include(x => x.Course)
                .Include(x => x.Student)
                .Where(x => enrollmentIdsSplit.Contains(x.EnrollmentID))
                .OrderBy(x => x.EnrollmentID)
                .ToListAsync();
                return results;
            }
        });

        foreach (var enrollmentIdsSplit in enrollmentIds.Split())
        {
            tasks.Add(tsk(enrollmentIdsSplit));
        }
        await Task.WhenAll(tasks);
        return tasks.SelectMany(x => x.Result).ToList();
    }
}

    public static class ListExtensions
    {
        public static IEnumerable<List<T>> Split<T>(this List<T> source, int size = 250)
        {
            for (int i = 0; i < source.Count; i += size)
            {
                yield return source.GetRange(i, Math.Min(size, source.Count - i));
            }
        }
    }

}
