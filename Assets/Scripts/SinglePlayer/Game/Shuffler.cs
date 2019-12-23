using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shuffler {  

    public List<string> Shuffle(List<string> listas)
    {
        for(int i = 0; i < listas.Count; i++)
        {
            string temp = listas[i];
            int randomIndex = Random.Range(i, listas.Count);
            listas[i] = listas[randomIndex];
            listas[randomIndex] = temp;
        }
        return listas;
    }
}
