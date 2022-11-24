using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

[System.Serializable]
public class PlayerData
{
    [SerializeField]
    public GameObject player;
    public string name;

    public Vector3 clinic_table_1;
    public Vector3 X_ray_monitor;
    public Vector3 X_ray_scan;
    public Vector3 Locker;
    public Vector3 operating_lamp;
    public Vector3 Surgical_table;
    public Vector3 clinic_table_2;
    public Vector3 Curtain;
    public Vector3 Manbody;
    public Vector3 lung;
    public Vector3 Heart;
    public Vector3 Digestive;

    public Vector3 rclinic_table_1;
    public Vector3 rX_ray_monitor;
    public Vector3 rX_ray_scan;
    public Vector3 rLocker;
    public Vector3 roperating_lamp;
    public Vector3 rSurgical_table;
    public Vector3 rclinic_table_2;
    public Vector3 rCurtain;
    public Vector3 rManbody;
    public Vector3 rlung;
    public Vector3 rHeart;
    public Vector3 rDigestive;


    public int age = 0;
    public bool b;
    public string[] items;


    //생성자
    //이 아래 내용을 모르겠고, null reference error계쏙 뜬다

    //public PlayerData(Text t_agebox, GameObject t_player, string t_name, Vector3 t_abc, int t_age, bool t_b, string[] t_items)
    //{
    //    agebox = t_agebox;
    //    player = t_player;
    //    name = t_name;
    //    abc = t_abc;
    //    age = t_age;
    //    b = t_b;
    //    items = t_items;

    //    abc = new Vector3 (t_abc.x, t_abc.y, t_abc.z);
    //}
}


public class PlayerController : MonoBehaviour
{

    public PlayerData playerData;

    [ContextMenu("To Json Data")]
    //뒤따라오는 메뉴를 추가해줌

    public void SavePlayerDataToJason()
    {
        string jsonData = JsonUtility.ToJson(playerData, true);
        string path = Application.dataPath + "/playerData.json";
        File.WriteAllText(path, jsonData);

    }

    [ContextMenu("Load Json Data")]

    public void LoadPlayerDataFromJason()
    {
        string path = Application.dataPath + "/playerData.json";
        string jsonData = File.ReadAllText(path);
        playerData = JsonUtility.FromJson<PlayerData>(jsonData);

    }
    //save, load는 버튼 구현됨. vector 좌표를 어떻게 가져와야하나

}

//player인 cube가 vector3 의 abc와 연동하고 싶음
