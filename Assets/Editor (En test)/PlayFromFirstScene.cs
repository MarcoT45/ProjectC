using UnityEditor;
using UnityEditor.SceneManagement;

// Yo Dawg ! Ce script sert juste à lancer unity directement sur titlescreen
// peu importe la scene sur laquelle tu fais play
// Pas besoin d'inclure le script ou que ce soit dans la scene
// Faudra le delete à la fin du projet
// SCRIPT A SUPPRIMER PLUS TARD

[InitializeOnLoad]
public static class PlayFromFirstScene {

    static PlayFromFirstScene() {
        EditorApplication.playModeStateChanged += OnPlayModeChanged;
    }

    private static void OnPlayModeChanged(PlayModeStateChange state) {
        if (state == PlayModeStateChange.ExitingEditMode) {
            // Demande de sauvegarde si des scènes sont modifiées
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                return;

            // Ouvre la première scène du Build Settings (index 0)
            EditorSceneManager.OpenScene(
                EditorBuildSettings.scenes[0].path
            );
        }
    }

}