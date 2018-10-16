// Copyright 2018 Ruben Buniatyan
// Licensed under the MIT License. For full terms, see LICENSE in the project root.

using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace Macaron.Widgets
{
    public abstract class SectionedAdapter : RecyclerView.Adapter
    {
        #region Methods
        public int GetGlobalPosition(int section, int positionInSection)
        {
            var offset = 0;

            for (var i = 0; i < section; i++)
                offset += GetItemCountInSection(i);

            return positionInSection + offset;
        }

        public abstract int GetItemCountInSection(int section);

        public override int GetItemViewType(int position)
        {
            var (positionInSection, section) = GetPositionInSection(position);

            return GetItemViewType(section, positionInSection);
        }

        public abstract int GetItemViewType(int section, int positionInSection);

        public virtual (int positionInSection, int section) GetPositionInSection(int position)
        {
            var offset = 0;
            var section = 0;
            var sectionCount = SectionCount;

            do
            {
                var itemCount = GetItemCountInSection(section);

                if (position < itemCount + offset)
                    return (position - offset, section);

                offset += itemCount;
            } while (++section < sectionCount);

            return (RecyclerView.NoPosition, RecyclerView.NoPosition);
        }

        public virtual int GetSection(int position)
        {
            var offset = 0;

            for (int section = 0, count = SectionCount; section < count; section++)
            {
                offset += GetItemCountInSection(section);

                if (position < offset)
                    return section;
            }

            return RecyclerView.NoPosition;
        }

        public virtual void NotifyItemChanged(int section, int positionInSection) => NotifyItemChanged(GetGlobalPosition(section, positionInSection));

        public virtual void NotifyItemInserted(int section, int positionInSection) => NotifyItemInserted(GetGlobalPosition(section, positionInSection));

        public virtual void NotifyItemRemoved(int section, int positionInSection) => NotifyItemRemoved(GetGlobalPosition(section, positionInSection));

        public virtual void NotifySectionChanged(int section)
        {
            var position = GetGlobalPosition(section, 0);

            NotifyItemRangeChanged(position, position + GetItemCountInSection(section));
        }

        public virtual void NotifySectionInserted(int section)
        {
            var position = GetGlobalPosition(section, 0);

            NotifyItemRangeInserted(position, position + GetItemCountInSection(section));
        }

        public virtual void NotifySectionRemoved(int section)
        {
            var position = GetGlobalPosition(section, 0);

            NotifyItemRangeRemoved(position, position + GetItemCountInSection(section));
        }
        #endregion

        #region Properties
        public override int ItemCount
        {
            get
            {
                var itemCount = 0;

                for (int section = 0, count = SectionCount; section < count; section++)
                    itemCount += GetItemCountInSection(section);

                return itemCount;
            }
        }

        public abstract int SectionCount { get; }
        #endregion

        #region View Holders
        protected class FooterViewHolder : RecyclerView.ViewHolder
        {
            public FooterViewHolder(View itemView) : base(itemView) => FooterText = (TextView)itemView;

            public TextView FooterText { get; }
        }

        protected class HeaderViewHolder : RecyclerView.ViewHolder
        {
            public HeaderViewHolder(View itemView) : base(itemView) => HeaderText = (TextView)itemView;

            public TextView HeaderText { get; }
        }
        #endregion
    }
}
