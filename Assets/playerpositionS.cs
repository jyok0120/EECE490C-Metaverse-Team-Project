using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerpositionS : MonoBehaviour
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
    //save는 되는데, load는 ?!??!
    public void saveC_P()
    {

        PlayerController posi = Manbody.GetComponent<PlayerController>();
        posi.playerData.clinic_table_1 = new Vector3(clinic_table_1.transform.position.x, clinic_table_1.transform.position.y, clinic_table_1.transform.position.z);
        posi.playerData.X_ray_monitor = new Vector3(X_ray_monitor.transform.position.x, X_ray_monitor.transform.position.y, X_ray_monitor.transform.position.z);
        posi.playerData.X_ray_scan = new Vector3(X_ray_scan.transform.position.x, X_ray_scan.transform.position.y, X_ray_scan.transform.position.z);
        posi.playerData.Locker = new Vector3(Locker.transform.position.x, Locker.transform.position.y, Locker.transform.position.z);
        posi.playerData.operating_lamp = new Vector3(operating_lamp.transform.position.x, operating_lamp.transform.position.y, operating_lamp.transform.position.z);
        posi.playerData.Surgical_table = new Vector3(Surgical_table.transform.position.x, Surgical_table.transform.position.y, Surgical_table.transform.position.z);
        posi.playerData.clinic_table_2 = new Vector3(clinic_table_2.transform.position.x, clinic_table_2.transform.position.y, clinic_table_2.transform.position.z);
        posi.playerData.Curtain = new Vector3(Curtain.transform.position.x, Curtain.transform.position.y, Curtain.transform.position.z);
        posi.playerData.Manbody = new Vector3(Manbody.transform.position.x, Manbody.transform.position.y, Manbody.transform.position.z);
        posi.playerData.lung = new Vector3(lung.transform.position.x, lung.transform.position.y, lung.transform.position.z);
        posi.playerData.Heart = new Vector3(Heart.transform.position.x, Heart.transform.position.y, Heart.transform.position.z);
        posi.playerData.Digestive = new Vector3(Digestive.transform.position.x, Digestive.transform.position.y, Digestive.transform.position.z);


        posi.playerData.rclinic_table_1 = new Vector3(rclinic_table_1.transform.eulerAngles.x, rclinic_table_1.transform.eulerAngles.y, rclinic_table_1.transform.eulerAngles.z);
        posi.playerData.rX_ray_monitor = new Vector3(rX_ray_monitor.transform.eulerAngles.x, rX_ray_monitor.transform.eulerAngles.y, rX_ray_monitor.transform.eulerAngles.z);
        posi.playerData.rX_ray_scan = new Vector3(rX_ray_scan.transform.eulerAngles.x, rX_ray_scan.transform.eulerAngles.y, rX_ray_scan.transform.eulerAngles.z);
        posi.playerData.rLocker = new Vector3(rLocker.transform.eulerAngles.x, rLocker.transform.eulerAngles.y, rLocker.transform.eulerAngles.z);
        posi.playerData.roperating_lamp = new Vector3(roperating_lamp.transform.eulerAngles.x, roperating_lamp.transform.eulerAngles.y, roperating_lamp.transform.eulerAngles.z);
        posi.playerData.rSurgical_table = new Vector3(rSurgical_table.transform.eulerAngles.x, rSurgical_table.transform.eulerAngles.y, rSurgical_table.transform.eulerAngles.z);
        posi.playerData.rclinic_table_2 = new Vector3(rclinic_table_2.transform.eulerAngles.x, rclinic_table_2.transform.eulerAngles.y, rclinic_table_2.transform.eulerAngles.z);
        posi.playerData.rCurtain = new Vector3(rCurtain.transform.eulerAngles.x, rCurtain.transform.eulerAngles.y, rCurtain.transform.eulerAngles.z);
        posi.playerData.rManbody = new Vector3(rManbody.transform.eulerAngles.x, rManbody.transform.eulerAngles.y, rManbody.transform.eulerAngles.z);
        posi.playerData.rlung = new Vector3(rlung.transform.eulerAngles.x, rlung.transform.eulerAngles.y, rlung.transform.eulerAngles.z);
        posi.playerData.rHeart = new Vector3(rHeart.transform.eulerAngles.x, rHeart.transform.eulerAngles.y, rHeart.transform.eulerAngles.z);
        posi.playerData.rDigestive = new Vector3(rDigestive.transform.eulerAngles.x, rDigestive.transform.eulerAngles.y, rDigestive.transform.eulerAngles.z);
        //cube위치 가져옴 여기까지는 잘 작동함

        //cubepl.transform.position = new Vector3(posi.playerData.abc.x, posi.playerData.abc.y, posi.playerData.abc.z);
    }
}
