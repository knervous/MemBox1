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
    class MemoryListViewAdapter : BaseAdapter<MemoryModel>
    {

        public List<MemoryModel> mMemories;
        private Context mContext;

        public MemoryListViewAdapter(Context context, List<MemoryModel> memories)
        {
            mContext = context;
            mMemories = memories;
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
    }
}