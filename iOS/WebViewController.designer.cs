// WARNING
//
// This file has been generated automatically by Xamarin Studio Community to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace mySafari.iOS
{
	[Register ("WebViewController")]
	partial class WebViewController
	{
		[Outlet]
		UIKit.UIButton btnMessage { get; set; }

		[Outlet]
		UIKit.UIWebView testWebView { get; set; }

		[Outlet]
		UIKit.UITextField txtMessage { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (btnMessage != null) {
				btnMessage.Dispose ();
				btnMessage = null;
			}

			if (testWebView != null) {
				testWebView.Dispose ();
				testWebView = null;
			}

			if (txtMessage != null) {
				txtMessage.Dispose ();
				txtMessage = null;
			}
		}
	}
}
