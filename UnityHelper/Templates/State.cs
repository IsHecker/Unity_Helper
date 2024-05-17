using UnityEngine;

namespace UnityHelper.Templates
{
    public abstract class State
    {
        protected StateMachine stateMachine;
        public float StartTime { get; protected set; }

        protected string animBoolName;
        public State( StateMachine stateMachine, string animBoolName)
        {
            this.stateMachine = stateMachine;
            this.animBoolName = animBoolName;
        }

        public virtual void Enter()
        {
            StartTime = Time.time;
            DoChecks();
        }
        public virtual void Exit() { }
        public virtual void LogicUpdate() { }
        public virtual void PhysicsUpdate() { DoChecks(); }
        public virtual void DoChecks() { }
    }
}
