using System.Diagnostics;
using BroadcastPluginSDK.abstracts;
using LocalCachePlugin.Properties;
using Microsoft.Extensions.Configuration;

namespace LocalCachePlugin;

public class Cache : BroadcastCacheBase
{
    private static Dictionary<string, string> _internalCache = [];
    private static readonly CachePage s_infoPage = new();
    private static readonly Image s_icon = Resources.green;

    public Cache(IConfiguration configuration) : base(configuration, s_infoPage, s_icon, "Local Cache", "Local" ,
        "Simple Cache")
    {
        _internalCache = new Dictionary<string, string>();
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