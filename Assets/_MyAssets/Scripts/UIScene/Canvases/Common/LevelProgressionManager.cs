using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelProgressionManager : MonoBehaviour
{
    [SerializeField] LevelProgressIcon[] levelProgressIcons;


    public void OnOpen()
    {
        int currentDisplayStageNum = StageTransManager.i.CurrentDisplayStageNum;
        int currentIndex = (currentDisplayStageNum - 1) % 5;
        int startStageNum = currentDisplayStageNum - currentIndex;
        for (int i = 0; i < levelProgressIcons.Length; i++)
        {
            levelProgressIcons[i].Show(
                level: startStageNum + i,
                isSelected: i == currentIndex,
                isCleared: i < currentIndex
                );
        }
    }

    public void Anim()
    {
        int currentDisplayStageNum = StageTransManager.i.CurrentDisplayStageNum;
        int currentIndex = (currentDisplayStageNum - 1) % 5;
        levelProgressIcons[currentIndex].Anim();
    }
}
