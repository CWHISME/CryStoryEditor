/**********************************************************
*Author: wangjiaying
*Date: 2016.6.16
*Func:
**********************************************************/

namespace CryStory.Runtime
{

    abstract public class Event : StoryNode
    {

        private bool _trigger = false;

        protected sealed override EnumResult OnStart()
        {
            RegistEvent();
            return EnumResult.Success;
        }

        protected sealed override EnumResult OnUpdate()
        {
            if (_trigger) return EnumResult.Success;
            return EnumResult.Running;
        }

        protected sealed override void OnEnd()
        {
            _trigger = false;
            UnRegistEvent();
        }

        protected abstract void RegistEvent();

        protected abstract void UnRegistEvent();

        protected void TriggerEvent()
        {
            _trigger = true;
        }
    }
}