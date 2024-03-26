using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using RollerCoaster.DataTransferObjects;
using RollerCoaster.Models;

namespace RollerCoaster.Services;

public class InvalidLoginError(string message): Exception(message)
{
    
} 

public class AuthService(DataBaseContext dataBaseContext, ITokenService tokenService) : IAuthService
{
    public async Task<CreatedUserMeta> Register(RegisterDTO registerDto)
    {
        bool isLoginAlreadyInUse = await dataBaseContext
            .Users.AnyAsync(u => u.Login == registerDto.Login);
        
        if (isLoginAlreadyInUse)
            throw new InvalidLoginError("Такой логин уже в использовании");
        
        string loginPattern = @"^[a-zA-Z][a-zA-Z\d_]{3,15}$";
        bool isLoginValid = Regex.IsMatch(registerDto.Login, loginPattern);

        if (!isLoginValid)
        {
            throw new InvalidLoginError("Логин должен быть от 4 до 16 символов и состоять из латиницы");
        }

        string passwordPattern = @"^.*(?=.{8,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!*@#$%^&+=]).*$";
        bool isPasswordValid = Regex.IsMatch(registerDto.Password, passwordPattern);

        if (!isPasswordValid)
        {
            throw new InvalidLoginError(
                "Пароль должен содержать хотя бы 1 заглавную букву, специальный символ, цифру и быть не менее 8 символов");
        }
        
        var user = new User
        {
            Login = registerDto.Login,
            PasswordHash = registerDto.Password // TODO: сделать хэш
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

        if (user is null)
            throw new InvalidLoginError("Такого логина не нашли");
        
        if (user.PasswordHash != loginDto.Password) // TODO: сделать хэш
            throw new InvalidLoginError("Пароль неверный");

        return tokenService.GenerateToken(user.Id);
    }
}