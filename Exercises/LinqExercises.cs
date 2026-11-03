
using LinqConsoleLab.EN.Data;

namespace LinqConsoleLab.EN.Exercises;

public sealed class LinqExercises
{
    // TASK 1 - Where
    // Find all students who live in Warsaw.
    public IEnumerable<string> Task01_StudentsFromWarsaw()
    {
        return UniversityData.Students
            .Where(s => s.City == "Warsaw")
            .Select(s => $"{s.IndexNumber} - {s.FirstName} {s.LastName} - {s.City}");
    }

    // TASK 2 - Select
    // Return all student email addresses.
    public IEnumerable<string> Task02_StudentEmailAddresses()
    {
        return UniversityData.Students
            .Select(s => s.Email);
    }

    // TASK 3 - OrderBy and ThenBy
    // Sort students by last name and first name.
    public IEnumerable<string> Task03_StudentsSortedAlphabetically()
    {
        return UniversityData.Students
            .OrderBy(s => s.LastName)
            .ThenBy(s => s.FirstName)
            .Select(s => $"{s.IndexNumber} - {s.FirstName} {s.LastName}");
    }

    // TASK 4 - FirstOrDefault
    // Find the first Analytics course.
    public IEnumerable<string> Task04_FirstAnalyticsCourse()
    {
        var course = UniversityData.Courses
            .FirstOrDefault(c => c.Category == "Analytics");

        return course == null
            ? new[] { "No Analytics course found." }
            : new[] { $"{course.Title} - {course.StartDate:yyyy-MM-dd}" };
    }

    // TASK 5 - Any
    // Check whether an inactive enrollment exists.
    public IEnumerable<string> Task05_IsThereAnyInactiveEnrollment()
    {
        bool exists = UniversityData.Enrollments
            .Any(e => !e.IsActive);

        return new[] { $"Inactive enrollment exists: {exists}" };
    }

    // TASK 6 - All
    // Check whether all lecturers have a department.
    public IEnumerable<string> Task06_DoAllLecturersHaveDepartment()
    {
        bool result = UniversityData.Lecturers
            .All(l => !string.IsNullOrWhiteSpace(l.Department));

        return new[] { $"All lecturers have a department: {result}" };
    }

    // TASK 7 - Count
    // Count active enrollments.
    public IEnumerable<string> Task07_CountActiveEnrollments()
    {
        int count = UniversityData.Enrollments
            .Count(e => e.IsActive);

        return new[] { $"Active enrollments: {count}" };
    }

    // TASK 8 - Distinct
    // Return unique student cities alphabetically.
    public IEnumerable<string> Task08_DistinctStudentCities()
    {
        return UniversityData.Students
            .Select(s => s.City)
            .Distinct()
            .OrderBy(city => city);
    }

    // TASK 9 - OrderByDescending and Take
    // Return the three newest enrollments.
    public IEnumerable<string> Task09_ThreeNewestEnrollments()
    {
        return UniversityData.Enrollments
            .OrderByDescending(e => e.EnrollmentDate)
            .Take(3)
            .Select(e =>
                $"{e.EnrollmentDate:yyyy-MM-dd} - Student: {e.StudentId}, Course: {e.CourseId}");
    }

    // TASK 10 - Skip and Take
    // Return the second page of courses (page size 2).
    public IEnumerable<string> Task10_SecondPageOfCourses()
    {
        return UniversityData.Courses
            .OrderBy(c => c.Title)
            .Skip(2)
            .Take(2)
            .Select(c => $"{c.Title} - {c.Category}");
    }

    // TASK 11 - Join
    // Join students with their enrollments.
    public IEnumerable<string> Task11_JoinStudentsWithEnrollments()
    {
        return UniversityData.Students
            .Join(
                UniversityData.Enrollments,
                s => s.Id,
                e => e.StudentId,
                (s, e) =>
                    $"{s.FirstName} {s.LastName} - {e.EnrollmentDate:yyyy-MM-dd}"
            );
    }

    // TASK 12 - SelectMany and Join
    // Prepare student-course pairs.
    public IEnumerable<string> Task12_StudentCoursePairs()
    {
        return UniversityData.Students
            .SelectMany(
                s => UniversityData.Enrollments
                    .Where(e => e.StudentId == s.Id),
                (s, e) => new
                {
                    Student = s,
                    Enrollment = e
                }
            )
            .Join(
                UniversityData.Courses,
                pair => pair.Enrollment.CourseId,
                c => c.Id,
                (pair, c) =>
                    $"{pair.Student.FirstName} {pair.Student.LastName} - {c.Title}"
            );
    }

