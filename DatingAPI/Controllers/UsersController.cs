using System.Security.Claims;
using AutoMapper;
using DatingAPI.Data;
using DatingAPI.DTOs;
using DatingAPI.Entities;
using DatingAPI.Extensions;
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
        private readonly IPhotoService _photoService;

        public UsersController(IUserRepository userRepository, IMapper mapper, IPhotoService photoService)
        {
            _mapper = mapper;
            _photoService = photoService;
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

        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDTO memberUpdateDTO)
        {
            /*ClaimsPrincipal ControllerBase.User { get; }
            Gets the ClaimsPrincipal for user associated with the executing action.*/
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

            if(user == null) return NotFound();
            _mapper.Map(memberUpdateDTO, user);
             
             if( await _userRepository.SaveAllAsync()) return NoContent();

             return BadRequest("Failed to update user");
        }
        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDTO>> AddPhoto(IFormFile file)
        {
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

            if(user == null) return NotFound();
            var result = await _photoService.AddPhotoAsync(file);
            if(result.Error != null) return BadRequest(result.Error.Message);

            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId

            };
            if(user.Photos.Count == 0) photo.IsMain = true;
            user.Photos.Add(photo);
            if(await _userRepository.SaveAllAsync()) 
            {
            //return _mapper.Map<PhotoDTO>(photo); 
            //the above returna a 200 OK response . We want a 201 content response telling the user the resource location.
            return CreatedAtAction(nameof(GetUser), new {username = user.UserName},
            _mapper.Map<PhotoDTO>(photo));

            }
            return BadRequest("Encountered some problem while adding photo.");
        }
        [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult> SetMainPhoto(int photoId)
        {
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());
            if(user ==  null)  return NotFound();

            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);
            if(photo == null)  return NotFound();
            if(photo.IsMain) return BadRequest("This is already your main photo.");

            var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);

            if(currentMain != null) currentMain.IsMain = false;
            photo.IsMain= true;

            if(await _userRepository.SaveAllAsync()) return NoContent();//it is an update, we are not returning any resource
            
            return BadRequest("Problem setting Main photo.");
        }
        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId)
        {
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());
            if(user ==  null)  return NotFound();

            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);
            if(photo == null) return NotFound();
            if(photo.IsMain) return BadRequest("You cannot delete your main photo.");
            if(photo.PublicId != null)
            {
                var result = await _photoService.DeletePhotoAsync(photo.PublicId);
                if(result.Error != null) return BadRequest(result.Error.Message);
            }
            user.Photos.Remove(photo);
            if(await _userRepository.SaveAllAsync()) return Ok();
            return BadRequest("Problem deleting photo.");

        }
    }
}