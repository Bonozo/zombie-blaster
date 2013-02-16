//
//  TapjoyManager.m
//  TapjoyTest
//
//  Created by Mike on 1/28/11.
//  Copyright 2011 Prime31 Studios. All rights reserved.
//

#import "TapjoyManager.h"
#import "JSONKit.h"


void UnitySendMessage( const char * className, const char * methodName, const char * param );

void UnityPause( bool shouldPause );

UIViewController *UnityGetGLViewController();



@implementation TapjoyManager

@synthesize adView, bannerPosition, adContentSize, publisherId, secretKey;

///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark NSObject

+ (TapjoyManager*)sharedManager
{
	static TapjoyManager *sharedSingleton;
	
	if( !sharedSingleton )
		sharedSingleton = [[TapjoyManager alloc] init];
	
	return sharedSingleton;
}


- (id)init
{
	if( (self = [super init]) )
	{
		// set the ad size
		// Must return one of TJC_AD_BANNERSIZE_320X50 or TJC_AD_BANNERSIZE_480X32 or TJC_AD_BANNERSIZE_768X90
		if( UI_USER_INTERFACE_IDIOM() == UIUserInterfaceIdiomPad )
			self.adContentSize = TJC_AD_BANNERSIZE_768X90;
		else
			self.adContentSize = TJC_AD_BANNERSIZE_320X50;
	}
	return self;
}


///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark Private

- (void)adjustAdViewFrameToShowAdView
{
	// fetch screen dimensions and useful values
	CGRect origFrame = self.adView.frame;
	
	CGFloat screenHeight = [UIScreen mainScreen].bounds.size.height;
	CGFloat screenWidth = [UIScreen mainScreen].bounds.size.width;
	
	if( UIInterfaceOrientationIsLandscape( UnityGetGLViewController().interfaceOrientation ) )
	{
		screenWidth = screenHeight;
		screenHeight = [UIScreen mainScreen].bounds.size.width;
	}
	
	
	switch( bannerPosition )
	{
		case TapjoyAdPositionTopLeft:
			origFrame.origin.x = 0;
			origFrame.origin.y = 0;
			self.adView.autoresizingMask = ( UIViewAutoresizingFlexibleRightMargin | UIViewAutoresizingFlexibleBottomMargin );
			break;
		case TapjoyAdPositionTopCenter:
			origFrame.origin.x = ( screenWidth / 2 ) - ( origFrame.size.width / 2 );
			origFrame.origin.y = 0;
			self.adView.autoresizingMask = ( UIViewAutoresizingFlexibleLeftMargin | UIViewAutoresizingFlexibleRightMargin | UIViewAutoresizingFlexibleBottomMargin );
			break;
		case TapjoyAdPositionTopRight:
			origFrame.origin.x = screenWidth - origFrame.size.width;
			origFrame.origin.y = 0;
			self.adView.autoresizingMask = ( UIViewAutoresizingFlexibleLeftMargin | UIViewAutoresizingFlexibleBottomMargin );
			break;
		case TapjoyAdPositionCentered:
			origFrame.origin.x = ( screenWidth / 2 ) - ( origFrame.size.width / 2 );
			origFrame.origin.y = ( screenHeight / 2 ) - ( origFrame.size.height / 2 );
			self.adView.autoresizingMask = ( UIViewAutoresizingFlexibleRightMargin | UIViewAutoresizingFlexibleLeftMargin | UIViewAutoresizingFlexibleTopMargin | UIViewAutoresizingFlexibleBottomMargin );
			break;
		case TapjoyAdPositionBottomLeft:
			origFrame.origin.x = 0;
			origFrame.origin.y = screenHeight - origFrame.size.height;
			self.adView.autoresizingMask = ( UIViewAutoresizingFlexibleRightMargin | UIViewAutoresizingFlexibleTopMargin );
			break;
		case TapjoyAdPositionBottomCenter:
			origFrame.origin.x = ( screenWidth / 2 ) - ( origFrame.size.width / 2 );
			origFrame.origin.y = screenHeight - origFrame.size.height;
			self.adView.autoresizingMask = ( UIViewAutoresizingFlexibleLeftMargin | UIViewAutoresizingFlexibleRightMargin | UIViewAutoresizingFlexibleTopMargin );
			break;
		case TapjoyAdPositionBottomRight:
			origFrame.origin.x = screenWidth - self.adView.frame.size.width;
			origFrame.origin.y = screenHeight - origFrame.size.height;
			self.adView.autoresizingMask = ( UIViewAutoresizingFlexibleLeftMargin | UIViewAutoresizingFlexibleTopMargin );
			break;
	}
	
	self.adView.frame = origFrame;
}


///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark Public

