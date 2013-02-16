#import "TapjoyConnectPlugin.h"

UIViewController *UnityGetGLViewController();

static TapjoyConnectPlugin *_sharedInstance = nil; //To make TapjoyConnect Singleton

@implementation TapjoyConnectPlugin

@synthesize keyFlagValueDict = keyFlagValueDict_, callbackHandlerName = callbackHandlerName_, displayAdSize = displayAdSize_, displayAdOrientation = displayAdOrientation_;


+ (void)initialize
{
    if (self == [TapjoyConnectPlugin class])
	{
		_sharedInstance = [[self alloc] init];
    }
}


+ (TapjoyConnectPlugin*)sharedTapjoyConnectPlugin
{
	return _sharedInstance;
}


- (id)init
{
	self = [super init];
    
    if (self)
    {
        tapPoints = 0;
        displayAdSize_ = TJC_DISPLAY_AD_SIZE_320X50;
        displayAdFrame = CGRectMake(0, 0, 320, 50);
        
        [[NSNotificationCenter defaultCenter] addObserver:self
                                                 selector:@selector(tjcConnectSuccess:)
                                                     name:TJC_CONNECT_SUCCESS
                                                   object:nil];
        [[NSNotificationCenter defaultCenter] addObserver:self selector:@selector(tjcConnectFail:) name:TJC_CONNECT_FAILED object:nil];
        
        // Add an observer for when Tap Points has been successfully retrieved.
        [[NSNotificationCenter defaultCenter] addObserver:self
                                                 selector:@selector(getUpdatedPoints:)
                                                     name:TJC_TAP_POINTS_RESPONSE_NOTIFICATION
                                                   object:nil];
        [[NSNotificationCenter defaultCenter] addObserver:self
                                                 selector:@selector(spendPoints:)
                                                     name:TJC_SPEND_TAP_POINTS_RESPONSE_NOTIFICATION
                                                   object:nil];
        [[NSNotificationCenter defaultCenter] addObserver:self
                                                 selector:@selector(awardPoints:)
                                                     name:TJC_AWARD_TAP_POINTS_RESPONSE_NOTIFICATION
                                                   object:nil];
        [[NSNotificationCenter defaultCenter] addObserver:self
                                                 selector:@selector(getUpdatedPointsError:)
                                                     name:TJC_TAP_POINTS_RESPONSE_NOTIFICATION_ERROR
                                                   object:nil];
        [[NSNotificationCenter defaultCenter] addObserver:self
                                                 selector:@selector(spendPointsError:)
                                                     name:TJC_SPEND_TAP_POINTS_RESPONSE_NOTIFICATION_ERROR
                                                   object:nil];
        [[NSNotificationCenter defaultCenter] addObserver:self 
                                                 selector:@selector(awardPointsError:) 
                                                     name:TJC_AWARD_TAP_POINTS_RESPONSE_NOTIFICATION_ERROR 
                                                   object:nil];
        
        // Add an observer for when a user has successfully earned currency.
        [[NSNotificationCenter defaultCenter] addObserver:self
                                                 selector:@selector(showEarnedCurrencyAlert:) 
                                                     name:TJC_TAPPOINTS_EARNED_NOTIFICATION 
                                                   object:nil];
        
        // Get fullscreen ad Call
        [[NSNotificationCenter defaultCenter] addObserver:self 
                                                 selector:@selector(getFullScreenAd:) 
                                                     name:TJC_FULL_SCREEN_AD_RESPONSE_NOTIFICATION 
                                                   object:nil];

        [[NSNotificationCenter defaultCenter] addObserver:self 
                                                 selector:@selector(getFullScreenAdError:) 
                                                     name:TJC_FULL_SCREEN_AD_RESPONSE_NOTIFICATION_ERROR
                                                   object:nil];

        // Get daily reward ad Call
        [[NSNotificationCenter defaultCenter] addObserver:self 
                                                 selector:@selector(getDailyRewardAd:) 
                                                     name:TJC_DAILY_REWARD_RESPONSE_NOTIFICATION 
                                                   object:nil];

        [[NSNotificationCenter defaultCenter] addObserver:self 
                                                 selector:@selector(getDailyRewardAdError:) 
                                                     name:TJC_DAILY_REWARD_RESPONSE_NOTIFICATION_ERROR
                                                   object:nil];
    }
	return self;
}


