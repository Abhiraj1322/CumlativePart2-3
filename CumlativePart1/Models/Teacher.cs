using Microsoft.AspNetCore.Mvc;

namespace CumlativePart1.Models
{
    public class Teacher
    {
        public int TeacherId { get; set; }

        public string? TeacherFName { get; set; }

        public string? TeacherLName { get; set; }

        public DateTime TeacherHireDate { get; set; }

        public Decimal? TeacherSalary { get; set; }

        public string? TeacherEmpNu { get; set; }

        public List<string> CourseNames { get; set; } = new List<string>();
    }
}
