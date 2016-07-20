/**********************************************************
*Author: wangjiaying
*Date: 2016.7.20
*Func:
**********************************************************/

namespace CryStory.Runtime
{
    public enum ValueFunctor
    {
        [ValueFunc(VarType.INT | VarType.FLOAT | VarType.BOOL | VarType.STRING)]
        Set,
        [ValueFunc(VarType.INT | VarType.FLOAT)]
        Add,
        [ValueFunc(VarType.INT | VarType.FLOAT)]
        Reduce,
        [ValueFunc(VarType.INT | VarType.FLOAT)]
        Multiply,
        [ValueFunc(VarType.INT | VarType.FLOAT)]
        Divide
    }
}
