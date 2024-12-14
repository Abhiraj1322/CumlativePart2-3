using Microsoft.AspNetCore.Mvc;

namespace CumlativePart1.Models
{
    public class Course
    {
        public int courseId { get; set; }

        public string? coursecode { get; set; }

        public int teacherid { get; set; }

        public DateTime startdate { get; set; }
        public DateTime finishdate { get; set; }

        public string? coursename { get; set; }

    }
}
