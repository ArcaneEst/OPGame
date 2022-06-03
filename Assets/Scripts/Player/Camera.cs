using UnityEngine;

public class Camera : MonoBehaviour
{
    private GameObject player;
    private Vector3 offset;

    private static Transform tr;
    private static float elapsed, duration, power, percentComplete;
    private static Vector3 originalPos;

    private void Awake()
    {
        if (SoundSetting.MusicOn)
            GetComponent<AudioSource>().Play();
        player = GameObject.FindWithTag(Tags.Player);
        percentComplete = 1;
        tr = GetComponent<Transform>();
        offset = tr.position - player.transform.position;
    }

    void Update()
    {
        transform.position = new Vector3(tr.position.x,player.transform.position.y + offset.y, -10);
        if (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            percentComplete = elapsed / duration;
            percentComplete = Mathf.Clamp01(percentComplete);
            Vector3 rnd = Random.insideUnitSphere * power * (1f - percentComplete);
            tr.localPosition = originalPos + new Vector3(rnd.x, 0, 0);
        }
    }

    public static void Shake(float duration, float power)
    {
        if (percentComplete == 1) originalPos = tr.localPosition;
        elapsed = 0;
        Camera.duration = duration;
        Camera.power = power;
    }
}
