using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitToDestroy : MonoBehaviour {
    public float waitTime;
    public bool destroy;
    
	void Update () {
        if (destroy)
        {
            StartCoroutine(WaitToDie());
            destroy = false;
        }
        
	}
	
	IEnumerator WaitToDie()
    {
        yield return new WaitForSeconds(waitTime);

        Destroy(gameObject);
    }
}
