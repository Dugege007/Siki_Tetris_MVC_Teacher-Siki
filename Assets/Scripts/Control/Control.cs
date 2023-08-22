using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Control 层，相当于桥梁，连接 Model 层和 View 层
// Model 层和 View 层之间不直接进行交互

public class Control : MonoBehaviour
{
    [HideInInspector]
    public Model model;
    [HideInInspector]
    public View view;
    [HideInInspector]
    public CameraManager cameraManager;
    [HideInInspector]
    public GameManager gameManager;
    [HideInInspector]
    public AudioManager audioManager;

    private FSMSystem fsm;

    private void Awake()
    {
        model = GameObject.FindGameObjectWithTag("Model").GetComponent<Model>();
        view = GameObject.FindGameObjectWithTag("View").GetComponent<View>();
        cameraManager = GetComponent<CameraManager>();
        gameManager= GetComponent<GameManager>();
        audioManager = GetComponent<AudioManager>();
    }

    private void Start()
    {
        MakeFSM();
    }

    private void MakeFSM()
    {
        fsm = new FSMSystem();
        FSMState[] states = GetComponentsInChildren<FSMState>();

        foreach (FSMState s in states)
        {
            fsm.AddState(s, this);
        }

        MenuState menuState = GetComponentInChildren<MenuState>();
        fsm.SetCurrentState(menuState);
    }
}
