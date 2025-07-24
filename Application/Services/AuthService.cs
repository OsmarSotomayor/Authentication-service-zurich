using Application.Dtos;
using Application.Interfaces;
using Domain.Interface;
using Domain.Models;
using Infraestructure.IUtils;
using Infraestructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Crypto.Generators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class AuthService: IAuthService
    {
        private readonly IUserRepository _repo;
        private readonly IJwtTokenGenerator _jwt;
        private readonly PasswordHasher<User> _passwordHasher = new();
        public AuthService(IUserRepository repo, IJwtTokenGenerator jwt)
        {
            _repo = repo;
            _jwt = jwt;
        }

        public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto requestDto)
        {
            var user = await _repo.GetUserByEmail(requestDto.Email, true);
            if (user == null)
                throw new Exception("Usuario no válido.");

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, requestDto.Password);
            var isSuccess = result == PasswordVerificationResult.Success;

            if (!isSuccess)
            {
                user.LastLoginAttempt = DateTime.UtcNow;
                await _repo.SaveAsync();
                throw new Exception("Credenciales invalidas");
            }
            var jwt = _jwt.Generate(user);

            return new LoginResponseDto
            {
                Token = jwt
            };
        }

        public async Task RegisterAsync(RegisterUserDto dto)
        {
            var userExist = await _repo.GetByUsernameAsync(dto.Username, false);
            if (userExist != null)
                throw new Exception("User already exist");

            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                Rol = dto.Rol,
                UserName = dto.Username,
                LastLoginAttempt = DateTime.UtcNow,
                PasswordHash = _passwordHasher.HashPassword(null!, dto.Password)
            };

            _repo.CreateUser(user);
            await _repo.SaveAsync();
        }
    }
}
