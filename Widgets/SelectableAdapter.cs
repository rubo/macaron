using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Android.Support.V7.Widget;

namespace Macaron.Widgets
{
    public abstract class SelectableAdapter : RecyclerView.Adapter
    {
        #region Selection Methods
        protected virtual void OnItemSelected(int position)
        {
            if (IsMultiSelect)
            {
                if (SelectedPositions.Contains(position))
                {
                    SelectedPositions.Remove(position);

                    NotifyItemChanged(position);
                }
                else
                    SelectItem(position);
            }
            else
                SelectItem(position);

            var handler = SelectedPositionChanged;

            handler?.Invoke(this, new SelectedPositionChangedEventArgs(SelectedPositions));
        }

        public virtual void SelectItem(int position)
        {
            if (IsMultiSelect)
            {
                if (!SelectedPositions.Contains(position))
                    SelectedPositions.Add(position);
            }
            else
            {
                if (SelectedPositions.Count == 0)
                    SelectedPositions.Add(position);
                else
                {
                    var selectedPosition = SelectedPositions[0];

                    SelectedPositions[0] = position;

                    NotifyItemChanged(selectedPosition);
                }
            }

            NotifyItemChanged(position);
        }
        #endregion

        #region Properties
        public bool IsMultiSelect { get; set; }

        protected IList<int> SelectedPositions { get; set; } = new List<int>();
        #endregion

        #region Events
        public event EventHandler<SelectedPositionChangedEventArgs> SelectedPositionChanged;

        public class SelectedPositionChangedEventArgs : EventArgs
        {
            public SelectedPositionChangedEventArgs(IList<int> selectedPositions) => SelectedPositions = new ReadOnlyCollection<int>(selectedPositions);

            public IList<int> SelectedPositions { get; }
        }
        #endregion
    }
}
