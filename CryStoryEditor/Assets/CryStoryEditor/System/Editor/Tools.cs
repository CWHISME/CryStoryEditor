/**********************************************************
*Author: wangjiaying
*Date: 2016.6.23
*Func:
**********************************************************/

using UnityEngine;

namespace CryStory.Editor
{
    public class Tools
    {

        public static bool IsValidMouseAABB(Rect area)
        {
            return Event.current.mousePosition.x > area.x && Event.current.mousePosition.y > area.y && Event.current.mousePosition.x < area.xMax && Event.current.mousePosition.y < area.yMax;
        }
    }
}