using System;
using System.ComponentModel;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Support.V7.Widget;
using Android.Widget;

namespace Xamarin.Forms.Platform.Android.AppCompat
{
	public class SwitchRenderer : ViewRenderer<Switch, SwitchCompat>, CompoundButton.IOnCheckedChangeListener
	{
		bool _disposed;
		ColorStateList defaultOnColor;
		PorterDuff.Mode defaultMode;

		public SwitchRenderer(Context context) : base(context)
		{
			AutoPackage = false;
		}

		[Obsolete("This constructor is obsolete as of version 2.5. Please use SwitchRenderer(Context) instead.")]
		public SwitchRenderer()
		{
			AutoPackage = false;
		}

		void CompoundButton.IOnCheckedChangeListener.OnCheckedChanged(CompoundButton buttonView, bool isChecked)
		{
			((IViewController)Element).SetValueFromRenderer(Switch.IsToggledProperty, isChecked);
			UpdateOnColor();
		}

		public override SizeRequest GetDesiredSize(int widthConstraint, int heightConstraint)
		{
			SizeRequest sizeConstraint = base.GetDesiredSize(widthConstraint, heightConstraint);

			if (sizeConstraint.Request.Width == 0)
			{
				int width = widthConstraint;
				if (widthConstraint <= 0)
					width = (int)Context.GetThemeAttributeDp(global::Android.Resource.Attribute.SwitchMinWidth);

				sizeConstraint = new SizeRequest(new Size(width, sizeConstraint.Request.Height), new Size(width, sizeConstraint.Minimum.Height));
			}

			return sizeConstraint;
		}

		protected override SwitchCompat CreateNativeControl()
		{
			return new SwitchCompat(Context);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && !_disposed)
			{
				_disposed = true;

				if (Element != null)
					Element.Toggled -= HandleToggled;

				Control.SetOnCheckedChangeListener(null);
			}

			base.Dispose(disposing);
		}

		protected override void OnElementChanged(ElementChangedEventArgs<Switch> e)
		{
			base.OnElementChanged(e);

			if (e.OldElement != null)
				e.OldElement.Toggled -= HandleToggled;

			if (e.NewElement != null)
			{
				if (Control == null)
				{
					SwitchCompat aswitch = CreateNativeControl();
					aswitch.SetOnCheckedChangeListener(this);
					SetNativeControl(aswitch);
				}
				else
					UpdateEnabled(); // Normally set by SetNativeControl, but not when the Control is reused.

				e.NewElement.Toggled += HandleToggled;
				Control.Checked = e.NewElement.IsToggled;
				defaultOnColor = Control.TrackTintList;
				defaultMode = Control.TrackTintMode;
				UpdateOnColor();
			}
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == Switch.OnColorProperty.PropertyName)
				UpdateOnColor();
		}

		private void UpdateOnColor()
		{
			if (Element != null)
			{
				if (Element.OnColor == Color.Default)
				{
					Control.TrackTintList = defaultOnColor;
					Control.TrackTintMode = defaultMode;
				}
				else
				{
					StateListDrawable drawable = new StateListDrawable();
					drawable.AddState(new int[] { global::Android.Resource.Attribute.StateChecked }, new ColorDrawable(Element.OnColor.ToAndroid()));
					drawable.AddState(new int[] { }, new ColorDrawable(Color.Red.ToAndroid()));

					Control.ThumbDrawable = drawable;
					
					//if (Control.Checked)
					//	Control.TrackTintList = ColorStateList.ValueOf(Element.OnColor.ToAndroid());
					//else
					//{
					//	Control.TrackTintList = defaultOnColor;
					//}
				}
			}
		}

		void HandleToggled(object sender, EventArgs e)
		{
			Control.Checked = Element.IsToggled;
		}

		void UpdateEnabled()
		{
			Control.Enabled = Element.IsEnabled;
		}
	}
}