- (BOOL)hasKeyFlag
{
    if (keyFlagValueDict_)
        return YES;
	return NO;
}


- (void)setFlagKey:(NSString*)key Value:(NSString*)value
{
	if (!keyFlagValueDict_)
		keyFlagValueDict_ = [[NSMutableDictionary alloc] init];
	[keyFlagValueDict_ setObject:value forKey:key];
}


- (void)tjcConnectSuccess:(NSNotification*)notifyObj
{
	UnitySendMessage([callbackHandlerName_ UTF8String], "TapjoyConnectSuccess", "Connect ping successful");
}


- (void)tjcConnectFail:(NSNotification*)notifyObj
{
	UnitySendMessage([callbackHandlerName_ UTF8String], "TapjoyConnectFail", "Connect ping failed");
}


- (void)getUpdatedPoints:(NSNotification*)notifyObj
{
    tapPoints = [notifyObj.object intValue];
	
	UnitySendMessage([callbackHandlerName_ UTF8String], "TapPointsLoaded", [[NSString stringWithFormat:@"Value: %i", tapPoints] UTF8String]);
}


- (void)getUpdatedPointsError:(NSNotification*)notifyObj
{
	UnitySendMessage([callbackHandlerName_ UTF8String], "TapPointsLoadedError", "Error loading Tap Points");
}


- (void)spendPoints:(NSNotification*)notifyObj
{
	tapPoints = [notifyObj.object intValue];
    
	UnitySendMessage([callbackHandlerName_ UTF8String], "TapPointsSpent", [[NSString stringWithFormat:@"Value: %i", tapPoints] UTF8String]);
}


- (void)spendPointsError:(NSNotification*)notifyObj
{
	UnitySendMessage([callbackHandlerName_ UTF8String], "TapPointsSpendError", "Error spending Tap Points");
}


- (void)awardPoints:(NSNotification*)notifyObj
{
	tapPoints = [notifyObj.object intValue];
	
	UnitySendMessage([callbackHandlerName_ UTF8String], "TapPointsAwarded", [[NSString stringWithFormat:@"Value: %i", tapPoints] UTF8String]);
}


- (void)awardPointsError:(NSNotification*)notifyObj
{
	UnitySendMessage([callbackHandlerName_ UTF8String], "TapPointsAwardError", "Error awarding Tap Points");
}


- (int)queryTapPoints
{
	return tapPoints;
}


- (void)getFullScreenAd:(NSNotification*)notifyObj
{
	// notifyObj will be returned as Nil in case of internet error or unavailibity of fullscreen ad
	// or its Max Number of count has exceeded its limit
	UnitySendMessage([callbackHandlerName_ UTF8String], "FullScreenAdLoaded", "Call ShowFullScreenAd to display full screen interstitial ad");
}


- (void)getFullScreenAdError:(NSNotification*)notifyObj
{
	// notifyObj will be returned as Nil in case of internet error or unavailibity of fullscreen ad
	// or its Max Number of count has exceeded its limit
	UnitySendMessage([callbackHandlerName_ UTF8String], "FullScreenAdError", "GetFullScreenAd failed");
}


- (void)getDailyRewardAd:(NSNotification*)notifyObj
{
	// notifyObj will be returned as Nil in case of internet error or unavailibity of daily reward ad
	UnitySendMessage([callbackHandlerName_ UTF8String], "DailyRewardAdLoaded", "Call ShowDailyRewardAd to display full screen interstitial ad");
}


- (void)getDailyRewardAdError:(NSNotification*)notifyObj
{
	// notifyObj will be returned as Nil in case of internet error or unavailibity of daily reward ad
	UnitySendMessage([callbackHandlerName_ UTF8String], "DailyRewardAdError", "GetDailyRewardAd failed");
}


- (void)showEarnedCurrencyAlert:(NSNotification*)notifyObj
{
	NSNumber *tapPointsEarned = notifyObj.object;
	earnedCurrencyAmount = [tapPointsEarned intValue];
	
	UnitySendMessage([callbackHandlerName_ UTF8String], "CurrencyEarned", "An alert can now be displayed informing the user that they just earned currency");
}


- (void)moveDislpayAdToX:(int)x toY:(int)y
{
	displayAdFrame.origin = CGPointMake(x, y);
	
	[self showDisplayAd];
}


