using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using HumanErrorProject.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace HumanErrorProject.Data.DataAccess.Repositories
{
    public class StudentRepository : Repository<Student, string>
    {
        public StudentRepository(HumanErrorProjectContext context) : base(context)
        {
        }

        public override async Task<Student> SingleOrDefault(Expression<Func<Student, bool>> predicate)
        {
            var student = await Entities.SingleOrDefaultAsync(predicate);
            if (student == null) return null;

            Context.Entry(student).Collection(x => x.StudentCourseClasses).Query()
                .Include(x => x.Class).ThenInclude(x => x.Assignments).ThenInclude(x => x.Solution)
                .ThenInclude(x => x.MethodDeclarations)
                .Include(x => x.Class).ThenInclude(x => x.Assignments).ThenInclude(x => x.TestProject)
                .ThenInclude(x => x.UnitTests)
                .Load();

            Context.Entry(student).Collection(x => x.Snapshots).Query()
                .Include(x => x.Assignment)
                .Include(x => x.SnapshotSubmission)
                .Load();

            Context.Entry(student).Collection(x => x.Submissions).Load();

            return student;
        }
    }
}
