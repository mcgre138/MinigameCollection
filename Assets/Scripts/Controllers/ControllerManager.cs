using UnityEngine;
using System.Collections;
using Rewired;
using Rewired.ControllerExtensions;

public class ControllerManager : MonoBehaviour {

    void Start()
    {
        ReInput.ControllerConnectedEvent += ReInput_ControllerConnectedEvent;
        //GameState.OnBackToMainMenu.AddListener(ResetControllers);
        AssignControllersToSystemPlayer();
    }

    private void ReInput_ControllerConnectedEvent(ControllerStatusChangedEventArgs obj)
    {
        AssignControllersToSystemPlayer();
    }

    void Update()
    {
        SetupPS4Controllers();
    }

    public void SetupPS4Controllers()
    {
        if (ReInput.controllers.GetControllers(ControllerType.Joystick) == null) return;
        foreach(Controller controller in ReInput.controllers.GetControllers(ControllerType.Joystick))
        {
            DualShock4Extension ds4 = controller.GetExtension<DualShock4Extension>();
            if (ds4 == null) continue;
            SetControllerColor(controller, ds4);
        }
    }

    void SetControllerColor(Controller controller, DualShock4Extension ds4)
    {
        for(int i = 0; i < ReInput.players.playerCount; i++)
        {
            if (ReInput.controllers.IsControllerAssignedToPlayer(controller.type, controller.id, i))
            {
                ds4.SetLightColor(Color.red);
                return;
            }
        }

        ds4.SetLightColor(Color.white);
    }

    static public void AssignControllersToSystemPlayer()
    {
        ReInput.players.SystemPlayer.controllers.hasKeyboard = true;
        if (ReInput.controllers.GetControllerCount(ControllerType.Joystick) != 0)
        {
            foreach (Controller targetController in ReInput.controllers.GetControllers(ControllerType.Joystick))
            {
                if (!ReInput.players.GetPlayer("SYSTEM").controllers.ContainsController(targetController.type, targetController.id) && !IsAssignedToPlayers(targetController))
                {
                    ReInput.players.SystemPlayer.controllers.AddController(targetController, false);
                }
            }
        }
        
        if(ReInput.controllers.GetControllerCount(ControllerType.Custom) != 0)
        {
            foreach (Controller targetController in ReInput.controllers.GetControllers(ControllerType.Custom))
            {
                if (!ReInput.players.GetPlayer("SYSTEM").controllers.ContainsController(targetController.type, targetController.id) && !IsAssignedToPlayers(targetController))
                {
                    ReInput.players.SystemPlayer.controllers.AddController(targetController, false);
                }
            }
        }
       
    }

    void OnApplicationQuit()
    {
        if (ReInput.controllers.GetControllers(ControllerType.Joystick) == null) return;
        foreach (Controller controller in ReInput.controllers.GetControllers(ControllerType.Joystick))
        {
            DualShock4Extension ds4 = controller.GetExtension<DualShock4Extension>();
            if (ds4 == null) continue;
            ds4.SetLightColor(Color.clear);
        }
    }

    static public bool IsAssignedToPlayers(Controller controller)
    {
        //if (controller.type == ControllerType.Keyboard) return false;

        if (ReInput.controllers.IsControllerAssignedToPlayer(controller.type, controller.id, 0)) return true;
        if (ReInput.controllers.IsControllerAssignedToPlayer(controller.type, controller.id, 1)) return true;
        if (ReInput.controllers.IsControllerAssignedToPlayer(controller.type, controller.id, 2)) return true;
        if (ReInput.controllers.IsControllerAssignedToPlayer(controller.type, controller.id, 3)) return true;

        return false;
    }

    static public int AddController(Controller controller)
    {
        int playerNumber = 1;
        int playerIndex = playerNumber - 1;

        ReInput.players.Players[playerIndex].controllers.ClearAllControllers();
        ReInput.players.Players[playerIndex].isPlaying = true;
        ReInput.players.Players[playerIndex].controllers.AddController(controller, true);

        if(controller.type == ControllerType.Keyboard)
        {
            ReInput.players.Players[playerIndex].controllers.hasKeyboard = true;

            int targetLayout = ReInput.mapping.GetLayoutId(ControllerType.Keyboard, "Player 1");
            int targetCategory;

            for (int i = 1; i < ReInput.mapping.MapCategories.Count; i++)
            {
                targetCategory = ReInput.mapping.MapCategories[i].id;
                ReInput.players.Players[playerNumber - 1].controllers.maps.LoadMap(ControllerType.Keyboard, controller.id, targetCategory, targetLayout, true);
            }
        }

        //GameData.players[playerIndex].input = ReInput.players.Players[playerIndex];

        RumbleManager.SetRumble(playerNumber, 1, .5f, .2f);

        return playerNumber;
    }

    static public void RemoveController(int playerNumber)
    {
        //ReInput.players.Players[playerNumber - 1].controllers.ClearAllControllers();
        //ColorManager.instance.colors[GameData.players[playerNumber - 1].colorIndex].isSelected = false;
        //GameData.players[playerNumber - 1].ClearPlayer();
        AssignControllersToSystemPlayer();
    }

    static public void ResetControllers()
    {
        for(int i = 0; i < 4; i++)
        {
            //ColorManager.instance.colors[GameData.players[i].colorIndex].isSelected = false;
            //GameData.players[i].ClearPlayer();
        }
        
        foreach (Controller controller in ReInput.controllers.Controllers)
        {
            ReInput.controllers.RemoveControllerFromAllPlayers(controller, true);
        }

        foreach (Player player in ReInput.players.Players)
        {
            player.controllers.maps.ClearMaps(ControllerType.Keyboard, false);
        }

        AssignControllersToSystemPlayer();
    }
}
