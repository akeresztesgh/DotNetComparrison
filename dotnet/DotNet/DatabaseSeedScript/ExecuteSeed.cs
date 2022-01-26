using Database;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseSeedScript
{
    internal class ExecuteSeed
    {
        public const int StudentCount = 1000;
        public const int CourseCount = 1000;
        public const int EnrollmentPerStudent = 10;

        private readonly SchoolContext schoolDb;

        public ExecuteSeed()
        {
            schoolDb = new SchoolContext(Configure.DbOptions);
        }

        public async Task Go()
        {
            Console.WriteLine("Seed starting...");

            var students = new List<Student>();
            for (var i = 0; i < StudentCount; i++)
            {
                var student = new Student()
                {
                    LastName = Path.GetRandomFileName(),
                    FirstMidName = Path.GetRandomFileName(),
                    EnrollmentDate = DateTime.UtcNow
                };
                students.Add(student);
                schoolDb.Students.Add(student);
                if (i % 100 == 0)
                {
                    await schoolDb.SaveChangesAsync();
                }
            }
            await schoolDb.SaveChangesAsync();
            Console.WriteLine("Students complete");

            var courses = new List<Course>();

            for (var i = 0; i < CourseCount; i++)
            {
                var course = new Course()
                {
                    Title = Path.GetRandomFileName(),
                    Credits = i
                };
                courses.Add(course);
                schoolDb.Courses.Add(course);
                if (i % 100 == 0)
                {
                    await schoolDb.SaveChangesAsync();
                }
            }
            await schoolDb.SaveChangesAsync();
            Console.WriteLine("Courses complete");

            var rand = new Random();
            var iterCount = 1;
            foreach (var student in students)
            {
                for(var i=0; i < EnrollmentPerStudent; i++)
                {
                    var courseId = courses[rand.Next(courses.Count())].CourseID;
                    var enrollment = new Enrollment
                    {
                        CourseID = courseId,
                        StudentID = student.ID,
                        Grade = (Grade)rand.Next(5)
                    };
                    schoolDb.Enrollments.Add(enrollment);
                    if(iterCount % 100 == 0)
                    {
                        await schoolDb.SaveChangesAsync();
                    }
                }
            }
            await schoolDb.SaveChangesAsync();
            Console.WriteLine("Complete...");
        }
    }
}
