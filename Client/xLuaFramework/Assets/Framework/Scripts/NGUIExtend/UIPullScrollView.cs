using UnityEngine;

/// <summary>
/// 对UIScrollView拖动的扩展
/// 实现下拉刷新、上拉加载功能
/// 注意：ScrollView子对象的Grid或者Table组件的中心点需要放到四周的边上根据拖动方向决定
/// </summary>
[XLua.ReflectionUse]
public class UIPullScrollView : MonoBehaviour
{
    /// <summary>
    /// 拉动属性类型
    /// </summary>
    public enum PullPropertyType
    {
        // 无相应
        None = 0,

        // 上拉
        PullUp = 1,

        // 下拉
        PullDown = 2,

        // 左拉
        PullLeft = 3,

        // 右拉
        PullRight = 4,

        // 上下拉
        PullUpOrDown = 5,

        // 左右拉
        PullLeftOrRight = 6,
    }

    /// <summary>
    /// 拉动状态类型，权重方代表中心点的方向，权轻方为中心点的对方向
    /// </summary>
    public enum PullStateType
    {
        // None
        None = 0,

        // 正常在中间拖动
        Normal = 1,

        // 权重方开始拉动
        StartPullWight = 2,

        // 权重方拉动中
        OnPullWight = 3,

        // 权重方成功拉动
        SucceedPullWight = 4,

        // 权重方失败拉动
        FaildPullWight = 5,

        // 权轻方开始拉动
        StartPullGently = 6,

        // 权轻方拉动中
        OnPullGently = 7,

        // 权轻方成功拉动
        SucceedPullGently = 8,

        // 权轻方失败拉动
        FaildPullGently = 9,

        /*// 开始上拉
        StartPullUp = 2,

        // 上拉中
        OnPullUp = 3,

        // 成功上拉
        SucceedPullUp = 4,

        // 失败上拉
        FaildPullUp = 5,

        // 开始下拉
        StartPullDown = 6,

        // 下拉中
        OnPullDown = 7,

        // 成功下拉
        SucceedPullDown = 8,

        // 失败下拉
        FaildPullDown = 9,

        // 开始左拉
        StartPullLeft = 10,

        // 左拉中
        OnPullLeft = 11,

        // 成功左拉
        SucceedPullLeft = 12,

        // 失败左拉
        FaildPullLeft = 13,

        // 开始右拉
        StartPullRight = 14,

        // 右拉中
        OnPullRight = 15,

        // 成功右拉
        SucceedPullRight = 16,

        // 失败右拉
        FaildPullRight = 17,*/
    }

    #region 声明委托

    public delegate void OnPullClippingMoved(UIPanel panel);

    public delegate void OnPullDragStarted();

    public delegate void OnPullDragFinished();

    public delegate void OnPullMomentumMove();

    public delegate void OnPullStoppedMoving();

    public delegate void OnPullingMoved(int type, Vector3 constraint, Vector3 weightBorderPos, Vector3 gentlyBorderPos);

    /// <summary>
    /// 当面板的剪辑区域移动时触发的事件回调
    /// </summary>
    public OnPullClippingMoved onPullClippingMove;

    /// <summary>
    /// UIScrollView 开始拖动触发的事件回调
    /// </summary>
    public OnPullDragStarted onPullDragStarted;

    /// <summary>
    /// UIScrollView 拖动结束触发的事件回调
    /// </summary>
    public OnPullDragFinished onPullDragFinished;

    /// <summary>
    /// UIScrollView OnDragFinished到OnStoppedMoving之间因为动量的移动触发的事件回调
    /// </summary>
    public OnPullMomentumMove onPullMomentumMove;

    /// <summary>
    /// UIScrollView 停止移动触发的事件回调
    /// </summary>
    public OnPullStoppedMoving onPullStoppedMoving;

    /// <summary>
    /// 开始拉动 传入当前拖动类型与对应类型的的UIPanel、
    /// 约束的偏移量、
    /// 权重边界的localPosition、
    /// 权轻边界的localPosition(当未产生约束，值一直变化，位置为UIPanel的下边界；当产生约束，值不变，为UIPanel的下边界减去约束值)
    /// </summary>
    public OnPullingMoved onPullMove;

