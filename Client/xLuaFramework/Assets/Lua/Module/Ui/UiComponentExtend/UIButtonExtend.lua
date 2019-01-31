--[[
    功能：按钮扩展
]]
local UIButtonExtend = {}
local socket = LuaSocket
UIButtonExtend.__index = UIButtonExtend
-- 按钮对象
UIButtonExtend.gameObject = nil
-- 按钮异常发生时，是否抛出返回
UIButtonExtend.isEhrowException = true
-- 点击冷却时间 精确到0.01s
UIButtonExtend.clickCoolingTime = 0
-- 双击冷却时间
UIButtonExtend.doubleClickCoolingTime = 0
-- 在冷却时间内允许的点击频率
UIButtonExtend.clickFrequency = 0
-- 在双击冷却时间内允许的点击频率
UIButtonExtend.doubleClickFrequency = 0
-- 频繁点击的处理方式
UIButtonExtend.frequentlyClickType = 0
-- 声音类型
UIButtonExtend.soundType = 0
-- 上一次点击的时间
UIButtonExtend.lastClickTime = 0
-- 上一次双击的时间
UIButtonExtend.lastDoubleClickTime = 0
-- 当前点击次数
UIButtonExtend.currFrequentlyClickCount = 0
-- 当前双击次数
UIButtonExtend.currFrequentlyDoubleClickCount = 0

-- 传入gameObject、点击的冷却时间、双击冷却时间、声音类型、频繁点击的处理方式
function UIButtonExtend:New(o)
    if (o == nil or o.gameObject == nil) then
        Log("按钮对象不存在", LogType.Error)
        return nil
    end
    local o = {
        gameObject = o.gameObject,
        isEhrowException = o.isEhrowException,
        clickCoolingTime = o.clickCoolingTime,
        doubleClickCoolingTime = o.doubleClickCoolingTime,
        frequentlyClickType = o.frequentlyClickType,
        clickFrequency = o.clickFrequency,
        doubleClickFrequency = o.doubleClickFrequency,
        soundType = o.soundType,
        lastClickTime = 0,
        lastDoubleClickTime = 0
    }
    setmetatable(o, self)
    return o
end

-- 点击事件
function UIButtonExtend:OnClick(callback)
    if (callback == nil or type(callback) ~= "function") then
        return
    end
    CS.UIEventListener.Get(self.gameObject).onClick =
        function(go)
        if (self.clickCoolingTime ~= 0) then
            if (self.lastClickTime ~= 0) then
                local nowTime = socket.gettime()
                local interval = nowTime - self.lastClickTime
                if (interval < self.clickCoolingTime) then
                    self.currFrequentlyClickCount = self.currFrequentlyClickCount + 1
                else
                    self.currFrequentlyClickCount = 1
                end
                --[[ Log(
                    "点击按钮" ..
                        self.gameObject.name ..
                            ", 时间间隔: " ..
                                interval ..
                                    ", 当前点击时间: " ..
                                        nowTime ..
                                            "(s), 上次点击时间: " ..
                                                self.lastClickTime .. ", 当前点击次数: " .. self.currFrequentlyClickCount,
                    LogType.Warning
                ) ]]
                -- 是否触发异常
                local isTriggerException = false
                if (interval < self.clickCoolingTime and self.currFrequentlyClickCount >= self.clickFrequency) then
                    self.currFrequentlyClickCount = 0
                    if (self.frequentlyClickType == UIButtonExceptionHandleType.None) then
                        isTriggerException = false
                    elseif (self.frequentlyClickType == UIButtonExceptionHandleType.Return) then
                        isTriggerException = true
                    elseif (self.frequentlyClickType == UIButtonExceptionHandleType.BubblingTips) then
                        ShowDialog("点击太频繁啦，请稍后再试")
                        isTriggerException = true
                    elseif (self.frequentlyClickType == UIButtonExceptionHandleType.PopUpTips) then
                        TwoDialogData = {
                            Type = TwoDialogType.OnClickFrequently,
                            Desc = "点击太频繁啦，请稍后再试",
                            Confirm = "确 认"
                        }
                        ShowUIPanelMinToMax(PanelType.TwoDialog)
                        isTriggerException = true
                    end
                end
                if (self.currFrequentlyClickCount >= self.clickFrequency) then
                    self.currFrequentlyClickCount = 0
                end
                if (interval >= self.clickCoolingTime) then
                    self.lastClickTime = nowTime
                -- Log("更新上次点击时间: " .. self.lastClickTime, LogType.Critical)
                end
                if self.isEhrowException and isTriggerException then
                    return
                end
            else
                self.lastClickTime = socket.gettime()
                self.currFrequentlyClickCount = 1
            end
        end
        callback(go)
    end
end

-- 双击事件
function UIButtonExtend:OnDoubleClick(callback)
    -- body
end

return UIButtonExtend
