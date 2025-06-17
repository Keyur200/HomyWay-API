using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HomyWayAPI.DTO;
using HomyWayAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.IdentityModel.Tokens;

namespace HomyWayAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly HomyWayContext _context;
        private readonly IConfiguration _config;

        public AuthController(HomyWayContext context, IConfiguration configuration)
        {
            _context = context;
            _config = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _context.Users.ToListAsync();
            return Ok(users);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserDTO userdto)
        {
            if (_context.Users.Any(u => u.Name == userdto.Name)) return BadRequest("User already exists");
            var groupExists = await _context.Groups.AnyAsync(g => g.Id == userdto.Gid);

            //Check if group exists
            if (!groupExists)
            {
                return BadRequest("Invalid Group ID");
            }

            //Created object of User model 
            var nuser = new User
            {
                Id = 0,
                Name = userdto.Name,
                Email = userdto.Email,
                Phone = userdto.Phone,
                Status = userdto.Status,
                Password = BCrypt.Net.BCrypt.HashPassword(userdto.Password),
                Gid = userdto.Gid,
            };

            //Save into Database 
            _context.Users.Add(nuser);
            await _context.SaveChangesAsync();

            return Ok("User registered successfully");

        }

        //Login user code 

        [HttpPost("login")]
        public IActionResult Login(LoginDTO users)
        {
            var Jwt = _config.GetSection("Jwt");
            var user = _context.Users.SingleOrDefault(u => u.Email == users.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(users.Password, user.Password)) return Ok("Invalid credentials.");

            if (user.Status == "pending") return Ok("Please wait for approval");
            if (user.Status == "block") return Ok("Your account has been block");

            var token = GenerateJwtToken(users.Email);
            return Ok(new { token, user = new { user.Name, user.Email, user.Phone, user.Gid } });

        }

        //Update status code
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateUserStatus(int id, [FromQuery] string status)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound("User not found");
            user.Status = status;
            await _context.SaveChangesAsync();
            return Ok("User status updated successfully");
        }

        [HttpPatch("updateUser/{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromQuery] string name, [FromQuery] string phone, [FromQuery] string email)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound("User not found");
            user.Email = email;
            user.Name = name;
            user.Phone = phone;
            await _context.SaveChangesAsync();
            return Ok("User updated successfully");
        }

        //generate jwt token code
        private string GenerateJwtToken(string username)
        {
            var jwtSettings = _config.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var user = _context.Users.FirstOrDefault(u => u.Email == username);

            var claims = new[]
            {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, user.Gid.ToString()),
        };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["ExpiryInMinutes"])),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        [HttpPost("forgot-password")]
        public IActionResult ForgotPassword([FromBody] ForgotPasswordRequestDto request)
        {
            var user = _context.Users.FirstOrDefault(u => u.Phone == request.Phone);
            if (user == null) return Ok("User not found");

            var otp = new Random().Next(100000, 999999).ToString();
            OtpMemoryStore.SaveOtp(request.Phone, otp);

            Console.WriteLine($"Sending OTP {otp} to phone {request.Phone}");

            return Ok($"OTP: {otp} sent to your phone.");
        }

        [HttpPost("verify-otp")]
        public IActionResult VerifyOtp([FromBody] VerifyOtpDto request)
        {
            bool isValid = OtpMemoryStore.VerifyOtp(request.Phone, request.Otp);
            if (!isValid) return Ok("Invalid or expired OTP");

            return Ok("OTP verified successfully.");
        }

        [HttpPost("reset-password")]
        public IActionResult ResetPassword([FromBody] ResetPasswordDto request)
        {
            var user = _context.Users.FirstOrDefault(u => u.Phone == request.Phone);
            if (user == null) return Ok("User not found");

            user.Password = BCrypt.Net.BCrypt.HashPassword(request.NewPassword); 
            _context.SaveChanges();

            return Ok("Password reset successfully.");
        }


        public static class OtpMemoryStore
        {
            private static readonly Dictionary<string, (string Otp, DateTime Expiry)> otpStore = new();

            public static void SaveOtp(string phone, string otp, int expiryMinutes = 5)
            {
                otpStore[phone] = (otp, DateTime.UtcNow.AddMinutes(expiryMinutes));
            }

            public static bool VerifyOtp(string phone, string otp)
            {
                if (otpStore.TryGetValue(phone, out var data))
                {
                    if (data.Otp == otp && data.Expiry > DateTime.UtcNow)
                    {
                        otpStore.Remove(phone);
                        return true;
                    }
                }
                return false;
            }
        }

    }
}