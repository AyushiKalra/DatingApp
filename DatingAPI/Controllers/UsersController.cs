using AutoMapper;
using DatingAPI.Data;
using DatingAPI.DTOs;
using DatingAPI.Entities;
using DatingAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingAPI.Controllers
{
    //When decorated on an assembly, all controllers in the assembly will be treated as controllers with API behavior.
    //Controller base is the base class we need to work with models and controllers. View will be derived through Angular.
    //can be accessed via GET/api/users URL.
    [Authorize]
    public class UsersController : BaseApiController
    {
        //we want to use the Data context service from Program.cs class so that we have access to a database session.
        //implementing dependency injection
        //in order to inject something into a class, we need to provide the class with a constructor.
        //constructor for the class.
        public readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public UsersController(IUserRepository userRepository, IMapper mapper)
        {
            _mapper = mapper;
            _userRepository = userRepository;

        }
        //ActionResult helps in returning an HTTP based response.
        //To make the code asynchronous so that one lengthy query to the database doesn't block other requests from the server, add async.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDTO>>> GetUser()
        {
            var users = await _userRepository.GetMembersAsync();//get all users

            return Ok(users);
        }
        
         [HttpGet("{username}")]
        public async Task<ActionResult<MemberDTO>> GetUserByUsername(string username)
        {
            return await _userRepository.GetMemberByUsernameAsync(username); //get user on the basis of primary key
        }
    }
}