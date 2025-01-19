using MinimoServer.Services;

namespace MinimoServer.Controllers;

public class QuestController(GameDbContext context, TimeService timeService, TableDataService tableDataService)
    : BaseController(context, timeService, tableDataService);