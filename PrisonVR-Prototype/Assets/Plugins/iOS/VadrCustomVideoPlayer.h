#pragma once

#import <CoreMedia/CMTime.h>
#import <UIKit/UIKit.h>
@class AVPlayer;


@interface VadrCustomVideoPlayerView : UIView {}
@property(nonatomic, retain) AVPlayer* player;
@end

@protocol VadrCustomVideoPlayerDelegate<NSObject>
- (void)onPlayerReady;
- (void)onPlayerDidFinishPlayingVideo;
@end

@interface VadrCustomVideoPlayer : NSObject
{
    id<VadrCustomVideoPlayerDelegate> delegate;
}
@property (nonatomic, retain) id delegate;

+ (BOOL)CanPlayToTexture:(NSURL*)url;

- (BOOL)loadVideo:(NSURL*)url;
- (BOOL)readyToPlay;
- (void)unloadPlayer;

- (BOOL)playToView:(VadrCustomVideoPlayerView*)view;
- (BOOL)playToTexture;
- (BOOL)isPlaying;
- (BOOL)getError;

- (intptr_t)curFrameTexture;

- (void)pause;
- (void)resume;

- (void)rewind;
- (void)seekToTimestamp:(CMTime)time;
- (void)seekTo:(float)timeSeconds;
- (void)setSpeed:(float)fSpeed;

- (BOOL)setAudioVolume:(float)volume;

- (CMTime)duration;
- (float)durationSeconds;
- (float)curTimeSeconds;
- (CGSize)videoSize;
- (void)setTextureID:(intptr_t)id;
@end