- (void)showDisplayAd
{
	// Get the TJCAdView, which is a subclass of UIView.
    TJCAdView *adView = [TapjoyConnect getDisplayAdView];
    // Set the frame, in case moveDisplayAd.. has changed it.
	[adView setFrame:displayAdFrame];
    // Get the Unity GL ViewController
    UIViewController *glView = UnityGetGLViewController();
    [glView.view addSubview:(UIView *)adView];
}


- (void)hideDisplayAd
{
	UIView *adView = (UIView*)[TapjoyConnect getDisplayAdView];
	
	[adView removeFromSuperview];
}


- (void)enableDisplayAdAutoRefresh:(BOOL)enable
{
	shouldAutoRefresh = enable;
}


- (void)dealloc
{
	[super dealloc];
}
	 

#pragma mark Tapjoy Display Ads Delegate Methods

- (void)didReceiveAd:(TJCAdView*)adView
{
	UnitySendMessage([callbackHandlerName_ UTF8String], "DisplayAdLoaded", "Display Ad has loaded");
}


- (void)didFailWithMessage:(NSString*)msg
{
	NSLog(@"No Tapjoy Display Ads available");
}


- (NSString*)adContentSize
{
	return displayAdSize_;
}


- (BOOL)shouldRefreshAd
{
	return shouldAutoRefresh;
}


#pragma mark Tapjoy Video Ads Delegate Methods

- (void)videoAdBegan
{
	UnitySendMessage([callbackHandlerName_ UTF8String], "VideoAdStart", "Video Ad has started");
}


- (void)videoAdClosed
{
	UnitySendMessage([callbackHandlerName_ UTF8String], "VideoAdCompleted", "Video Ad has been closed");
}

- (void)videoAdError:(NSString *)errorMsg
{
	UnitySendMessage([callbackHandlerName_ UTF8String], "VideoAdError", [errorMsg UTF8String]);
}

@end






// Converts C style string to NSString
NSString* CreateNSString (const char* string)
{
	if (string)
		return [NSString stringWithUTF8String: string];
	else
		return [NSString stringWithUTF8String: ""];
}

// Helper method to create C string copy
char* MakeStringCopy (const char* string)
{
	if (string == NULL)
		return NULL;
	
	char* res = (char*)malloc(strlen(string) + 1);
	strcpy(res, string);
	return res;
}

