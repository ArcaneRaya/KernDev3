using UnityEditor;
using UnityEngine;

public static class Screenshot {

    [MenuItem("Screenshot/Take screenshot")]
    static void TakeScreenshot() {
        ScreenCapture.CaptureScreenshot("Assets/Screenshots/Screenshot.png");
        AssetDatabase.Refresh();
    }
}