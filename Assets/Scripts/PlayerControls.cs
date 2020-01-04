using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{

    public float mSpeed;
    public float rotateSpeed;

    Vector3 mVelocity;

    public GameObject selected;

    private void Update()
    {

        mVelocity = Vector3.zero;

        Vector3 pos = transform.position;
        
        Vector3 rot = transform.rotation.eulerAngles;

        Quaternion rotation = transform.rotation;
        
        if (Input.GetKey("w"))
        {
            mVelocity.y = 1.0f * Time.deltaTime;
        }

        if (Input.GetKey("s"))
        {
            mVelocity.y = -1.0f * Time.deltaTime;
        }

        if (Input.GetKey("a"))
        {
            mVelocity.x =  -1.0f * Time.deltaTime;
        }

        if (Input.GetKey("d"))
        {
            mVelocity.x = 1.0f * Time.deltaTime;
        }

        if (Input.GetKey("r"))
        {
            mVelocity.z = 1.0f * Time.deltaTime;
        }

        if (Input.GetKey("f"))
        {
            mVelocity.z = -1.0f * Time.deltaTime;
        }

        if (Input.GetKey("q"))
        {
            rot.z += rotateSpeed * Time.deltaTime;
        }

        if (Input.GetKey("e"))
        {
           rot.z -= rotateSpeed * Time.deltaTime;
        }

        transform.Translate(mVelocity.normalized * Time.deltaTime * mSpeed);

        rotation.eulerAngles = rot;
        transform.rotation = rotation;
    }

}
