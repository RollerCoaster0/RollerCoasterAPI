using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RollerCoaster.Controllers.Session;

[Route("sessions")]
[ApiController]
[Authorize]
public class GameSessionsController: ControllerBase
{
    // TODO: implement
}