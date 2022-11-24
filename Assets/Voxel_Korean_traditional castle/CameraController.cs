using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 1f;
    private void Start()
    {
        Debug.Log($"이동: 화살표, 회전: WASD");
        Debug.Log($"위 이동: SPACE, 아래 이동: C");
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.UpArrow))
        {
            // 앞으로 이동
            this.transform.Translate(0f, 0f, this.moveSpeed);
        }
        if(Input.GetKey(KeyCode.DownArrow))
        {
            // 뒤로 이동
            this.transform.Translate(0f, 0f, -this.moveSpeed);
        }
        if(Input.GetKey(KeyCode.LeftArrow))
        {
            // 왼쪽으로 이동
            this.transform.Translate(-this.moveSpeed, 0f, 0f);
        }
        if(Input.GetKey(KeyCode.RightArrow))
        {
            // 오른쪽으로 이동
            this.transform.Translate(this.moveSpeed, 0f, 0);
        }
        if(Input.GetKey(KeyCode.Space))
        {
            // 위으로 이동
            this.transform.Translate(0f, this.moveSpeed, 0f);
        }
        if(Input.GetKey(KeyCode.C))
        {
            // 아래 이동
            this.transform.Translate(0f, -this.moveSpeed, 0f);
        }

        
        if(Input.GetKey(KeyCode.W))
        {
            // 앞으로 회전
            this.transform.Rotate(-this.moveSpeed, 0f, 0f);
        }
        if(Input.GetKey(KeyCode.S))
        {
            // 뒤로 회전
            this.transform.Rotate(this.moveSpeed, 0f, 0f);
        }
        if(Input.GetKey(KeyCode.A))
        {
            // 왼쪽으로 회전
            this.transform.Rotate(0f, -this.moveSpeed, 0f);
        }
        if(Input.GetKey(KeyCode.D))
        {
            // 오른쪽으로 회전
            this.transform.Rotate(0f, this.moveSpeed, 0f);
        }
    }
}
