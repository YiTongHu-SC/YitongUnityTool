using System;
using System.Collections.Generic;
using UnityEngine;

namespace HuYitong.GameBaseTool.FSM
{
    /// <summary>
    ///     状态机实现
    /// </summary>
    /// ///
    /// <author> HuYiTong</author>
    /// <team>MatrixPlay</team>
    public class StateMachine<T, TE, TT>
        where T : MonoBehaviour
        where TE : Enum
        where TT : Enum
    {
        private readonly Dictionary<TE, FsmState<T, TE, TT>> _statesTable = new();

        private readonly T _context;

        public StateMachine(T context)
        {
            _context = context;
        }

        public TE CurrentStateID { get; private set; }

        public FsmState<T, TE, TT> CurrentState { get; private set; }

        public void AddState(FsmState<T, TE, TT> state)
        {
            if (state == null) Debug.LogError("FSM ERROR: Null reference is not allowed");

            if (_statesTable.ContainsKey(state.Id))
                Debug.LogError("FSM ERROR: Impossible to add state " + state.Id +
                               " because state has already been added");

            state.Context = _context;
            _statesTable.Add(state.Id, state);
        }

        public void DeleteState(TE id)
        {
            if (_statesTable.ContainsKey(id))
                _statesTable.Remove(id);
            else
                Debug.LogError("FSM ERROR: Impossible to delete state " + id +
                               ". It was not on the dict of states");
        }

        public void PerformTransition(TT trans)
        {
            if (CurrentState == null)
            {
                Debug.LogError("FSM ERROR: No current state set");
                return;
            }

            if (!CurrentState.Contains(trans)) return;

            CurrentState.Exit();
            var nextStateId = CurrentState.GetTargetState(trans);

            if (!_statesTable.ContainsKey(nextStateId))
            {
                Debug.LogError($"FSM ERROR: Target state {nextStateId} does not exist");
                return;
            }

            var nextState = _statesTable[nextStateId];
            CurrentStateID = nextStateId;
            CurrentState = nextState;
            nextState.Enter();
        }

        public void SetCurrent(TE id)
        {
            if (!_statesTable.ContainsKey(id))
            {
                Debug.LogError($"FSM ERROR: State {id} does not exist in the state machine");
                return;
            }

            CurrentState?.Exit(); // 退出当前状态
            CurrentStateID = id;
            CurrentState = _statesTable[id];
            CurrentState.Enter();
        }

        public void Tick(float deltaTime = 0)
        {
            if (CurrentState == null)
            {
                Debug.LogWarning("FSM WARNING: No current state to tick");
                return;
            }

            CurrentState.Reason(deltaTime);
            CurrentState.Act(deltaTime);
        }

        public bool IsValidState(TE id)
        {
            return _statesTable.ContainsKey(id);
        }

        public bool HasCurrentState()
        {
            return CurrentState != null;
        }
    }

    public abstract class FsmState<T, TE, TT>
        where T : MonoBehaviour
        where TE : Enum
        where TT : Enum
    {
        protected Dictionary<TT, TE> Map = new();

        protected FsmState(TE stateId)
        {
            Id = stateId;
        }

        public T Context { get; set; }

        public TE Id { get; }

        public void AddTransition(TT trans, TE id)
        {
            // Since this is a Deterministic FSM,
            //   check if the current transition was already inside the map
            if (Map.ContainsKey(trans))
            {
                Debug.LogError("FSMState ERROR: State " + Id + " already has transition " +
                               trans +
                               "Impossible to assign to another state");
                return;
            }

            Map.Add(trans, id);
        }

        public void DeleteTransition(TT trans)
        {
            // Check if the pair is inside the map before deleting
            if (Map.ContainsKey(trans))
            {
                Map.Remove(trans);
                return;
            }

            Debug.LogError("FSMState ERROR: Transition " + trans + " passed to " + Id +
                           " was not on the state's transition list");
        }

        public TE GetTargetState(TT trans)
        {
            if (!Map.ContainsKey(trans))
                Debug.LogError("FSMState ERROR : Transition DO NOT exist : Transition " + trans);

            return Map[trans];
        }

        public abstract void Enter();
        public abstract void Exit();
        public abstract void Reason(float deltaTime = 0);
        public abstract void Act(float deltaTime = 0);

        public bool Contains(TT trans)
        {
            return Map.ContainsKey(trans);
        }
    }
}