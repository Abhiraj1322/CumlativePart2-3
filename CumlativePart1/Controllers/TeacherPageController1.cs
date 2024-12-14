
using Microsoft.AspNetCore.Mvc;
using CumlativePart1.Controllers;
using CumlativePart1.Models;
using Google.Protobuf.WellKnownTypes;


namespace CumlativePart1.Controllers
{
    public class TeacherPageController : Controller
    {
        private readonly TeacherAPIController _api;

        public TeacherPageController(TeacherAPIController api)
        {
            _api = api;
        }

        [HttpGet]
        public IActionResult List(DateTime? StartDate, DateTime? EndDate)
        {

            List<Teacher> Teachers = _api.ListTeachers(StartDate, EndDate);
            return View(Teachers);
        }

        [HttpGet]
        public IActionResult Show(int id)
        {

            if (id <= 0)
            {
                ViewBag.ErrorMessage = "Invalid Teacher ID. Please provide a valid ID.";
                return View("Error");
            }


            var selectedTeacher = _api.FindTeacher(id);


            if (selectedTeacher == null)
            {
                ViewBag.ErrorMessage = "The specified teacher does not exist. Please check the Teacher ID.";
                return View("Error");
            }


            var teacherCourses = _api.GetCoursesByTeacher(id);


            if (teacherCourses == null || teacherCourses.Count == 0)
            {
                ViewBag.ErrorMessage = $"No courses found for the teacher with ID {id}.";
                return View("Error");
            }

            var viewModel = new TeacherCoursesViewModel
            {
                Teacher = selectedTeacher,
                Courses = teacherCourses
            };


            return View(viewModel);
        }
        [HttpGet]
        public IActionResult New(int id)
        {
            return View();
        }
        // POST: AuthorPage/Create
        [HttpPost]
        public IActionResult Create(Teacher NewTeacher)
        {
            int TeacherId = _api.AddTeacher(NewTeacher);

            // redirects to "Show" action on "Author" cotroller with id parameter supplied
            return RedirectToAction("Show", new { id = TeacherId });
        }


        // GET : AuthorPage/DeleteConfirm/{id}
        [HttpGet]
        public IActionResult DeleteConfirm(int id)
        {
           Teacher SelectedAuthor = _api.FindTeacher(id);
            return View(SelectedAuthor);
        }

        // POST: AuthorPage/Delete/{id}
        [HttpPost]
        public IActionResult Delete(int id)
        {
            int TeacherId = _api.DeleteTeacher(id);
            // redirects to list action
            return RedirectToAction("List");
        }



        [HttpGet]
        public IActionResult Edit(int id)
        {
            Teacher SelectedTeacher = _api.FindTeacher(id);
            return View(SelectedTeacher);
        }



        [HttpPost]
       public IActionResult Update(int id, string TeacherFName, string TeacherLName, DateTime TeacherHireDate, string TeacherEmpNu, int TeacherSalary)
{
    if (TeacherSalary < 0)  // Add validation to ensure salary is non-negative
    {
        // Return an error message if the salary is invalid
        ModelState.AddModelError("TeacherSalary", "Salary cannot be negative.");
        return View();  // Or redirect to a view that handles the error
    }

    Teacher UpdatedTeacher = new Teacher
    {
        TeacherFName = TeacherFName,
        TeacherLName = TeacherLName,
        TeacherHireDate = TeacherHireDate,
        TeacherEmpNu = TeacherEmpNu,
        TeacherSalary = TeacherSalary
    };

    _api.UpdateTeacher(id, UpdatedTeacher);

    // Redirect to the Show action
    return RedirectToAction("Show", new { id = id });
}


    }



}
