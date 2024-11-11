using System.Data;
using System.Data.SqlClient;
using Dapper;
class DatabaseRepository
{
    public IDbConnection Connect()
    {
        string connectionString = File.ReadAllText("connectionString.txt");
        return new SqlConnection(connectionString);
    }

    public List<Student> GetAllStudents()
    {
        string sql = "SELECT Name, Email, DateOfBirth FROM Students";
        using IDbConnection conn = Connect();
        return conn.Query<Student>(sql).AsList();
    }

    public Student GetStudentById(int id)
    {
        string sql = "SELECT Name, Email, DateOfBirth FROM Students WHERE id=" + id;
        //Console.WriteLine(sql);
        using IDbConnection conn = Connect();
        return conn.QuerySingle<Student>(sql);
    }

    public void AddStudent(string name, string email, string date)
    {
        string sql = "INSERT INTO Students (Name, Email, DateOfBirth) VALUES(@Name, @Email, @Date)";
        var parameters = new { Name = name, Email = email, Date = date };
        //Console.WriteLine(sql);
        using IDbConnection conn = Connect();
        conn.Execute(sql, parameters);
    }

    public List<(string Name, double AverageGrade)> GetAverageGradeForAllCourses()
    {
        string sql = @"SELECT Courses.Name, AVG(CAST(Enrollments.Grade AS FLOAT)) AS AverageGrade
                        FROM Courses
                        JOIN Enrollments ON Courses.Id = Enrollments.CourseId
                        GROUP BY Courses.Id, Courses.Name;";
        using IDbConnection conn = Connect();
        return conn.Query<(string Name, double AverageGrade)>(sql).AsList();
    }
}