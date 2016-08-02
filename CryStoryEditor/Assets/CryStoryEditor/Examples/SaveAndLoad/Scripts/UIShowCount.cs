/**********************************************************
*Author: wangjiaying
*Date: 
*Func:
**********************************************************/
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using CryStory.Runtime;

namespace CryStory.Examples
{
    public class UIShowCount : MonoBehaviour
    {
        public Text _text;
        public StoryRunner _story;

        public void Update()
        {
            _text.text = "Space Button Down:  " + _story.StoryManager.Story._valueContainer["ButtonDownCount"].StringValue;
        }
    }
}