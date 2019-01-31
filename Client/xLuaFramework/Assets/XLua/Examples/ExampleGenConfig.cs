/*
 * Tencent is pleased to support the open source community by making xLua available.
 * Copyright (C) 2016 THL A29 Limited, a Tencent company. All rights reserved.
 * Licensed under the MIT License (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the License at
 * http://opensource.org/licenses/MIT
 * Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
*/

using System.Collections.Generic;
using System;
using System.IO;
using Debuger;
using GameFrame;
using GameShare;
using UnityEngine;
using UnityEngine.SceneManagement;
using XLua;
//using System.Reflection;
//using System.Linq;

//配置的详细介绍请看Doc下《XLua的配置.doc》
public static class ExampleGenConfig
{
    //lua中要使用到C#库的配置，比如C#标准库，或者Unity API，第三方库等。
    [LuaCallCSharp] public static List<Type> LuaCallCSharp = new List<Type>()
    {
        typeof(System.Object),
        typeof(UnityEngine.Object),
        typeof(Vector2),
        typeof(Vector3),
        typeof(Vector4),
        typeof(Quaternion),
        typeof(Color),
        typeof(Ray),
        typeof(Bounds),
        typeof(Ray2D),
        typeof(Time),
        typeof(GameObject),
        typeof(Component),
        typeof(Behaviour),
        typeof(Transform),
        typeof(Resources),
        typeof(TextAsset),
        typeof(Keyframe),
        typeof(AnimationCurve),
        typeof(AnimationClip),
        typeof(MonoBehaviour),
        typeof(ParticleSystem),
        typeof(SkinnedMeshRenderer),
        typeof(Renderer),
        typeof(WWW),
        typeof(Light),
        typeof(Mathf),
        typeof(List<int>),
        typeof(Action<string>),
        typeof(Debug),
        //Unity 需要添加的配置
        typeof(Application),
        typeof(TouchPhase),
        typeof(Texture2D),
        typeof(Sprite),
        typeof(DateTime),
        typeof(SystemInfo),
        typeof(Collider),
        typeof(Collider2D),
        typeof(Coroutine),
        typeof(Math),
        typeof(SceneManager),
        typeof(AsyncOperation),
        typeof(System.Random),
        typeof(Path),
        typeof(AudioClip),
        //NGUI 配置 => Interaction
        typeof(EnvelopContent),
        typeof(LanguageSelection),
        typeof(TypewriterEffect),
        typeof(UIButton),
        typeof(UIButtonActivate),
        typeof(UIButtonColor),
        typeof(UIButtonKeys),
        typeof(UIButtonMessage),
        typeof(UIButtonOffset),
        typeof(UIButtonRotation),
        typeof(UIButtonScale),
        typeof(UICenterOnChild),
        typeof(UICenterOnClick),
        typeof(UIDragCamera),
        typeof(UIDragDropContainer),
        typeof(UIDragDropItem),
        typeof(UIDragDropRoot),
        typeof(UIDraggableCamera),
        typeof(UIDragObject),
        typeof(UIDragResize),
        typeof(UIDragScrollView),
        typeof(UIEventTrigger),
        typeof(UIForwardEvents),
        typeof(UIGrid),
        typeof(UIImageButton),
        typeof(UIKeyBinding),
        typeof(UIKeyNavigation),
        typeof(UIPlayAnimation),
        typeof(UIPlaySound),
        typeof(UIPlayTween),
        typeof(UIPopupList),
        typeof(UIProgressBar),
        typeof(UISavedOption),
        typeof(UIScrollBar),
        typeof(UIScrollView),
        typeof(UIShowControlScheme),
        typeof(UISlider),
        typeof(UISoundVolume),
        typeof(UITable),
        typeof(UIToggle),
        typeof(UIToggledComponents),
        typeof(UIToggledObjects),
        typeof(UIWidgetContainer),
        typeof(UIWrapContent),
        //NGUI 配置 => Internal
        typeof(ActiveAnimation),
        typeof(BMFont),
        typeof(BMGlyph),
        typeof(BMSymbol),
        typeof(ByteReader),
        typeof(EventDelegate),
        typeof(Localization),
        typeof(MinMaxRangeAttribute),
        typeof(NGUIDebug),
        typeof(NGUIMath),
        typeof(NGUIText),
        typeof(NGUITools),
        typeof(PropertyBinding),
        typeof(PropertyReference),
        typeof(RealTime),
        typeof(SpringPanel),
        typeof(UIBasicSprite),
        typeof(UIDrawCall),
        typeof(UIEventListener),
        typeof(UIGeometry),
        typeof(UIRect),
        typeof(UISnapshotPoint),
        typeof(UIWidget),
        //NGUI 配置 => Tweening
        typeof(AnimatedAlpha),
        typeof(AnimatedColor),
        typeof(AnimatedWidget),
        typeof(SpringPosition),
        typeof(TweenAlpha),
        typeof(TweenColor),
        typeof(TweenFill),
        typeof(TweenFOV),
        typeof(TweenHeight),
        typeof(TweenLetters),
        typeof(TweenOrthoSize),
        typeof(TweenPosition),
        typeof(TweenRotation),
        typeof(TweenScale),
        typeof(TweenTransform),
        typeof(TweenVolume),
        typeof(TweenWidth),
        typeof(UITweener),
        //NGUI 配置 => UI
        typeof(UI2DSprite),
        typeof(UI2DSpriteAnimation),
        typeof(UIAnchor),
        typeof(UIAtlas),
        typeof(UICamera),
        typeof(UIColorPicker),
        typeof(UIFont),
        typeof(UIInput),
        typeof(UIInputOnGUI),
        typeof(UILabel),
        typeof(UILocalize),
        typeof(UIOrthoCamera),
        typeof(UIPanel),
        typeof(UIRoot),
        typeof(UISprite),
        typeof(UISpriteAnimation),
        typeof(UISpriteData),
        typeof(UIStretch),
        typeof(UITextList),
        typeof(UITexture),
        typeof(UITooltip),
        typeof(UIViewport),
        //AssetBundle 中的配置
        typeof(AssetBundle),
        typeof(AssetBundleRequest),
        typeof(AssetBundleCreateRequest),
        //FrameworkForUnity.dll 库中的配置
        typeof(WebSocketHelper),
        typeof(AuthConnectionServer),
        typeof(Connection),
        typeof(NetworkMessage),
        typeof(Opcodes),
        typeof(ConnectionPool),
        typeof(SocketMaster),
        typeof(BufferManager),
        typeof(ConnectionManager),
        typeof(SocketAsyncEventArgsPool),
        typeof(TcpClient),
        typeof(TcpServer),
        typeof(TexasHelper),
        typeof(PokerInfo),
        //Debuger.dll 库中的配置
        typeof(DebugerHelper),
        //自己写的配置
        typeof(Reporter),
    };

