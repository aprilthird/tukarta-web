using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TUKARTA.PE.DATA.Context;
using TUKARTA.PE.ENTITIES.Models;
using TUKARTA.PE.SERVICE.Resources;
using TUKARTA.PE.WEB.API.Factories;
using TUKARTA.PE.WEB.API.Responses;

namespace TUKARTA.PE.WEB.API.Controllers
{
    [Route("usuario")]
    [ApiController]
    public class AccountController : BaseApiController
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;

        public AccountController(
            TuKartaDbContext context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration)
            : base(context, userManager)
        {
            _signInManager = signInManager;
            _configuration = configuration;
        }

        [HttpPost("ingresar")]
        public async Task<IActionResult> Login(LoginResource resource)
        {
            var result = await _signInManager.PasswordSignInAsync(resource.Email, resource.Password, false, false);

            if (result.Succeeded)
            {
                var appUser = await _context.Users
                    .Include(x => x.UserRoles)
                    .ThenInclude(x => x.Role)
                    .FirstOrDefaultAsync(r => r.Email == resource.Email);
                return Ok(new DataResponse()
                {
                    Data = new
                    {
                        user = new UserResource
                        {
                            Id = appUser.Id,
                            Name = appUser.Name,
                            Surname = appUser.Surname,
                            BirthDate = appUser.BirthDate,
                            PhoneNumber = appUser.PhoneNumber,
                            Email = appUser.Email,
                            CreatedAt = appUser.CreatedAt,
                            UpdatedAt = appUser.UpdatedAt
                        },
                        token = GenerateJwtToken(appUser)
                    }
                });
            }

            return BadRequest(new ErrorResponse()
            {
                Message = "El correo y contraseña no se encuentran registrados."
            });
        }

        [Authorize]
        [HttpGet("info")]
        public async Task<IActionResult> UserInfo()
        {
            var appUser = await GetUserAsync();
            var result = new DataResponse()
            {
                Data = new UserResource
                {
                    Id = appUser.Id,
                    Name = appUser.Name,
                    Surname = appUser.Surname,
                    BirthDate = appUser.BirthDate,
                    PhoneNumber = appUser.PhoneNumber,
                    Email = appUser.Email,
                    CreatedAt = appUser.CreatedAt,
                    UpdatedAt = appUser.UpdatedAt
                }
            };
            return Ok(result);
        }

        [HttpPost("registro")]
        public async Task<IActionResult> Register(UserResource resource)
        {
            if(!string.IsNullOrEmpty(resource.Password))
            {
                var validatePassword = await new PasswordValidator<ApplicationUser>().ValidateAsync(_userManager, null, resource.Password);
                if (!validatePassword.Succeeded)
                {
                    return BadRequest(new ErrorResponse()
                    {
                        Message = string.Join(", ", validatePassword.Errors.Select(e => $"{e.Code}: {e.Description}").ToList())
                    });
                }
            }
            else
            {
                return BadRequest(new ErrorResponse()
                {
                    Message = "Se necesita una contraseña para el registro."
                });
            }

            if(await _userManager.FindByEmailAsync(resource.Email) == null)
            {
                return BadRequest(new ErrorResponse()
                {
                    Message = "El correo electrónico ya se encuentra registrado."
                });
            }

            var user = new ApplicationUser
            {
                UserName = resource.Email,
                Email = resource.Email,
                Name = resource.Name,
                Surname = resource.Surname,
                PhoneNumber = resource.PhoneNumber,
                BirthDate = resource.BirthDate,
            };

            var result = await _userManager.CreateAsync(user, resource.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                var appUser = await _context.Users
                    .Include(x => x.UserRoles)
                    .ThenInclude(x => x.Role)
                    .FirstOrDefaultAsync(r => r.Email == resource.Email);
                return Ok(new DataResponse()
                {
                    Data = new
                    {
                        user = new UserResource
                        {
                            Id = appUser.Id,
                            Name = appUser.Name,
                            Surname = appUser.Surname,
                            BirthDate = appUser.BirthDate,
                            PhoneNumber = appUser.PhoneNumber,
                            Email = appUser.Email,
                            CreatedAt = appUser.CreatedAt,
                            UpdatedAt = appUser.UpdatedAt
                        },
                        token = GenerateJwtToken(appUser)
                    }
                });
            }

            return BadRequest(new ErrorResponse()
            {
                Message = "No se pudo crear el usuario."
            });
        }

        [HttpPut("editar")]
        public async Task<IActionResult> Edit(UserResource resource)
        {
            if (!string.IsNullOrEmpty(resource.Password))
            {
                var validatePassword = await new PasswordValidator<ApplicationUser>().ValidateAsync(_userManager, null, resource.Password);
                if (!validatePassword.Succeeded)
                {
                    return BadRequest(new ErrorResponse()
                    {
                        Message = string.Join(", ", validatePassword.Errors.Select(e => $"{e.Code}: {e.Description}").ToList())
                    });
                }
            }

            var user = await GetUserAsync();
            var repeatedUser = await _userManager.FindByEmailAsync(resource.Email);

            if (repeatedUser != null)
            {
                if(repeatedUser.Id != user.Id)
                {
                    return BadRequest(new ErrorResponse()
                    {
                        Message = "El correo electrónico ya se encuentra registrado."
                    });
                }
            }

            user.Name = resource.Name;
            user.Surname = resource.Surname;
            user.BirthDate = resource.BirthDate;
            user.Email = resource.Email;
            user.UserName = resource.Email;
            user.PhoneNumber = resource.PhoneNumber;
            if(!string.IsNullOrEmpty(resource.Password))
            {
                user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, resource.Password);
            }
            user.UpdatedAt = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

            return Ok();
        }

        private object GenerateJwtToken(ApplicationUser user)
        {
            var claims = ClaimsPrincipalFactory.Create(user);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["JwtExpireDays"]));

            var token = new JwtSecurityToken(
                _configuration["JwtIssuer"],
                _configuration["JwtIssuer"],
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}