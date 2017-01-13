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

        protected bool _running = false;
        private bool _isInit = false;

        public override EnumResult Tick(NodeContent content)
        {
            return Tick();
        }

        public virtual EnumResult Tick()
        {
            EnumResult result;
            if (_isInit)
            {
                result = OnUpdate();
            }
            else
            {
                _running = true;
                result = OnStart();
                if (result == EnumResult.Success)
                    _isInit = true;
                else if (result == EnumResult.Failed) return EnumResult.Failed;
                return EnumResult.Running;
            }

            if (result != EnumResult.Running)
            {
                OnEnd();
                _isInit = false;
                _running = false;
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
            w.Write(_running);
            if (_running) OnGameSave(w);
        }

        protected override void OnLoaded(BinaryReader r)
        {
            base.OnLoaded(r);
            _isInit = r.ReadBoolean();
            _running = r.ReadBoolean();
            if (_running)
                OnGameLoad(r);
        }

        /// <summary>
        /// 当游戏运行中被保存
        /// </summary>
        protected virtual void OnGameSave(BinaryWriter w) { }
        /// <summary>
        /// 当游戏中被加载
        /// </summary>
        protected virtual void OnGameLoad(BinaryReader r) { }
    }
}