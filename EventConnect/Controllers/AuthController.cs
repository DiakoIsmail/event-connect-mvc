using EventConnect.Data.Auth;
using EventConnect.Dtos.User;
using EventConnect.Entities;
using EventConnect.Models.ServiceResponce;
using Microsoft.AspNetCore.Mvc;

namespace EventConnect.Controllers;


[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthRepository _authRepo;

    public AuthController(IAuthRepository authRepo)
    {
        _authRepo = authRepo;
    }

    [HttpPost("Register")]
    public async Task<ActionResult<ServiceResponse<int>>> Register(UserRegisterDto request)
    {
        var response = await _authRepo.Register(
            new User { Firstname= request.Firstname, Lastname = request.Lastname,Email = request.Email}, request.Password
        );
        if(!response.Success)
        {
            return BadRequest(response);
        }
        return Ok(response);
    }

    [HttpPost("Login")]
    public async Task<ActionResult<ServiceResponse<int>>> Login(UserLoginDto request)
    {
        var response = await _authRepo.Login(request.Email, request.Password);
        if(!response.Success)
        {
            return BadRequest(response);
        }
        return Ok(response);
    }
    

}
