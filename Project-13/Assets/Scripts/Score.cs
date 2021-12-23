using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour {

    public Transform BlueFlag;
    public Transform RedFlag;

    public GameObject[] playerAndBlueFlag;
    public GameObject[] playerAndRedFlag;

    public int radius = 3;

    public GameObject[] detectPlayerRed;
    public GameObject[] detectPlayerBlue;

    public int[] distanceFromFlag;

    public bool flagReplacement;

    [SerializeField] GameObject closest = null;
    FixedJoint joint;

    public static int blueTeamScore = 0;
    public static int redTeamScore = 0;

    public GameObject[] blueFlag;
    public GameObject[] redFlag;

    // Start is called before the first frame update
    void Awake()
    {
        detectPlayerRed = GameObject.FindGameObjectsWithTag("RedTeamPlayer");
        detectPlayerBlue = GameObject.FindGameObjectsWithTag("BlueTeamPlayer");

        playerAndRedFlag = GameObject.FindGameObjectsWithTag("RedFlag");
        playerAndBlueFlag = GameObject.FindGameObjectsWithTag("BlueFlag");
        
    }

    // Update is called once per frame
    void Update() {

        for(int i = 0; i < detectPlayerRed.Length; i++) {
            Transform Players = detectPlayerRed [i].GetComponent<Transform>();
            distanceFromFlag[i] = (int)Vector3.Distance (Players.position,RedFlag.position);
            System.Array.Sort(distanceFromFlag);

            if(distanceFromFlag[0] < radius && gameObject.GetComponent<FixedJoint>() == false) {
                StartCoroutine("RedPickUpDelay");
            }

            GameObject[] a;
            a = GameObject.FindGameObjectsWithTag("RedFlag");
            float distance = Mathf.Infinity;
            Vector3 pos = transform.position;
            foreach(GameObject b in a) {
                Vector3 diff = b.transform.position - pos;
                float currentDistance = diff.magnitude;
                if(currentDistance < distance && currentDistance < radius) {
                    closest = b;
                    distance = currentDistance;
                }
            }

            if(flagReplacement == true && gameObject.GetComponent<FixedJoint>() == false) {
                StartCoroutine("RedFlagRecapture");
            }

        }

        for(int i = 0; i < detectPlayerBlue.Length; i++) {
            Transform Players = detectPlayerBlue [i].GetComponent<Transform>();
            distanceFromFlag[i] = (int)Vector3.Distance (Players.position,BlueFlag.position);
            System.Array.Sort(distanceFromFlag);

            if(distanceFromFlag[0] < radius && gameObject.GetComponent<FixedJoint>() == false) {
                StartCoroutine("BluePickUpDelay");
            }

            GameObject[] a;
            a = GameObject.FindGameObjectsWithTag("BlueFlag");
            float distance = Mathf.Infinity;
            Vector3 pos = transform.position;
            foreach(GameObject b in a) {
                Vector3 diff = b.transform.position - pos;
                float currentDistance = diff.magnitude;
                if(currentDistance < distance && currentDistance < radius) {
                    closest = b;
                    distance = currentDistance;
                }
            }

            if(flagReplacement == true && gameObject.GetComponent<FixedJoint>() == false) {
                StartCoroutine("BlueFlagRecapture");
            }

        }
        
    }

    IEnumerator RedPickUpDelay() {
        yield return new WaitForSeconds(1f);
        if(flagReplacement == false && gameObject.GetComponent<FixedJoint>() == false && distanceFromFlag[0] < radius) {
            RedFlag.position = closest.GetComponent<Transform>().position;
            RedFlag.rotation = closest.GetComponent<Transform>().rotation;
        }
        yield return new WaitForSeconds(0.01f);
        if(distanceFromFlag[0] < radius && gameObject.GetComponent<FixedJoint>() == false) {
            gameObject.AddComponent<FixedJoint>();
            joint = gameObject.GetComponent<FixedJoint>();
            joint.connectedBody = closest.GetComponentInParent<Rigidbody>();
            flagReplacement = true;
            gameObject.GetComponentInParent<Rigidbody>().velocity = Vector3.zero;
            yield return new WaitForSeconds(1f);
            joint.breakForce = 500;

        }
    }

    IEnumerator BluePickUpDelay() {
        yield return new WaitForSeconds(1f);
        if(flagReplacement == false && gameObject.GetComponent<FixedJoint>() == false && distanceFromFlag[0] < radius) {
            BlueFlag.position = closest.GetComponent<Transform>().position;
            BlueFlag.rotation = closest.GetComponent<Transform>().rotation;
        }
        yield return new WaitForSeconds(0.01f);
        if(distanceFromFlag[0] < radius && gameObject.GetComponent<FixedJoint>() == false) {
            gameObject.AddComponent<FixedJoint>();
            joint = gameObject.GetComponent<FixedJoint>();
            joint.connectedBody = closest.GetComponentInParent<Rigidbody>();
            flagReplacement = true;
            gameObject.GetComponentInParent<Rigidbody>().velocity = Vector3.zero;
            yield return new WaitForSeconds(1f);
            joint.breakForce = 500;

        }
    }
    
    IEnumerator RedFlagRecapture() {
        yield return new WaitForSeconds(2f);
        flagReplacement = false;
    }

    IEnumerator BlueFlagRecapture() {
        yield return new WaitForSeconds(2f);
        flagReplacement = false;
    }

}
