using BroadcastPluginSDK;
using System.Windows.Forms;
using System.Diagnostics;

namespace LocalCachePlugin
{
    internal class InternalCache : PluginData
    {

    }

    public class Cache : BroadcastPlugin, ICache
    {
        private InternalCache internalCache = new InternalCache();
        public override UserControl? InfoPage { get => _infoPage; set => _infoPage = value ?? throw new NullReferenceException(); }
        private UserControl _infoPage = new CachePage();

        public Cache()
        {
            Name = "Local Cache";
            Icon = Properties.Resources.green;
            Description = "Simple local cache";

        }

        public void Clear()
        {
                    internalCache = new InternalCache();
        }

        public IEnumerable<KeyValuePair<string, string>> Read(List<string> values)
        {
            foreach (var value in values)
            {
                if (internalCache.TryGetValue(value, out var data))
                {
                    yield return new KeyValuePair<string, string>(value, data);
                }
            }
        }

        public void Write(PluginData data)
        {
            var mergedDict = internalCache.Concat(data).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            if( mergedDict is InternalCache c)
            {
                internalCache = c;
            }

            if (_infoPage is CachePage p)
            {
                p.redraw( mergedDict );
            }
        }
    }
}
