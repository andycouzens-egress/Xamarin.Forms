using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using AndroidX.AppCompat.App;
using Xamarin.Forms;
using AndroidX.Core.Widget;
using Xamarin.Platform;

namespace Sample.Droid
{
	[Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
	public class MainActivity : AppCompatActivity
	{
		ViewGroup _page;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			Xamarin.Essentials.Platform.Init(this, savedInstanceState);

			Xamarin.Forms.Forms.Init(this, savedInstanceState);
			SetContentView(Resource.Layout.activity_main);

			AndroidX.AppCompat.Widget.Toolbar toolbar = FindViewById<AndroidX.AppCompat.Widget.Toolbar>(Resource.Id.toolbar);
			SetSupportActionBar(toolbar);

			_page = FindViewById<ViewGroup>(Resource.Id.pageLayout);

			var app = new MyApp();
			Add(app.CreateView());
		}

		void Add(params IView[] views)
		{
			foreach (var view in views)
			{
				var result = view.ToNative(this);
				view.Arrange(new Rectangle(0, 0, 300, 50));
				_page.AddView(result);
			}
		}

		public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
		{
			Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

			base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
		}
	}
}