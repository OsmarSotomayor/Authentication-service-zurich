using Application.Dtos;
using Application.Interfaces;
using Domain.Interface;
using Domain.Models;
using Infraestructure.IUtils;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Org.BouncyCastle.Crypto.Generators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class AuthenticationService: IAuthenticationService
    {
        private readonly IUserRepository _repo;
        private readonly IJwtTokenGenerator _jwt;
        private readonly PasswordHasher<User> _passwordHasher = new();
        public AuthenticationService(IUserRepository repo, IJwtTokenGenerator jwt)
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
    }
}
