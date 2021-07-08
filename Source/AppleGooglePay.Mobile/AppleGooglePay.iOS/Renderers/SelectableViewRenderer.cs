using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using AppleGooglePay.iOS.Renderers;
using AppleGooglePay.Mobile.Controls;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(SelectableViewCell), typeof(SelectableViewRenderer))]
namespace AppleGooglePay.iOS.Renderers
{
	public class SelectableViewRenderer : ViewCellRenderer
	{
		public override UITableViewCell GetCell(Cell item, UITableViewCell reusableCell, UITableView tv)
		{
			var cell = base.GetCell(item, reusableCell, tv);
			var view = item as SelectableViewCell;

			cell.SelectedBackgroundView = new UIView
			{
				BackgroundColor = view.SelectedBackgroundColor.ToUIColor()
			};
			cell.BackgroundView = new UIView
			{
				BackgroundColor = view.UnselectedBackgroundColor.ToUIColor()
			};

			return cell;
		}

		public override void SetBackgroundColor(UITableViewCell tableViewCell, Cell cell, UIColor color)
		{
			if (cell is SelectableViewCell viewCell)
			{
				viewCell.Selected = tableViewCell.Selected;
			}
			base.SetBackgroundColor(tableViewCell, cell, color);
		}
	}
}