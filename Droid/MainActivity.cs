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

using AndroidHUD;
using Debug = System.Diagnostics.Debug ;

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

			#region WebView

			//_InputMethodManager = (InputMethodManager)GetSystemService (Context.InputMethodService);

			var txtUrl = FindViewById<EditText> (Resource.Id.register_test_txtUrl);
			var btnGo = FindViewById<Button> (Resource.Id.register_test_btnGO);
			var webView = FindViewById<WebView> (Resource.Id.register_test_webView);

			var client = new ContentWebViewClient ();
			//webView.SetWebViewClient (client);
			webView.SetWebChromeClient(client);
			//webView.SetWebViewClient (new BrowserClient ());
			//webView.Settings.UserAgentString =@"";
			//webView.Settings.UserAgentString = @"Mozilla/5.0 (iPad; CPU OS 7_0 like Mac OS X) AppleWebKit/537.51.1 (KHTML, like Gecko) CriOS/30.0.1599.12 Mobile/11A465 Safari/8536.25 (3B92C18B-D9DE-4CB7-A02A-22FD2AF17C8F)";
			webView.Settings.JavaScriptEnabled = true;
			webView.Settings.UserAgentString = @"Android";

			// 負責與頁面溝通 - WebView -> Native
			MyJSInterface myJSInterface = new MyJSInterface( this );

			webView.AddJavascriptInterface(myJSInterface, "TP");
			myJSInterface.CallFromPageReceived += delegate (object sender, MyJSInterface.CallFromPageReceivedEventArgs e) {
				Debug.WriteLine(e.Result);
			};

			// 負責與頁面溝通 - Native -> WebView
			JavaScriptResult callResult = new JavaScriptResult ();
			callResult.JavaScriptResultReceived += (object sender, JavaScriptResult.JavaScriptResultReceivedEventArgs e) => {

				Debug.WriteLine(e.Result);
			};

			// 載入一般網頁
			//MyWebView.LoadUrl ("http://stackoverflow.com/");
			// 載入以下程式碼進行互動

			webView.LoadDataWithBaseURL (
				null
				,@"<html>
						<head>
							<title>Local String</title>
							<style type='text/css'>p{font-family : Verdana; color : purple }</style>
							<script language='JavaScript'> 
								var lookup = '中文訊息'
								function msg(){ window.location = 'callfrompage://Hi'  }
							</script>
						</head>
						<body><p>Hello World!</p><br />
							<button type='button' onclick='TP.CallFromPage(lookup)' text='Hi From Page'>Hi From Page</button>
						</body>
					</html>"
				, "text/html"
				, "utf-8"
				,null);



			#endregion

			#region EditText

			_InputMethodManager  = 
				(InputMethodManager)GetSystemService(Context.InputMethodService);


			/*
			TxtUrl = FindViewById<EditText> (Resource.Id.txtUrl);
			TxtUrl.TextChanged += (object sender,
				Android.Text.TextChangedEventArgs e) => {
				Debug.WriteLine( TxtUrl.Text +":"+ e.Text );
			};
			*/

			#endregion


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

				RunOnUiThread(()=>{

					webView.EvaluateJavascript( @"alert('michael test');", callResult );
				});

				/*
				_InputMethodManager.HideSoftInputFromWindow(txtUrl.WindowToken, 0);
				webView.LoadUrl( txtUrl.Text );
				//轉圈圈
				RunOnUiThread(
					()=>{
						AndHUD.Shared.Show(this, "Status Message", -1, MaskType.Clear);
					}

				);*/
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

		// Call from Page
		public class MyJSInterface : Java.Lang.Object
		{
			Context Context { get; set; }

			public MyJSInterface (Context context)
			{
				this.Context = context;
			}

			[Export]
			[JavascriptInterface]
			public void CallFromPage (string parameter){
				Debug.WriteLine ($"CallFromPage:{parameter}");

				EventHandler<CallFromPageReceivedEventArgs> handler = 
					CallFromPageReceived;

				if (null != handler) {
					handler (this, 
						new CallFromPageReceivedEventArgs{ 
							Result = parameter });
				}
			}

			public event EventHandler<CallFromPageReceivedEventArgs> CallFromPageReceived;

			public class CallFromPageReceivedEventArgs : EventArgs{
				public string Result { get; set; }
			} 
		}

		// Call Page
		public class JavaScriptResult : Java.Lang.Object, IValueCallback{

			public void OnReceiveValue(Java.Lang.Object result) {
				Java.Lang.String json = (Java.Lang.String)result;

				var resultString = json.ToString();

				EventHandler<JavaScriptResultReceivedEventArgs> handler = 
					JavaScriptResultReceived;

				if (null != handler) {
					handler (this, 
						new JavaScriptResultReceivedEventArgs{ 
							Result = resultString ?? "" });
				}


			}

			public event EventHandler<JavaScriptResultReceivedEventArgs> JavaScriptResultReceived;

			public class JavaScriptResultReceivedEventArgs : EventArgs{
				public string Result { get; set; }
			} 

		}

		public class MyWebClient : WebViewClient{}

		public class ContentWebViewClient : WebChromeClient
		{

			public event EventHandler<WebViewLocaitonChangedEventArgs> WebViewLocaitonChanged;

			public event EventHandler<WebViewLoadCompletedEventArgs> WebViewLoadCompleted;

			/*public override bool ShouldOverrideUrlLoading (WebView view, string url)
			{
				EventHandler<WebViewLocaitonChangedEventArgs> handler = 
					WebViewLocaitonChanged;

				if (null != handler) {
					handler (this, 
						new WebViewLocaitonChangedEventArgs{ 
							CommandString = url });
				}

				return base.ShouldOverrideUrlLoading (view, url);

			}*/


			/*public override void OnPageFinished (WebView view, string url)
			{
				base.OnPageFinished (view, url);

				EventHandler<WebViewLoadCompletedEventArgs> handler = 
					WebViewLoadCompleted;

				if (null != handler) {
					handler (this, 
						new WebViewLoadCompletedEventArgs());
				}
			}*/

			public class WebViewLocaitonChangedEventArgs : EventArgs{

				public string CommandString {get;set;}
			}

			public class WebViewLoadCompletedEventArgs : EventArgs{

			}
		}

	}
}


