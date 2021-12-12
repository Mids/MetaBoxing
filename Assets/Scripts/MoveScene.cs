using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveScene : MonoBehaviour
{
    public string sceneName;

    public void Load1PPPhysics()
    {
        Load(false, true);
    }

    public void Load3PPPhysics()
    {
        Load(true, true);
    }

    public void Load1PPKinematic()
    {
        Load(false, false);
    }

    public void Load3PPKinematic()
    {
        Load(true, false);
    }

    public void Load(bool is3pp, bool isPhysics)
    {
        PlayerPrefs.SetInt("IS3PP", is3pp ? 1 : 0);
        PlayerPrefs.SetInt("ISPHYSICS", isPhysics ? 1 : 0);
        Load();
    }

    public void Load()
    {
        SceneManager.LoadScene(sceneName);
    }
}