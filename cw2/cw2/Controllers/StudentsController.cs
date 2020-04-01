using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using cw2.Models;
using System.Data.SqlClient;



namespace cwiczenia2.Controllers
{
    [Route("api/students")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        
        string conString = "Data Source=db-mssql;Initial Catalog=s16502; Integrated Security=True";

        public StudentsController()
        {
        }

        [HttpGet]
        public IActionResult GetStudents(string orderBy)
        {
            var list = new List<StudentInfoDTO>();
            using (SqlConnection con = new SqlConnection(conString))
            using (SqlCommand com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText = "select s.FirstName, s.LastName, s.BirthDate, st.Name, e.Semester from Student s join Enrollment e on e.IdEnrollment = s.IdEnrollment join Studies st on st.IdStudy = e.IdStudy";
                con.Open();

                SqlDataReader dr = com.ExecuteReader();

                while (dr.Read())
                {
                    var st = new StudentInfoDTO
                    {
                        FirstName = dr["FirstName"].ToString(),
                        LastName = dr["LastName"].ToString(),
                        BirthDate = dr["BirthDate"].ToString(),
                        Name = dr["Name"].ToString(),
                        Semester = dr["Semester"].ToString()
                    };
                    list.Add(st);
                }

            }
            return Ok(list);
        }
       //4.3

       /* [HttpGet("{id}")]
        public IActionResult GetStudent([FromRoute]string id)
        {
            var st = new StudentInfoDTO();
            using (SqlConnection con = new SqlConnection(conString))
            using (SqlCommand com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText = "select s.FirstName, s.LastName, s.BirthDate, s.IndexNumber, st.Name, e.Semester from Student s " +
                    "join Enrollment e on e.IdEnrollment = s.IdEnrollment join Studies st on st.IdStudy = e.IdStudy " +
                    $"where s.IndexNumber = '{id}'";
                con.Open();

                SqlDataReader dr = com.ExecuteReader();
                dr.Read();

                st.FirstName = dr["FirstName"].ToString();
                st.LastName = dr["LastName"].ToString();
                st.BirthDate = dr["BirthDate"].ToString();
                st.Name = dr["Name"].ToString();
                st.Semester = dr["Semester"].ToString();
            }
            return Ok(st);
        } */

        /* 4.4 
         * https://localhost:44308/api/students/aa';DROP TABLE Student;--
         */

        // 4.5
        [HttpGet("{id}")]
        public IActionResult GetStudent([FromRoute]string id)
        {
            var st = new StudentInfoDTO();
            using (SqlConnection con = new SqlConnection(conString))
            using (SqlCommand com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText = "select s.FirstName, s.LastName, s.BirthDate, s.IndexNumber, st.Name, e.Semester from Student s " +
                    "join Enrollment e on e.IdEnrollment = s.IdEnrollment join Studies st on st.IdStudy = e.IdStudy " +
                    $"where s.IndexNumber = @id";
                com.Parameters.AddWithValue("id", id);
                con.Open();

                SqlDataReader dr = com.ExecuteReader();
                dr.Read();

                st.FirstName = dr["FirstName"].ToString();
                st.LastName = dr["LastName"].ToString();
                st.BirthDate = dr["BirthDate"].ToString();
                st.Name = dr["Name"].ToString();
                st.Semester = dr["Semester"].ToString();
            }
            return Ok(st);
        }

        [HttpPost]
        public IActionResult CreateStudent(Student student)
        {
            student.IndexNumber = $"s{new Random().Next(1, 20000)}";
            return Ok(student);
        }

    }
}
