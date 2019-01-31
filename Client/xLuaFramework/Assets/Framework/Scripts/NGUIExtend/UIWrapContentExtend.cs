/// <summary>
/// 对NGUI的UIWrapContent进行的扩展
/// 实现下拉刷新、上拉加载功能
/// </summary>
[XLua.ReflectionUse]
class UIWrapContentExtend : UIWrapContent
{
    /// <summary>
    /// 由于WrapContent组件会设置所属的UIPanel的onClipMove，所以如果还需要使用UIPanel的onClipMove，需要继承出来
    /// </summary>
    public delegate void OnWrapContentMoved(UIPanel panel);

    public OnWrapContentMoved onWrapContentMove;

    protected override void Start()
    {
        base.Start();
    }

    protected override void OnMove(UIPanel panel)
    {
        base.OnMove(panel);
        if (onWrapContentMove != null)
        {
            onWrapContentMove(panel);
        }
    }

    public override void SortBasedOnScrollMovement()
    {
        base.SortBasedOnScrollMovement();
    }

    public override void SortAlphabetically()
    {
        base.SortAlphabetically();
    }

    protected override void ResetChildPositions()
    {
        base.ResetChildPositions();
    }

    public override void WrapContent()
    {
        base.WrapContent();
    }
}

