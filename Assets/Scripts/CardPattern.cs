using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPattern : MonoBehaviour
{
    public static List<int> FieldCardPattern = new List<int>()
    {
        9,
        4,
        8,
        5,
        5
    };
    public static List<List<int>> MyCardPattern = new List<List<int>>()
    {
        new List<int>(){8,5,9,4,2},
        new List<int>(){11,5,13,8,7},
        new List<int>(){3,8,5,9,10},
        new List<int>(){5,4,12,7,9},
        new List<int>(){9,8,12,7,10},
    };
    public static List<List<int>> YourCardPattern = new List<List<int>>()
    {
        new List<int>(){8, 5,9,4,2},
        new List<int>(){11,5,13,8,7},
        new List<int>(){3,8,5,9,10},
        new List<int>(){5,4,12,7,9},
        new List<int>(){9,8,12,7,10},
    };
    public static List<int> FieldCardPatternSuit = new List<int>()
    {
        2,
        1,
        1,
        0,
        2
    };
    public static List<List<int>> MyCardPatternSuit = new List<List<int>>()
    {
        new List<int>(){2,1,3,1,1},
        new List<int>(){1,0,3,0,1},
        new List<int>(){2,3,1,3,2},
        new List<int>(){1,3,0,1,0},
        new List<int>(){1,2,3,1,0},
    };
    public static List<List<int>> YourCardPatternSuit = new List<List<int>>()
    {
        new List<int>(){2,1,3,1,1},
        new List<int>(){1,0,3,0,1},
        new List<int>(){2,3,1,3,2},
        new List<int>(){1,3,0,1,0},
        new List<int>(){1,2,3,1,0},
    };
}
