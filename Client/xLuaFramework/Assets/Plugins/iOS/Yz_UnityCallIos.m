//
//  Yz_UnityCallIos.m
//  Yz_UnityCallIos
//
//  Created by 张海涛 on 2018/9/29.
//  Copyright © 2018年 张海涛. All rights reserved.
//

#import "Yz_UnityCallIos.h"

static NSString* gameObjectName;
static NSString* imageName;

@implementation Yz_UnityCallIos
    
-(void) OpenTarget : (UIImagePickerControllerSourceType)type
{
    UIImagePickerController *picker;
    picker = [[UIImagePickerController alloc] init];
    
    picker.delegate = self;
    picker.allowsEditing = YES;
    picker.sourceType = type;
    
    if(picker.sourceType == UIImagePickerControllerSourceTypePhotoLibrary && [[UIDevice currentDevice] userInterfaceIdiom] == UIUserInterfaceIdiomPad)
    {
        picker.modalPresentationStyle = UIModalPresentationPopover;
        UIPopoverPresentationController *popover = picker.popoverPresentationController;
        popover.delegate = self;
        popover.sourceRect = CGRectMake(0, 0, 0, 0);
        popover.sourceView = self.view;
        popover.permittedArrowDirections = UIPopoverArrowDirectionAny;
        [self presentViewController:picker animated:YES completion:nil];
    }
    else
    {
        [self presentViewController:picker animated:YES completion:^{}];
    }
}
    
-(void)imagePickerController:(UIImagePickerController *)picker didFinishPickingMediaWithInfo:(NSDictionary<NSString *,id> *)info
{
    [picker dismissViewControllerAnimated:YES completion:^{}];
    UIImage *image = [info objectForKey:@"UIImagePickerControllerEditedImage"];
    if (image == nil)
    {
        image = [info objectForKey:@"UIImagePickerControllerOriginalImage"];
    }
    
    if (image.imageOrientation != UIImageOrientationUp)
    {
        image = [self fixOrientation:image];
    }
    NSString *imagePath = [self GetSavePath:@"image.jpg"];
    [self SaveFileToDoc:image path:imagePath];
}
//获取保存文件的路径 如果有返回路径 没有创建一个返回
-(NSString*)GetSavePath:(NSString *)filename
{
    NSArray *pathArray = NSSearchPathForDirectoriesInDomains(NSDocumentDirectory, NSUserDomainMask, YES);
    NSString *docPath = [pathArray objectAtIndex:0];
    return [docPath stringByAppendingPathComponent:filename];
}
//将图片保存到沙盒种
-(void) SaveFileToDoc:(UIImage *)image path:(NSString *)path
{
    NSData * data;
    if(UIImagePNGRepresentation(image) == nil)
    {
        data = UIImageJPEGRepresentation(image, 1);
    }
    else
    {
        data = UIImagePNGRepresentation(image);
    }
    [data writeToFile:path atomically:YES];
    UnitySendMessage([gameObjectName UTF8String],"onImagePath",[imageName UTF8String]);
}
    
    
#pragma mark 图片处理方法//图片旋转处理
- (UIImage *)fixOrientation:(UIImage *)aImage
{
    CGAffineTransform transform = CGAffineTransformIdentity;
    switch (aImage.imageOrientation) {
        case UIImageOrientationDown:
        case UIImageOrientationDownMirrored:
            transform = CGAffineTransformTranslate(transform, aImage.size.width, aImage.size.height);         transform = CGAffineTransformRotate(transform, M_PI);
            break;
        case UIImageOrientationLeft:
        case UIImageOrientationLeftMirrored:
            transform = CGAffineTransformTranslate(transform, aImage.size.width, 0);
            transform = CGAffineTransformRotate(transform, M_PI_2);
            break;
        case UIImageOrientationRight:
        case UIImageOrientationRightMirrored:
            transform = CGAffineTransformTranslate(transform, 0, aImage.size.height);
            transform = CGAffineTransformRotate(transform, -M_PI_2);
            break;
        default:
            break;
    }
    switch (aImage.imageOrientation) {
        case UIImageOrientationUpMirrored:
        case UIImageOrientationDownMirrored:
            transform = CGAffineTransformTranslate(transform, aImage.size.width, 0);
            transform = CGAffineTransformScale(transform, -1, 1);
            break;
        case UIImageOrientationLeftMirrored:
        case UIImageOrientationRightMirrored:
            transform = CGAffineTransformTranslate(transform, aImage.size.height, 0);
            transform = CGAffineTransformScale(transform, -1, 1);
            break;
        default:
            break;
    }        // Now we draw the underlying CGImage into a new context, applying the transform    // calculated above.
    CGContextRef ctx = CGBitmapContextCreate(NULL, aImage.size.width, aImage.size.height,                                             CGImageGetBitsPerComponent(aImage.CGImage), 0,                                             CGImageGetColorSpace(aImage.CGImage),                                             CGImageGetBitmapInfo(aImage.CGImage));
    CGContextConcatCTM(ctx, transform);
    switch (aImage.imageOrientation) {
        case UIImageOrientationLeft:
        case UIImageOrientationLeftMirrored:
        case UIImageOrientationRight:
        case UIImageOrientationRightMirrored:
            CGContextDrawImage(ctx, CGRectMake(0,0,aImage.size.height,aImage.size.width), aImage.CGImage);
            break;
        default:
            CGContextDrawImage(ctx, CGRectMake(0,0,aImage.size.width,aImage.size.height), aImage.CGImage);
            break;
    }
    // And now we just create a new UIImage from the drawing context
    CGImageRef cgimg = CGBitmapContextCreateImage(ctx);
    UIImage *img = [UIImage imageWithCGImage:cgimg];
    CGContextRelease(ctx);
    CGImageRelease(cgimg);
    return img;
}
    
void Yz_IOS_OpenAlbum(char *pathName,char* _imageName)
{
    imageName = [NSString stringWithUTF8String:_imageName];
    gameObjectName = [NSString stringWithUTF8String:pathName];
    Yz_UnityCallIos *app = [[Yz_UnityCallIos alloc]init];
    UIViewController *vc = UnityGetGLViewController();
    [vc.view addSubview:app.view];
    [app OpenTarget:UIImagePickerControllerSourceTypeSavedPhotosAlbum];
}

void Yz_IOS_CopyText(char *text)
{
    UIPasteboard *pasteboard = [UIPasteboard generalPasteboard];
    pasteboard.string = [NSString stringWithUTF8String:text];
}

const char* Yz_IOS_GetPasteboard()
{
    UIPasteboard *pasteboard = [UIPasteboard generalPasteboard];
    const char* str = "";
    if (pasteboard.string != NULL) {
        str = [pasteboard.string UTF8String];
    }
    char* res = (char*)malloc(strlen(str)+1);
    strcpy(res, str);
    return res;
}  

@end


