using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 오브젝트의 위치 저장 및 로드 
// player preference로 간단히 구현
// 여러 object 구현이 어려움


public class playprefs : MonoBehaviour
{
    public GameObject cubeT;
    float cubex;
    float cubey;
    float cubez;

    public void saveP()
    {
        cubex = cubeT.transform.position.x;
        cubey = cubeT.transform.position.y;
        cubez = cubeT.transform.position.z;

        PlayerPrefs.SetFloat("xPos", cubex);
        PlayerPrefs.SetFloat("yPos", cubey);
        PlayerPrefs.SetFloat("zPos", cubez);

        Debug.Log(cubex);
        Debug.Log(cubey);
        Debug.Log(cubez);


    }

    public void loadP()
    {
        float a = PlayerPrefs.GetFloat("xPos");
        float b = PlayerPrefs.GetFloat("yPos");
        float c = PlayerPrefs.GetFloat("zPos");

        cubeT.transform.position = new Vector3(a, b, c);
        Debug.Log(a);
    }
}
