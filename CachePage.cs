using BroadcastPluginSDK.Interfaces;

namespace LocalCachePlugin;

public partial class CachePage : UserControl, IInfoPage
{
    internal string? pDescription;
    internal Image? pIcon;
    internal string? pName;
    internal string? pVersion;

    public CachePage()
    {
        InitializeComponent();
    }

    public Image? Icon
    {
        get => pIcon;
        set => pIcon = value;
    }

    public new string Name
    {
        set => pName = value;
        get => pName ?? string.Empty;
    }

    public string Version
    {
        set => pVersion = value;
        get => pVersion ?? string.Empty;
    }

    public string Description
    {
        set => pDescription = value;
        get => pDescription ?? string.Empty;
    }

    public Control GetControl()
    {
        return this;
    }

    public void redraw(Dictionary<string, string> myDict)
    {
        if (listView1.InvokeRequired)
        {
            if (listView1 != null && listView1.IsHandleCreated && !listView1.IsDisposed)
                listView1.Invoke(() => redraw(myDict));
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