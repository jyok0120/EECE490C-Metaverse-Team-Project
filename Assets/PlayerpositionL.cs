using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerpositionL : MonoBehaviour
{
    public GameObject clinic_table_1;
    public GameObject X_ray_monitor;
    public GameObject X_ray_scan;
    public GameObject Locker;
    public GameObject operating_lamp;
    public GameObject Surgical_table;
    public GameObject clinic_table_2;
    public GameObject Curtain;
    public GameObject Manbody;
    public GameObject lung;
    public GameObject Heart;
    public GameObject Digestive;

    public GameObject rclinic_table_1;
    public GameObject rX_ray_monitor;
    public GameObject rX_ray_scan;
    public GameObject rLocker;
    public GameObject roperating_lamp;
    public GameObject rSurgical_table;
    public GameObject rclinic_table_2;
    public GameObject rCurtain;
    public GameObject rManbody;
    public GameObject rlung;
    public GameObject rHeart;
    public GameObject rDigestive;



    // cube의 위치값을, json 데이터와 연결함.
    //save처음 누르면, cube의 위치vector 값을 가져옴. 다시 save누르면 이 값 저장
    //load 누를경우, 저장했던 위치로 이동
    public void LoadC_P()
    {
        PlayerController posi = Manbody.GetComponent<PlayerController>();
        //cube위치 가져옴
        clinic_table_1.transform.position = new Vector3(posi.playerData.clinic_table_1.x, posi.playerData.clinic_table_1.y, posi.playerData.clinic_table_1.z);
        X_ray_monitor.transform.position = new Vector3(posi.playerData.X_ray_monitor.x, posi.playerData.X_ray_monitor.y, posi.playerData.X_ray_monitor.z);
        X_ray_scan.transform.position = new Vector3(posi.playerData.X_ray_scan.x, posi.playerData.X_ray_scan.y, posi.playerData.X_ray_scan.z);
        Locker.transform.position = new Vector3(posi.playerData.Locker.x, posi.playerData.Locker.y, posi.playerData.Locker.z);
        operating_lamp.transform.position = new Vector3(posi.playerData.operating_lamp.x, posi.playerData.operating_lamp.y, posi.playerData.operating_lamp.z);
        Surgical_table.transform.position = new Vector3(posi.playerData.Surgical_table.x, posi.playerData.Surgical_table.y, posi.playerData.Surgical_table.z);
        clinic_table_2.transform.position = new Vector3(posi.playerData.clinic_table_2.x, posi.playerData.clinic_table_2.y, posi.playerData.clinic_table_2.z);
        Curtain.transform.position = new Vector3(posi.playerData.Curtain.x, posi.playerData.Curtain.y, posi.playerData.Curtain.z);
        Manbody.transform.position = new Vector3(posi.playerData.Manbody.x, posi.playerData.Manbody.y, posi.playerData.Manbody.z);
        lung.transform.position = new Vector3(posi.playerData.lung.x, posi.playerData.lung.y, posi.playerData.lung.z);
        Heart.transform.position = new Vector3(posi.playerData.Heart.x, posi.playerData.Heart.y, posi.playerData.Heart.z);
        Digestive.transform.position = new Vector3(posi.playerData.Digestive.x, posi.playerData.Digestive.y, posi.playerData.Digestive.z);



        rclinic_table_1.transform.rotation = Quaternion.Euler(new Vector3(posi.playerData.rclinic_table_1.x, posi.playerData.rclinic_table_1.y, posi.playerData.rclinic_table_1.z));
        rX_ray_monitor.transform.rotation = Quaternion.Euler(new Vector3(posi.playerData.rX_ray_monitor.x, posi.playerData.rX_ray_monitor.y, posi.playerData.rX_ray_monitor.z));
        rX_ray_scan.transform.rotation = Quaternion.Euler(new Vector3(posi.playerData.rX_ray_scan.x, posi.playerData.rX_ray_scan.y, posi.playerData.rX_ray_scan.z));
        rLocker.transform.rotation = Quaternion.Euler(new Vector3(posi.playerData.rLocker.x, posi.playerData.rLocker.y, posi.playerData.rLocker.z));
        roperating_lamp.transform.rotation = Quaternion.Euler(new Vector3(posi.playerData.roperating_lamp.x, posi.playerData.roperating_lamp.y, posi.playerData.roperating_lamp.z));
        rSurgical_table.transform.rotation = Quaternion.Euler(new Vector3(posi.playerData.rSurgical_table.x, posi.playerData.rSurgical_table.y, posi.playerData.rSurgical_table.z));
        rclinic_table_2.transform.rotation = Quaternion.Euler(new Vector3(posi.playerData.rclinic_table_2.x, posi.playerData.rclinic_table_2.y, posi.playerData.rclinic_table_2.z));
        rCurtain.transform.rotation = Quaternion.Euler(new Vector3(posi.playerData.rCurtain.x, posi.playerData.rCurtain.y, posi.playerData.rCurtain.z));
        rManbody.transform.rotation = Quaternion.Euler(new Vector3(posi.playerData.rManbody.x, posi.playerData.rManbody.y, posi.playerData.rManbody.z));
        rlung.transform.rotation = Quaternion.Euler(new Vector3(posi.playerData.rlung.x, posi.playerData.rlung.y, posi.playerData.rlung.z));
        rHeart.transform.rotation = Quaternion.Euler(new Vector3(posi.playerData.rHeart.x, posi.playerData.rHeart.y, posi.playerData.rHeart.z));
        rDigestive.transform.rotation = Quaternion.Euler(new Vector3(posi.playerData.rDigestive.x, posi.playerData.rDigestive.y, posi.playerData.rDigestive.z));




    }
}
