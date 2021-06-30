using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBoardManager : MonoBehaviour
{
    public Text text = null;

    private static ScoreBoardManager instance = null;

    private 

    object crit;

    private void Awake()
    {
        instance = this;
        text.text = "";
        crit = new object();
    }

    private void Update()
    {
        lock (crit)
        {
            //string str = "";
            //for (int j = 0; j < vo.Count; ++j)
            //{
            //    str += $"{j + 1}µî: {vo[j].name}, Kill: {vo[j].kill}, Death{vo[j].death}\r\n";
            //}

            //int i = 1;
            //foreach (var item in vo)
            //{
            //    Debug.Log(item.socketId + " , " + item.kill);
            //    if (item.kill == 0 && item.death == 0) continue;
            //    instance.text.text = str;
            //}
        }
    }


    public static void ResetBoard(List<TransformVO> vo)
    {
        //vo.Sort((x, y) => { return y.kill.CompareTo(x.kill); });

        //lock(instance.crit)
        //{

        //}


    }
}
