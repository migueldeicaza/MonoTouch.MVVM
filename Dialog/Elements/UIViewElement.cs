//
// UIViewElement.cs
//
// Author:
//   Miguel de Icaza (miguel@gnome.org)
//
// Copyright 2010, Novell, Inc.
//
// Code licensed under the MIT X11 license
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
namespace MonoTouch.Dialog
{
	using System.Drawing;
	using MonoTouch.Foundation;
	using MonoTouch.UIKit;

	/// <summary>
	///   This element can be used to insert an arbitrary UIView
	/// </summary>
	/// <remarks>
	///   There is no cell reuse here as we have a 1:1 mapping
	///   in this case from the UIViewElement to the cell that
	///   holds our view.
	/// </remarks>
	public class UIViewElement : Element<UIView>, IElementSizing
	{
		public CellFlags Flags;

		public enum CellFlags
		{
			Transparent = 1,
			DisableSelection = 2
		}

		/// <summary>
		///   Constructor
		/// </summary>
		/// <param name="caption">
		/// The caption, only used for IRoot that might want to summarize results
		/// </param>
		/// <param name="view">
		/// The view to display
		/// </param>
		/// <param name="transparent">
		/// If this is set, then the view is responsible for painting the entire area,
		/// otherwise the default cell paint code will be used.
		/// </param>
		public UIViewElement(string caption, UIView view, bool transparent) : base(caption)
		{
			Value = view;
			Flags = transparent ? CellFlags.Transparent : 0;
		}

		public override void InitializeCell(UITableView tableView)
		{
			if ((Flags & CellFlags.Transparent) != 0)
			{
				Cell.BackgroundColor = UIColor.Clear;

				//
				// This trick is necessary to keep the background clear, otherwise
				// it gets painted as black
				//
				Cell.BackgroundView = new UIView(RectangleF.Empty) { BackgroundColor = UIColor.Clear };
			}
			if ((Flags & CellFlags.DisableSelection) != 0)
				Cell.SelectionStyle = UITableViewCellSelectionStyle.None;

			if (Value != null)
				Cell.ContentView.AddSubview(Value);
		}

		public virtual float GetHeight(UITableView tableView, NSIndexPath indexPath)
		{
			if (Value != null)
				return Value.Bounds.Height;
			
			return 0;
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			if (disposing)
			{
				Value.Dispose();
				Value = null;
			}
		}
	}
}