    //C#静态调用Lua的配置（包括事件的原型），仅可以配delegate，interface
    [CSharpCallLua]
    public static List<Type> CSharpCallLua = new List<Type>() {
                typeof(Action),
                typeof(Func<double, double, double>),
                typeof(Action<string>),
                typeof(Action<double>),
                typeof(UnityEngine.Events.UnityAction),
                typeof(System.Collections.IEnumerator)
            };

    //黑名单
    [BlackList]
    public static List<List<string>> BlackList = new List<List<string>>()  {
                new List<string>(){"System.Xml.XmlNodeList", "ItemOf"},
                new List<string>(){"UnityEngine.WWW", "movie"},
    #if UNITY_WEBGL
                new List<string>(){"UnityEngine.WWW", "threadPriority"},
    #endif
                new List<string>(){"UnityEngine.Texture2D", "alphaIsTransparency"},
                new List<string>(){"UnityEngine.Security", "GetChainOfTrustValue"},
                new List<string>(){"UnityEngine.CanvasRenderer", "onRequestRebuild"},
                new List<string>(){"UnityEngine.Light", "areaSize"},
                new List<string>(){"UnityEngine.Light", "lightmapBakeType"},
                new List<string>(){"UnityEngine.WWW", "MovieTexture"},
                new List<string>(){"UnityEngine.WWW", "GetMovieTexture"},
                new List<string>(){"UnityEngine.AnimatorOverrideController", "PerformOverrideClipListCleanup"},
    #if !UNITY_WEBPLAYER
                new List<string>(){"UnityEngine.Application", "ExternalEval"},
    #endif
                new List<string>(){"UnityEngine.GameObject", "networkView"}, //4.6.2 not support
                new List<string>(){"UnityEngine.Component", "networkView"},  //4.6.2 not support
                new List<string>(){"System.IO.FileInfo", "GetAccessControl", "System.Security.AccessControl.AccessControlSections"},
                new List<string>(){"System.IO.FileInfo", "SetAccessControl", "System.Security.AccessControl.FileSecurity"},
                new List<string>(){"System.IO.DirectoryInfo", "GetAccessControl", "System.Security.AccessControl.AccessControlSections"},
                new List<string>(){"System.IO.DirectoryInfo", "SetAccessControl", "System.Security.AccessControl.DirectorySecurity"},
                new List<string>(){"System.IO.DirectoryInfo", "CreateSubdirectory", "System.String", "System.Security.AccessControl.DirectorySecurity"},
                new List<string>(){"System.IO.DirectoryInfo", "Create", "System.Security.AccessControl.DirectorySecurity"},
                new List<string>(){"UnityEngine.MonoBehaviour", "runInEditMode"},
            };
}
