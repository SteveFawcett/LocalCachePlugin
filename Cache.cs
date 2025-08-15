using BroadcastPluginSDK;
using LocalCachePlugin.Properties;

namespace LocalCachePlugin;

public class Cache : BroadcastCacheBase
{
    private static Dictionary<string, string> _internalCache = [];
    private UserControl _infoPage = new CachePage();

    public Cache()
    {
        Master = false;
        if (_infoPage is CachePage p)
        {
            p.pName = "Local Cache";
            Icon = p.pIcon = Resources.green;
            p.pDescription = "Simple local cache";
        }
    }

    public override UserControl? InfoPage
    {
        get => _infoPage;
        set => _infoPage = value ?? throw new NullReferenceException();
    }

    public override string Stanza => "Local";

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

        if (_infoPage is CachePage p) p.redraw(_internalCache);
    }

    public override List<KeyValuePair<string, string>> CacheReader(List<string> values)
    {
        if (values.Count == 0) return Read().ToList();

        return Read( values).ToList();
    }

    public override string Start()
    {
        var value = Configuration?["master"];

        if (value != null)
        {
            if (bool.TryParse(value, out var result))
            {
                Master = result;
                return $"{Name} plugin marked as Master";
            }

            return $"{Name} Invalid boolean value for 'master': {value}";
        }

        return $"{Name} 'master' key not found in configuration";
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
        foreach (var kvp in _internalCache )
        {
            yield return new KeyValuePair<string, string>( kvp.Key, kvp.Value );
        }
    }

    public static KeyValuePair<string, string> ReadValue( string value)
    {
        if (_internalCache.TryGetValue(value, out var data))
            return new KeyValuePair<string, string>(value, data);
        return new KeyValuePair<string, string>(value, string.Empty);
    }
}