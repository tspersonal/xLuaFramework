using System;
using UnityEngine;

public enum OnChangeType
{
    None,
    Input,//输入框
    ProgressBar,//进度条
    PopupList,//下拉框
    Toggle,//文本框
    Widget,//
}

/// <summary>
/// 用于UI组建的监听事件，方便加音效
/// </summary>
[XLua.ReflectionUse]
public class UIRegister
{
    /// <summary>
    /// 点击处理
    /// </summary>
    /// <param name="go"></param>
    /// <param name="fun"></param>
    /// <param name="bUseDefaultSound">是否使用默认音频</param>
    public static void OnClickAddListener(GameObject go, Action<GameObject> fun, bool bUseDefaultSound = true)
    {
        UIEventListener.Get(go).onClick = obj =>
        {
            //TODO:音效控制
            fun(obj);
        };
    }

    /// <summary>
    /// 双击处理
    /// </summary>
    /// <param name="go"></param>
    /// <param name="fun"></param>
    /// <param name="bUseDefaultSound"></param>
    public static void OnDoubleClickAddListener(GameObject go, Action<GameObject> fun, bool bUseDefaultSound = true)
    {
        UIEventListener.Get(go).onDoubleClick = obj =>
        {
            //TODO:音效控制
            fun(obj);
        };
    }

    /// <summary>
    /// 变化处理
    /// </summary>
    /// <param name="go"></param>
    /// <param name="changeType"></param>
    /// <param name="fun"></param>
    /// <param name="bUseDefaultSound"></param>
    public static void OnChangeAddListener(GameObject go, OnChangeType changeType, Action fun, bool bUseDefaultSound = true)
    {
        switch (changeType)
        {
            case OnChangeType.None:
                break;
            case OnChangeType.Input:
                UIInput inp = go.GetComponent<UIInput>();
                if (inp != null)
                {
                    EventDelegate.Add(inp.onChange, () =>
                    {
                        //TODO:音效控制
                        fun();
                    });
                }
                else
                {
                    throw new Exception("UIInput组件不存在，注册事件<" + fun + ">失败！");
                }
                break;
            case OnChangeType.ProgressBar:
                UIProgressBar pro = go.GetComponent<UIProgressBar>();
                if (pro != null)
                {
                    EventDelegate.Add(pro.onChange, () =>
                    {
                        //TODO:音效控制
                        fun();
                    });
                }
                else
                {
                    throw new Exception("UIInput组件不存在，注册事件<" + fun + ">失败！");
                }
                break;
            case OnChangeType.PopupList:
                UIPopupList pop = go.GetComponent<UIPopupList>();
                if (pop != null)
                {
                    EventDelegate.Add(pop.onChange, () =>
                    {
                        //TODO:音效控制
                        fun();
                    });
                }
                else
                {
                    throw new Exception("UIInput组件不存在，注册事件<" + fun + ">失败！");
                }
                break;
            case OnChangeType.Toggle:
                UIToggle tog = go.GetComponent<UIToggle>();
                if (tog != null)
                {
                    EventDelegate.Add(tog.onChange, () =>
                    {
                        //TODO:音效控制
                        fun();
                    });
                }
                else
                {
                    throw new Exception("UIInput组件不存在，注册事件<" + fun + ">失败！");
                }
                break;
        }
    }

    /// <summary>
    /// 提交处理，即手机输入法的完成键
    /// </summary>
    /// <param name="go"></param>
    /// <param name="fun"></param>
    /// <param name="bUseDefaultSound"></param>
    public static void OnSubmitAddListener(GameObject go, Action<GameObject> fun, bool bUseDefaultSound = true)
    {
        UIEventListener.Get(go).onSubmit = obj =>
        {
            //TODO:音效控制
            fun(obj);
        };
    }

    /// <summary>
    /// 停留处理
    /// </summary>
    /// <param name="go"></param>
    /// <param name="fun"></param>
    /// <param name="bUseDefaultSound"></param>
    public static void OnHoverAddListener(GameObject go, Action<GameObject, bool> fun, bool bUseDefaultSound = true)
    {

        UIEventListener.Get(go).onHover = (obj, isHover) =>
        {
            if (isHover)
            {
                //TODO:音效控制
            }
            else
            {

            }
            fun(obj, isHover);
        };
    }

