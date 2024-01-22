using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public int generatePseudoRandom()
    {
        return Random.Range(0, 99999);
    }
    public string stringFilter(string str, List<char> charsToRemove)
    {
        foreach (char c in charsToRemove)
        {
            str = str.Replace(c.ToString(), " ");
        }
        return str;
    }

}

