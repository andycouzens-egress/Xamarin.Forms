using System;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Platform.Android;
using Xamarin.Platform;

namespace Xamarin.Forms
{
	public class RendererToHandlerShim : Xamarin.Platform.IViewHandler
	{
		readonly IVisualElementRenderer _visualElementRenderer;

		public static RendererToHandlerShim CreateShim(object renderer)
		{
			if (renderer is IVisualElementRenderer ivr)
				return new RendererToHandlerShim(ivr);

			return null;
		}

		public RendererToHandlerShim(IVisualElementRenderer visualElementRenderer)
		{
			_visualElementRenderer = visualElementRenderer;
		}

		public object NativeView => _visualElementRenderer.View;

		public bool HasContainer
		{
			get;
			set;
		}

		public SizeRequest GetDesiredSize(double widthConstraint, double heightConstraint)
		{
			var returnValue = _visualElementRenderer.GetDesiredSize((int)widthConstraint, (int)heightConstraint);
			return returnValue;
		}

		public void SetFrame(Rectangle frame)
		{
			var context = _visualElementRenderer.View.Context;
			var width = MeasureSpecFactory.MakeMeasureSpec((int)Platform.Android.ContextExtensions.ToPixels(context, frame.Width), global::Android.Views.MeasureSpecMode.Exactly);
			var height = MeasureSpecFactory.MakeMeasureSpec((int)Platform.Android.ContextExtensions.ToPixels(context, frame.Height), global::Android.Views.MeasureSpecMode.Exactly);

			_visualElementRenderer.View.Measure(width, height);
		}

		public void SetView(IView view)
		{
			_visualElementRenderer.SetElement((VisualElement)view);
		}

		public void TearDown()
		{
			_visualElementRenderer.Dispose();
		}

		public void UpdateValue(string property)
		{
			if(property == "Frame")
			{
				SetFrame(_visualElementRenderer.Element.Bounds);
			}

			//_visualElementRenderer.Element.
			// need to invoke something here
			//_visualElementRenderer.ElementPropertyChanged?.in
		}
	}
}
