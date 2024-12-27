using MinimoServer.Models;

namespace MinimoServer.Services;

public class TimeService
{
    private readonly GameDbContext _context;
    public DateTime CurrentTime => DateTime.UtcNow + _offset;
    private TimeSpan _offset = TimeSpan.Zero;
    
    public TimeService(GameDbContext context)
    {
        _context = context;
        if (!_context.Time.Any())
        {
            _context.Time.Add(new Time { Offset = TimeSpan.Zero });
            _context.SaveChanges();
        }
        _offset = _context.Time.First().Offset;
    }
    
    private static readonly object _dbLock = new object();

    public void SetTime(DateTime targetTime)
    {
        lock (_dbLock)
        {
            _offset = _context.Time.First().Offset = targetTime - DateTime.UtcNow;
            _context.SaveChanges();
        }
    }
}