    #endregion

    #region 字段与属性

    /// <summary>
    /// 提供外部选择拉动类型
    /// </summary>
    [SerializeField] private PullPropertyType _pullProperty;

    /// <summary>
    /// 提供外部选择拉动权重类型，即中心点在哪个方向，权重就在哪边
    /// </summary>
    [SerializeField] private PullPropertyType _pullPropertyWeight;

    /// <summary>
    /// 是否使用了无限滑动组件
    /// </summary>
    [SerializeField] private UIWrapContentExtend _uiWrapContentExtend;

    /// <summary>
    /// 到达开始拉动所需偏移量
    /// </summary>
    [SerializeField] private float _fWeightStartPullOffset = 0.0f;

    /// <summary>
    /// 到达拉动中所需偏移量
    /// </summary>
    [SerializeField] private float _fWeightOnPullOffset = 0.0f;

    /// <summary>
    /// 到达拉动成功所需偏移量
    /// </summary>
    [SerializeField] private float _fWeightSucceedPullOffset = 0.0f;

    /// <summary>
    /// 到达开始拉动所需偏移量
    /// </summary>
    [SerializeField] private float _fGentlyStartPullOffset = 0.0f;

    /// <summary>
    /// 到达拉动中所需偏移量
    /// </summary>
    [SerializeField] private float _fGentlyOnPullOffset = 0.0f;

    /// <summary>
    /// 到达拉动成功所需偏移量
    /// </summary>
    [SerializeField] private float _fGentlySucceedPullOffset = 0.0f;

    /// <summary>
    /// 所属UIPanel
    /// </summary>
    private UIPanel _panel;

    /// <summary>
    /// 所属UIScrollView
    /// </summary>
    private UIScrollView _view;

    /// <summary>
    /// 是否正在移动
    /// </summary>
    private bool _bMoving;

    /// <summary>
    /// 是否正在拖动
    /// </summary>
    private bool _bPulling;

    /// <summary>
    /// 当前拖动状态
    /// </summary>
    private PullStateType _pullState;



    public PullPropertyType PullProperty
    {
        get { return _pullProperty; }
    }

    public PullPropertyType PullPropertyWeight
    {
        get { return _pullPropertyWeight; }
    }

    public UIPanel Panel
    {
        get { return _panel; }
    }

    public UIScrollView View
    {
        get { return _view; }
    }

    public bool BPulling
    {
        get { return _bPulling; }
    }

    public bool BMoving
    {
        get { return _bMoving; }
    }

    public float FWeightStartPullOffset
    {
        get { return _fWeightStartPullOffset; }

        set { _fWeightStartPullOffset = value; }
    }

    public float FWeightOnPullOffset
    {
        get { return _fWeightOnPullOffset; }

        set { _fWeightOnPullOffset = value; }
    }

    public float FWeightSucceedPullOffset
    {
        get { return _fWeightSucceedPullOffset; }

        set { _fWeightSucceedPullOffset = value; }
    }

    public float FGentlyStartPullOffset
    {
        get { return _fGentlyStartPullOffset; }

        set { _fGentlyStartPullOffset = value; }
    }

    public float FGentlyOnPullOffset
    {
        get { return _fGentlyOnPullOffset; }

        set { _fGentlyOnPullOffset = value; }
    }

    public float FGentlySucceedPullOffset
    {
        get { return _fGentlySucceedPullOffset; }

        set { _fGentlySucceedPullOffset = value; }
    }

    public PullStateType PullState
    {
        get { return _pullState; }
    }

    #endregion

    private void Awake()
    {
        _panel = GetComponent<UIPanel>();
        _view = GetComponent<UIScrollView>();
    }

