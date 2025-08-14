using BroadcastPluginSDK;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;

namespace LocalCachePlugin
{
    internal class InternalCache : PluginData
    {

    }
    public class Cache : BroadcastPlugin, ICache
    {
        private InternalCache _internalCache = [];
        public override UserControl? InfoPage { get => _infoPage; set => _infoPage = value ?? throw new NullReferenceException(); }
        private UserControl _infoPage = new CachePage();
        public bool Master { get; set; } = false;
        public override string Stanza => "Local";

        public Cache() : base()
        {
            Name = "Local Cache";
            Icon = Properties.Resources.green;
            Description = "Simple local cache";

        }

        public override string Start()
        {
            var value = Configuration?["master"];

            if (value != null)
            {
                if (bool.TryParse(value, out var result))
                {
                    Master = result;
                    return($"{Name} plugin marked as Master");
                }
                else
                {
                    return($"{Name} Invalid boolean value for 'master': {value}");
                }
            }
            else
            {
                return($"{Name} 'master' key not found in configuration");
            }
        }


        public void Clear()
        {
            _internalCache = [];
        }

        public IEnumerable<KeyValuePair<string, string>> Read(List<string> values)
        {
            foreach (var value in values)
            {
                if (_internalCache.TryGetValue(value, out var data))
                {
                    yield return new KeyValuePair<string, string>(value, data);
                }
            }
        }

        public void Write(PluginData data)
        {
            var mergedDict = _internalCache.Concat(data).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            if( mergedDict is InternalCache c)
            {
                _internalCache = c;
            }

            if (_infoPage is CachePage p)
            {
                p.redraw( mergedDict );
            }
        }
    }
}
