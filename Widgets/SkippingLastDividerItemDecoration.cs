// Copyright 2018 Ruben Buniatyan
// Licensed under the MIT License. For full terms, see LICENSE in the project root.

using System;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace Macaron.Widgets
{
    /// <summary>
    /// SkippingLastDividerItemDecoration is a <see cref="RecyclerView.ItemDecoration"/> that can be used as a divider
    /// between items of a <see cref="LinearLayoutManager"/>. It doesn't decorate the last item.
    /// It supports both horizontal and vertical orientations.
    /// </summary>
    public class SkippingLastDividerItemDecoration : RecyclerView.ItemDecoration
    {
        #region Fields
        private static readonly int[] Attrs = { Android.Resource.Attribute.ListDivider };

        private readonly Rect _bounds = new Rect();
        private Drawable _divider;
        private Orientation _orientation;
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a divider <see cref="RecyclerView.ItemDecoration"/> that can be used with a <see cref="LinearLayoutManager"/>.
        /// 
        /// </summary>
        /// <param name="context">Current context, it will be used to access resources.</param>
        /// <param name="orientation">Divider orientation.</param>
        public SkippingLastDividerItemDecoration(Context context, Orientation orientation)
        {
            using (var a = context.ObtainStyledAttributes(Attrs))
            {
                _divider = a.GetDrawable(0);

                if (_divider == null)
                    Log.Warn("DividerItem", "@android:attr/listDivider was not set in the theme used for this DividerItemDecoration. Please set that attribute all call SetDrawable()");

                a.Recycle();
            }

            SetOrientation(orientation);
        }
        #endregion

        public override void GetItemOffsets(Rect outRect, View view, RecyclerView parent, RecyclerView.State state)
        {
            if (_divider == null)
            {
                outRect.SetEmpty();
                return;
            }

            if (_orientation == Orientation.Vertical)
                outRect.Set(0, 0, 0, _divider.IntrinsicHeight);
            else
                outRect.Set(0, 0, _divider.IntrinsicWidth, 0);
        }

        public override void OnDraw(Canvas c, RecyclerView parent, RecyclerView.State state)
        {
            if (parent.GetLayoutManager() == null || _divider == null)
                return;

            if (_orientation == Orientation.Vertical)
                DrawVertical(c, parent);
            else
                DrawHorizontal(c, parent);
        }

        private void DrawHorizontal(Canvas canvas, RecyclerView parent)
        {
            canvas.Save();

            int top;
            int bottom;

            if (parent.ClipToPadding)
            {
                top = parent.PaddingTop;
                bottom = parent.Height - parent.PaddingBottom;

                canvas.ClipRect(parent.PaddingLeft, top, parent.Width - parent.PaddingRight, bottom);
            }
            else
            {
                top = 0;
                bottom = parent.Height;
            }

            for (int i = 0, count = parent.ChildCount - 1; i < count; i++)
            {
                var child = parent.GetChildAt(i);

                parent.GetLayoutManager().GetDecoratedBoundsWithMargins(child, _bounds);

                var right = _bounds.Right + (int)Math.Round(child.TranslationX);
                var left = right - _divider.IntrinsicWidth;

                _divider.SetBounds(left, top, right, bottom);
                _divider.Draw(canvas);
            }

            canvas.Restore();
        }

        private void DrawVertical(Canvas canvas, RecyclerView parent)
        {
            canvas.Save();

            int left;
            int right;

            if (Build.VERSION.SdkInt < BuildVersionCodes.Lollipop || parent.ClipToPadding)
            {
                left = parent.PaddingLeft;
                right = parent.Width - parent.PaddingRight;

                canvas.ClipRect(left, parent.PaddingTop, right, parent.Height - parent.PaddingBottom);
            }
            else
            {
                left = 0;
                right = parent.Width;
            }

            for (int i = 0, count = parent.ChildCount - 1; i < count; i++)
            {
                var child = parent.GetChildAt(i);

                parent.GetDecoratedBoundsWithMargins(child, _bounds);

                var bottom = _bounds.Bottom + (int)Math.Round(child.TranslationY);
                var top = bottom - _divider.IntrinsicHeight;

                _divider.SetBounds(left, top, right, bottom);
                _divider.Draw(canvas);
            }

            canvas.Restore();
        }

        /// <summary>
        /// Sets the <see cref="Drawable"/> for this divider.
        /// </summary>
        /// <param name="drawable">Drawable that should be used as a divider.</param>
        public void SetDrawable(Drawable drawable) => _divider = drawable ?? throw new ArgumentNullException(nameof(drawable));

        /// <summary>
        /// Sets the orientation for this divider.
        /// This should be called if <see cref="RecyclerView.LayoutManager"/> changes orientation.
        /// </summary>
        /// <param name="orientation">Divider orientation.</param>
        public void SetOrientation(Orientation orientation) => _orientation = orientation;
    }
}
