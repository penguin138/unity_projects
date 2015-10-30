using UnityEngine;
using System.Collections;

public class SceneChanger : MonoBehaviour {
    public void changeScene(string sceneName) {
        Application.LoadLevel(sceneName);
    }
}
