using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using Android.OS;
using Android.App;
using Android.Widget;
using Android.Webkit;
using Android.Views;
using Android.Content;
using Android.Runtime;
using Android.Views.InputMethods;

using Java.Interop;
using Debug = System.Diagnostics.Debug;
using AndroidHUD;

namespace mySafari.Droid
{
	[Activity (Label = "mySafari", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : Activity
	{
		//int count = 1;
		private InputMethodManager _InputMethodManager;

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			_InputMethodManager = (InputMethodManager)GetSystemService (Context.InputMethodService);

			var txtUrl = FindViewById<EditText> (Resource.Id.register_test_txtUrl);
			var btnGo = FindViewById<Button> (Resource.Id.register_test_btnGO);
			var webView = FindViewById<WebView> (Resource.Id.register_test_webView);
			//webView.SetWebViewClient (new BrowserClient ());

			var client = new ContentWebViewClient ();
			webView.SetWebViewClient (client);
			//webView.Settings.UserAgentString =@"";
			//webView.Settings.UserAgentString = @"Mozilla/5.0 (iPad; CPU OS 7_0 like Mac OS X) AppleWebKit/537.51.1 (KHTML, like Gecko) CriOS/30.0.1599.12 Mobile/11A465 Safari/8536.25 (3B92C18B-D9DE-4CB7-A02A-22FD2AF17C8F)";
			webView.Settings.JavaScriptEnabled = true;


			btnGo.Click += (object sender, System.EventArgs e) =>{

//				AlertDialog.Builder alert = new AlertDialog.Builder(this);
//
//				alert.SetTitle("URL");
//				alert.SetMessage( txtUrl.Text);
//				alert.SetPositiveButton("確認", (alertsender,args)=>{
//					Debug.WriteLine("Confirm");});
//				alert.SetNegativeButton("取消", (alertsender,args)=>{
//					Debug.WriteLine("Cancel");});
//
//				RunOnUiThread(()=>{
//					alert.Show();
//
//				});

				_InputMethodManager.HideSoftInputFromWindow(txtUrl.WindowToken, 0);
				webView.LoadUrl( txtUrl.Text );
				//轉圈圈
				RunOnUiThread(
					()=>{
						AndHUD.Shared.Show(this, "Status Message", -1, MaskType.Clear);
					}

				);
			};

			client.WebViewLocaitonChanged += (object sender, ContentWebViewClient.WebViewLocaitonChangedEventArgs e) => {

				Debug.WriteLine(  e.CommandString );
			};

			client.WebViewLoadCompleted += (object sender, ContentWebViewClient.WebViewLoadCompletedEventArgs e) => {

				RunOnUiThread(()=>{

					AndHUD.Shared.Dismiss(this);

				});

			};


		}

		//public class BrowserClient : WebViewClient{}

		public class ContentWebViewClient : WebViewClient
		{

			public event EventHandler<WebViewLocaitonChangedEventArgs> WebViewLocaitonChanged;

			public event EventHandler<WebViewLoadCompletedEventArgs> WebViewLoadCompleted;

			public override bool ShouldOverrideUrlLoading (WebView view, string url)
			{
				EventHandler<WebViewLocaitonChangedEventArgs> handler = 
					WebViewLocaitonChanged;

				if (null != handler) {
					handler (this, 
						new WebViewLocaitonChangedEventArgs{ 
							CommandString = url });
				}

				return base.ShouldOverrideUrlLoading (view, url);

			}

			public override void OnPageFinished (WebView view, string url)
			{
				base.OnPageFinished (view, url);

				EventHandler<WebViewLoadCompletedEventArgs> handler = 
					WebViewLoadCompleted;

				if (null != handler) {
					handler (this, 
						new WebViewLoadCompletedEventArgs());
				}
			}

			public class WebViewLocaitonChangedEventArgs : EventArgs{

				public string CommandString {get;set;}
			}

			public class WebViewLoadCompletedEventArgs : EventArgs{

			}
		}
	}
}


