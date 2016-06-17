/**********************************************************
*Author: wangjiaying
*Date: 2016.6.16
*Func:
**********************************************************/

namespace CryStory.Runtime
{
    abstract public class UpdateNode : StoryNode
    {

        private bool _isInit = false;

        public override EnumResult Tick()
        {
            EnumResult result;
            if (_isInit)
            {
                result = OnUpdate();
            }
            else {
                result = OnStart();
                if (result == EnumResult.Success)
                    _isInit = true;
            }

            if (result != EnumResult.Running)
            {
                OnEnd();
                return result;
            }

            return EnumResult.Running;
        }

        protected abstract EnumResult OnStart();
        protected abstract EnumResult OnUpdate();
        protected abstract void OnEnd();
    }
}