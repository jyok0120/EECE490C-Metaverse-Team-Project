using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    //이동속도
    public float speed = 5.0f;
    //중력
    public float gravity= -20.0f;
    //캐릭터 컨트롤러 컴포넌트 변수
    CharacterController cc;
    //수직속도
    float yVelocity;
    //점프 크기
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

        //방향 설정
        Vector3 dir = new Vector3(h, 0, v);
        //사용자가 바라보는 방향으로 전환
        dir = Camera.main.transform.TransformDirection(dir);

        //중력 적용
        yVelocity += gravity * Time.deltaTime;
        dir.y = yVelocity;

        if (cc.isGrounded)
        {
            yVelocity = 0;
        }
        //점프 구현
        if (ARAVRInput.GetDown(ARAVRInput.Button.Two,ARAVRInput.Controller.RTouch))
        {
            yVelocity = jumpPower;
        }

        //이동 반영
        cc.Move(dir * speed * Time.deltaTime);

    }
}
