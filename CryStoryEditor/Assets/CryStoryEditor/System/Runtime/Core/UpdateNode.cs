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
                else if (result == EnumResult.Failed) return EnumResult.Failed;
                return EnumResult.Running;
            }

            //UnityEngine.Debug.Log("Tick Node :" + _name + "    " + result);

            if (result != EnumResult.Running)
            {
                OnEnd();
                _isInit = false;
                return result;
            }

            return EnumResult.Running;
        }

        protected virtual EnumResult OnStart() { return EnumResult.Success; }
        protected virtual EnumResult OnUpdate() { return EnumResult.Success; }
        protected virtual void OnEnd() { }
    }
}