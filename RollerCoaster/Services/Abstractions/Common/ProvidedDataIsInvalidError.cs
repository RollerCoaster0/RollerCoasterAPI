namespace RollerCoaster.Services.Abstractions.Common;

// Должно выкидываться, когда пользователь ввел некорретные данные (например, слишком длинный ник)
// 400 http error (BadRequest)
public class ProvidedDataIsInvalidError(string message): Exception(message);