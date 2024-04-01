using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using RollerCoaster.DataBase;
using RollerCoaster.DataBase.Models;
using RollerCoaster.DataTransferObjects.Users;
using RollerCoaster.Services.Abstractions.Common;
using RollerCoaster.Services.Abstractions.Users;

namespace RollerCoaster.Services.Realisations.Users;

public class AuthService(
    DataBaseContext dataBaseContext,
    ITokenService tokenService,
    IPasswordHashService passwordHashService) : IAuthService
{
    public async Task<CreatedUserMeta> Register(RegisterDTO registerDto)
    {
        bool isLoginAlreadyInUse = await dataBaseContext
            .Users.AnyAsync(u => u.Login == registerDto.Login);
        
        if (isLoginAlreadyInUse)
            throw new ProvidedDataIsInvalidError("Такой логин уже в использовании");
        
        const string loginPattern = @"^[a-zA-Z][a-zA-Z\d_]{3,15}$";
        bool isLoginValid = Regex.IsMatch(registerDto.Login, loginPattern);

        if (!isLoginValid)
            throw new ProvidedDataIsInvalidError("Логин должен быть от 4 до 16 символов и состоять из латиницы");

        string passwordPattern = @"^.*(?=.{8,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!*@#$%^+=]).*$";
        bool isPasswordValid = Regex.IsMatch(registerDto.Password, passwordPattern);

        if (!isPasswordValid)
        {
            throw new ProvidedDataIsInvalidError(
                "Пароль должен содержать хотя бы 1 заглавную букву, специальный символ, цифру и быть не менее 8 символов");
        }
        
        var user = new User
        {
            Login = registerDto.Login,
            PasswordHash = passwordHashService.GenerateHash(registerDto.Password)
        };
        await dataBaseContext.Users.AddAsync(user);
        await dataBaseContext.SaveChangesAsync();

        return new CreatedUserMeta
        {
            UserId = user.Id,
            AccessToken = tokenService.GenerateToken(user.Id)
        };
    }

    public async Task<string> Login(LoginDTO loginDto)
    {
        var user = await dataBaseContext.Users.FirstOrDefaultAsync(u => u.Login == loginDto.Login);
        
        if (user is null || user.PasswordHash != passwordHashService.GenerateHash(loginDto.Password))
            throw new AccessDeniedError("Указаны неверные данные для авторизации.");

        return tokenService.GenerateToken(user.Id);
    }
}