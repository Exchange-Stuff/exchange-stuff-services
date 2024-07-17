using System.Collections.Concurrent;

namespace ExchangeStuff.Hubs;

public static class DictonaryChat
{
    private static readonly ConcurrentDictionary<string, string> _connections =
       new ConcurrentDictionary<string, string>();

    public static ConcurrentDictionary<string, string> connections => _connections;
}
