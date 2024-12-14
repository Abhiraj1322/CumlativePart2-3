using CumlativePart1.Controllers;
using CumlativePart1.Models;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Mvc;

namespace CumlativePart1.Controllers
{
    public class CoursePageController : Controller
    {
        private readonly CourseAPIController _api;

        public CoursePageController(CourseAPIController api)
        {
            _api = api;
        }

        public IActionResult List()
        {
            List<Course> Courses = _api.ListCourse();
            return View(Courses);
        }



        // POST: CoursePage/Create
        [HttpPost]
        public IActionResult Create(Course NewCourse)
        {
            int Courseid = _api.AddCourse(NewCourse);

            // redirects to "Show" action on "Course" cotroller with id parameter supplied
            return RedirectToAction("Show", new { id = Courseid });
        }


        [HttpGet]
        public IActionResult DeleteConfirm(int id)
        {
            Course Selectedcourse = _api.FindCourse(id);
            return View(Selectedcourse);
        }

        // POST: CoursePage/Delete/{id}
        [HttpPost]
        public IActionResult Delete(int id)
        {
            int AuthorId = _api.DeleteCourse(id);
            // redirects to list action
            return RedirectToAction("List");
        }


        [HttpGet]
        public IActionResult Edit(int id)
        {
            // Fetch the course using the API
            Course SelectedCourse = _api.FindCourse(id);
            return View(SelectedCourse);
        }





        //GET : AuthorPage/Show/{id}
        [HttpGet]
        public IActionResult Show(int id)
        {
            Course SelectedCourse = _api.FindCourse(id);
            return View(SelectedCourse);
        }
        // GET : CoursePage/New
        [HttpGet]
        public IActionResult New(int id)
        {
            return View();
        }
        [HttpPost]
        public IActionResult Update(int id, string CourseCode, int TeacherId, DateTime StartDate, DateTime FinishDate, string CourseName)
        {

            Course UpdatedCourse = new Course();

            UpdatedCourse.coursecode = CourseCode;
            UpdatedCourse.teacherid = TeacherId;
            UpdatedCourse.startdate = StartDate;
            UpdatedCourse.finishdate = FinishDate;
            UpdatedCourse.coursename = CourseName;


            // Update the course using the API (assuming _api.UpdateCourse works similarly to the Teacher example)
            _api.UpdateCourse(id, UpdatedCourse);

            // Redirect to the 'Show' action to display the updated course

            return RedirectToAction("Show", new { id = id });
        }








    }
}