- (void)startWithAppId:(NSString*)appId
{
	// default notifications
	[[NSNotificationCenter defaultCenter] addObserver:self selector:@selector(tapjoyConnectionSuccessful:) name:TJC_CONNECT_SUCCESS object:nil];
	[[NSNotificationCenter defaultCenter] addObserver:self selector:@selector(tapjoyConnectionFailed:) name:TJC_CONNECT_FAILED object:nil];
	
	// TapPoint retrieval notificaitons
	[[NSNotificationCenter defaultCenter] addObserver:self selector:@selector(receivedTapPoints:) name:TJC_TAP_POINTS_RESPONSE_NOTIFICATION object:nil];
	[[NSNotificationCenter defaultCenter] addObserver:self selector:@selector(receivedTapPointsFailed:) name:TJC_TAP_POINTS_RESPONSE_NOTIFICATION_ERROR object:nil];
	
	// TapPoint spending notificaitons
	[[NSNotificationCenter defaultCenter] addObserver:self selector:@selector(spentTapPoints:) name:TJC_SPEND_TAP_POINTS_RESPONSE_NOTIFICATION object:nil];
	[[NSNotificationCenter defaultCenter] addObserver:self selector:@selector(spentTapPointsFailed:) name:TJC_SPEND_TAP_POINTS_RESPONSE_NOTIFICATION_ERROR object:nil];

	// TJC_VIEW_CLOSED_NOTIFICATION and TJC_TAPPOINTS_EARNED_NOTIFICATION
	[[NSNotificationCenter defaultCenter] addObserver:self selector:@selector(viewClosedNotification:) name:TJC_VIEW_CLOSED_NOTIFICATION object:nil];
	[[NSNotificationCenter defaultCenter] addObserver:self selector:@selector(tappointsEarnedNotification:) name:TJC_TAPPOINTS_EARNED_NOTIFICATION object:nil];	
	
	// daily reward ad
	[[NSNotificationCenter defaultCenter] addObserver:self selector:@selector(dailyRewardAdLoaded:) name:TJC_DAILY_REWARD_RESPONSE_NOTIFICATION object:nil];
	[[NSNotificationCenter defaultCenter] addObserver:self selector:@selector(dailyRewardAdFailed:) name:TJC_DAILY_REWARD_RESPONSE_NOTIFICATION_ERROR object:nil];
	
	// connect to Tapjoy
	[TapjoyConnect requestTapjoyConnect:appId secretKey:self.secretKey];
	
	[TapjoyConnect setTransitionEffect:TJCTransitionFadeEffect];
	
	// featured app
	[[NSNotificationCenter defaultCenter] addObserver:self selector:@selector(receivedFullScreenAd:) name:TJC_FULL_SCREEN_AD_RESPONSE_NOTIFICATION object:nil];
	[TapjoyConnect getFullScreenAd];
}


- (void)setTransitionEffect:(int)effect
{
	[TapjoyConnect setTransitionEffect:(TJCTransitionEnum)effect];
}


- (void)getFullScreenAdWithCurrencyId:(NSString*)currencyId
{
	if( currencyId )
		[TapjoyConnect getFullScreenAdWithCurrencyID:currencyId];
	else
		[TapjoyConnect getFullScreenAd];
}


- (void)showFullScreenAd
{
	if( !_isFeaturedAppReady )
	{
		NSLog( @"full screen ad is not yet ready" );
		return;
	}
	
	UnityPause( true );
	[TapjoyConnect showFullScreenAdWithViewController:UnityGetGLViewController()];
}


- (void)setFullScreenAdDelayCount:(int)delayCount
{
	[TapjoyConnect setFullScreenAdDelayCount:delayCount];
}


- (void)getDailyRewardAd
{
	[TapjoyConnect getDailyRewardAd];
}


- (void)showDailyRewardAd
{
	[TapjoyConnect showDailyRewardAdWithViewController:UnityGetGLViewController()];
}


- (void)createBannerWithPosition:(TapjoyAdPosition)position
{
	// kill the current adView if we have one
	if( self.adView )
		[self destroyBanner];
	
	bannerPosition = position;
	[TapjoyConnect getDisplayAdWithDelegate:self];
}


- (void)destroyBanner
{
	[TapjoyConnect getDisplayAdWithDelegate:nil];
	
	if( self.adView )
	{
		[self.adView removeFromSuperview];
		self.adView = nil;
	}
}


- (void)refreshBanner
{
	[TapjoyConnect getDisplayAdWithDelegate:self];
	[self adjustAdViewFrameToShowAdView];
}


- (void)showOffers
{
	UnityPause( true );
	[TapjoyConnect showOffersWithViewController:UnityGetGLViewController()];
}


- (void)getTapPoints
{
	[TapjoyConnect getTapPoints];
}


- (void)spendTapPoints:(int)points
{
	[TapjoyConnect spendTapPoints:points];
}


- (void)awardTapPoints:(int)points
{
	[TapjoyConnect awardTapPoints:points];
}


- (BOOL)isFeaturedAppReady
{
	return _isFeaturedAppReady;
}


