/**********************************************************
*Author: wangjiaying
*Date: 
*Func:
**********************************************************/
using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour
{

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            CryStory.Runtime.Story story = new CryStory.Runtime.Story();
            story._missionList.Add(new CryStory.Runtime.Mission());
            story.graphCenter = new Vector2(55555, 2255);
            string s = JsonUtility.ToJson(story);
            Debug.Log(s);
            CryStory.Runtime.Story ss = JsonUtility.FromJson<CryStory.Runtime.Story>(s);
            Debug.Log(ss.graphCenter);
        }
    }
}
