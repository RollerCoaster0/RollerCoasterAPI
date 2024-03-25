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
        
        // TODO: сделать валидацию логина

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
            throw new InvalidLoginError("Пароль невернйы");

        return tokenService.GenerateToken(user.Id);
    }
}