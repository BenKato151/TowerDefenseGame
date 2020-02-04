using UnityEngine;

public class Sound : MonoBehaviour {

    private AudioSource audiofile;
	// Use this for initialization
	void Start () {
        audiofile = this.gameObject.GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
        if (PhaseManager.isInFight)
        {
            if (!audiofile.isPlaying)
            {
                audiofile.PlayDelayed(0.5f);
                audiofile.loop = true;
            }
        }
        else
        {
            if (audiofile.isPlaying)
            {
                audiofile.Pause();
            }
        }
	}
}