    /// <summary>
    /// 按压处理
    /// </summary>
    /// <param name="go"></param>
    /// <param name="fun"></param>
    /// <param name="bUseDefaultSound"></param>
    public static void OnPressAddListener(GameObject go, Action<GameObject, bool> fun, bool bUseDefaultSound = true)
    {

        UIEventListener.Get(go).onPress = (obj, isPress) =>
        {
            if (isPress)
            {
                //TODO:音效控制
            }
            else
            {

            }
            fun(obj, isPress);
        };

    }

    /// <summary>
    /// 选中处理
    /// </summary>
    /// <param name="go"></param>
    /// <param name="fun"></param>
    /// <param name="bUseDefaultSound"></param>
    public static void OnSelectAddListener(GameObject go, Action<GameObject, bool> fun, bool bUseDefaultSound = true)
    {

        UIEventListener.Get(go).onSelect = (obj, isSelect) =>
        {
            if (isSelect)
            {
                //TODO:音效控制
            }
            else
            {

            }
            fun(obj, isSelect);
        };
    }

    /// <summary>
    /// 滑动处理
    /// </summary>
    /// <param name="go"></param>
    /// <param name="fun"></param>
    /// <param name="bUseDefaultSound"></param>
    public static void OnScrollAddListener(GameObject go, Action<GameObject, float> fun, bool bUseDefaultSound = true)
    {

        UIEventListener.Get(go).onScroll = (obj, delta) =>
        {
            //TODO:音效控制
            fun(obj, delta);
        };
    }

    /// <summary>
    /// 开始拖动处理
    /// </summary>
    /// <param name="go"></param>
    /// <param name="fun"></param>
    /// <param name="bUseDefaultSound"></param>
    public static void OnDragStartAddListener(GameObject go, Action<GameObject> fun, bool bUseDefaultSound = true)
    {

        UIEventListener.Get(go).onDragStart = obj =>
        {
            //TODO:音效控制
            fun(obj);
        };
    }

    /// <summary>
    /// 拖动处理
    /// </summary>
    /// <param name="go"></param>
    /// <param name="fun"></param>
    /// <param name="bUseDefaultSound"></param>
    public static void OnDragAddListener(GameObject go, Action<GameObject, Vector2> fun, bool bUseDefaultSound = true)
    {

        UIEventListener.Get(go).onDrag = (obj, delta) =>
        {
            //TODO:音效控制
            fun(obj, delta);
        };
    }

    /// <summary>
    /// 结束拖动处理
    /// </summary>
    /// <param name="go"></param>
    /// <param name="fun"></param>
    /// <param name="bUseDefaultSound"></param>
    public static void OnDragEndAddListener(GameObject go, Action<GameObject> fun, bool bUseDefaultSound = true)
    {

        UIEventListener.Get(go).onDragEnd = obj =>
        {
            //TODO:音效控制
            fun(obj);
        };
    }

    /// <summary>
    /// 拖动松开处理
    /// </summary>
    /// <param name="go"></param>
    /// <param name="fun"></param>
    /// <param name="bUseDefaultSound"></param>
    public static void OnDropAddListener(GameObject go, Action<GameObject, GameObject> fun, bool bUseDefaultSound = true)
    {

        UIEventListener.Get(go).onDrop = (obj, objDrop) =>
        {
            //TODO:音效控制
            fun(obj, objDrop);
        };
    }

    /// <summary>
    /// 键盘按下处理
    /// </summary>
    /// <param name="go"></param>
    /// <param name="fun"></param>
    /// <param name="bUseDefaultSound"></param>
    public static void OnKeyAddListener(GameObject go, Action<GameObject, KeyCode> fun, bool bUseDefaultSound = true)
    {

        UIEventListener.Get(go).onKey = (obj, key) =>
        {
            //TODO:音效控制
            fun(obj, key);
        };
    }
}
