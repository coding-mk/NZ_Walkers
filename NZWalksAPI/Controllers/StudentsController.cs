using Microsoft.AspNetCore.Mvc;

namespace NZWalksAPI.Controllers;

// https://localhost:port/api/students
[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    [HttpGet]
    public IActionResult GetAllStudents()
    {
      string[] studentNames = new string[] { "Manish", "Aditya", "Shravanti", "Shraddha", "Parth"};

      return Ok(studentNames);
    }
}