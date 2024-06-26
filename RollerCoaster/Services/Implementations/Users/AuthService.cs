using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using RollerCoaster.DataBase;
using RollerCoaster.DataBase.Models;
using RollerCoaster.DataTransferObjects.Users.Auth;
using RollerCoaster.Services.Abstractions.Common;
using RollerCoaster.Services.Abstractions.Users;

namespace RollerCoaster.Services.Implementations.Users;

public class AuthService(
    DataBaseContext dataBaseContext,
    ITokenService tokenService,
    IPasswordHashService passwordHashService) : IAuthService
{
    public async Task<AuthorizedUserMeta> Register(RegisterDTO registerDto)
    {
        bool isLoginAlreadyInUse = await dataBaseContext
            .Users.AnyAsync(u => u.Login == registerDto.Login);
        
        if (isLoginAlreadyInUse)
            throw new ProvidedDataIsInvalidError("Такой логин уже в использовании.");
        
        const string loginPattern = @"^[a-zA-Z][a-zA-Z\d_]{3,15}$";
        bool isLoginValid = Regex.IsMatch(registerDto.Login, loginPattern);

        if (!isLoginValid)
            throw new ProvidedDataIsInvalidError("Логин должен быть от 4 до 16 символов и состоять из латиницы.");

        const string passwordPattern = @"^.*(?=.{8,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!*@#$%^+=]).*$";
        bool isPasswordValid = Regex.IsMatch(registerDto.Password, passwordPattern);

        if (!isPasswordValid)
        {
            throw new ProvidedDataIsInvalidError(
                "Пароль должен содержать хотя бы 1 заглавную и строчную букву, " +
                "специальный символ, цифру и быть не менее 8 символов.");
        }
        
        var user = new User
        {
            Login = registerDto.Login,
            PasswordHash = passwordHashService.GenerateHash(registerDto.Password)
        };
        await dataBaseContext.Users.AddAsync(user);
        await dataBaseContext.SaveChangesAsync();

        return new AuthorizedUserMeta
        {
            Id = user.Id,
            Token = tokenService.GenerateToken(user.Id)
        };
    }

    public async Task<AuthorizedUserMeta> Login(LoginDTO loginDto)
    {
        var user = await dataBaseContext.Users.FirstOrDefaultAsync(u => u.Login == loginDto.Login);
        
        if (user is null || user.PasswordHash != passwordHashService.GenerateHash(loginDto.Password))
            throw new AccessDeniedError("Указаны неверные данные для авторизации.");

        return new AuthorizedUserMeta
        {
            Id = user.Id,
            Token = tokenService.GenerateToken(user.Id)
        };
    }
}