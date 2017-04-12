//
//  VadrWebviewWrapper.h
//  Unity-iPhone
//
//  Created by Abhishek Bansal on 06/10/16.
//
//

#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>
#import "WebKit/WebKit.h"
#import <Metal/Metal.h>
#import <QuartzCore/QuartzCore.h>

@interface VadrWebviewWrapper : NSObject<WKNavigationDelegate>
-(id)initWithWebpage:(NSString *)URLToLoad;
-(void) SetGraphics:(int)graphicsType;
- (intptr_t) setupTexture;
- (intptr_t) updateTexture;
- (void) destroyVadrWebview;
// Webview interaction functions
- (void) ClickOnWebviewWithX: (int) xCoordinate andY: (int) yCoordinate;
- (void) ScrollWebViewByX: (float) xScrollAmount andY: (float) yScrollAmount;
- (void) WebviewGoBack;
- (int) CanWebviewGoBack;

@end

@interface VadrManageNotifications : NSObject
+(void) vadrRegisterNotification;
+(void) vadrCreateNotification: (NSString *)title withDescription:(NSString *)description andRedirectLink:(NSString *)redirectUrl;
+(void) vadrHandleNotification: (UILocalNotification *)notification;
@end

@interface VadrUnityiOSPlugin : NSObject
// Created a singleton class
+ (id) sharedVadrPlugin;
// Gives out the advertising id
-(NSString *)getAdvertisingId;
// Gives out limit ad tracking as integer (0 or 1)
-(int) getLimitAdTracking;
// Gives out Home Carrier name; need not be the actual carrier
-(NSString *)getCarrierName;
// Gives out the connection type; Wifi or any other
-(NSString *)getConnectionType;
// Prints out the device information
-(void)printMobileData;
// get cache directory path
+(NSString *) getCacheDirectoryLocation;
//get png from gif
+(int) getFirstFrameOfGif: (NSString *)gifFileLocation;
//get device info
-(NSString *) getDeviceData;
//get device user agent
-(NSString *) getUserAgent;
@end

@interface VadrWebViewLib : NSObject
@property (strong) WKWebView *vadrWebview;
-(void) SetGraphics:(int)graphicsType;
@property id<MTLTexture> vadrTexture;
- (id)initWithWidth:(int)sizeW andHeight:(int)sizeH;
- (intptr_t) setupTexture;
- (intptr_t) updateTexture;
- (void) destroyVadrWebview;
// Webview interaction functions
- (void) ClickOnWebviewWithX: (int) xCoordinate andY: (int) yCoordinate;
- (void) ScrollWebViewByX: (float) xScrollAmount andY: (float) yScrollAmount;
- (void) WebviewGoBack;
-(int) CanWebviewGoBack;
-(void)loadWebPage: (NSString *)urlStringToLoad;
@end


