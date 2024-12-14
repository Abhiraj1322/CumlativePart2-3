using CumlativePart1.Models;
using Microsoft.AspNetCore.Mvc;

namespace CumlativePart1.Models
{
    public class TeacherCoursesViewModel : Controller
    {

        public Teacher Teacher { get; set; }
        public List<string> Courses { get; set; }
    }
}