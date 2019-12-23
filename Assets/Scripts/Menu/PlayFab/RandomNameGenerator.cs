using System.Collections.Generic;
using UnityEngine;

public class RandomNameGenerator {

    List<string> randomNames = new List<string>();

    string name = "Genny Aegis Brown Globe Clark Pumpk Pisce Arche Tunic Shri Bauma Dummy Paint Tool Falch " +
        "Ressi Bucky Kelps Banne Grows Marin Bows Sand Comp Dio Simpl Sleep Ingot Brand " +
        "Samir Patie Scarl Cindy Esqui Image Kylie Light Storm Baron Josep Gulli Galin Cody Vally Spark " +
        "Twink Rain Spark WildJ Cool CaptJ Marro Bonet Chad Bone DrRob ilda Swin" +
        "Stark Ezra Bug Boney Beego";

    public RandomNameGenerator()
    {
        string[] names = name.Split(' ');

        foreach (string a in names)
        {
            randomNames.Add(a);
        }
    }

    public string GetRandomName()
    {
        int randomNumber = Random.Range(0, randomNames.Count - 1);

        int randomLastNumbers = Random.Range(100, 999);

        return randomNames[randomNumber] + randomLastNumbers;
    }
}