    private void Start()
    {
        if (_pullProperty == PullPropertyType.None)
        {
            enabled = false;
            return;
        }
        if (_view != null)
        {
            if (_view.movement == UIScrollView.Movement.Horizontal)
            {
                if (_pullProperty == PullPropertyType.PullDown || _pullProperty == PullPropertyType.PullUp ||
                    _pullProperty == PullPropertyType.PullUpOrDown ||
                    _pullPropertyWeight == PullPropertyType.PullDown ||
                    _pullPropertyWeight == PullPropertyType.PullUp ||
                    _pullPropertyWeight == PullPropertyType.PullUpOrDown ||
                    _pullPropertyWeight == PullPropertyType.PullLeftOrRight)
                {
                    enabled = false;
                    Debug.LogError("UIScrollView组件的拖动为Horizontal，不允许Vertical方向上的拉动类型以及权重类型");
                    return;
                }
            }
            else if (_view.movement == UIScrollView.Movement.Vertical)
            {
                if (_pullProperty == PullPropertyType.PullLeft || _pullProperty == PullPropertyType.PullRight ||
                    _pullProperty == PullPropertyType.PullLeftOrRight ||
                    _pullPropertyWeight == PullPropertyType.PullLeft ||
                    _pullPropertyWeight == PullPropertyType.PullRight ||
                    _pullPropertyWeight == PullPropertyType.PullUpOrDown ||
                    _pullPropertyWeight == PullPropertyType.PullLeftOrRight)
                {
                    enabled = false;
                    Debug.LogError("UIScrollView组件的拖动为Vertical，不允许Horizontal方向上的拉动类型以及权重类型");
                    return;
                }
            }
            else
            {
                enabled = false;
                Debug.LogError("UIScrollView组件的拖动方向错误，只允许Horizontal或者Vertical");
                return;
            }
            //添加回调
            if (_uiWrapContentExtend != null)
                _uiWrapContentExtend.onWrapContentMove = OnViewClipMove;
            else
                _panel.onClipMove = OnViewClipMove;
            _view.onDragStarted = OnViewDragStarted;
            _view.onDragFinished = OnViewDragFinished;
            _view.onMomentumMove = OnViewMomentumMove;
            _view.onStoppedMoving = OnViewStoppedMoving;
        }
    }

    #region 事件回调

    /// <summary>
    /// 当面板的剪辑区域移动时触发的事件回调
    /// </summary>
    private void OnViewClipMove(UIPanel panel)
    {
        OnClippingMoved(panel);
    }

    /// <summary>
    /// UIScrollView 开始拖动触发的事件回调
    /// </summary>
    private void OnViewDragStarted()
    {
        OnDragStarted();
    }

    /// <summary>
    /// UIScrollView 拖动结束触发的事件回调
    /// </summary>
    private void OnViewDragFinished()
    {
        OnDragFinished();
    }

    /// <summary>
    /// UIScrollView OnDragFinished到OnStoppedMoving之间因为动量的移动触发的事件回调
    /// </summary>
    private void OnViewMomentumMove()
    {
        OnMomentumMove();
    }

    /// <summary>
    /// UIScrollView 停止移动触发的事件回调
    /// </summary>
    private void OnViewStoppedMoving()
    {
        OnStoppedMoving();
    }

    #endregion

    #region 事件回调的逻辑

    /// <summary>
    /// 当面板的剪辑区域移动时，计算拉动事件的触发
    /// </summary>
    public virtual void OnClippingMoved(UIPanel panel)
    {
        //监听拉动
        _bMoving = true;
        CalculatePullResultType(_pullPropertyWeight);

        //触发回调
        if (onPullClippingMove != null)
        {
            onPullClippingMove(panel);
        }
    }

    /// <summary>
    /// UIScrollView 开始拖动
    /// </summary>
    public virtual void OnDragStarted()
    {
        _bPulling = true;
        _view.DisableSpring();
        //触发回调
        if (onPullDragStarted != null)
        {
            onPullDragStarted();
        }
    }

    /// <summary>
    /// UIScrollView 拖动结束
    /// </summary>
    public virtual void OnDragFinished()
    {
        _bPulling = false;
        //触发回调
        if (onPullDragStarted != null)
        {
            onPullDragFinished();
        }
    }

    /// <summary>
    /// UIScrollView OnDragFinished到OnStoppedMoving之间因为动量的移动
    /// </summary>
    public virtual void OnMomentumMove()
    {
        //触发回调
        if (onPullDragStarted != null)
        {
            onPullMomentumMove();
        }
    }

