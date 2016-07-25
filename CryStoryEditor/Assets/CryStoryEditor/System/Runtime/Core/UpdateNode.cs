/**********************************************************
*Author: wangjiaying
*Date: 2016.6.16
*Func:
**********************************************************/

using System.IO;

namespace CryStory.Runtime
{

    abstract public class UpdateNode : NodeModifier
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

        /// <summary>
        /// 处于Runnig中，将会一直执行，当成功后进入Update，失败则直接退出
        /// </summary>
        protected virtual EnumResult OnStart() { return EnumResult.Success; }
        protected virtual EnumResult OnUpdate() { return EnumResult.Success; }
        protected virtual void OnEnd() { }

        protected override void OnSaved(BinaryWriter w)
        {
            base.OnSaved(w);
            w.Write(_isInit);
        }

        protected override void OnLoaded(BinaryReader r)
        {
            base.OnLoaded(r);
            _isInit = r.ReadBoolean();
        }
    }
}