#include "VadrCustomVideoPlayer.h"
//#include "iPhone_View.h"

#import <UIKit/UIKit.h>

#include <stdlib.h>
#include <string.h>
#include <stdint.h>

extern "C" __attribute__((visibility ("default"))) NSString *const kUnityViewDidRotate;

@interface VadrCustomVideoPlayerInterface : NSObject <VadrCustomVideoPlayerDelegate> {
@public
    VadrCustomVideoPlayer *player;
    CGRect margin;
    bool bLoop;
    
    bool m_bFinish;
    bool m_bUnload;
    bool m_bLoading;
    
    bool m_bLoopPlay;
    NSURL* m_videoURL;
}

- (void)onPlayerReady;

- (void)onPlayerDidFinishPlayingVideo;


@end

@implementation VadrCustomVideoPlayerInterface

- (void)loadVideo:(NSURL *)videoURL {
    m_bFinish = false;
    [player loadVideo:videoURL];
}
- (void)playVideo{

    if ([player readyToPlay])
        [self play];
}



- (void)onPlayerReady {
    

    m_bLoading = false;
    
    if(m_bUnload == true)
    {
        m_bUnload = false;
        [self unload];
    }
    
    if( m_bLoopPlay == true)
    {
        [self play];
        m_bLoopPlay =false;
    }
   
    
    
}

- (void)resizeView {
   /* //FIXME Orientationが変更された時にうまくリサイズされていない view frame更新

    CGFloat scale = UnityGetGLView().contentScaleFactor;
    UIDeviceOrientation orientation = [[UIDevice currentDevice] orientation];
    CGRect bounds;

    if (orientation) {
        bounds.size.width = UnityGetGLView().bounds.size.width - (margin.origin.x + margin.size.width) / scale;
        bounds.size.height = UnityGetGLView().bounds.size.height - (margin.origin.y + margin.size.height) / scale;
    } else {
        bounds.size.width = UnityGetGLView().bounds.size.height - (margin.origin.x + margin.size.width) / scale;
        bounds.size.height = UnityGetGLView().bounds.size.width - (margin.origin.y + margin.size.height) / scale;
    }

    view.bounds = bounds;
    view.center = CGPointMake(view.bounds.size.width / 2 + margin.origin.x / scale, view.bounds.size.height / 2 + margin.origin.y / scale);*/
}

- (void)play {
    m_bFinish = false;
    
    [player playToTexture];
    
}

- (void)unload {
    
  
    if( m_bLoading == true)
    {
        m_bUnload = true;
        return;
    }

    [player unloadPlayer];
}

- (void)onPlayerDidFinishPlayingVideo {
    
    
    if(bLoop)
    {
        if( [m_videoURL isFileURL])
        {
            [player seekTo:0.0f];
            [self play];
        }
        else
        {
            [self unload];
            [self loadVideo:m_videoURL];
            m_bLoopPlay = true;
        }
        
        
        
    }
    else
    {
        //[self unload];
        m_bFinish = true;
    }
    
}
@end



const int PLAYER_MAX = 8;
static VadrCustomVideoPlayerInterface * _Player[PLAYER_MAX];
static bool _PlayerUsed[PLAYER_MAX] = {0,0,0,0,0,0,0,0};

static VadrCustomVideoPlayerInterface *_GetPlayer(int iID) {
    
    if(iID < 0 || iID >= PLAYER_MAX)
        return  nil;
    
    if (!_Player[iID]) {
        _Player[iID] = [[VadrCustomVideoPlayerInterface alloc] init];
        _Player[iID]->player = [[VadrCustomVideoPlayer alloc] init];
        _Player[iID]->player.delegate = _Player[iID].self;
        _PlayerUsed[iID] = true;
    }

    if (!_Player[iID]->player) {
        _Player[iID]->player = [[VadrCustomVideoPlayer alloc] init];
        _Player[iID]->player.delegate = _Player[iID]->player.self;
    }

    return _Player[iID];
}

static NSURL *_GetUrl(const char *videoURL) {
    NSURL *url = nil;
    if (::strstr(videoURL, "://"))
        url = [NSURL URLWithString:[NSString stringWithUTF8String:videoURL]];
    else
        url = [NSURL fileURLWithPath:[[[[NSBundle mainBundle] bundlePath] stringByAppendingPathComponent:@"Data/Raw/"] stringByAppendingPathComponent:[NSString stringWithUTF8String:videoURL]]];
    return url;
}

extern "C" int VadrVideoPlayerPluginCreateInstance()
{
    for(int i = 0; i < PLAYER_MAX; i++)
    {
        if(_PlayerUsed[i] == false)
        {
            VadrCustomVideoPlayerInterface * player = _GetPlayer(i);
            player->bLoop = false;
            return i;
        }
    }
    
    return -1;
}

extern "C" void VadrVideoPlayerPluginDestroyInstance(int iID)
{
    if(iID < 0 || iID >= PLAYER_MAX)
        return;
    
    if(_Player[iID])
    {
        if(_Player[iID]->player)
        {
            [_Player[iID]->player unloadPlayer];
            //[_Player[iID]->player dealloc];
            _Player[iID]->player  = NULL;
            
            
        }
        
        
        //[_Player[iID] dealloc];
        _Player[iID] = NULL;
        
    }
    
    _PlayerUsed[iID] = false;
    
    
}


