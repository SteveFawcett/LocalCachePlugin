using BroadcastPluginSDK.abstracts;
using BroadcastPluginSDK.Interfaces;
using LocalCachePlugin.Properties;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace LocalCachePlugin;

public class Cache : BroadcastCacheBase
{
    private const string PluginName = "LocalCachePlugin";
    private const string PluginDescription = "Local Cache PluginBase.";
    private const string Stanza = "Local";

    private static Dictionary<string, string> _internalCache = [];
    private static readonly CachePage s_infoPage = new();
    private static readonly Image s_icon = Resources.green;
    private ILogger<IPlugin> _logger;

    public Cache(IConfiguration configuration , ILogger<IPlugin> logger) :
        base(configuration, s_infoPage, s_icon, PluginName, Stanza , PluginDescription)
    {
        _logger = logger;
        _internalCache = new Dictionary<string, string>();
        _logger.LogInformation(PluginDescription);
    }

    public override void Clear()
    {
        _internalCache = [];
    }

    public override void Write(Dictionary<string, string> data)
    {
        foreach (var kv in data) _internalCache[kv.Key] = kv.Value;

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