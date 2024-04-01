namespace RollerCoaster.Services.Abstractions.Common;

// Должно выкидываться, когда пользователь сует нос туда, куда сувать нос ему не следует 
// 403 http error (Forbidden)
public class AccessDeniedError(string message): Exception(message);