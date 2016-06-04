using System;
		
using UIKit;
using Foundation;
using Debug = System.Diagnostics.Debug;

namespace mySafari.iOS
{
	public partial class ViewController : UIViewController
	{

		public ViewController (IntPtr handle) : base (handle)
		{		
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			btnGo.TouchUpInside += (object sender, EventArgs e) => {
				var alertController = UIAlertController.Create("URL", txtUrl.Text, UIAlertControllerStyle.Alert);
				alertController.AddAction(UIAlertAction.Create("確認", UIAlertActionStyle.Default, (confirm)=>{Debug.WriteLine("Confirm");}));

				PresentViewController( alertController, true, ()=>{Debug.WriteLine("Show Alert");});

				InvokeOnMainThread (()=>{

					if( txtUrl.IsFirstResponder){
						txtUrl.ResignFirstResponder();
					}

					UIView.Animate (5, () => {

						btnGoBottomConstraint.Constant =  20 ;
						this.View.LayoutIfNeeded();


					});

				});



				webView.LoadRequest( new NSUrlRequest( new NSUrl( txtUrl.Text)));

			};

			NSNotificationCenter.DefaultCenter.AddObserver (UIKeyboard.WillChangeFrameNotification, KeyboardFrameChangedhandle);
		}

		private void KeyboardFrameChangedhandle(NSNotification notification){

			if(notification.UserInfo.ContainsKey( new NSString(UIKeyboard.FrameEndUserInfoKey.ToString()))){

				NSString key = new NSString(UIKeyboard.FrameEndUserInfoKey.ToString());

				NSObject objRect = notification.UserInfo[key];

				if (objRect is NSValue) {
					var v = (NSValue)objRect;

					var rect = v.RectangleFValue;

					InvokeOnMainThread (()=>{

						UIView.Animate (1, () => {

							btnGoBottomConstraint.Constant =  rect.Height + 10 ;
							this.View.LayoutIfNeeded();


						});

					});

					Debug.WriteLine ("Will Height:" + rect.Height);
				}
			}
		}



		public override void DidReceiveMemoryWarning ()
		{		
			base.DidReceiveMemoryWarning ();		
			// Release any cached data, images, etc that aren't in use.		
		}
	}
}
