using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace MemoryBox
{
    public class MemoryListViewAdapter : BaseAdapter<MemoryModel>
    {

        private List<MemoryModel> mMemories = new List<MemoryModel>();
        private Context mContext;



        public MemoryListViewAdapter(Context context)
        {
            mContext = context;
        }

        public override MemoryModel this[int position]
        {
            get
            {
                return mMemories[position];
            }
        }

        public override int Count
        {
            get { return mMemories.Count; }
        }

        public List<MemoryModel> Memories
        {
            get { return mMemories; }
            set { mMemories = value;  }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View row = convertView;
            if(row == null)
            {
                row = LayoutInflater.From(mContext).Inflate(Resource.Layout.MemoryListView, null, false);
            }
            TextView boxName = row.FindViewById<TextView>(Resource.Id.MemBoxDisplay);
            boxName.Text = mMemories[position].Name;

            return row;
        }

        public void Add (MemoryModel item)
        {
            mMemories.Add(item);
            NotifyDataSetChanged();
        }

        public void Clear()
        {
            mMemories.Clear();
            NotifyDataSetChanged();
        }

        public void Remove (MemoryModel item)
        {
            mMemories.Remove(item);
            NotifyDataSetChanged();
        }
    }
}