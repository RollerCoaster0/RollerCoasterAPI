namespace RollerCoaster.Services.Abstractions.Common;

// Должно выкидываться, когда не получилось найти нужные данные
// 404 http error (NotFound)
public class NotFoundError(string message): Exception(message);