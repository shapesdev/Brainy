using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BadgeController : MonoBehaviour {

    public VerticalLayoutGroup verticalLayout;
    public GameObject prefab;

	// Use this for initialization
	void Start () {
        CheckHeight();
        ShowBadges();
	}

    private void CheckHeight()
    {
        if(Screen.height < 2240)
        {
            RectOffset tempRes = new RectOffset(0, 0, 140, 530);
            verticalLayout.padding = tempRes;
            verticalLayout.spacing = 130;
        }
    }

    private void ShowBadges()
    {
        int top = 0;

        if(PlayerConfig.instance.achievements.Count != 0)
        {
            foreach(var item in PlayerConfig.instance.achievements)
            {
                GameObject instance = Instantiate(prefab, transform);
                instance.transform.position = new Vector3(instance.transform.position.x, instance.transform.position.y - top, instance.transform.position.z);
                instance.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = item.Key.achievementImage;
                instance.transform.GetChild(1).gameObject.GetComponent<Text>().text = item.Key.achievementName;
                instance.transform.GetChild(2).gameObject.GetComponent<Text>().text = item.Key.description;
                instance.transform.GetChild(3).gameObject.GetComponent<Text>().text = "Completed: " + item.Value + " of " + item.Key.goal;
                top += 370;

                if (item.Value != item.Key.goal)
                {
                    instance.transform.GetChild(0).gameObject.GetComponent<Image>().color = new Color32(255, 255, 255, 150);
                    instance.transform.GetChild(1).gameObject.GetComponent<Text>().color = new Color32(255, 255, 255, 150);
                    instance.transform.GetChild(2).gameObject.GetComponent<Text>().color = new Color32(255, 255, 255, 150);
                    instance.transform.GetChild(3).gameObject.GetComponent<Text>().color = new Color32(255, 255, 255, 150);
                }
            }
        }
    }
}
