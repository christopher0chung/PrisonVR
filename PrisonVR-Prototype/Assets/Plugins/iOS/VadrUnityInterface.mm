//
//  VadrUnityInterface.m
//  VadrUnityPlugin
//
//  Created by Abhishek Bansal on 20/09/16.
//  Copyright © 2016 Abhishek Bansal. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>
#import "VadrWebviewWrapper.h"
static VadrUnityiOSPlugin* vadrUnityPlugin;
static VadrWebviewWrapper *vadrWebViewWrapper;

void initVadrUnityPlugin(){
    if (vadrUnityPlugin == nil){
        vadrUnityPlugin = [VadrUnityiOSPlugin sharedVadrPlugin];
    }
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

//Wrapper to call unitySendMessageFucntion
//static void VadrSendUnityMessage

extern "C"{
    const char* _getPhoneAdId(){
        initVadrUnityPlugin();
        NSLog(@"User info debug: %@", [vadrUnityPlugin getAdvertisingId]);
        return MakeStringCopy([[vadrUnityPlugin getAdvertisingId] UTF8String]);
    }
    
    const int _getPhoneLimitAdTracking(){
        initVadrUnityPlugin();
        NSLog(@"User info debug: %i", [vadrUnityPlugin getLimitAdTracking]);
        return [vadrUnityPlugin getLimitAdTracking];
    }
    
    const char* _getConnectionType(){
        initVadrUnityPlugin();
        return MakeStringCopy([[vadrUnityPlugin getConnectionType] UTF8String]);
    }
    
    const char* _getCarrierName(){
        initVadrUnityPlugin();
        return MakeStringCopy([[vadrUnityPlugin getCarrierName] UTF8String]);
    }
    
    const char* _getDeviceInfo(){
        initVadrUnityPlugin();
        return MakeStringCopy([[vadrUnityPlugin getDeviceData] UTF8String]);
    }

    const char* _getUserAgent(){
        initVadrUnityPlugin();
        return MakeStringCopy([[vadrUnityPlugin getUserAgent] UTF8String]);
    }

    void _setNotification(char* title, char* description, char* actionToPerform, char* link){
        NSString *notifTitle = [NSString stringWithUTF8String:title];
        NSString *notifDesc = [NSString stringWithUTF8String:description];
        //NSString *notifAction = [NSString stringWithUTF8String:actionToPerform];
        NSString *notifLink = [NSString stringWithUTF8String:link];
        [VadrManageNotifications vadrCreateNotification:notifTitle withDescription:notifDesc andRedirectLink:notifLink];
    }
    
    const char* _getChacheDirectoryString(){
        return MakeStringCopy([[VadrUnityiOSPlugin getCacheDirectoryLocation] UTF8String]);
    }
    
    // Convert GIF to png
    int _getPngFromGif(char* fileLocation){
        NSString *nsFileLocation = [NSString stringWithUTF8String:fileLocation];
        return [VadrUnityiOSPlugin getFirstFrameOfGif:nsFileLocation];
    }
    
    //Webview to Unity interface
    void _startWebview(int graphicType, char* urlToLoad){
        if (vadrWebViewWrapper == nil){
            NSLog(@"Creating Library object");
            vadrWebViewWrapper = [[VadrWebviewWrapper alloc] initWithWebpage:[NSString stringWithUTF8String:urlToLoad]];
            [vadrWebViewWrapper SetGraphics:graphicType];
            //            [vadrWebViewLib loadWebPage:[NSString stringWithUTF8String:urlToLoad]];
        }
    }
    intptr_t _createTexture(){
        if (vadrWebViewWrapper != nil){
            return [vadrWebViewWrapper setupTexture];
        }else{
            return NULL;
        }
    }
    
    intptr_t _updateTexture(){
        if (vadrWebViewWrapper != nil){
            return [vadrWebViewWrapper updateTexture];
        }else{
            return NULL;
        }
    }
    
    void _scrollWebview(float x, float y){
        NSLog(@"Scroll webview received");
        if (vadrWebViewWrapper != nil){
            [vadrWebViewWrapper ScrollWebViewByX:x andY:y];
        }
    }
    
    void _clickWebview(int x, int y){
        NSLog(@"%@", [NSString stringWithFormat:@"Click on webview recieved for %d, %d", x, y]);
        if (vadrWebViewWrapper != nil){
            [vadrWebViewWrapper ClickOnWebviewWithX:x andY:y];
        }
    }
    
    void _webviewGoBack(){
        if (vadrWebViewWrapper != nil){
            [vadrWebViewWrapper WebviewGoBack];
        }
    }

    int _getWebviewCanGoBack(){
        return [vadrWebViewWrapper CanWebviewGoBack];
    }
    
    void _destroyWebview(){
        if (vadrWebViewWrapper != nil){
            [vadrWebViewWrapper destroyVadrWebview];
            vadrWebViewWrapper = nil;
        }
    }

}
#import "VadrWebviewWrapper.h"
#include "UnityMetalSupport.h"

@interface VadrWebviewWrapper()
@property (strong) VadrWebViewLib *vadrWebviewLib;
// stores what graphics framework is used: 0. Metal 1. OpenGL
@property int graphicType;
@end

@implementation VadrWebviewWrapper
-(id)initWithWebpage:(NSString *)URLToLoad{
    if (self = [super init]){
        self.vadrWebviewLib = [[VadrWebViewLib alloc] initWithWidth:512 andHeight:512];
        [self.vadrWebviewLib.vadrWebview setNavigationDelegate:self];
        [self.vadrWebviewLib loadWebPage:URLToLoad];
    }
    return self;
}

-(void) SetGraphics:(int)graphicsType{
    self.graphicType = graphicsType;
    [self.vadrWebviewLib SetGraphics:graphicsType];
}

-(intptr_t) setupTexture{
    if (self.graphicType == 0){
        return [self CreateMetalTextureWithWidth:512 andHeight:512];
    }
    else{
        return [self.vadrWebviewLib setupTexture];
    }
}
-(intptr_t) updateTexture{
    return [self.vadrWebviewLib updateTexture];
}

// close everything
- (void) destroyVadrWebview{
    [self.vadrWebviewLib destroyVadrWebview];
    self.vadrWebviewLib.vadrWebview.navigationDelegate = nil;
    self.vadrWebviewLib = nil;
}

// Webview interaction functions
// Click points on Webview
- (void) ClickOnWebviewWithX: (int) xCoordinate andY: (int) yCoordinate{
    [self.vadrWebviewLib ClickOnWebviewWithX:xCoordinate andY:yCoordinate];
}
// Scroll Webview
- (void) ScrollWebViewByX: (float) xScrollAmount andY: (float) yScrollAmount{
    [self.vadrWebviewLib ScrollWebViewByX:xScrollAmount andY:yScrollAmount];
}

// Goback in webview
- (void) WebviewGoBack{
    [self.vadrWebviewLib WebviewGoBack];
}
-(int) CanWebviewGoBack{
    return [self.vadrWebviewLib CanWebviewGoBack];
}

- (uintptr_t) CreateMetalTextureWithWidth:(unsigned) w andHeight:(unsigned) h
{
#if defined(__IPHONE_8_0) && !TARGET_IPHONE_SIMULATOR
    Class MTLTextureDescriptorClass = [UnityGetMetalBundle() classNamed:@"MTLTextureDescriptor"];
    
    MTLTextureDescriptor* texDesc =
    [MTLTextureDescriptorClass texture2DDescriptorWithPixelFormat:MTLPixelFormatRGBA8Unorm width:w height:h mipmapped:NO];
    
    self.vadrWebviewLib.vadrTexture = [UnityGetMetalDevice() newTextureWithDescriptor:texDesc];
    [self.vadrWebviewLib setupTexture];
    return (uintptr_t)(__bridge_retained void*)self.vadrWebviewLib.vadrTexture;
#else
    return 0;
#endif
}

// implementing WKNavigationDelegate functions
- (void)webView:(WKWebView *)webView decidePolicyForNavigationAction:(WKNavigationAction *)navigationAction decisionHandler:(void (^)(WKNavigationActionPolicy))decisionHandler{
    if (webView == self.vadrWebviewLib.vadrWebview){
        NSURLRequest *urlToVisit = navigationAction.request;
        NSString *urlString = urlToVisit.URL.absoluteString;
        NSLog(@"Reason for navigatio is: %ld", (long)navigationAction.navigationType);
        NSLog(@"Webview Debug: Calling navigation to url: %@", urlString);
        bool open = YES;
        if ([urlString hasPrefix:@"http://"] || [urlString hasPrefix:@"https://"]){
            NSLog(@"Contains an http request");
        }else{
            if (![urlString hasPrefix:@"about:blank"]){
                NSLog(@"Can open link: %@", [[UIApplication sharedApplication] canOpenURL:urlToVisit.URL]? @"Yes":@"No");
                if ([[UIApplication sharedApplication] canOpenURL:urlToVisit.URL]){
                    if([urlString hasPrefix:@"itms"]){
                        NSLog(@"Redirects to appstore");
                        if ([urlString containsString:@"id="]){
                            NSArray *stringArray = [urlString componentsSeparatedByString:@"id="];
                            NSCharacterSet *delimiterSet = [NSCharacterSet characterSetWithCharactersInString:@"/&"];
                            NSArray *objectIds = [stringArray[1] componentsSeparatedByCharactersInSet:delimiterSet];
                            UnitySendMessage("VadrWebview(Clone)", "OpenAppStore", [[NSString stringWithFormat:@"%@___%@",objectIds[0], urlString] UTF8String]);
                            open = NO;
                        } else if([urlString containsString:@"/id"]){
                            NSArray *stringArray = [urlString componentsSeparatedByString:@"/id"];
                            NSCharacterSet *delimiterSet = [NSCharacterSet characterSetWithCharactersInString:@"?/&"];
                            NSArray *objectIds = [stringArray[1] componentsSeparatedByCharactersInSet:delimiterSet];
                            NSLog(@"Webview Debug: Calling navigation to url: %@", @"Giving command to c# script");
                            UnitySendMessage("VadrWebview(Clone)", "OpenAppStore", [[NSString stringWithFormat:@"%@___%@",objectIds[0], urlString] UTF8String]);
                            open = NO;
                        }else{
                            UnitySendMessage("VadrWebview(Clone)", "DeferredOpeningSomethinElse", [urlString UTF8String]);
                            open = YES;
                        }
                    }else{
                        NSLog(@"Some other kind of app to handle");
                        UnitySendMessage("WebviewPlane", "DeferredOpeningSomethinElse", [urlString UTF8String]);
                        open = NO;
                    }
                }
            }
        }
        if (open){
            decisionHandler(WKNavigationActionPolicyAllow);
        }else{
            decisionHandler(WKNavigationActionPolicyCancel);
            NSLog(@"Run function to open appstore");
        }
    }
}

@end

