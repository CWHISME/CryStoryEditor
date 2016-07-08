/**********************************************************
*Author: wangjiaying
*Date: 2016.6.16
*Func:
**********************************************************/
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CryStory.Runtime
{
    public class Story : DragModifier
    {
        public int ID;

        //Runtime===========================

        //public override EnumResult Tick()
        //{
        //    EnumResult result = EnumResult.Failed;
        //    Mission mission;
        //    for (int i = 0; i < _missionList.Count; i++)
        //    {
        //        mission = _missionList[i];
        //        result = mission.Tick();
        //        if (result != EnumResult.Running)
        //        {
        //            for (int j = 0; j < mission._nextNodeList.Count; j++)
        //            {
        //                RunMission(mission._nextNodeList[i] as Mission);
        //            }
        //            _missionList.RemoveAt(i);
        //        }
        //    }
        //    return result;
        //}

        //public void RunMission(Mission mission)
        //{
        //    if (mission == null) return;
        //    //不允许重复添加同一个任务
        //    if (_missionList.Find((m) => m._prototypeId == mission._prototypeId) == null)
        //        _missionList.Add(mission);
        //}

        //public void EndMission(Mission mission)
        //{
        //    if (_missionList.Contains(mission))
        //    {
        //        _missionList.Remove(mission);
        //        mission.OnAbort();
        //    }
        //}
    }
}