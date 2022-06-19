using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TugceErciyesProject.Models;
using Microsoft.EntityFrameworkCore;
namespace TugceErciyesProject
{
    public static class SeedData
    {
        public static async Task CreateDbAndInsertAsync(Context context)
        {
            if ((await context.Database.GetPendingMigrationsAsync().ConfigureAwait(false)).Any())
                await context.Database.MigrateAsync().ConfigureAwait(false);

            if(!await context.Courses.AnyAsync().ConfigureAwait(false))
            {
                await context.Courses.AddRangeAsync(
                    new CourseModel
                    {
                        CourseCode = "SE101",
                        CourseName = "Introduction to Software Engineering",
                        ECTSCredits = 4,
                        LetterGrade = "BB",
                        Term = TermEnum.Fall
                    },
                    new CourseModel
                    {
                        CourseCode = "COME102",
                        CourseName = "Introductiong to algorithms and programming",
                        ECTSCredits = 4,
                        LetterGrade = "BA",
                        Term = TermEnum.Spring
                    },
                    new CourseModel
                    {
                        CourseCode = "CHEM101",
                        CourseName = "General Chemistry",
                        ECTSCredits = 6,
                        LetterGrade = "AA",
                        Term = TermEnum.Fall
                    },
                    new CourseModel
                    {
                        CourseCode = "ENG101",
                        CourseName = "English 1",
                        ECTSCredits = 3,
                        LetterGrade = "BA",
                        Term = TermEnum.Fall
                    },
                    new CourseModel
                    {
                        CourseCode = "MATH101",
                        CourseName = "Calculus 1",
                        ECTSCredits = 6,
                        LetterGrade = "CB",
                        Term = TermEnum.Fall
                    },
                    new CourseModel
                    {
                        CourseCode = "COME104",
                        CourseName = "Discrete Mathematics",
                        ECTSCredits = 4,
                        LetterGrade = "BB",
                        Term = TermEnum.Spring
                    },
                    new CourseModel
                    {
                        CourseCode = "ENG102",
                        CourseName = "English 2",
                        ECTSCredits = 3,
                        LetterGrade = "AA",
                        Term = TermEnum.Spring
                    },
                    new CourseModel
                    {
                        CourseCode = "MATH102",
                        CourseName = "Calculus 2",
                        ECTSCredits = 6,
                        LetterGrade = "DD",
                        Term = TermEnum.Spring
                    }).ConfigureAwait(false);
                await context.SaveChangesAsync().ConfigureAwait(false);
                context.ChangeTracker.Clear();
            }
        }
    }
}
