using UnityEngine;
using UnityEngine.SceneManagement;

class SoundSetting
{
    public static bool EffectsOn = true;
    public static bool MusicOn = true;
}

public class SceneLoader : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
            SceneManager.LoadScene(0);
    }

    public void SceneLoad(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Effects()
    {
        SoundSetting.EffectsOn = !SoundSetting.EffectsOn;
    }
    
    public void Music()
    {
        SoundSetting.MusicOn = !SoundSetting.MusicOn;
    }
}
