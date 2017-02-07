/**********************************************************
*Author: wangjiaying
*Date: 2017.1.14
*Func:
**********************************************************/

namespace CryStory.Runtime
{
    public class Vector3Helper : System.Attribute
    {

        private VectorSyceMode _mode;
        public VectorSyceMode Mode { get { return _mode; } }

        public Vector3Helper(VectorSyceMode mode)
        {
            _mode = mode;
        }
    }

    public enum VectorSyceMode
    {
        Position,
        EulerAngle,
    }
}