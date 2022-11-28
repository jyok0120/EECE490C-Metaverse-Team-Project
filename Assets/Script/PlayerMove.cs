using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    //�̵��ӵ�
    public float speed = 5.0f;
    //�߷�
    public float gravity= -20.0f;
    //ĳ���� ��Ʈ�ѷ� ������Ʈ ����
    CharacterController cc;
    //�����ӵ�
    float yVelocity;
    //���� ũ��
    public float jumpPower = 5f;

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        float h = ARAVRInput.GetAxis("Horizontal");
        float v = ARAVRInput.GetAxis("Vertical");

        //���� ����
        Vector3 dir = new Vector3(h, 0, v);
        //����ڰ� �ٶ󺸴� �������� ��ȯ
        dir = Camera.main.transform.TransformDirection(dir);

        //�߷� ����
        yVelocity += gravity * Time.deltaTime;
        dir.y = yVelocity;

        if (cc.isGrounded)
        {
            yVelocity = 0;
        }
        //���� ����
        if (ARAVRInput.GetDown(ARAVRInput.Button.Two,ARAVRInput.Controller.RTouch))
        {
            yVelocity = jumpPower;
        }

        //�̵� �ݿ�
        cc.Move(dir * speed * Time.deltaTime);

    }
}
