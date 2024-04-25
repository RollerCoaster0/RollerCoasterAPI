using System.Text.RegularExpressions;
using Minio;
using Minio.DataModel.Args;
using RollerCoaster.DataBase;
using RollerCoaster.DataBase.Models.Game;
using RollerCoaster.DataTransferObjects.Game.Creation;
using RollerCoaster.DataTransferObjects.Game.Fetching;
using RollerCoaster.Services.Abstractions.Common;
using RollerCoaster.Services.Abstractions.Game;

namespace RollerCoaster.Services.Realisations.Game;

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
            BasePosition = npc.BasePosition,
            AvatarFilePath = npc.AvatarFilePath
        };
    }

    public async Task<int> Create(int accessorId, NonPlayableCharacterCreationDTO npcCreationDto)
    {
        var game = await dataBaseContext.Games.FindAsync(npcCreationDto.GameId);
        
        if (game is null)
            throw new NotFoundError("Игра не найдена.");
        
        if (game.CreatorId != accessorId)
            throw new AccessDeniedError("У вас нет доступа к этой игре.");
        
        const string basePositionPattern = @"\d+x\d+";
        bool isBasePositionValid = Regex.IsMatch(npcCreationDto.BasePosition, basePositionPattern);
        
        if (!isBasePositionValid)
            throw new ProvidedDataIsInvalidError("Пример верного формата размера: 300х200");

        var npc = new NonPlayableCharacter
        {
            GameId = npcCreationDto.GameId,
            Name = npcCreationDto.Name,
            BaseLocationId = npcCreationDto.BaseLocationId,
            BasePosition = npcCreationDto.BasePosition,
            AvatarFilePath = null
        };

        await dataBaseContext.NonPlayableCharacters.AddAsync(npc);
        await dataBaseContext.SaveChangesAsync();

        return npc.Id;
    }

    public async Task LoadAvatar(int accessorId, NonPlayableCharacterAvatarLoadDTO nonPlayableCharacterAvatarLoadDto)
    {
        var nonPlayableCharacter = await dataBaseContext.NonPlayableCharacters.FindAsync(nonPlayableCharacterAvatarLoadDto.NonPlayableCharacterId);
        if (nonPlayableCharacter is null)
            throw new NotFoundError("NPC не найден.");
        
        var game = await dataBaseContext.Games.FindAsync(nonPlayableCharacter.GameId);
        if (game is null)
            throw new NotFoundError("Игра не найдена.");
        
        if (game.CreatorId != accessorId)
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

    public async Task Delete(int accessorId, int id)
    {
        var npc = await dataBaseContext.NonPlayableCharacters.FindAsync(id);
        if (npc is null)
            throw new NotFoundError("NPC не найден.");
        
        var game = await dataBaseContext.Games.FindAsync(npc.GameId);
        if (game is null)
            throw new NotFoundError("Игра не найдена.");
        
        if (game.CreatorId != accessorId)
            throw new AccessDeniedError("У вас нет доступа к этой игре.");

        dataBaseContext.NonPlayableCharacters.Remove(npc);
        await dataBaseContext.SaveChangesAsync();
    }
}