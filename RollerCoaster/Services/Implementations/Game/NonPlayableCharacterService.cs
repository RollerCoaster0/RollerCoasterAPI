using Minio;
using Minio.DataModel.Args;
using RollerCoaster.DataBase;
using RollerCoaster.DataBase.Models.Game;
using RollerCoaster.DataTransferObjects.Game.Locations;
using RollerCoaster.DataTransferObjects.Game.NonPlayableCharacters;
using RollerCoaster.Services.Abstractions.Common;
using RollerCoaster.Services.Abstractions.Game;

namespace RollerCoaster.Services.Implementations.Game;

public class NonPlayableCharacterService(
    DataBaseContext dataBaseContext,
    IFileTypeValidator fileTypeValidator,
    IMinioClient minioClient
    ): INonPlayableCharacterService
{
    public async Task<NonPlayableCharacterDTO> Get(int id)
    {
        var npc = await dataBaseContext.NonPlayableCharacters.FindAsync(id);

        if (npc is null)
            throw new NotFoundError("NPC не найден.");

        return new NonPlayableCharacterDTO
        {
            Id = npc.Id,
            GameId = npc.GameId,
            Name = npc.Name,
            BaseLocationId = npc.BaseLocationId,
            BaseXPosition = npc.BaseXPosition,
            BaseYPosition = npc.BaseYPosition,
            AvatarFilePath = npc.AvatarFilePath
        };
    }

    public async Task<int> Create(int accessorUserId, NonPlayableCharacterCreationDTO npcCreationDto)
    {
        var game = await dataBaseContext.Games.FindAsync(npcCreationDto.GameId);
        if (game is null)
            throw new NotFoundError("Игра не найдена.");

        var location = await dataBaseContext.Locations.FindAsync(npcCreationDto.BaseLocationId);
        if (location is null)
            throw new NotFoundError("Локация не найдена.");

        if (location.GameId != npcCreationDto.GameId)
            throw new ProvidedDataIsInvalidError("Эта локация не принадлежит этой игре.");
        
        if (game.CreatorUserId != accessorUserId)
            throw new AccessDeniedError("У вас нет доступа к этой игре.");

        if (npcCreationDto.BaseXPosition < 0 || npcCreationDto.BaseXPosition >= location.Width)
            throw new ProvidedDataIsInvalidError(
                "BaseXPosition должна быть в диапазоне от 0 до ширины карты.");

        if (npcCreationDto.BaseYPosition < 0 || npcCreationDto.BaseYPosition >= location.Height)
            throw new ProvidedDataIsInvalidError(
                "BaseYPosition должна быть в диапазоне от 0 до высоты карты.");

        var npc = new NonPlayableCharacter
        {
            GameId = npcCreationDto.GameId,
            Name = npcCreationDto.Name,
            BaseLocationId = npcCreationDto.BaseLocationId,
            BaseXPosition = npcCreationDto.BaseXPosition,
            BaseYPosition = npcCreationDto.BaseYPosition,
            AvatarFilePath = null
        };

        await dataBaseContext.NonPlayableCharacters.AddAsync(npc);
        await dataBaseContext.SaveChangesAsync();

        return npc.Id;
    }

    public async Task LoadAvatar(
        int accessorUserId, int id, NonPlayableCharacterAvatarLoadDTO nonPlayableCharacterAvatarLoadDto)
    {
        var nonPlayableCharacter = await dataBaseContext.NonPlayableCharacters.FindAsync(id);
        if (nonPlayableCharacter is null)
            throw new NotFoundError("NPC не найден.");
        
        var game = await dataBaseContext.Games.FindAsync(nonPlayableCharacter.GameId);
        if (game is null)
            throw new NotFoundError("Игра не найдена.");
        
        if (game.CreatorUserId != accessorUserId)
            throw new AccessDeniedError("У вас нет доступа к этой игре.");
        
        if (!fileTypeValidator.ValidateImageFileType(nonPlayableCharacterAvatarLoadDto.File))
            throw new ProvidedDataIsInvalidError("Формат файла не поддерживается.");
        
        string uniqName = Guid.NewGuid().ToString("N");
        string ext = nonPlayableCharacterAvatarLoadDto.File.FileName.Split(".").Last().ToLower();
        string objectName = $"{uniqName}.{ext}";
        
        await minioClient.PutObjectAsync(
            new PutObjectArgs()
                .WithBucket("images")
                .WithObject(objectName) 
                .WithObjectSize(nonPlayableCharacterAvatarLoadDto.File.Length)
                .WithStreamData(nonPlayableCharacterAvatarLoadDto.File.OpenReadStream()));

        nonPlayableCharacter.AvatarFilePath = $"images/{objectName}";
        
        await dataBaseContext.SaveChangesAsync();
    }

    public async Task Delete(int accessorUserId, int id)
    {
        var npc = await dataBaseContext.NonPlayableCharacters.FindAsync(id);
        if (npc is null)
            throw new NotFoundError("NPC не найден.");
        
        var game = await dataBaseContext.Games.FindAsync(npc.GameId);
        if (game is null)
            throw new NotFoundError("Игра не найдена.");
        
        if (game.CreatorUserId != accessorUserId)
            throw new AccessDeniedError("У вас нет доступа к этой игре.");

        dataBaseContext.NonPlayableCharacters.Remove(npc);
        await dataBaseContext.SaveChangesAsync();
    }
}