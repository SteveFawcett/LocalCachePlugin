using BroadcastPluginSDK;
using LocalCachePlugin.Properties;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using System.Diagnostics;

namespace LocalCachePlugin;

public class Cache : BroadcastCacheBase
{
    private static Dictionary<string, string> _internalCache = [];
    private static readonly CachePage s_infoPage = new CachePage();
    private static readonly Image s_icon = Resources.green;

    public Cache(IConfiguration configuration) : base(null, s_infoPage, s_icon, "Local Cache", "Local", true, "Simple Cache")
    {
        _internalCache = new Dictionary<string, string>();

        var config = configuration.GetSection("Local").GetChildren();

        // Fix for CS0021: Use LINQ to find the "Master" configuration section
        var masterSection = config.FirstOrDefault(section => section.Key == "Master");
        Master = masterSection != null && bool.TryParse(masterSection.Value, out var masterValue) && masterValue;
    }


    public override void Clear()
    {
        _internalCache = [];
    }

    public override void Write(Dictionary<string, string> data)
    {
        foreach (var kv in data)
        {
            _internalCache[kv.Key] = kv.Value;
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
        foreach (var value in values)
        {
            yield return ReadValue(value);
        }
    }

    public static IEnumerable<KeyValuePair<string, string>> Read()
    {
        foreach (var kvp in _internalCache)
        {
            yield return new KeyValuePair<string, string>(kvp.Key, kvp.Value);
        }
    }
    public static KeyValuePair<string, string> ReadValue(string value)
    {
        if (_internalCache.TryGetValue(value, out var data))
            return new KeyValuePair<string, string>(value, data);
        return new KeyValuePair<string, string>(value, string.Empty);
    }
}