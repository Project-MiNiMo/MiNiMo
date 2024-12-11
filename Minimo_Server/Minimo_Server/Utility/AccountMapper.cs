using MinimoServer;
using MinimoServer.Models;
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
            Experience = account.Experience
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
            LastLogin = DateTime.UtcNow // Setting LastLogin to current date-time
        };
    }

    // Convert List of Players to List of PlayerDTOs
    public static IEnumerable<AccountDTO> ToAccountDTOs(IEnumerable<Account> players)
    {
        return players.Select(player => ToAccountDTO(player)).ToList();
    }
}