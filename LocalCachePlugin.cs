using BroadcastPluginSDK.abstracts;
using BroadcastPluginSDK.Interfaces;
using LocalCachePlugin.Properties;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace LocalCachePlugin;

public class LocalCachePlugin : BroadcastCacheBase
{
    private const string Stanza = "Local";

    private static ConcurrentDictionary<string, string> _internalCache = new();
    private static readonly CachePage s_infoPage = new();
    private static readonly Image s_icon = Resources.green;
    private ILogger<IPlugin> _logger;

    public LocalCachePlugin(IConfiguration configuration , ILogger<IPlugin> logger) :
        base(configuration, s_infoPage, s_icon,  Stanza )
    {
        _logger = logger;
        
    }

    public override void Clear()
    {
        _internalCache = [];
    }

    public override void Write(Dictionary<string, string> data)
    {
        foreach (var kv in data) _internalCache[kv.Key] = kv.Value;
        // Removed reassignment of _internalCache to avoid thread-safety issues.
    }

    public override void Clear()
    {
        lock (_cacheLock)
        {
            _internalCache.Clear();
        }
    }

    public override void Write(Dictionary<string, string> data)
    {
        lock (_cacheLock)
        {
            foreach (var kv in data) _internalCache[kv.Key] = kv.Value;
        }
        s_infoPage.redraw(_internalCache);
    }

    public override List<KeyValuePair<string, string>> CacheReader(List<string> values)
    {
        if (values.Count == 0) return Read().ToList();

        return Read(values).ToList();
    }

    public static IEnumerable<KeyValuePair<string, string>> Read(List<string> values)
    {
        foreach (var value in values) yield return ReadValue(value);
    }

    public static IEnumerable<KeyValuePair<string, string>> Read()
    {
        foreach (var kvp in _internalCache) yield return new KeyValuePair<string, string>(kvp.Key, kvp.Value);
    }

    public static KeyValuePair<string, string> ReadValue(string value)
    {
        if (_internalCache.TryGetValue(value, out var data))
            return new KeyValuePair<string, string>(value, data);
        return new KeyValuePair<string, string>(value, string.Empty);
    }
}