extern "C" void VadrVideoPlayerPluginLoadVideo(int iID,const char *videoURL) {

    if(iID < 0 || iID >= PLAYER_MAX)
        return;
    
    
    
    if (_GetPlayer(iID)->player.isPlaying) {
        [_GetPlayer(iID)->player unloadPlayer];
    }
    
    _GetPlayer(iID)->m_bFinish = false;
    _GetPlayer(iID)->m_bLoading = true;
    
    _GetPlayer(iID)->m_videoURL = _GetUrl(videoURL);

    [_GetPlayer(iID) loadVideo:_GetUrl(videoURL)];
    
}

extern "C" void VadrVideoPlayerPluginPlayVideo(int iID) {
    
    if(iID < 0 || iID >= PLAYER_MAX)
        return;
    
    _GetPlayer(iID)->m_bFinish = false;
    
    [_GetPlayer(iID) playVideo];
}

extern "C" void VadrVideoPlayerPluginSetLoop(int iID,bool bLoop) {
    
    if(iID < 0 || iID >= PLAYER_MAX)
        return;
    
    _GetPlayer(iID)->bLoop = bLoop;
}

extern "C" void VadrVideoPlayerPluginSetVolume(int iID,float fVolume) {
    
    if(iID < 0 || iID >= PLAYER_MAX)
        return;
    
    [_GetPlayer(iID)->player setAudioVolume:fVolume];
}

extern "C" void VadrVideoPlayerPluginPauseVideo(int iID) {

    if(iID < 0 || iID >= PLAYER_MAX)
        return;
    
    [_GetPlayer(iID)->player pause];
}

extern "C" void VadrVideoPlayerPluginResumeVideo(int iID) {

    if(iID < 0 || iID >= PLAYER_MAX)
        return;
    
    [_GetPlayer(iID)->player resume];
}

extern "C" void VadrVideoPlayerPluginRewindVideo(int iID) {

    if(iID < 0 || iID >= PLAYER_MAX)
        return;
    
}
extern "C" bool VadrVideoPlayerPluginCanOutputToTexture(const char *videoURL) {

    return [VadrCustomVideoPlayer CanPlayToTexture:_GetUrl(videoURL)];
}

extern "C" bool VadrVideoPlayerPluginPlayerReady(int iID) {

    if(iID < 0 || iID >= PLAYER_MAX)
        return false;
    
    return [_GetPlayer(iID)->player readyToPlay];
}

extern "C" float VadrVideoPlayerPluginDurationSeconds(int iID) {

    if(iID < 0 || iID >= PLAYER_MAX)
        return 0.0f;
    
    return [_GetPlayer(iID)->player durationSeconds];
}

extern "C" void VadrVideoPlayerPluginExtents(int iID,int *w, int *h) {

    if(iID < 0 || iID >= PLAYER_MAX)
        return;
    
    CGSize sz = [_GetPlayer(iID)->player videoSize];
    *w = (int) sz.width;
    *h = (int) sz.height;
}


extern "C" void VadrVideoPlayerPluginSetTexture(int iID,int iTextureID)
{
    if(iID < 0 || iID >= PLAYER_MAX)
        return;
    
    [_GetPlayer(iID)->player setTextureID:iTextureID];
}

extern "C" intptr_t VadrVideoPlayerPluginCurFrameTexture(int iID) {

    if(iID < 0 || iID >= PLAYER_MAX)
        return 0;
    
    return [_GetPlayer(iID)->player curFrameTexture];
}

extern "C" void VadrVideoPlayerPluginSeekToVideo(int iID,float time) {

    if(iID < 0 || iID >= PLAYER_MAX)
        return;
    
    [_GetPlayer(iID)->player seekTo:time];
}

extern "C" float VadrVideoPlayerPluginCurTimeSeconds(int iID) {
    
    if(iID < 0 || iID >= PLAYER_MAX)
        return 0.0f;
    
    return [_GetPlayer(iID)->player curTimeSeconds];
}

extern "C" bool VadrVideoPlayerPluginIsPlaying(int iID) {

    if(iID < 0 || iID >= PLAYER_MAX)
        return false;
    
    if (!_GetPlayer(iID)->player)return false;
    return [_GetPlayer(iID)->player isPlaying];
}

extern "C" void VadrVideoPlayerPluginStopVideo(int iID) {

    if(iID < 0 || iID >= PLAYER_MAX)
        return;
    
    if (_GetPlayer(iID)->player) {
        [_GetPlayer(iID) unload];
    }
}

extern "C" bool VadrVideoPlayerPluginFinish(int iID) {
    if(iID < 0 || iID >= PLAYER_MAX)
        return false;
    
    if (_GetPlayer(iID)->player) {
        return _GetPlayer(iID)->m_bFinish;
    }
    
    return false;

}

extern "C" bool VadrVideoPlayerPluginError(int iID) {
    if(iID < 0 || iID >= PLAYER_MAX)
        return false;
    
    if (_GetPlayer(iID)->player) {
        return [_GetPlayer(iID)->player getError ];
        //return _GetPlayer(iID)->player get;
        
    }
    
    return false;
}

extern "C" void VadrVideoPlayerPluginSetSpeed(int iID,float fSpeed) {
        if(iID < 0 || iID >= PLAYER_MAX)
            return;
        
        if (_GetPlayer(iID)->player) {
            [_GetPlayer(iID)->player setSpeed:fSpeed ];
            //return _GetPlayer(iID)->player get;
            
        }
    
    
    
}