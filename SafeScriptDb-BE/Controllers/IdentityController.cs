using Business_Logic_Layer.Interfaces;
using Business_Logic_Layer.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Models;

[ApiController]
[Route("api/[controller]")]
public class IdentityController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IConfiguration _config;
    private readonly ILoginService _loginService;
    private readonly ISignInService _signInService;

    public IdentityController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IConfiguration config,
        ILoginService loginService,
        ISignInService signInService
    )
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _config = config;
        _loginService = loginService;
        _signInService = signInService;
    }

    [HttpPost("Login")]
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    public async Task<IActionResult> Login([FromBody] LoginModel login)
    {
        if (!ModelState.IsValid)
            return BadRequest(new { error = "Invalid login request." });

        try
        {
            var loginResult = await _loginService.LoginUser(login);
            return Ok(loginResult);
        }
        catch (Exception ex)
        {
            return Unauthorized(new { error = ex.Message });
        }
    }

    [HttpPost("SignUp")]
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    public async Task<IActionResult> SignUp([FromBody] RegisterModel register)
    {
        if (!ModelState.IsValid)
            return BadRequest(new { error = "Invalid registration data." });

        var (succeeded, message, errors) = await _signInService.RegisterUser(register);

        if (succeeded)
        {
            return Ok(new { message });
        }

        return BadRequest(new { errors });
    }
}
