using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementCloser : MonoBehaviour {

    public void CloseAchievement()
    {
        this.gameObject.SetActive(false);
    }

}
