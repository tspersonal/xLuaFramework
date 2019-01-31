//
//  Yz_UnityCallIos.h
//  Yz_UnityCallIos
//
//  Created by 张海涛 on 2018/9/29.
//  Copyright © 2018年 张海涛. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>

@interface Yz_UnityCallIos : UIViewController<UIImagePickerControllerDelegate,UINavigationControllerDelegate>
    
    //extern C
    void Yz_IOS_OpenAlbum(char *pathName,char* _imageName);
    void Yz_IOS_CopyText(char *text);
	const char* Yz_IOS_GetPasteboard();
@end
