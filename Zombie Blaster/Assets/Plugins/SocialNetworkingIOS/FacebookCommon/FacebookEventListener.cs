using UnityEngine;
using System.Collections;


public class FacebookEventListenerIOS : MonoBehaviour
{
#if UNITY_IPHONE || UNITY_ANDROID
	// Listens to all the events.  All event listeners MUST be removed before this object is disposed!
	void OnEnable()
	{
		FacebookManagerIOS.sessionOpenedEvent += sessionOpenedEvent;
		FacebookManagerIOS.loginFailedEvent += loginFailedEvent;
		
		FacebookManagerIOS.dialogCompletedWithUrlEvent += dialogCompletedEvent;
		FacebookManagerIOS.dialogFailedEvent += dialogFailedEvent;
		FacebookManagerIOS.dialogCompletedEvent += facebokDialogCompleted;
		FacebookManagerIOS.dialogDidNotCompleteEvent += dialogDidNotCompleteEvent;
		
		FacebookManagerIOS.graphRequestCompletedEvent += graphRequestCompletedEvent;
		FacebookManagerIOS.graphRequestFailedEvent += facebookCustomRequestFailed;
		FacebookManagerIOS.restRequestCompletedEvent += restRequestCompletedEvent;
		FacebookManagerIOS.restRequestFailedEvent += restRequestFailedEvent;
		FacebookManagerIOS.facebookComposerCompletedEvent += facebookComposerCompletedEvent;
		
		FacebookManagerIOS.reauthorizationFailedEvent += reauthorizationFailedEvent;
		FacebookManagerIOS.reauthorizationSucceededEvent += reauthorizationSucceededEvent;
	}

	
	void OnDisable()
	{
		// Remove all the event handlers when disabled
		FacebookManagerIOS.sessionOpenedEvent -= sessionOpenedEvent;
		FacebookManagerIOS.loginFailedEvent -= loginFailedEvent;

		FacebookManagerIOS.dialogCompletedEvent -= facebokDialogCompleted;
		FacebookManagerIOS.dialogCompletedWithUrlEvent -= dialogCompletedEvent;
		FacebookManagerIOS.dialogDidNotCompleteEvent -= dialogDidNotCompleteEvent;
		FacebookManagerIOS.dialogFailedEvent -= dialogFailedEvent;
		
		FacebookManagerIOS.graphRequestCompletedEvent -= graphRequestCompletedEvent;
		FacebookManagerIOS.graphRequestFailedEvent -= facebookCustomRequestFailed;
		FacebookManagerIOS.restRequestCompletedEvent -= restRequestCompletedEvent;
		FacebookManagerIOS.restRequestFailedEvent -= restRequestFailedEvent;
		FacebookManagerIOS.facebookComposerCompletedEvent -= facebookComposerCompletedEvent;
		
		FacebookManagerIOS.reauthorizationFailedEvent -= reauthorizationFailedEvent;
		FacebookManagerIOS.reauthorizationSucceededEvent -= reauthorizationSucceededEvent;
	}

	

	void sessionOpenedEvent()
	{
		Debug.Log( "Successfully logged in to Facebook" );
	}
	
	
	void loginFailedEvent( string error )
	{
		Debug.Log( "Facebook login failed: " + error );
	}


	void dialogCompletedEvent( string url )
	{
		Debug.Log( "dialogCompletedEvent: " + url );
	}
	
	
	void dialogFailedEvent( string error )
	{
		Debug.Log( "dialogFailedEvent: " + error );
	}
	
	
	void facebokDialogCompleted()
	{
		Debug.Log( "facebokDialogCompleted" );
	}

	
	void dialogDidNotCompleteEvent()
	{
		Debug.Log( "facebookDialogDidntComplete" );
	}

	
	void graphRequestCompletedEvent( object obj )
	{
		Debug.Log( "graphRequestCompletedEvent" );
		Prime31.Utils.logObject( obj );
	}
	
	
	void facebookCustomRequestFailed( string error )
	{
		Debug.Log( "facebookCustomRequestFailed failed: " + error );
	}
	
	
	void restRequestCompletedEvent( object obj )
	{
		Debug.Log( "restRequestCompletedEvent" );
		Prime31.Utils.logObject( obj );
	}
	
	
	void restRequestFailedEvent( string error )
	{
		Debug.Log( "restRequestFailedEvent failed: " + error );
	}
	
	
	void facebookComposerCompletedEvent( bool didSucceed )
	{
		Debug.Log( "facebookComposerCompletedEvent did succeed: " + didSucceed );
	}


	void reauthorizationSucceededEvent()
	{
		Debug.Log( "reauthorizationSucceededEvent" );
	}
	
	
	void reauthorizationFailedEvent( string error )
	{
		Debug.Log( "reauthorizationFailedEvent: " + error );
	}
	
#endif
}