    // TASK 13 - GroupBy
    // Count enrollments for each course.
    public IEnumerable<string> Task13_GroupEnrollmentsByCourse()
    {
        return UniversityData.Enrollments
            .GroupBy(e => e.CourseId)
            .Join(
                UniversityData.Courses,
                group => group.Key,
                c => c.Id,
                (group, c) =>
                    $"{c.Title} - {group.Count()} enrollments"
            );
    }

    // TASK 14 - Average
    // Calculate average final grade per course.
    public IEnumerable<string> Task14_AverageGradePerCourse()
    {
        return UniversityData.Enrollments
            .Where(e => e.FinalGrade.HasValue)
            .GroupBy(e => e.CourseId)
            .Join(
                UniversityData.Courses,
                group => group.Key,
                c => c.Id,
                (group, c) =>
                    $"{c.Title} - average grade: {group.Average(e => e.FinalGrade!.Value):F2}"
            );
    }

    // TASK 15 - GroupJoin
    // Count courses assigned to each lecturer.
    public IEnumerable<string> Task15_LecturersAndCourseCounts()
    {
        return UniversityData.Lecturers
            .GroupJoin(
                UniversityData.Courses,
                l => l.Id,
                c => c.LecturerId,
                (l, courses) =>
                    $"{l.FirstName} {l.LastName} - {courses.Count()} courses"
            );
    }

    // TASK 16 - Max
    // Find the highest final grade for each student.
    public IEnumerable<string> Task16_HighestGradePerStudent()
    {
        return UniversityData.Students
            .GroupJoin(
                UniversityData.Enrollments
                    .Where(e => e.FinalGrade.HasValue),
                s => s.Id,
                e => e.StudentId,
                (s, grades) => new
                {
                    Student = s,
                    Grades = grades
                }
            )
            .Where(x => x.Grades.Any())
            .Select(x =>
                $"{x.Student.FirstName} {x.Student.LastName} - highest grade: {x.Grades.Max(e => e.FinalGrade!.Value):F2}"
            );
    }

    // CHALLENGE 1
    // Students with more than one active course.
    public IEnumerable<string> Challenge01_StudentsWithMoreThanOneActiveCourse()
    {
        return UniversityData.Enrollments
            .Where(e => e.IsActive)
            .GroupBy(e => e.StudentId)
            .Where(group => group.Count() > 1)
            .Join(
                UniversityData.Students,
                group => group.Key,
                s => s.Id,
                (group, s) =>
                    $"{s.FirstName} {s.LastName} - {group.Count()} active courses"
            );
    }

    // CHALLENGE 2
    // April 2026 courses without final grades.
    public IEnumerable<string> Challenge02_AprilCoursesWithoutFinalGrades()
    {
        return UniversityData.Courses
            .Where(c =>
                c.StartDate.Year == 2026 &&
                c.StartDate.Month == 4)
            .GroupJoin(
                UniversityData.Enrollments,
                c => c.Id,
                e => e.CourseId,
                (c, enrollments) => new
                {
                    Course = c,
                    Enrollments = enrollments
                }
            )
            .Where(x =>
                x.Enrollments.Any() &&
                x.Enrollments.All(e => !e.FinalGrade.HasValue))
            .Select(x => x.Course.Title);
    }

    // CHALLENGE 3
    // Average final grade for every lecturer.
    public IEnumerable<string> Challenge03_LecturersAndAverageGradeAcrossTheirCourses()
    {
        return UniversityData.Lecturers
            .GroupJoin(
                UniversityData.Courses,
                l => l.Id,
                c => c.LecturerId,
                (l, courses) => new
                {
                    Lecturer = l,
                    Courses = courses
                }
            )
            .Select(x => new
            {
                x.Lecturer,
                Grades = x.Courses
                    .SelectMany(c =>
                        UniversityData.Enrollments
                            .Where(e => e.CourseId == c.Id))
                    .Where(e => e.FinalGrade.HasValue)
                    .Select(e => e.FinalGrade!.Value)
            })
            .Select(x =>
                $"{x.Lecturer.FirstName} {x.Lecturer.LastName} - average grade: " +
                (x.Grades.Any()
                    ? x.Grades.Average().ToString("F2")
                    : "no grades")
            );
    }

    // CHALLENGE 4
    // Cities and number of active enrollments.
    public IEnumerable<string> Challenge04_CitiesAndActiveEnrollmentCounts()
    {
        return UniversityData.Students
            .Join(
                UniversityData.Enrollments
                    .Where(e => e.IsActive),
                s => s.Id,
                e => e.StudentId,
                (s, e) => s.City
            )
            .GroupBy(city => city)
            .OrderByDescending(group => group.Count())
            .ThenBy(group => group.Key)
            .Select(group =>
                $"{group.Key} - {group.Count()} active enrollments"
            );
    }
}
