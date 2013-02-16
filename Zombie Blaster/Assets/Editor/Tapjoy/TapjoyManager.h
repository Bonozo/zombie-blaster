//
//  TapjoyManager.h
//  TapjoyTest
//
//  Created by Mike on 1/28/11.
//  Copyright 2011 Prime31 Studios. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "TapjoyConnect.h"


typedef enum
{
	TapjoyAdPositionTopLeft,
	TapjoyAdPositionTopCenter,
	TapjoyAdPositionTopRight,
	TapjoyAdPositionCentered,
	TapjoyAdPositionBottomLeft,
	TapjoyAdPositionBottomCenter,
	TapjoyAdPositionBottomRight
} TapjoyAdPosition;


@interface TapjoyManager : NSObject <TJCAdDelegate, TJCVideoAdDelegate>
{
@private
	BOOL _isFeaturedAppReady;
}
@property (nonatomic, retain) UIView *adView;
@property (nonatomic) TapjoyAdPosition bannerPosition;
@property (nonatomic, copy) NSString *publisherId;
@property (nonatomic, copy) NSString *secretKey;
@property (nonatomic, copy) NSString *adContentSize;


+ (TapjoyManager*)sharedManager;


- (void)startWithAppId:(NSString*)appId;

- (void)setTransitionEffect:(int)effect;

- (void)getFullScreenAdWithCurrencyId:(NSString*)currencyId;

- (void)showFullScreenAd;

- (void)setFullScreenAdDelayCount:(int)delayCount;

- (void)getDailyRewardAd;

- (void)showDailyRewardAd;

- (void)createBannerWithPosition:(TapjoyAdPosition)position;

- (void)destroyBanner;

- (void)refreshBanner;

- (void)showOffers;

- (void)getTapPoints;

- (void)spendTapPoints:(int)points;

- (void)awardTapPoints:(int)points;

- (BOOL)isFeaturedAppReady;

- (void)actionComplete:(NSString*)action;

- (void)initVideoAd;

- (void)setVideoCacheCount:(int)count;

@end
