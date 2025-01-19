using MinimoServer.Models;
using MinimoShared;

namespace MinimoServer.Utility;

public class QuestMapper
{
    public static QuestDTO ToQuestDTO(Quest quest)
    {
        return new QuestDTO
        {
            Id = quest.Id,
            QuestType = quest.QuestType,
        };
    }
}