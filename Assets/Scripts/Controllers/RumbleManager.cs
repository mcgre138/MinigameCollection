using UnityEngine;
using System.Collections;
using Rewired;
using System.Collections.Generic;

public class RumbleManager : MonoBehaviour {

    static private RumbleManagerHelper[] players = new RumbleManagerHelper[4];

    static public bool isOn = true;

    void Start()
    {
        for(int i = 0; i < ReInput.players.playerCount; i++)
        {
            players[i] = new RumbleManagerHelper();
            players[i].player = ReInput.players.GetPlayer(i);
        }
    }

    void Update()
    {
        for(int i = 0; i < ReInput.players.playerCount; i++)
        {
            players[i].UpdateRumble();
        }
    }

    public void ToggleRumble(bool value)
    {
        isOn = value;
    }

    public static void SetRumble(int playerNumber, float vibrationIntensity, float duration)
    {
        SetRumble(playerNumber, 0, vibrationIntensity, duration);
    }

    public static void SetRumble(int playerNumber, int motorIndex, float vibrationIntensity, float duration)
    {
        if (isOn) players[playerNumber - 1].AddRumble(motorIndex, vibrationIntensity, duration);
    }

    public static void ClearAllRumble()
    {
        for(int i = 0; i < ReInput.controllers.joystickCount; i++)
        {
            ReInput.controllers.Joysticks[i].SetVibration(0, 0);
        }
    }

    public class RumbleManagerHelper
    {
        public Player player;
        public float rumbleDuration = 0;
        public float rumbleLevel = 0;

        private float rumbleRate = 0;

        private int activeMotor = 0;

        public void AddRumble(float intensity, float duration)
        {
            AddRumble(0, intensity, duration);
        }

        public void AddRumble(int motorNumber, float intensity, float duration)
        {
            activeMotor = motorNumber;
            rumbleLevel = intensity;
            rumbleDuration += duration;
        }

        public void UpdateRumble()
        {
            if(rumbleRate >= rumbleDuration)
            {
                StopVibration();

                rumbleDuration = 0;
                rumbleRate = 0;
                rumbleLevel = 0;
            }
            else
            {
                rumbleRate += Time.unscaledDeltaTime;
                SetVibration();
            }
        }

        void SetVibration()
        {
            player.SetVibration(activeMotor, rumbleLevel);
            //foreach (Joystick joystick in player.controllers.Joysticks)
            //{
            //    joystick.SetVibration(activeMotor, rumbleLevel);
            //}
        }

        void StopVibration()
        {
            player.StopVibration();
            //foreach (Joystick joystick in player.controllers.Joysticks)
            //{
            //    joystick.StopVibration();
            //}
        }
    }

}
