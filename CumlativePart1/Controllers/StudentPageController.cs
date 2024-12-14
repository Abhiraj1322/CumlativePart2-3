
using Microsoft.AspNetCore.Mvc;
using CumlativePart1.Models;

namespace CumlativePart1.Controllers
{

    public class StudentPageController : Controller
    {
        private readonly StudentAPIController _api;

        public StudentPageController(StudentAPIController api)
        {
            _api = api;
        }
        public IActionResult List()
        {
            List<Student> Students = _api.ListStudent();
            return View(Students);
        }

        public IActionResult Show(int id)
        {
            Student SelectedStudent = _api.FindStudent(id);
            return View(SelectedStudent);
        }
        // GET : StudentPage/New
        [HttpGet]
        public IActionResult New(int id)
        {
            return View();
        }

        // POST: StudentPage/Create
        [HttpPost]
        public IActionResult Create(Student NewStudent)
        {
            int StudentId = _api.AddStudent(NewStudent);

            // redirects to "Show" action on "Student" cotroller with id parameter supplied
            return RedirectToAction("Show", new { id =StudentId});
        }

        // GET : StudentPage/DeleteConfirm/{id}
        [HttpGet]
        public IActionResult DeleteConfirm(int id)
        {
            Student SelectedStudent = _api.FindStudent(id);
            return View(SelectedStudent);
        }

        // POST: StudentPage/Delete/{id}
        [HttpPost]
        public IActionResult Delete(int id)
        {
            int AuthorId = _api.DeleteStudent(id);
            // redirects to list action
            return RedirectToAction("List");
        }

        public IActionResult Edit(int id)
        {
            // Find the selected student by ID
            Student SelectedStudent = _api.FindStudent(id);
            return View(SelectedStudent);  // Pass the student data to the Edit view
        }
        [HttpPost]
        public IActionResult Update(int id, string StudentFName, string StudentLName, DateTime EnrollDate, string StudentNumber)
        {
            // Create a new Student object to store the updated values
            Student UpdatedStudent = new Student();
            UpdatedStudent.StudentFName = StudentFName;
            UpdatedStudent.StudentLName = StudentLName;
            UpdatedStudent.EnrollDate = EnrollDate;
            UpdatedStudent.StudentNumber = StudentNumber;

            // Call the API to update the student in the database
            _api.UpdateStudent(id, UpdatedStudent);

            // Redirect to the Show page to display the updated student details
            return RedirectToAction("Show", new { id = id });
        }

    }

}
