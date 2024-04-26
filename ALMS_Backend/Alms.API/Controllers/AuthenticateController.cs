using Alms.DAL.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Alms.DAL.Helper;
using Microsoft.EntityFrameworkCore;
using Alms.DAL.Models;
using Alms.BLL.Helpers;
using Alms.DAL.ViewModels;
namespace Alms.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;
        public WbLearningContext WbLearningContext { get; }

        public AuthenticateController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, WbLearningContext wbLearningContext)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            _configuration = configuration;
            WbLearningContext = wbLearningContext;
        }
        /// <summary>
        /// Controller action for handling HTTP POST requests to perform user login.
        /// </summary>
        /// <param name="model">containing login credentials.</param>
        /// <returns>representing the result of the login operation.</returns>
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            // Attempt to find the user by username
            var user = await userManager.FindByNameAsync(model.UsernameOrEmail);

            if (user == null)
            {
                user = await userManager.FindByEmailAsync(model.UsernameOrEmail);
            }

            // Check if the user exists and the provided password is correct
            if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
            {
                // Retrieve the roles associated with the user
                var userRoles = await userManager.GetRolesAsync(user);


                // Prepare authentication claims
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.NameIdentifier, user.EmployeeId.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                // Add user roles as claims
                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                // Get the JWT secret key and token validity duration from configuration
                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
                _ = int.TryParse(_configuration["JWT:TokenValidityInMinutes"], out int tokenValidityInMinutes);

                // Create a JWT token
                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddMinutes(tokenValidityInMinutes),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );

                // Return a successful response with the generated token and its expiration
                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }
            else
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new DAL.ViewModels.Response("Username or Password is incorrect", false));
            }

            // Return Unauthorized if the login is unsuccessful
            return Unauthorized();
        }

        /// <summary>
        /// Controller action for handling HTTP POST requests to register a new user.
        /// </summary>
        /// <param name="model">containing user registration details.</param>
        /// <returns>representing the result of the registration operation</returns>
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            // Check if a user with the same username already exists
            var userExists = await userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                //return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

                return StatusCode(StatusCodes.Status500InternalServerError, new DAL.ViewModels.Response("User already exists!", false));

            var emailExists = await userManager.FindByEmailAsync(model.Email);
            if (emailExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new DAL.ViewModels.Response("Email already exists!", false));

            int nextEmployeeId = GenerateNextEmployeeId();
            // Create a new user with the provided registration details
            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username,
                EmployeeId = nextEmployeeId,
                FirstName = model.FirstName,
                LastName = model.LastName,
                DateOfJoin = DateHelper.ConvertStringToDateOnly(model.DateOfJoin)
            };

            // Attempt to create the user using the user manager
            var result = await userManager.CreateAsync(user, model.Password);

            // If user creation is unsuccessful, return an error response
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new DAL.ViewModels.Response("User creation failed! Please check user details and try again.", false));

            // Check if the "Admin," "Manager," and "User" roles exist, create them if not
            if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
            if (!await roleManager.RoleExistsAsync(UserRoles.Manager))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.Manager));
            if (!await roleManager.RoleExistsAsync(UserRoles.User))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.User));


            // Check if the role exists and if the role requested in the model is "User/Manager/Admin". 
            // If both conditions are true, assign the user to the "User/Manager/Admin" role.
            if (await roleManager.RoleExistsAsync(UserRoles.User) && model.Role == UserRoles.User)
            {
                await userManager.AddToRoleAsync(user, UserRoles.User);
            }

            if (await roleManager.RoleExistsAsync(UserRoles.Manager) && model.Role == UserRoles.Manager)
            {
                await userManager.AddToRoleAsync(user, UserRoles.Manager);
            }

            if (await roleManager.RoleExistsAsync(UserRoles.Admin) && model.Role == UserRoles.Admin)
            {
                await userManager.AddToRoleAsync(user, UserRoles.Admin);
            }


            // Return a successful response if the user is created successfully
            return Ok(new DAL.ViewModels.Response("User created successfully!", true));
        }

        private int GenerateNextEmployeeId()
        {
            var lastEmployee = WbLearningContext.Users.OrderByDescending(u => u.EmployeeId).FirstOrDefault();
            if (lastEmployee != null)
            {
                return lastEmployee.EmployeeId + 1;
            }
            else
            {
                // If there are no existing users, start with EmployeeId = 1
                return 1;
            }
        }


        /// <summary>
        /// Controller action for handling HTTP POST requests to register a new admin user.
        /// </summary>
        /// <param name="model">containing admin user registration details.</param>
        /// <returns>representing the result of the admin user registration operation.</returns>
        [HttpPost]
        [Route("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel model)
        {
            // Check if a user with the same username already exists
            var userExists = await userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new DAL.ViewModels.Response("User already exists!", false));

            // Create a new admin user with the provided registration details
            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };

            // Attempt to create the admin user using the user manager
            var result = await userManager.CreateAsync(user, model.Password);

            // If admin user creation is unsuccessful, return an error response
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new DAL.ViewModels.Response("User creation failed! Please check user details and try again.", false));

            // Check if the "Admin," "Manager," and "User" roles exist, create them if not
            if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
            if (!await roleManager.RoleExistsAsync(UserRoles.Manager))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.Manager));
            if (!await roleManager.RoleExistsAsync(UserRoles.User))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.User));


            // Add the admin user to the "Admin" role
            if (await roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await userManager.AddToRoleAsync(user, UserRoles.Admin);
            }

            // Return a successful response if the admin user is created successfully
            return Ok(new DAL.ViewModels.Response("User created successfully!", true));
        }
    }
}