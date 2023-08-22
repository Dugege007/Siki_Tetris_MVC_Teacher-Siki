using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public enum Transition
{
    NullTransition = 0,
    StartBtnClick,
    PauseBtnClick,
    RestartBtn,

}

public enum StateID
{
    NullState = 0,
    Menu,
    Play,
    Pause,
    GameOver
}

// 有限状态机
public class FSMSystem : MonoBehaviour
{
    private List<FSMState> states;

    private StateID currentStateID;
    private FSMState currentState;
    private Dictionary<StateID, FSMState> stateDic = new Dictionary<StateID, FSMState>();

    public StateID CurrentStateID { get { return currentStateID; } }
    public FSMState CurrentState { get { return currentState; } }

    public FSMSystem()
    {
        states = new List<FSMState>();
    }

    public void Update()
    {
        currentState.Act();
        currentState.Reason();
    }

    public void SetCurrentState(FSMState state)
    {
        if (currentState == null)
        {
            currentState = state;
            currentStateID = state.ID;
            state.DoBeforeEntering();
        }
    }

    /// <summary>
    /// 添加新状态
    /// </summary>
    public void AddState(FSMState state, Control ctrl)
    {
        if (state == null)
        {
            Debug.LogError("FSMState不能为空");
            return;
        }

        state.FSM = this;
        state.Ctrl = ctrl;

        //if (stateDic.ContainsKey(state.ID))
        //{
        //    Debug.LogError("状态" + state.ID + "已经存在，无法重复添加");
        //    return;
        //}
        //stateDic.Add(state.ID, state);

        foreach (FSMState s in states)
        {
            if (state.ID == s.ID)
            {
                Debug.LogError("状态" + state.ID + "已经存在，无法重复添加");
                return;
            }
        }
        states.Add(state);
    }

    /// <summary>
    /// 移除状态
    /// </summary>
    public void DeleteState(FSMState state)
    {
        if (state.ID == StateID.NullState)
        {
            Debug.LogError("无法删除空状态");
            return;
        }

        //if (!stateDic.ContainsKey(state.ID))
        //{
        //    Debug.LogError("无法删除不存在的状态");
        //    return;
        //}
        //stateDic.Remove(state.ID);

        foreach (FSMState s in states)
        {
            if (state.ID == s.ID)
            {
                Debug.LogError("状态" + state.ID + "已经存在，无法重复添加");
                return;
            }
        }
    }

    /// <summary>
    /// 执行过渡条件满足时对应状态该做的事
    /// </summary>
    public void PerformTransition(Transition transition)
    {
        if (transition == Transition.NullTransition)
        {
            Debug.LogError("无法执行空的转换条件");
            return;
        }

        StateID id = currentState.GetOutputState(transition);
        if (id == StateID.NullState)
        {
            Debug.LogWarning("当前状态" + currentStateID + "无法根据转换条件" + transition + "发生转换");
            return;
        }

        //if (!stateDic.ContainsKey(id))
        //{
        //    Debug.LogError("在状态机里面不存在状态" + id + ",无法进行状态转换");
        //    return;
        //}
        //FSMState state = stateDic[id];
        //currentState.DoBeforeLeaving();
        //currentState = state;
        //currentStateID = state.ID;
        //currentState.DoBeforeEntering();

        currentStateID = id;
        foreach (FSMState s in states)
        {
            if (s.ID == currentStateID)
            {
                currentState.DoBeforeLeaving();
                currentState = s;
                currentState.DoBeforeEntering();
                return;
            }
        }

        Debug.LogError("在状态机里面不存在状态" + id + ",无法进行状态转换");
        return;
    }
}

public abstract class FSMState : MonoBehaviour
{
    protected Control ctrl;
    protected FSMSystem fsm;
    protected StateID stateID = StateID.NullState;
    protected Dictionary<Transition, StateID> map = new Dictionary<Transition, StateID>();

    public Control Ctrl { get { return ctrl; } set { ctrl = value; } }
    public FSMSystem FSM { set { fsm = value; } }
    public StateID ID { get { return stateID; } }

    //public FSMState(FSMSystem fSMSystem)
    //{
    //    this.fSMSystem = fSMSystem;
    //}

    /// <summary>
    /// 添加转换条件
    /// </summary>
    /// <param name="trans">转换条件</param>
    /// <param name="id">转换条件满足时执行的状态</param>
    public void AddTransition(Transition trans, StateID id)
    {
        if (trans == Transition.NullTransition)
        {
            Debug.LogError("不允许NullTransition");
            return;
        }

        if (id == StateID.NullState)
        {
            Debug.LogError("不允许NullStateID");
            return;
        }

        if (map.ContainsKey(trans))
        {
            Debug.LogError("添加转换条件的时候" + trans + "已经存在于transitionStateDic中");
            return;
        }
        map.Add(trans, id);
    }

    /// <summary>
    /// 移除转换条件
    /// </summary>
    public void RemoveTransition(Transition trans)
    {
        if (trans == Transition.NullTransition)
        {
            Debug.LogError("不允许NullTransition");
            return;
        }

        if (!map.ContainsKey(trans))
        {
            Debug.LogError("删除转换条件的时候" + trans + "不存在于transitionStateDic中");
            return;
        }

        map.Remove(trans);
    }

    /// <summary>
    /// 获取当前转换条件下的状态
    /// </summary>
    public StateID GetOutputState(Transition trans)
    {
        if (map.ContainsKey(trans))
        {
            return map[trans];
        }

        return StateID.NullState;
    }

    /// <summary>
    /// 进入新状态之前做的事
    /// </summary>
    public virtual void DoBeforeEntering() { }

    /// <summary>
    /// 离开当前状态时做的事
    /// </summary>
    public virtual void DoBeforeLeaving() { }

    //public virtual void Reason(GameObject player, GameObject npc) { }
    //public virtual void Act(GameObject player, GameObject npc) { }
    // 控制游戏状态不需要参数

    /// <summary>
    /// 当前状态所做的事
    /// </summary>
    public virtual void Act() { }

    /// <summary>
    /// 在某一状态执行过程中，新的转换条件满足时做的事
    /// </summary>
    public virtual void Reason() { }//判断转换条件
}