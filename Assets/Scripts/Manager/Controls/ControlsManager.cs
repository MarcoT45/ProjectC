using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControlsManager : MonoBehaviour {

    private static ControlsManager instance = null;
    public static ControlsManager Instance => instance;

    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(this.gameObject);
            return;
        } else {
            instance = this;
        }

        DontDestroyOnLoad(this.gameObject);

        playerInput = GetComponent<PlayerInput>();
        SetupInputActions();
    }

    // En haut le singleton 
    // En bas la partie jeu

    private PlayerInput playerInput;

    private InputAction deplacer;
    private InputAction trinket1;
    private InputAction trinket2;
    private InputAction pause;
    private InputAction valider;
    private InputAction fermer;
    private InputAction inventaire;
    private InputAction actionSpeciale;
    private InputAction camera;

    public Vector2 DeplacerValue { get; private set; }
    public bool DeplacerPressed { get; private set; }
    public bool Trinket1Pressed { get; private set; }
    public bool Trinket2Pressed { get; private set; }
    public bool PausePressed { get; private set; }
    public bool ValiderPressed { get; private set; }
    public bool FermerPressed { get; private set; }
    public bool InventairePressed { get; private set; }
    public bool ActionSpecialePressed { get; private set; }
    public Vector2 CameraValue { get; private set; }
    public bool CameraPressed { get; private set; }

    private void Update() {
        DeplacerValue = deplacer.ReadValue<Vector2>();
        DeplacerPressed = deplacer.WasPressedThisFrame();
        Trinket1Pressed = trinket1.WasPressedThisFrame();
        Trinket2Pressed = trinket2.WasPressedThisFrame();
        PausePressed = pause.WasPressedThisFrame();
        ValiderPressed = valider.WasPressedThisFrame();
        FermerPressed = fermer.WasPressedThisFrame();
        InventairePressed = inventaire.WasPressedThisFrame();
        ActionSpecialePressed = actionSpeciale.WasPressedThisFrame();
        CameraValue = camera.ReadValue<Vector2>();
        CameraPressed = camera.WasPressedThisFrame();
    }

    private void SetupInputActions() {
        deplacer = playerInput.actions["Deplacer"];
        trinket1 = playerInput.actions["Trinket 1"];
        trinket2 = playerInput.actions["Trinket 2"];
        pause = playerInput.actions["Pause"];
        valider = playerInput.actions["Valider"];
        fermer = playerInput.actions["Fermer"];
        inventaire = playerInput.actions["Inventaire"];
        actionSpeciale = playerInput.actions["Action Speciale"];
        camera = playerInput.actions["Camera"];
    }


    public ControlsState controlsState = 0;

    public void UpdateState(int newState) {
        controlsState = (ControlsState) newState;
    }

}

public enum ControlsState {
    InputTest = 0,
    TitleScreen = 1,
    Options = 2,
    PopUpCorrupted = 3,
    PopUpOuiNon = 4,
    CharacterHub = 5,
    Dialogue = 6,
    DialogueChoice = 7,
    Carte = 300
}