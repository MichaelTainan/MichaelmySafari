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
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		UIKit.UIButton btnGo { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint btnGoBottomConstraint { get; set; }

		[Outlet]
		UIKit.UIButton Button { get; set; }

		[Outlet]
		UIKit.UITextField txtUrl { get; set; }

		[Outlet]
		UIKit.UIWebView webView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (webView != null) {
				webView.Dispose ();
				webView = null;
			}

			if (btnGo != null) {
				btnGo.Dispose ();
				btnGo = null;
			}

			if (txtUrl != null) {
				txtUrl.Dispose ();
				txtUrl = null;
			}

			if (btnGoBottomConstraint != null) {
				btnGoBottomConstraint.Dispose ();
				btnGoBottomConstraint = null;
			}

			if (Button != null) {
				Button.Dispose ();
				Button = null;
			}
		}
	}
}