- (void)actionComplete:(NSString*)action
{
	[TapjoyConnect actionComplete:action];
}


///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark NSNotifications

- (void)tapjoyConnectionSuccessful:(NSNotification*)note
{
	NSLog( @"tapjoyConnectionSuccessful" );
}


- (void)tapjoyConnectionFailed:(NSNotification*)note
{
	NSLog( @"tapjoyConnectionFailed" );
}


- (void)receivedTapPoints:(NSNotification*)note
{
	NSNumber *tapPoints = note.object;

	UnitySendMessage( "TapjoyManager", "tapPointsDidLoad", [[NSString stringWithFormat:@"%i", [tapPoints intValue]] UTF8String] );
}


- (void)receivedTapPointsFailed:(NSNotification*)note
{
	UnitySendMessage( "TapjoyManager", "receivedTapPointsFailed", "" );
}


// This notification is fired after spendTapPoints has been called, and indicates that the user has successfully spent currency.
- (void)spentTapPoints:(NSNotification*)note
{
	NSNumber *tapPoints = note.object;
	UnitySendMessage( "TapjoyManager", "spentTapPoints", [[NSString stringWithFormat:@"%i", [tapPoints intValue]] UTF8String] );
}


// Error notification for spendTapPoints
- (void)spentTapPointsFailed:(NSNotification*)note
{
	UnitySendMessage( "TapjoyManager", "spentTapPointsFailed", "" );
}


- (void)viewClosedNotification:(NSNotification*)note
{
	UnityPause( false );
	UnitySendMessage( "TapjoyManager", "viewClosed", "" );
}


- (void)tappointsEarnedNotification:(NSNotification*)note
{
	NSNumber *tappointsEarned = note.object;
	NSString *tappointsString = [NSString stringWithFormat:@"%i", [tappointsEarned intValue]];
	
	// Pops up a UIAlert notifying the user that they have successfully earned some currency. 
	// This is the default alert
	[TapjoyConnect showDefaultEarnedCurrencyAlert];
	
	UnitySendMessage( "TapjoyManager", "tappointsEarned", tappointsString.UTF8String );
}


- (void)receivedFullScreenAd:(NSNotification*)note
{
	UnitySendMessage( "TapjoyManager", "fullScreenAdDidLoad", "" );
	
	// Show the custom Tapjoy full screen full screen ad view.
	_isFeaturedAppReady = YES;
}


- (void)dailyRewardAdLoaded:(NSNotification*)note
{
	UnitySendMessage( "TapjoyManager", "dailyRewardAdLoaded", "1" );
}


- (void)dailyRewardAdFailed:(NSNotification*)note
{
	UnitySendMessage( "TapjoyManager", "dailyRewardAdLoaded", "0" );
}


///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark - Private


- (void)initVideoAd
{
	[TapjoyConnect cacheVideosWithDelegate:self];
}


- (void)setVideoCacheCount:(int)count
{
	[TapjoyConnect setVideoCacheCount:count];
}


///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark Tapjoy

- (void)didReceiveAd:(TJCAdView*)adView
{
	if( !self.adView )
	{
		self.adView = (UIView*)[TapjoyConnect getDisplayAdView];
		
		if( [adContentSize isEqual:TJC_DISPLAY_AD_SIZE_320X50] )
			self.adView.frame = CGRectMake( 0, 0, 320, 50 );
		else if( [adContentSize isEqual:TJC_DISPLAY_AD_SIZE_640X100] )
			self.adView.frame = CGRectMake( 0, 0, 640, 100 );
		else if( [adContentSize isEqual:TJC_DISPLAY_AD_SIZE_768X90] )
			self.adView.frame = CGRectMake( 0, 0, 768, 90 );
		else
			self.adView.frame = CGRectMake( 0, 0, 320, 50 );
		
		
		[self adjustAdViewFrameToShowAdView];
		[UnityGetGLViewController().view addSubview:self.adView];
	}
	
	[self adjustAdViewFrameToShowAdView];
	UnitySendMessage( "TapjoyManager", "didReceiveAd", "" );
}


- (void)didFailWithMessage:(NSString*)msg
{
	UnitySendMessage( "TapjoyManager", "didFailToReceiveAd", msg.UTF8String );
}


- (NSString*)adContentSize
{
	return adContentSize;
}


- (BOOL)shouldRefreshAd
{
	return YES;
}


///////////////////////////////////////////////////////////////////////////////////////////////////
#pragma mark TJCVideoAdDelegate

- (void)videoAdBegan
{
	UnityPause( true );
	UnitySendMessage( "TapjoyManager", "videoAdBegan", "" );
}


- (void)videoAdClosed
{
	UnitySendMessage( "TapjoyManager", "videoAdClosed", "" );
}


- (void)videoAdError:(NSString*)errorMsg
{
	NSLog( @"videoAdError: %@", errorMsg );
}


@end
