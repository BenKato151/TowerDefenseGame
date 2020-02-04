using UnityEngine;

public class CameraManager : MonoBehaviour {

    // Update is called once per frame

    void Update () {

        // TODO: New Camera Position
        if (!PhaseManager.isInFight)
        {
            this.gameObject.transform.position = new Vector3(38.4f, 150.3f, 131f);
            this.gameObject.transform.eulerAngles = new Vector3(90f, 90f, -0.003f);
            this.gameObject.GetComponent<Camera>().fieldOfView = 51f;
        }
        else
        {
            this.gameObject.transform.position = new Vector3(38.4f, 50f, -53.6f);
            this.gameObject.transform.eulerAngles = new Vector3(17.836f, 0f, 0f);
            this.gameObject.GetComponent<Camera>().fieldOfView = 56.90254f;
        }
	}
}