// When native code plugin is implemented in .mm / .cpp file, then functions
// should be surrounded with extern "C" block to conform C function naming rules
extern "C" {
	

	void _SetCallbackHandler(const char* handlerName)
	{
		[[TapjoyConnectPlugin sharedTapjoyConnectPlugin] setCallbackHandlerName:[NSString stringWithUTF8String:handlerName]];
		NSLog(@"callbackHandlerName: %@", [[TapjoyConnectPlugin sharedTapjoyConnectPlugin] callbackHandlerName]);
	}
	
	
	void _RequestTapjoyConnect(const char* appID, const char* secretKey)
	{
		[TapjoyConnect setPlugin:@"unity"];

		if ([[TapjoyConnectPlugin sharedTapjoyConnectPlugin] keyFlagValueDict])
			[TapjoyConnect requestTapjoyConnect:CreateNSString(appID) secretKey:CreateNSString(secretKey) options:[[TapjoyConnectPlugin sharedTapjoyConnectPlugin] keyFlagValueDict]];
		else
			[TapjoyConnect requestTapjoyConnect:CreateNSString(appID) secretKey:CreateNSString(secretKey)];
	}


	void _SetFlagKeyValue(const char* flagKey, const char* flagValue)
	{
		[[TapjoyConnectPlugin sharedTapjoyConnectPlugin] setFlagKey:CreateNSString(flagKey) Value:CreateNSString(flagValue)];
	}
	
	
	void _EnableLogging(bool enable)
	{
		[TapjoyConnect enableLogging:enable];
	}


	void _ActionComplete(const char* actionID)
	{
		[TapjoyConnect actionComplete:CreateNSString(actionID)];
	}
	
	
	void _SetUserID(const char* userID)
	{
		[TapjoyConnect setUserID:CreateNSString(userID)];
	}
	
	
	void _ShowOffers(void)
	{
		// Displays the offer wall.
		[TapjoyConnect showOffersWithViewController:UnityGetGLViewController()];
	}
    
    
    void _ShowOffersWithCurrencyID(const char* currencyID, bool isSelectorVisible)
    {
        // Displays the offer wall.
		[TapjoyConnect showOffersWithCurrencyID:CreateNSString(currencyID) withViewController:UnityGetGLViewController() withCurrencySelector:isSelectorVisible];
    }	
	

	void _GetFullScreenAd(void)
	{
		// Initiates a request to get fullscreen ad data.
		[TapjoyConnect getFullScreenAd];
	}
    
    
    void _GetFullScreenAdWithCurrencyID(const char* currencyID)
    {
        // Initiates a request to get fullscreen ad data.
		[TapjoyConnect getFullScreenAdWithCurrencyID:CreateNSString(currencyID)];
    }


    void _SetCurrencyMultiplier(float multiplier)
    {
    	// Sets the currency multiplier, and fires off a network call to notify the server.
    	[TapjoyConnect setCurrencyMultiplier:multiplier];
    }
	

	void _ShowFullScreenAd(void)
	{
		// Shows the default full screen fullscreen ad ad.
		[TapjoyConnect showFullScreenAdWithViewController:UnityGetGLViewController()];
	}


	void _GetDailyRewardAd(void)
	{
		[TapjoyConnect getDailyRewardAd];
	}


	void _GetDailyRewardAdWithCurrencyID(const char* currencyID)
	{
		[TapjoyConnect getDailyRewardAdWithCurrencyID:CreateNSString(currencyID)];
	}


	void _ShowDailyRewardAd(void)
	{
		// Shows the default daily reward ad.
		[TapjoyConnect showDailyRewardAdWithViewController:UnityGetGLViewController()];
	}
	
	
	void _ShowDefaultEarnedCurrencyAlert(void)
	{
		// Pops up a UIAlert notifying the user that they have successfully earned some currency.
		// This is the default alert, so you may place a custom alert here if you choose to do so.
		[TapjoyConnect showDefaultEarnedCurrencyAlert];
	}
	
	
	void _GetTapPoints(void)
	{	
		[TapjoyConnect getTapPoints];
	}
	
	
	void _SpendTapPoints(int points)
	{
		[TapjoyConnect spendTapPoints:points];
	}
	
	
	void _AwardTapPoints(int points)
	{	
		[TapjoyConnect awardTapPoints:points];
	}
	
	
	int _QueryTapPoints(void)
	{
		return [[TapjoyConnectPlugin sharedTapjoyConnectPlugin] queryTapPoints];
	}
	
	
	void _GetDisplayAd(void)
	{
		[TapjoyConnect getDisplayAdWithDelegate:[TapjoyConnectPlugin sharedTapjoyConnectPlugin]];
	}
	

	void _ShowDisplayAd(void)
	{
		[[TapjoyConnectPlugin sharedTapjoyConnectPlugin] showDisplayAd];
	}


	void _HideDisplayAd(void)
	{
		[[TapjoyConnectPlugin sharedTapjoyConnectPlugin] hideDisplayAd];
	}
	

    void _GetDisplayAdWithCurrencyID(const char* currencyID)
	{
		[TapjoyConnect getDisplayAdWithDelegate:[TapjoyConnectPlugin sharedTapjoyConnectPlugin] currencyID:CreateNSString(currencyID)];
	}
    
    
	void _SetDisplayAdSize(const char* size)
	{
		[[TapjoyConnectPlugin sharedTapjoyConnectPlugin] setDisplayAdSize:CreateNSString(size)];
	}


	void _EnableDisplayAdAutoRefresh(bool enable)
	{
		[[TapjoyConnectPlugin sharedTapjoyConnectPlugin] enableDisplayAdAutoRefresh:enable];
	}
	
    
	void _MoveDisplayAd(int x, int y)
	{
		[[TapjoyConnectPlugin sharedTapjoyConnectPlugin] moveDislpayAdToX:x toY:y];
	}
	
	
	void _SetTransitionEffect(int transition)
	{
		[TapjoyConnect setTransitionEffect:(TJCTransitionEnum)transition];
	}
	

	void _SendIAPEvent(const char*  name, float price, int quantity, const char*  currencyCode)
	{
		[TapjoyConnect sendIAPEvent:CreateNSString(name) price:price quantity:quantity currencyCode:CreateNSString(currencyCode)];
	}
}