    /// <summary>
    /// UIScrollView 停止移动
    /// </summary>
    public virtual void OnStoppedMoving()
    {
        _bMoving = false;
        //触发回调
        if (onPullDragStarted != null)
        {
            onPullStoppedMoving();
        }
    }

    #endregion

    #region 计算逻辑

    /// <summary>
    /// 计算拉动所属类型，并产生回调
    /// </summary>
    private void CalculatePullResultType(PullPropertyType pullPropertyWeight)
    {
        if (pullPropertyWeight == PullPropertyType.PullUp)
        {
            CalculateForUpWeight();
        }
        else if (pullPropertyWeight == PullPropertyType.PullDown)
        {

        }
        else if (pullPropertyWeight == PullPropertyType.PullLeft)
        {

        }
        else if (pullPropertyWeight == PullPropertyType.PullRight)
        {

        }
    }

    /// <summary>
    /// 权重在上方
    /// </summary>
    private void CalculateForUpWeight()
    {
        Vector3 constraint = GetConstrainOffset();
        //权重边界
        Vector3 weightBorderPos = Vector3.zero;
        //权轻边界
        Vector3 gentlyBorderPos = Vector3.zero;
        weightBorderPos =
            new Vector3(0, _panel.height - _panel.clipSoftness.y, 0) / 2 +
            new Vector3(0, _panel.baseClipRegion.y, 0);
        gentlyBorderPos = new Vector3(0, _panel.clipOffset.y + _panel.baseClipRegion.y, 0) -
                          new Vector3(0, _panel.height + _panel.clipSoftness.y, 0) / 2;
        if (constraint.sqrMagnitude > 0.1)
        {
            float offsetY = constraint.y;
            if (offsetY > 0)
            {
                //到顶部然后下拉
                offsetY = Mathf.Abs(offsetY);
                if (offsetY > 0 && offsetY < _fWeightStartPullOffset)
                {
                    //开始下拉
                    if (_bPulling)
                        _pullState = PullStateType.StartPullWight;
                }
                else if (offsetY >= _fWeightStartPullOffset && offsetY < _fWeightOnPullOffset)
                {
                    //下拉中
                    if (_bPulling)
                        _pullState = PullStateType.OnPullWight;
                }
                else if (offsetY >= _fWeightOnPullOffset)
                {
                    //下拉到成功的区域
                    if (_bPulling)
                        _pullState = PullStateType.SucceedPullWight;
                }
            }
            else
            {
                //到底部然后上拉
                offsetY = Mathf.Abs(offsetY);
                if (offsetY > 0 && offsetY < _fGentlyStartPullOffset)
                {
                    //开始上拉
                    if (_bPulling)
                        _pullState = PullStateType.StartPullGently;
                }
                else if (offsetY >= _fGentlyStartPullOffset && offsetY < _fGentlyOnPullOffset)
                {
                    //上拉中
                    if (_bPulling)
                        _pullState = PullStateType.OnPullGently;
                }
                else if (offsetY >= _fGentlyOnPullOffset)
                {
                    //上拉到成功的区域
                    if (_bPulling)
                        _pullState = PullStateType.SucceedPullGently;
                }
                gentlyBorderPos.y = gentlyBorderPos.y + offsetY;
            }
        }
        else
        {
            if (_bPulling)
                _pullState = PullStateType.Normal;
        }
        //string log = "状态: {0}, 约束偏移: {1}, Weight权重边界: {2}, Gently权轻边界: {3}";
        //DllTools.Debuger.Log(string.Format(log, _pullState.ToString(), constraint, weightBorderPos, gentlyBorderPos),
        //    DllTools.Debuger.LevelType.Warning);

        //触发回调
        if (onPullMove != null)
        {
            onPullMove((int) _pullState, constraint, weightBorderPos, gentlyBorderPos);
        }
    }

    /// <summary>
    /// 获取滚动视图约束的距离
    /// </summary>
    private Vector3 GetConstrainOffset()
    {
        //手动取消滚动视图的边界，以便下次更新
        _view.InvalidateBounds();
        //计算小部件使用的边界
        var bounds = _view.bounds;
        //计算需要在面板范围内约束的偏移量
        Vector3 constraint = _panel.CalculateConstrainOffset(bounds.min, bounds.max);
        return constraint;
    }

    #endregion
}
