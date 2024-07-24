using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayBall : MonoBehaviour
{
    [SerializeField] BlackJackManager _BlackJackManager;

    // Update is called once per frame
    void Update()
    {
        if (_BlackJackManager.hasPracticeSet)
        {
            if (_BlackJackManager._PracticeSet.BlackJackState == PracticeSet.BlackJackStateList.ShowResult)
            {
                this.transform.position = new Vector3(0, 0, -10f);
                Console.WriteLine(this.transform.position.x.ToString());
            }

        }
    }
}
