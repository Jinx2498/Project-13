using UnityEngine;

public class Recoil : MonoBehaviour
{
    public bool isADS;                      //Aiming down sight boolean

    private Vector3 currentRotation;        //Roatations
    private Vector3 targetRotation;

    public float recoilX = -2;                   //Hip fire recoil
    public float recoilY = 2;
    public float recoilZ = 0.35f;

    public float adsRecoilX = -1.5f;                //ADS recoil
    public float adsRecoilY = 1;
    public float adsRecoilZ = 0.3f;

    public float snapiness = 6;                 //Settings
    public float returnSpeed = 2;

    void Update()
    {
        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, returnSpeed * Time.deltaTime);          //crreate new aiming target
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, snapiness * Time.deltaTime);       //path from current target to new target
        transform.localRotation = Quaternion.Euler(currentRotation);                                        //begin movement from current target to new target
    }

    public void RecoilFire()
    {
        if (isADS)
        {
            targetRotation += new Vector3(adsRecoilX, Random.Range(-adsRecoilY, adsRecoilY), Random.Range(-adsRecoilZ, adsRecoilZ));
        } else
        {
            targetRotation += new Vector3(recoilX, Random.Range(-recoilY, recoilY), Random.Range(-recoilZ, recoilZ));
        }
    }
}
