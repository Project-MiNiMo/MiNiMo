using MinimoServer;
using MinimoServer.Models;
using MinimoServer.Shared;
using MinimoShared;

public static class AccountMapper
{
    // Convert Player to PlayerDTO
    public static AccountDTO ToAccountDTO(Account account)
    {
        return new AccountDTO
        {
            ID = account.Id,
            Nickname = account.Nickname,
            Level = account.Level,
            Experience = account.Experience,
            Currency = CurrencyMapper.ToCurrencyDTO(account.Currency),
            Buildings = account.Buildings?.Select(BuildingMapper.ToBuildingDTO).ToList() ?? new List<BuildingDTO>(),
            Items = account.Items?.Select(ItemMapper.ToItemDTO).ToList() ?? new List<ItemDTO>()
        };
    }

    // Convert PlayerDTO to Player
    public static Account ToAccount(AccountDTO accountDto)
    {
        return new Account
        {
            Id = accountDto.ID,
            Nickname = accountDto.Nickname,
            Level = accountDto.Level,
            Experience = accountDto.Experience,
            LastLogin = DateTime.UtcNow, // Setting LastLogin to current date-time
            Currency = accountDto.Currency != null ? CurrencyMapper.ToCurrency(accountDto.Currency) : null,
            Buildings = accountDto.Buildings?.Select(BuildingMapper.ToBuilding).ToList(),
            Items = accountDto.Items?.Select(ItemMapper.ToItem).ToList()
        };
    }

    // Convert List of Players to List of PlayerDTOs
    public static IEnumerable<AccountDTO> ToAccountDTOs(IEnumerable<Account> players)
    {
        return players.Select(player => ToAccountDTO(player)).ToList();
    }
    
    public static IEnumerable<Account> ToAccounts(IEnumerable<AccountDTO> playerDtos)
    {
        return playerDtos.Select(ToAccount).ToList();
    }
}