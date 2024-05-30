// Copyright © 2024 llllyyyynnnn. All rights reserved.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEditor.Rendering;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class LynnUI : MonoBehaviour, InputActions.IUIActions
{ // todo: fix hardcoded variables, get actual screen ress
    [SerializeField] int padding = 60;
    [SerializeField] int titleSize = 64;
    [SerializeField] int generalSize = 32;
    [SerializeField] int titleSpacing = 32;
    [SerializeField] int spacingFactor = 12;
    [SerializeField] int buttonWidth = 340;
    [SerializeField] float speed = 1.5f;

    [SerializeField] Color backgroundColor;
    [SerializeField] Color idleColor;
    [SerializeField] Color hoveredColor;
    [SerializeField] Color secondaryColor;

    [SerializeField] Font font;
    [SerializeField] Font font2;
    [SerializeField] Font font3;
    [SerializeField] Texture downGradient;

    [SerializeField] bool startscene = false;

    private Color backgroundColorActive;
    private Color idleColorActive;
    private Color hoveredColorActive;
    private Color secondaryColorActive;

    private float titleSpacingActive;
    private float sideScrollLength = 200;
    private float sideScrollSize = 2;

    private Vector2 resolution;

    private int buttonIndex = 0;
    private Vector2 refFontSize;
    private Vector2 controllerUIOffset;
    private bool clickedCurrentFrame = false;

    private Vector2 last_mousepos = Vector2.zero;
    private bool disable_mouseinput = false;

    private InputActions inputRouter;

    private Vector2 labelSz;
    bool menu_toggled = false;

    void initStyle()
    {
        backgroundColorActive = backgroundColor;
        idleColorActive = idleColor;
        hoveredColorActive = hoveredColor;
        titleSpacingActive = spacingFactor;
        secondaryColorActive = secondaryColor;
    }

    void Awake()
    {
        inputRouter = new InputActions();
        inputRouter.UI.AddCallbacks(this);

        initStyle();

        if (startscene)
            menu_toggled = true;
    }

    private void OnEnable() => inputRouter.UI.Enable();
    private void OnDisable() => inputRouter.UI.Disable();
    public void OnClick(InputAction.CallbackContext context) { if (!context.performed) return; }

    void drawBackground()
    {
        GUI.Box(new Rect(0, 0, resolution.x, resolution.y), "", LynnUI_Generate.colorStyle(backgroundColorActive));
    }
    void drawTitle()
    {
        GUI.skin.font = font;
        GUIStyle label = GUI.skin.label;

        string title = "LYNN";
        string title2 = "UI";

        label.fontSize = titleSize;

        GUIContent content = new GUIContent(title);
        GUIContent content2 = new GUIContent(title2);

        Vector2 textSize = label.CalcSize(content);
        Vector2 textSize2 = label.CalcSize(content2);

        Color contentColorBackup = GUI.contentColor;

        GUI.contentColor = secondaryColorActive;
        GUI.Label(new Rect(padding, padding, textSize.x, textSize.y), title);

        GUI.contentColor = hoveredColorActive;
        GUI.Label(new Rect(padding + textSize.x, padding, textSize2.x, textSize2.y), title2);

        GUI.contentColor = contentColorBackup;
    }

    bool drawButton(string title)
    {
        Color contentColorBackup = GUI.contentColor;
        GUI.contentColor = idleColorActive;

        GUIStyle box = GUI.skin.box;
        GUIStyle label = GUI.skin.label;

        box.fontSize = generalSize;
        label.fontSize = box.fontSize;

        buttonIndex += 1;

        Vector2 labelSize = box.CalcSize(new GUIContent("I"));
        labelSz = labelSize;
        float yOffset = padding + (2 * titleSpacing) + (buttonIndex * labelSize.y + (titleSpacingActive * buttonIndex));

        Vector2 mousePos = Input.mousePosition;
        last_mousepos = mousePos;
        if(!disable_mouseinput)
            mousePos = new Vector2(mousePos.x, resolution.y - mousePos.y); // todo: mouse + key / controller
        else
            mousePos = new Vector2(padding, padding + (2 * titleSpacingActive) + (labelSize.y) + controllerUIOffset.y);

        Vector2 min = new Vector2(padding, yOffset);
        Vector2 max = new Vector2(padding + buttonWidth, yOffset + labelSize.y);

        if ((mousePos.x >= min.x && mousePos.x <= max.x) && (mousePos.y >= min.y && mousePos.y <= max.y))
        {
            sideScrollLength = LynnUI_Animations.Smooth(sideScrollLength, yOffset, speed * 3);
            GUI.contentColor = hoveredColorActive;

            float factor = (yOffset - sideScrollLength);
            if (factor > labelSz.y / 2)
                factor = labelSz.y / 2;
            if (factor < -labelSz.y / 2)
                factor = -labelSz.y / 2;

            GUI.Label(new Rect(padding - 6 - labelSize.x, yOffset, buttonWidth, labelSize.y), "I");
            GUI.Label(new Rect(padding - 6 - labelSize.x, yOffset - factor, buttonWidth, labelSize.y), "I");

            Font font_backup = GUI.skin.font;
            GUI.skin.font = font3;

            GUI.Label(new Rect(padding + Mathf.Abs(factor) / 4, yOffset, buttonWidth, labelSize.y), title);

            GUI.skin.font = font_backup;
            GUI.contentColor = contentColorBackup;
            if (clickedCurrentFrame && menu_toggled)
                return true;
        }
        else
        {
            GUI.Label(new Rect(padding, yOffset, buttonWidth, labelSize.y), title);
        }

        GUI.contentColor = contentColorBackup;
        return false;
    }
    void resetVariables()
    {
        buttonIndex = 0;
        clickedCurrentFrame = false;
    }
    void limitScroll()
    {
        if (controllerUIOffset.y > (((buttonIndex + 1) * (spacingFactor + labelSz.y))))
            controllerUIOffset = new Vector2(controllerUIOffset.x, padding);
        else if (controllerUIOffset.y < padding)
            controllerUIOffset = new Vector2(controllerUIOffset.x, padding);
    }
    void lerpActive()
    {
        if (!menu_toggled)
        {
            idleColorActive = LynnUI_Animations.Smooth(new Color(idleColorActive.r, idleColorActive.g, idleColorActive.b, idleColorActive.a ), new Color(0, 0, 0, 0), speed);
            hoveredColorActive = LynnUI_Animations.Smooth(new Color(hoveredColorActive.r, hoveredColorActive.g, hoveredColorActive.b, hoveredColorActive.a), new Color(0, 0, 0, 0), speed);
            backgroundColorActive = LynnUI_Animations.Smooth(backgroundColorActive, new Color(0, 0, 0, 0), speed);
            secondaryColorActive = LynnUI_Animations.Smooth(secondaryColorActive, new Color(0, 0, 0, 0), speed);

            titleSpacingActive = LynnUI_Animations.Smooth(titleSpacingActive, 0, speed);
        }
        else
        {
            idleColorActive = LynnUI_Animations.Smooth(idleColorActive, new Color(idleColor.r, idleColor.g, idleColor.b, idleColor.a), speed);
            hoveredColorActive = LynnUI_Animations.Smooth(hoveredColorActive, new Color(hoveredColor.r, hoveredColor.g, hoveredColor.b, hoveredColor.a), speed);
            backgroundColorActive = LynnUI_Animations.Smooth(backgroundColorActive, backgroundColor, speed);
            secondaryColorActive = LynnUI_Animations.Smooth(secondaryColorActive, secondaryColor, speed);

            titleSpacingActive = LynnUI_Animations.Smooth(titleSpacingActive, spacingFactor, speed * 2);
        }
    }
    bool cancelUI = false;
    void OnGUI()    
    {
        lerpActive();

        resolution = new Vector2(Screen.width, Screen.height);

        /*
        if (!startscene)
        {
            GUIStyle box = GUI.skin.box;
            Vector2 labelSize = box.CalcSize(new GUIContent(((int)vehicle_speed).ToString()));
            GUI.Label(new Rect(resolution.x / 2 - labelSize.x / 2, resolution.y - padding, 120, 40), ((int)vehicle_speed).ToString());
        }
        */

        if (cancelUI)
        {
            clickedCurrentFrame = false;
            backgroundColor = new Color(0f, 0f, 0f, 1f);
            if (backgroundColorActive.a >= 0.95f)
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        if (menu_toggled || (!menu_toggled && (idleColorActive.a > 0.005 || hoveredColorActive.a > 0.005 || backgroundColorActive.a > 0.005)))
        {
            if(!cancelUI)
                drawBackground();
            drawTitle();

            if (disable_mouseinput && last_mousepos != (Vector2)Input.mousePosition)
                disable_mouseinput = false;

            GUI.skin.font = font2;

            if (startscene && drawButton("PLAY"))
                cancelUI = true;
            if(!startscene && drawButton("RESUME"))
                menu_toggled = false;
            if(!startscene && drawButton("RESET"))
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            if (drawButton("SETTINGS"))
                notifications_add("unavailable in demo");
            if (drawButton("QUIT"))
                Application.Quit();

            if(cancelUI)
                drawBackground();

            limitScroll();
        }

        notifications_run();
        resetVariables();
    }
    void InputActions.IUIActions.OnNavigate(InputAction.CallbackContext context) // todo: adjust for resize
    {
        disable_mouseinput = (Vector2)Input.mousePosition == last_mousepos;
        controllerUIOffset = new Vector2(controllerUIOffset.x, controllerUIOffset.y + (context.action.ReadValue<Vector2>().y * -1 * ((spacingFactor + labelSz.y))));
    }
    void InputActions.IUIActions.OnSubmit(InputAction.CallbackContext context) => clickedCurrentFrame = context.action.IsPressed();
    //void InputActions.IUIActions.OnClick(InputAction.CallbackContext context) => clickedCurrentFrame = context.action.IsPressed();
    void InputActions.IUIActions.OnCancel(InputAction.CallbackContext context)
    {
        menu_toggled = !menu_toggled;
    }

    struct notification
    {
        public string desc;
        public float time_initiated;
        public Vector2 position;
        public float alpha;
    }

    List<notification> notifications = new List<notification>();
    void notifications_run()
    {
        for(int i = 0; i < notifications.Count; i++)
        {
            notification x = notifications[i];
            Color contentColorBackup = GUI.contentColor;
            GUI.contentColor = new Color(1f,1f,1f,x.alpha);
            float y = LynnUI_Animations.Smooth(x.position.y, 100 + 50 * i, speed);
            GUI.Label(new Rect(padding, -50 + padding + (((buttonIndex + 1) * (spacingFactor + labelSz.y))) + y, 1000, 100), x.desc);
            x.position = new Vector2(x.position.x, y);
            x.alpha -= (Time.deltaTime * speed / 12);
            notifications[i] = x;
            GUI.contentColor = contentColorBackup;

            if(x.alpha <= 0)
                notifications.Remove(notifications[i]);
        }
    }

    public void notifications_add(string desc, int duration = 1)
    {
        notification x = new notification();
        x.desc = desc;
        x.time_initiated = Time.time;
        x.alpha = duration;
        notifications.Add(x);
    }
}
