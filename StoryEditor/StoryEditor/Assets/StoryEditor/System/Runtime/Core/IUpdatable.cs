/**********************************************************
*Author: wangjiaying
*Date: 2016.6.16
*Func:
**********************************************************/

namespace CryStory.Runtime
{
    public interface IUpdatable
    {
        EnumResult OnStart();
        EnumResult OnUpdate();
        void OnEnd();
    }
}