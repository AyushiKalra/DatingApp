using DatingAPI.Data;
using DatingAPI.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingAPI.Controllers
{
    //When decorated on an assembly, all controllers in the assembly will be treated as controllers with API behavior.
    //Controller base is the base class we need to work with models and controllers. View will be derived through Angular.
    [ApiController]
    [Route("api/[controller]")] //can be accessed via GET/api/users URL.
    public class UsersController : ControllerBase
    {
        //we want to use the Data context service from Program.cs class so that we have access to a database session.
        //implementing dependency injection
        //in order to inject something into a class, we need to provide the class with a constructor.
        private DataContext _context;
        //constructor for the class.
        public UsersController(DataContext context)
        {
            _context = context;

        }
        //ActionResult helps in returning an HTTP based response.
        //To make the code asynchronous so that one lengthy query to the database doesn't block other requests from the server, add async.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUser()
        {
            var users = await _context.Users.ToListAsync();//get all users

            return users;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<AppUser>> GetUser(int id)
        {
            return await _context.Users.FindAsync(id); //get all users
        }
    }
}