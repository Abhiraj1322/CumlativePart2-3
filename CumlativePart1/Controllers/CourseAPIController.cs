using CumlativePart1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace CumlativePart1.Controllers
{
    // Define the route for the API controller
    [Route("api/Course")]
    [ApiController]
    public class CourseAPIController : ControllerBase
    {
        private readonly SchooldbContext _context;

        public CourseAPIController(SchooldbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// get a list of courses from the database.
        /// </summary>
        /// <returns>A list of Course  with details of as course ID, teacher ID, course code, 
        /// course name, start date, and finish date.</returns>
        [HttpGet]
        [Route(template: "listCourse")]
        public List<Course> ListCourse()
        {
            
            List<Course> Courses = new List<Course>();

            // Open a connection to the databse
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                // Open the connection to the MySQL database
                Connection.Open();

                // Create a command to execute the SQL query
                MySqlCommand Command = Connection.CreateCommand();

                // Define the SQL query to rall records from the 'courses' table
                Command.CommandText = "Select * from courses";

               
                Command.Prepare();

                // Execute the query and read the results
                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {
                    // Loop through each row in the result set
                    while (ResultSet.Read())
                    {
                        // Extract data from each column in the current row
                        int CourseId = Convert.ToInt32(ResultSet["courseid"]);
                        int TeacherId = Convert.ToInt32(ResultSet["teacherid"]);
                        string CourseCode = ResultSet["coursecode"].ToString();
                        string CourseName = ResultSet["coursename"].ToString();
                        DateTime StartDate = Convert.ToDateTime(ResultSet["startdate"]);
                        DateTime FinishDate = Convert.ToDateTime(ResultSet["finishdate"]);

                        // Create a new Course object with the extracted data
                        Course CurrentCourse = new Course()
                        {
                            courseId = CourseId,
                            teacherid = TeacherId,
                            coursecode = CourseCode,
                            coursename = CourseName,
                            startdate = StartDate,
                            finishdate = FinishDate
                        };

                        // Add the current course to the list
                        Courses.Add(CurrentCourse);
                    }
                }
            }

           
            return Courses;
        }
        /// <summary>
        /// Retrieves details of a specific course by its course ID.
        /// </summary>
        /// <param name="id">The ID of the course to retrieve.</param>
        /// <returns>
        /// The details of the specified course, including its course ID, teacher ID, course code, course name, start date, and finish date.
        /// </returns>
        /// <remarks>
        /// This method connects to the database and retrieves the details of a specific course from the `courses` table
        /// based on the provided course ID. If the course is found, the details will be returned.
        /// </remarks>
        /// <example>
        /// GET api/Course/FindCourse/1 -> {"courseId":1,"coursecode":"http5101","teacherid":1,"startdate":"2018-09-04T00:00:00","finishdate":"2018-12-14T00:00:00","coursename":"Web Application Development"}
        /// </example>
        [HttpGet]
        [Route(template: "FindCourse/{id}")]
        public Course FindCourse(int id)
        {

            Course SelectedCourse = new Course();

            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                MySqlCommand Command = Connection.CreateCommand();

                Command.CommandText = "Select * from courses WHERE courseid = @id";
                Command.Parameters.AddWithValue("@id", id);

                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {
                    while (ResultSet.Read())
                    {
                        int Courseid = Convert.ToInt32(ResultSet["courseid"]);
                        int teacherId = Convert.ToInt32(ResultSet["teacherid"]);
                        string CourseCode = ResultSet["coursecode"].ToString();
                        string CourseName = ResultSet["coursename"].ToString();
                        DateTime StartDate = Convert.ToDateTime(ResultSet["startdate"]);
                        DateTime FinishDate = Convert.ToDateTime(ResultSet["finishdate"]);

                        SelectedCourse.courseId = Courseid;
                        SelectedCourse.teacherid = teacherId;
                        SelectedCourse.coursecode = CourseCode;
                        SelectedCourse.coursename = CourseName;
                        SelectedCourse.startdate = StartDate;
                        SelectedCourse.finishdate = FinishDate;
                    }
                }
            }
            return SelectedCourse;
        }






        /// <summary>
        /// Adds an author to the database
        /// </summary>
        /// <param name="AddCourse">Author Object</param>
        /// <example>
        /// POST: api/AuthorData/AddAuthor
        /// Headers: Content-Type: application/json
        /// Request Body:
        /// {
        
        /// } -> 409
        /// </example>
        /// <returns>
        /// The inserted Author Id from the database if successful. 0 if Unsuccessful
        /// </returns>
        [HttpPost(template: "AddCourse")]
        public int AddCourse([FromBody] Course CourseData)
        {
            // 'using' will close the connection after the code executes
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                //Establish a new command (query) for our database
                MySqlCommand Command = Connection.CreateCommand();

                // CURRENT_DATE() for the author join date in this context
                // Other contexts the join date may be an input criteria!
                Command.CommandText = "insert into courses (coursecode, coursename, startdate, finishdate, teacherid) values (@coursecode, @coursename, @startdate, @finishdate,@teacherid )";
                Command.Parameters.AddWithValue("@coursecode", CourseData.coursecode);
                Command.Parameters.AddWithValue("@coursename", CourseData.coursename);
                Command.Parameters.AddWithValue("@startdate", CourseData.startdate);
                Command.Parameters.AddWithValue("@finishdate", CourseData.finishdate);
                Command.Parameters.AddWithValue("@teacherid", CourseData.teacherid);
                Command.ExecuteNonQuery();

                return Convert.ToInt32(Command.LastInsertedId);

            }
            // if failure
            return 0;
        }


        /// <summary>
        /// Deletes an Author from the database
        /// </summary>
        /// <param name="Courseid">Primary key of the author to delete</param>
        /// <example>
        /// DELETE: api/AuthorData/DeleteAuthor -> 1
        /// </example>
        /// <returns>
        /// Number of rows affected by delete operation.
        /// </returns>
        [HttpDelete(template: "Delete/{Courseid}")]
        public int DeleteCourse(int Courseid)
        {
            // 'using' will close the connection after the code executes
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                //Establish a new command (query) for our database
                MySqlCommand Command = Connection.CreateCommand();


                Command.CommandText = "delete from courses where courseid=@id";
                Command.Parameters.AddWithValue("@id", Courseid);
                return Command.ExecuteNonQuery();

            }
            // if failure
            return 0;
        }



        [HttpPut("UpdateCourse/{courseId}")]
        public Course UpdateCourse(int CourseId, [FromBody] Course CourseData)
        {
            // 'using' will close the connection after the code executes
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();

                // Establish a new command (query) for our database
                MySqlCommand Command = Connection.CreateCommand();

                // Parameterized query to prevent SQL injection
                Command.CommandText = "UPDATE courses SET coursecode=@coursecode, teacherid=@teacherid, startdate=@startdate, finishdate=@finishdate, coursename=@coursename WHERE courseid=@id";
                Command.Parameters.AddWithValue("@coursecode", CourseData.coursecode);
                Command.Parameters.AddWithValue("@teacherid", CourseData.teacherid);
                Command.Parameters.AddWithValue("@startdate", CourseData.startdate);
                Command.Parameters.AddWithValue("@finishdate", CourseData.finishdate);
                Command.Parameters.AddWithValue("@coursename", CourseData.coursename);
                Command.Parameters.AddWithValue("@id", CourseId);


                Command.ExecuteNonQuery();



            }

            // Return the updated course details
            return FindCourse(CourseId); // Return the updated course
        }


    }






}

