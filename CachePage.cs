using PluginBase;
using System.Drawing;
using System.Windows.Forms;

namespace LocalCachePlugin
{
    public partial class CachePage : UserControl 
    {
        private string? pName;
        private string? pDescription;
        private string? pVersion;
        public Image? Icon
        {
            get;set;
        }

        public new string Name { set => pName = value; get => pName ?? string.Empty; }
        public string Version { set => pVersion = value; get => pVersion ?? string.Empty; }
        public string Description { set => pDescription = value; get => pDescription ?? string.Empty; }

        public CachePage()
        {
            InitializeComponent();
        }

        public void redraw(Dictionary<string, string> myDict)
        {
            if (listView1.InvokeRequired)
            {
                listView1.Invoke(new Action(() => redraw( myDict)));
                return;
            }

            // All UI interaction happens below this line — safely on the UI thread

            if (listView1.View != View.Details)
                listView1.View = View.Details;

            if (listView1.Columns.Count < 2)
            {
                listView1.Columns.Clear();
                listView1.Columns.Add("Key", 250);
                listView1.Columns.Add("Value", 150);
            }

            foreach (var kvp in myDict)
            {
                if (kvp.Value == null)
                    continue;

                var items = listView1.Items.Find(kvp.Key, false);

                if (items.Length > 0)
                {
                    items[0].SubItems[1].Text = kvp.Value;
                }
                else
                {
                    var item = new ListViewItem(kvp.Key)
                    {
                        Name = kvp.Key
                    };
                    item.SubItems.Add(kvp.Value);
                    listView1.Items.Add(item);
                }
            }

            //listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
        }

    }
}
