using CumlativePart1.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace CumlativePart1.Controllers
{
    [Route("api/Student")]
    [ApiController]
    public class StudentAPIController : ControllerBase
    {
        private readonly SchooldbContext _context;

        /// <summary>
        /// Constructor that initializes the StudentAPIController with a database context.
        /// </summary>
        /// <param name="context">The database context used for accessing the database.</param>
        public StudentAPIController(SchooldbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves a list of all students from the database.
        /// </summary>
        /// <returns>A list of Student objects containing details such as student ID, first name, last name, student number, and enrollment date.</returns>
        [HttpGet]
        [Route(template: "listStudents")]
        public List<Student> ListStudent()
        {
            // Initialize a list to store the students retrieved from the database
            List<Student> Students = new List<Student>();

            // Open a connection to the database using the context's AccessDatabase method
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                // Open the connection to the MySQL database
                Connection.Open();

                // Create a command to execute the SQL query
                MySqlCommand Command = Connection.CreateCommand();

                // SQL query to retrieve all students from the 'students' table
                Command.CommandText = "Select * from students";

                // Prepare the command to prevent SQL injection
                Command.Prepare();

                // Execute the query and read the results
                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {
                    // Loop through each row in the result set
                    while (ResultSet.Read())
                    {
                        // Extract data from each column for the current student
                        int id = Convert.ToInt32(ResultSet["studentid"]);
                        string FirstName = ResultSet["studentfname"].ToString();
                        string LastName = ResultSet["studentlname"].ToString();
                        string StudentNumber = ResultSet["studentnumber"].ToString();
                        DateTime EnrolDate = Convert.ToDateTime(ResultSet["enroldate"]);

                        // Create a new Student object with the extracted data
                        Student CurrentStudent = new Student()
                        {
                            StudentId = id,
                            StudentFName = FirstName,
                            StudentLName = LastName,
                            EnrollDate = EnrolDate,
                            StudentNumber = StudentNumber
                        };

                        // Add the student to the list
                        Students.Add(CurrentStudent);
                    }
                }
            }

            // Return the list of students
            return Students;
        }

        /// <summary>
        /// Finds and retrieves a student by their ID from the database.
        /// </summary>
        /// <param name="id">The ID of the student to retrieve.</param>
        /// <returns>A Student object with the details of the student matching the provided ID.</returns>
        [HttpGet]
        [Route(template: "FindStudent/{id}")]
        public Student FindStudent(int id)
        {
            // Create a Student object to store the selected student's data
            Student SelectedStudents = new Student();

            // Open a connection to the database using the context's AccessDatabase method
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                // Open the connection to the MySQL database
                Connection.Open();

                // Create a command to execute the SQL query
                MySqlCommand Command = Connection.CreateCommand();

                // SQL query to retrieve a student by ID from the 'students' table
                Command.CommandText = "Select * from students WHERE studentid = @id";

                // Add the student ID parameter to the query
                Command.Parameters.AddWithValue("@id", id);

                // Execute the query and read the results
                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {
                    // Loop through each row in the result set
                    while (ResultSet.Read())
                    {
                        // get data from each column for the student with the specified ID
                        int StudentId = Convert.ToInt32(ResultSet["studentid"]);
                        string FirstName = ResultSet["studentfname"].ToString();
                        string LastName = ResultSet["studentlname"].ToString();
                        string StudentNumber = ResultSet["studentnumber"].ToString();
                        DateTime EnrolDate = Convert.ToDateTime(ResultSet["enroldate"]);

                        // Assign the  data to the SelectedStudents object
                        SelectedStudents.StudentId = StudentId;
                        SelectedStudents.StudentFName = FirstName;
                        SelectedStudents.StudentLName = LastName;
                        SelectedStudents.EnrollDate = EnrolDate;
                        SelectedStudents.StudentNumber = StudentNumber;
                    }
                }
            }

            // Return the selected student
            return SelectedStudents;
        }
        /// <summary>
        /// Adds an author to the database
        /// </summary>
        /// <param name="StudentData">Author Object</param>
        /// <example>
        /// POST: api/AuthorData/AddAuthor
        /// Headers: Content-Type: application/json
        /// Request Body:
        /// {
        ///	    "AuthorFname":"Christine",
        ///	    "AuthorLname":"Bittle",
        ///	    "AuthorBio":"Likes Coding!",
        ///	    "AuthorEmail":"christine@test.ca"
        /// } -> 409
        /// </example>
        /// <returns>
        /// The inserted Author Id from the database if successful. 0 if Unsuccessful
        /// </returns>
        [HttpPost(template: "AddStudent")]
        public int AddStudent([FromBody] Student StudentData)
        {
            // 'using' will close the connection after the code executes
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                //Establish a new command (query) for our database
                MySqlCommand Command = Connection.CreateCommand();

                // CURRENT_DATE() for the author join date in this context
                // Other contexts the join date may be an input criteria!
                Command.CommandText = "insert into students (studentfname, studentlname, enroldate, studentnumber) values (@studentfname, @studentlname,@EnrollDate,@studentnumber)";
                Command.Parameters.AddWithValue("@studentfname", StudentData.StudentFName);
                Command.Parameters.AddWithValue("@studentLname", StudentData.StudentLName);
                Command.Parameters.AddWithValue("@studentnumber",StudentData.StudentNumber);
                Command.Parameters.AddWithValue("@EnrollDate", StudentData.EnrollDate);

                Command.ExecuteNonQuery();

                return Convert.ToInt32(Command.LastInsertedId);

            }
            // if failure
            return 0;
        }

        /// <summary>
        /// Deletes an Author from the database
        /// </summary>
        /// <param name="StudentId">Primary key of the author to delete</param>
        /// <example>
        /// DELETE: api/AuthorData/DeleteAuthor -> 1
        /// </example>
        /// <returns>
        /// Number of rows affected by delete operation.
        /// </returns>
        [HttpDelete(template: "DeleteStudent/{StudentId}")]
        public int DeleteStudent(int StudentId)
        {
            // 'using' will close the connection after the code executes
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                //Establish a new command (query) for our database
                MySqlCommand Command = Connection.CreateCommand();


                Command.CommandText = "delete from students where studentid=@id";
                Command.Parameters.AddWithValue("@id", StudentId);
                return Command.ExecuteNonQuery();

            }
            // if failure
            return 0;
        }
        /// <summary>
        /// Updates a Student in the database. Data is a Student object, request query contains Student ID.
        /// </summary>
        /// <param name="StudentData">Student Object containing updated student details</param>
        /// <param name="StudentId">The Student ID primary key</param>
        /// <example>
        /// PUT: api/Student/UpdateStudent/4
        /// Headers: Content-Type: application/json
        /// Request Body:
        /// {
        ///     "StudentFName": "",
        ///     "StudentLName": "",
        ///     "EnrollDate": "",
        ///     "StudentNumber": ""
        /// } -> 
        /// {
        ///     "StudentId": 4,
        ///     "StudentFName": "Sumit",
        ///     "StudentLName": "Singh",
        ///     "EnrollDate": "2024-01-01",
        ///     "StudentNumber": "S12345"
        /// }
        /// </example>
        /// <returns>
        /// The updated Student object
        /// </returns>

        [HttpPut(template: "UpdateStudent/{StudentId}")]
        public Student UpdateStudent(int StudentId, [FromBody] Student StudentData)
        {
            // 'using' will close the connection after the code executes
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                //Establish a new command (query) for our database
                MySqlCommand Command = Connection.CreateCommand();

                // parameterize query
                Command.CommandText = "UPDATE students SET studentfname=@studentfname, studentlname=@studentlname,enroldate=@enroldate, studentnumber=@studentnum WHERE studentid=@id";
                Command.Parameters.AddWithValue("@studentfname", StudentData.StudentFName);
                Command.Parameters.AddWithValue("@studentlname", StudentData.StudentLName);
                Command.Parameters.AddWithValue("@enroldate", StudentData.EnrollDate);
                Command.Parameters.AddWithValue("@studentnum", StudentData.StudentNumber);

                // Add student ID to specify which student to update
                Command.Parameters.AddWithValue("@id", StudentId);

                // Execute the query to update the student's details
                Command.ExecuteNonQuery();
            }

            // Return the updated student details
            return FindStudent(StudentId);
        }
    }
}







