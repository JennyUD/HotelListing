using AutoMapper;
using HotelListing.Data;
using HotelListing.Models;
using HotelListing.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelListing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        private readonly UserManager<ApiUser> _UserManager;
        private readonly ILogger<AccountController> _Logger;
        private readonly IMapper _Mapper;
        private readonly IAuthManager _IAuthmanager;

        public AccountController(
            UserManager<ApiUser>userManager,
            ILogger <AccountController> logger,
            IMapper mapper,
             IAuthManager Authmanager )
            {
            _UserManager=userManager;
            _Logger = logger;
            _Mapper = mapper;
            _IAuthmanager = Authmanager;
            }


        [HttpPost]
        [Route("register")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register  ([FromBody] UserDTO userDTO)
        {
          if (!ModelState.IsValid)
            {  _Logger.LogInformation($"Registeration Attempt for {userDTO.Email}");
                return BadRequest(ModelState);
            }
            try
            {
                var user = _Mapper.Map<ApiUser>(userDTO);
                user.UserName = userDTO.Email;
                var result = await _UserManager.CreateAsync(user, userDTO.Password);
              
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }
                    return BadRequest(ModelState);
                }
                await _UserManager.AddToRolesAsync(user, userDTO.Roles);
                return Accepted();
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, $"Something  went wrong in the {nameof(Register)}");
                return Problem( $"Something went wrong in the {nameof(Register)}", statusCode: 500 );
            }
            
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDTO userDTO)
        {
            _Logger.LogInformation($"Login Attempt for {userDTO.Email}");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                //var result = await _IAuthmanager.PasswordSignInAsync(userDTO.Email, userDTO.Password,
                //    false, false);
                if (await _IAuthmanager.ValidateUser(userDTO)==false)
                {
                    return Unauthorized(userDTO);
                }
                return Accepted(new { Token = await _IAuthmanager.CreateToken() } ); 
            }

            catch (Exception ex)
            {
                _Logger.LogError(ex, $"Something  went wrong in the {nameof(Register)}");
                return Problem($"Something went wrong in the {nameof(Register)}", statusCode: 500);
            }

        }

        [HttpGet]
        [Route("GetAllUsers")
                 //  , Authorize
                 ]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllUsers()
        {

            try
            {
                var user = await _IAuthmanager.GetAllUsers();
                var results = _Mapper.Map<IList<HotelDTO>>(user);
                return Ok(results);
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, $"Something  went wrong in the {nameof(Register)}");
                return Problem($"Something went wrong in the {nameof(Register)}", statusCode: 500);
            }
        }








    }
}
