using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnGui : MonoBehaviour
{
    GUIStyle style = new GUIStyle();

    float rect_pos_x = 5f;
    float rect_pos_y = 5f;
    float w = Screen.width;
    float h = 20f;
    float hp;

    private void Update()
    {
        hp = GameObject.Find("Boss").GetComponent<Enemy>().hp;
    }

    private void Awake()
    {
        style.normal.textColor = Color.black;
        style.fontSize = 60;
    }

    private void OnGUI()
    {
        if(hp > 0)
        {
            GUI.Label(new Rect(rect_pos_x, rect_pos_y, w, h), "체력 : " + hp, style);
        }
    }
}
