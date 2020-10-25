// GENERATED AUTOMATICALLY FROM 'Packages/com.redowl.foundation/Resources/AvatarControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace RedOwl.Engine
{
    public class @AvatarControls : IInputActionCollection, IDisposable
    {
        public InputActionAsset asset { get; }
        public @AvatarControls()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""AvatarControls"",
    ""maps"": [
        {
            ""name"": ""Avatar"",
            ""id"": ""df70fa95-8a34-4494-b137-73ab6b9c7d37"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""351f2ccd-1f9f-44bf-9bec-d62ac5c5f408"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": ""NormalizeVector2"",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Look"",
                    ""type"": ""Value"",
                    ""id"": ""efc3eda3-75ab-446c-be89-363d2c621c85"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": ""NormalizeVector2"",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ButtonSouth"",
                    ""type"": ""Button"",
                    ""id"": ""6c2ab1b8-8984-453a-af3d-a3c78ae1679a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ButtonEast"",
                    ""type"": ""Button"",
                    ""id"": ""370a7d50-afa6-481d-a74f-3060cf4526ac"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ButtonWest"",
                    ""type"": ""Button"",
                    ""id"": ""b2231b45-ff5b-406b-bf81-eccc74104498"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ButtonNorth"",
                    ""type"": ""Button"",
                    ""id"": ""37c1ff29-05d1-4289-9bb4-fa7c34416547"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""TriggerRight"",
                    ""type"": ""Button"",
                    ""id"": ""ddc6f962-d4f6-4f5e-97c0-678cedb5736f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""TriggerLeft"",
                    ""type"": ""Button"",
                    ""id"": ""7af96386-cdfe-4f61-9142-fdd87ebdc522"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ShoulderRight"",
                    ""type"": ""Button"",
                    ""id"": ""62bfb552-4c6f-4707-980e-34154df582a4"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ShoulderLeft"",
                    ""type"": ""Button"",
                    ""id"": ""d23abde5-d7b3-46d9-ba69-b1d8a3d0d354"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SpecialRight"",
                    ""type"": ""Button"",
                    ""id"": ""4b5f2b86-2e31-4d87-a0e6-c7e70ebb4fb8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SpecialLeft"",
                    ""type"": ""Button"",
                    ""id"": ""c24ac141-e03f-4cd1-ab48-2e6bcbc12f15"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""North"",
                    ""type"": ""Button"",
                    ""id"": ""e22e3b0b-cdb4-4f95-90c9-efda266a3612"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""South"",
                    ""type"": ""Button"",
                    ""id"": ""d65ec952-3383-49d5-accd-c473c78f0914"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""East"",
                    ""type"": ""Button"",
                    ""id"": ""3c89e774-4d52-4b2c-b2da-cfd0443048f6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""West"",
                    ""type"": ""Button"",
                    ""id"": ""af478e3c-af49-43e7-ac90-12a527dac6d5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""978bfe49-cc26-4a3d-ab7b-7d7a29327403"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player1;Player2"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""WASD"",
                    ""id"": ""00ca640b-d935-4593-8157-c05846ea39b3"",
                    ""path"": ""Dpad"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""e2062cb9-1b15-46a2-838c-2f8d72a0bdd9"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Player1"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""up"",
                    ""id"": ""8180e8bd-4097-4f4e-ab88-4523101a6ce9"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player2"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""320bffee-a40b-4347-ac70-c210eb8bc73a"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Player1"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""1c5327b5-f71c-4f60-99c7-4e737386f1d1"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player2"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""d2581a9b-1d11-4566-b27d-b92aff5fabbc"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Player1"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""2e46982e-44cc-431b-9f0b-c11910bf467a"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player2"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""fcfe95b8-67b9-4526-84b5-5d0bc98d6400"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Player1"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""77bff152-3580-4b21-b6de-dcd0c7e41164"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player2"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""143bb1cd-cc10-4eca-a2f0-a3664166fe91"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player1;Player2"",
                    ""action"": ""ButtonSouth"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""05f6913d-c316-48b2-a6bb-e225f14c7960"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player1"",
                    ""action"": ""ButtonSouth"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""886e731e-7071-4ae4-95c0-e61739dad6fd"",
                    ""path"": ""<Touchscreen>/primaryTouch/tap"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player1;Player2"",
                    ""action"": ""ButtonSouth"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7a9c5b1a-e38b-4078-a78e-2dbe7403cd74"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player2"",
                    ""action"": ""ButtonSouth"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a5ed2862-4a32-4842-9430-8eee484e4515"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse;Player1"",
                    ""action"": ""LeftClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d72046a3-b515-42d9-9602-132cbce05a3d"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player1;Player2"",
                    ""action"": ""ButtonEast"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""97804e13-3f07-40dd-ad5d-ea838fb1199e"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player1"",
                    ""action"": ""ButtonEast"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""90e55a9a-99cd-4ba1-9601-eb2d1ebbe176"",
                    ""path"": ""<Keyboard>/rightCtrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player2"",
                    ""action"": ""ButtonEast"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3f396a43-3216-4557-a40f-ae3d11d91d12"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player1;Player2"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8d7cc1f2-3110-47d6-9330-b68bff610a26"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player1"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""47bbc3bd-d576-4929-b999-86a6804ce77c"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player1;Player2"",
                    ""action"": ""ButtonWest"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fb77f4b1-626f-4f11-8b1a-18e754f2c30f"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player1"",
                    ""action"": ""ButtonWest"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""415e467c-1e32-45e3-8f89-3bd1a6a1bfa8"",
                    ""path"": ""<Keyboard>/numpad1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player2"",
                    ""action"": ""ButtonWest"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4278a146-bdf6-42cf-9d70-0bcb9620b824"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player1;Player2"",
                    ""action"": ""ButtonNorth"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b15abd0c-673b-473b-8454-227bfa53d39c"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player1"",
                    ""action"": ""ButtonNorth"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""aebb7381-81cc-4dfb-bd15-3c94279ae573"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player2"",
                    ""action"": ""ButtonNorth"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""18c3b8d0-394a-4a19-a8d2-87d60dc7c9b0"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player1;Player2"",
                    ""action"": ""TriggerRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fbb1b386-fe9c-4051-9cc2-9513b08996eb"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player1;Player2"",
                    ""action"": ""TriggerLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0b1d852f-a031-4014-8bed-cde8ea06b6f2"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player1"",
                    ""action"": ""TriggerLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0cdf43a3-f21c-4e43-afd5-0687460e5cc4"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player1;Player2"",
                    ""action"": ""ShoulderRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c2201500-08ef-4ee7-b75d-f34d13baa2c7"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player1"",
                    ""action"": ""ShoulderRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7d0ef9a5-da50-495f-b9f7-16a9e16e3101"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player1;Player2"",
                    ""action"": ""ShoulderLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""48d33a42-63ae-4eac-b2ca-be04f19e1206"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player1"",
                    ""action"": ""ShoulderLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0b8f730e-c296-49a4-9a8a-e1f9116671d9"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player1;Player2"",
                    ""action"": ""SpecialRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""684a9dc6-90e3-4372-a7e7-f0697ba1afcb"",
                    ""path"": ""<Keyboard>/enter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player1"",
                    ""action"": ""SpecialRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8fc15af4-e730-4b5f-b5ef-91077a819460"",
                    ""path"": ""<Gamepad>/select"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player1;Player2"",
                    ""action"": ""SpecialLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""53e5d330-7674-4517-9e0b-febfd1e0ddea"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player1"",
                    ""action"": ""SpecialLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c1c6a235-7cf4-4545-812a-3437b9397af0"",
                    ""path"": ""<Gamepad>/dpad/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player1;Player2"",
                    ""action"": ""North"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b28733a4-2203-4c34-9843-315bfeb05c8a"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player1"",
                    ""action"": ""North"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2bc6bf3f-a42a-4a24-9dfc-30e5f6433959"",
                    ""path"": ""<Gamepad>/dpad/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player1;Player2"",
                    ""action"": ""South"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ac5fadae-5eb3-4960-8f27-6d2f81c3c150"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player1"",
                    ""action"": ""South"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2a2eaa92-18e7-463b-b0dc-a8f0c6009404"",
                    ""path"": ""<Gamepad>/dpad/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player1;Player2"",
                    ""action"": ""East"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""63faa427-7614-4160-b564-b6c841560904"",
                    ""path"": ""<Keyboard>/3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player1"",
                    ""action"": ""East"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f0c9f11a-8afa-4003-87fa-fbf087679f0a"",
                    ""path"": ""<Gamepad>/dpad/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player1;Player2"",
                    ""action"": ""West"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1048c179-662e-4865-9e65-5c3f8ff2e9ac"",
                    ""path"": ""<Keyboard>/4"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player1"",
                    ""action"": ""West"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""UI"",
            ""id"": ""8dd38899-b4c1-4407-8519-6593f3b464c5"",
            ""actions"": [
                {
                    ""name"": ""Submit"",
                    ""type"": ""Button"",
                    ""id"": ""1715e135-e387-44b6-9b89-aeb01e4a217d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Cancel"",
                    ""type"": ""Button"",
                    ""id"": ""717b45e8-38f4-4e37-b246-5664763548a8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Point"",
                    ""type"": ""PassThrough"",
                    ""id"": ""f69d0d7a-0ef2-448c-8b90-aa5f76eae06a"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""LeftClick"",
                    ""type"": ""PassThrough"",
                    ""id"": ""1b22aca5-bed0-412a-ad91-1c267bb2f343"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MiddleClick"",
                    ""type"": ""PassThrough"",
                    ""id"": ""9453688a-40bf-44e0-9bab-ddf179359c26"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""RightClick"",
                    ""type"": ""PassThrough"",
                    ""id"": ""4eef96c0-2107-43da-a803-1bb0d388658b"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ScrollWheel"",
                    ""type"": ""PassThrough"",
                    ""id"": ""99ab8b0b-172d-406f-9429-6faca101e4e8"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""f97d1a07-cbc7-4c4a-aad9-98b0fb717375"",
                    ""path"": ""*/{Submit}"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player1"",
                    ""action"": ""Submit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cabc96be-22d8-46c7-9618-58d36c4edf41"",
                    ""path"": ""*/{Cancel}"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Player1"",
                    ""action"": ""Cancel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""10a30e19-2018-4834-9eff-cd7a4f7d33b6"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse;Player1"",
                    ""action"": ""Point"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6ae5c6cf-171f-4c6f-ae41-fa1f639bceea"",
                    ""path"": ""<Pen>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse;Player1"",
                    ""action"": ""Point"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7046e6ea-d2a4-46d6-9c90-ab6f7815631e"",
                    ""path"": ""<Touchscreen>/touch*/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Touch;Player1"",
                    ""action"": ""Point"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""24817946-4d0e-4899-8092-b59bfcf2c932"",
                    ""path"": ""<Mouse>/middleButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse;Player1"",
                    ""action"": ""MiddleClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fad8267d-1547-4658-b06a-07dc6f0c3743"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse;Player1"",
                    ""action"": ""RightClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""dfd0e89b-34d6-4444-b7cb-5b910f0b089b"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse;Player1"",
                    ""action"": ""LeftClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ff429762-0db3-456f-986f-ad507bf122a2"",
                    ""path"": ""<Pen>/tip"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse;Player1"",
                    ""action"": ""LeftClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1745301e-b09c-49b8-9deb-e809dc1825d4"",
                    ""path"": ""<Touchscreen>/touch*/press"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Touch;Player1"",
                    ""action"": ""LeftClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""99a02aad-53ed-47ee-8fb0-e989c93cca2c"",
                    ""path"": ""<Mouse>/scroll"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse;Player1"",
                    ""action"": ""ScrollWheel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Player1"",
            ""bindingGroup"": ""Player1"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": true,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Player2"",
            ""bindingGroup"": ""Player2"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": true,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
            // Avatar
            m_Avatar = asset.FindActionMap("Avatar", throwIfNotFound: true);
            m_Avatar_Move = m_Avatar.FindAction("Move", throwIfNotFound: true);
            m_Avatar_Look = m_Avatar.FindAction("Look", throwIfNotFound: true);
            m_Avatar_ButtonSouth = m_Avatar.FindAction("ButtonSouth", throwIfNotFound: true);
            m_Avatar_ButtonEast = m_Avatar.FindAction("ButtonEast", throwIfNotFound: true);
            m_Avatar_ButtonWest = m_Avatar.FindAction("ButtonWest", throwIfNotFound: true);
            m_Avatar_ButtonNorth = m_Avatar.FindAction("ButtonNorth", throwIfNotFound: true);
            m_Avatar_TriggerRight = m_Avatar.FindAction("TriggerRight", throwIfNotFound: true);
            m_Avatar_TriggerLeft = m_Avatar.FindAction("TriggerLeft", throwIfNotFound: true);
            m_Avatar_ShoulderRight = m_Avatar.FindAction("ShoulderRight", throwIfNotFound: true);
            m_Avatar_ShoulderLeft = m_Avatar.FindAction("ShoulderLeft", throwIfNotFound: true);
            m_Avatar_SpecialRight = m_Avatar.FindAction("SpecialRight", throwIfNotFound: true);
            m_Avatar_SpecialLeft = m_Avatar.FindAction("SpecialLeft", throwIfNotFound: true);
            m_Avatar_North = m_Avatar.FindAction("North", throwIfNotFound: true);
            m_Avatar_South = m_Avatar.FindAction("South", throwIfNotFound: true);
            m_Avatar_East = m_Avatar.FindAction("East", throwIfNotFound: true);
            m_Avatar_West = m_Avatar.FindAction("West", throwIfNotFound: true);
            // UI
            m_UI = asset.FindActionMap("UI", throwIfNotFound: true);
            m_UI_Submit = m_UI.FindAction("Submit", throwIfNotFound: true);
            m_UI_Cancel = m_UI.FindAction("Cancel", throwIfNotFound: true);
            m_UI_Point = m_UI.FindAction("Point", throwIfNotFound: true);
            m_UI_LeftClick = m_UI.FindAction("LeftClick", throwIfNotFound: true);
            m_UI_MiddleClick = m_UI.FindAction("MiddleClick", throwIfNotFound: true);
            m_UI_RightClick = m_UI.FindAction("RightClick", throwIfNotFound: true);
            m_UI_ScrollWheel = m_UI.FindAction("ScrollWheel", throwIfNotFound: true);
        }

        public void Dispose()
        {
            UnityEngine.Object.Destroy(asset);
        }

        public InputBinding? bindingMask
        {
            get => asset.bindingMask;
            set => asset.bindingMask = value;
        }

        public ReadOnlyArray<InputDevice>? devices
        {
            get => asset.devices;
            set => asset.devices = value;
        }

        public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

        public bool Contains(InputAction action)
        {
            return asset.Contains(action);
        }

        public IEnumerator<InputAction> GetEnumerator()
        {
            return asset.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Enable()
        {
            asset.Enable();
        }

        public void Disable()
        {
            asset.Disable();
        }

        // Avatar
        private readonly InputActionMap m_Avatar;
        private IAvatarActions m_AvatarActionsCallbackInterface;
        private readonly InputAction m_Avatar_Move;
        private readonly InputAction m_Avatar_Look;
        private readonly InputAction m_Avatar_ButtonSouth;
        private readonly InputAction m_Avatar_ButtonEast;
        private readonly InputAction m_Avatar_ButtonWest;
        private readonly InputAction m_Avatar_ButtonNorth;
        private readonly InputAction m_Avatar_TriggerRight;
        private readonly InputAction m_Avatar_TriggerLeft;
        private readonly InputAction m_Avatar_ShoulderRight;
        private readonly InputAction m_Avatar_ShoulderLeft;
        private readonly InputAction m_Avatar_SpecialRight;
        private readonly InputAction m_Avatar_SpecialLeft;
        private readonly InputAction m_Avatar_North;
        private readonly InputAction m_Avatar_South;
        private readonly InputAction m_Avatar_East;
        private readonly InputAction m_Avatar_West;
        public struct AvatarActions
        {
            private @AvatarControls m_Wrapper;
            public AvatarActions(@AvatarControls wrapper) { m_Wrapper = wrapper; }
            public InputAction @Move => m_Wrapper.m_Avatar_Move;
            public InputAction @Look => m_Wrapper.m_Avatar_Look;
            public InputAction @ButtonSouth => m_Wrapper.m_Avatar_ButtonSouth;
            public InputAction @ButtonEast => m_Wrapper.m_Avatar_ButtonEast;
            public InputAction @ButtonWest => m_Wrapper.m_Avatar_ButtonWest;
            public InputAction @ButtonNorth => m_Wrapper.m_Avatar_ButtonNorth;
            public InputAction @TriggerRight => m_Wrapper.m_Avatar_TriggerRight;
            public InputAction @TriggerLeft => m_Wrapper.m_Avatar_TriggerLeft;
            public InputAction @ShoulderRight => m_Wrapper.m_Avatar_ShoulderRight;
            public InputAction @ShoulderLeft => m_Wrapper.m_Avatar_ShoulderLeft;
            public InputAction @SpecialRight => m_Wrapper.m_Avatar_SpecialRight;
            public InputAction @SpecialLeft => m_Wrapper.m_Avatar_SpecialLeft;
            public InputAction @North => m_Wrapper.m_Avatar_North;
            public InputAction @South => m_Wrapper.m_Avatar_South;
            public InputAction @East => m_Wrapper.m_Avatar_East;
            public InputAction @West => m_Wrapper.m_Avatar_West;
            public InputActionMap Get() { return m_Wrapper.m_Avatar; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(AvatarActions set) { return set.Get(); }
            public void SetCallbacks(IAvatarActions instance)
            {
                if (m_Wrapper.m_AvatarActionsCallbackInterface != null)
                {
                    @Move.started -= m_Wrapper.m_AvatarActionsCallbackInterface.OnMove;
                    @Move.performed -= m_Wrapper.m_AvatarActionsCallbackInterface.OnMove;
                    @Move.canceled -= m_Wrapper.m_AvatarActionsCallbackInterface.OnMove;
                    @Look.started -= m_Wrapper.m_AvatarActionsCallbackInterface.OnLook;
                    @Look.performed -= m_Wrapper.m_AvatarActionsCallbackInterface.OnLook;
                    @Look.canceled -= m_Wrapper.m_AvatarActionsCallbackInterface.OnLook;
                    @ButtonSouth.started -= m_Wrapper.m_AvatarActionsCallbackInterface.OnButtonSouth;
                    @ButtonSouth.performed -= m_Wrapper.m_AvatarActionsCallbackInterface.OnButtonSouth;
                    @ButtonSouth.canceled -= m_Wrapper.m_AvatarActionsCallbackInterface.OnButtonSouth;
                    @ButtonEast.started -= m_Wrapper.m_AvatarActionsCallbackInterface.OnButtonEast;
                    @ButtonEast.performed -= m_Wrapper.m_AvatarActionsCallbackInterface.OnButtonEast;
                    @ButtonEast.canceled -= m_Wrapper.m_AvatarActionsCallbackInterface.OnButtonEast;
                    @ButtonWest.started -= m_Wrapper.m_AvatarActionsCallbackInterface.OnButtonWest;
                    @ButtonWest.performed -= m_Wrapper.m_AvatarActionsCallbackInterface.OnButtonWest;
                    @ButtonWest.canceled -= m_Wrapper.m_AvatarActionsCallbackInterface.OnButtonWest;
                    @ButtonNorth.started -= m_Wrapper.m_AvatarActionsCallbackInterface.OnButtonNorth;
                    @ButtonNorth.performed -= m_Wrapper.m_AvatarActionsCallbackInterface.OnButtonNorth;
                    @ButtonNorth.canceled -= m_Wrapper.m_AvatarActionsCallbackInterface.OnButtonNorth;
                    @TriggerRight.started -= m_Wrapper.m_AvatarActionsCallbackInterface.OnTriggerRight;
                    @TriggerRight.performed -= m_Wrapper.m_AvatarActionsCallbackInterface.OnTriggerRight;
                    @TriggerRight.canceled -= m_Wrapper.m_AvatarActionsCallbackInterface.OnTriggerRight;
                    @TriggerLeft.started -= m_Wrapper.m_AvatarActionsCallbackInterface.OnTriggerLeft;
                    @TriggerLeft.performed -= m_Wrapper.m_AvatarActionsCallbackInterface.OnTriggerLeft;
                    @TriggerLeft.canceled -= m_Wrapper.m_AvatarActionsCallbackInterface.OnTriggerLeft;
                    @ShoulderRight.started -= m_Wrapper.m_AvatarActionsCallbackInterface.OnShoulderRight;
                    @ShoulderRight.performed -= m_Wrapper.m_AvatarActionsCallbackInterface.OnShoulderRight;
                    @ShoulderRight.canceled -= m_Wrapper.m_AvatarActionsCallbackInterface.OnShoulderRight;
                    @ShoulderLeft.started -= m_Wrapper.m_AvatarActionsCallbackInterface.OnShoulderLeft;
                    @ShoulderLeft.performed -= m_Wrapper.m_AvatarActionsCallbackInterface.OnShoulderLeft;
                    @ShoulderLeft.canceled -= m_Wrapper.m_AvatarActionsCallbackInterface.OnShoulderLeft;
                    @SpecialRight.started -= m_Wrapper.m_AvatarActionsCallbackInterface.OnSpecialRight;
                    @SpecialRight.performed -= m_Wrapper.m_AvatarActionsCallbackInterface.OnSpecialRight;
                    @SpecialRight.canceled -= m_Wrapper.m_AvatarActionsCallbackInterface.OnSpecialRight;
                    @SpecialLeft.started -= m_Wrapper.m_AvatarActionsCallbackInterface.OnSpecialLeft;
                    @SpecialLeft.performed -= m_Wrapper.m_AvatarActionsCallbackInterface.OnSpecialLeft;
                    @SpecialLeft.canceled -= m_Wrapper.m_AvatarActionsCallbackInterface.OnSpecialLeft;
                    @North.started -= m_Wrapper.m_AvatarActionsCallbackInterface.OnNorth;
                    @North.performed -= m_Wrapper.m_AvatarActionsCallbackInterface.OnNorth;
                    @North.canceled -= m_Wrapper.m_AvatarActionsCallbackInterface.OnNorth;
                    @South.started -= m_Wrapper.m_AvatarActionsCallbackInterface.OnSouth;
                    @South.performed -= m_Wrapper.m_AvatarActionsCallbackInterface.OnSouth;
                    @South.canceled -= m_Wrapper.m_AvatarActionsCallbackInterface.OnSouth;
                    @East.started -= m_Wrapper.m_AvatarActionsCallbackInterface.OnEast;
                    @East.performed -= m_Wrapper.m_AvatarActionsCallbackInterface.OnEast;
                    @East.canceled -= m_Wrapper.m_AvatarActionsCallbackInterface.OnEast;
                    @West.started -= m_Wrapper.m_AvatarActionsCallbackInterface.OnWest;
                    @West.performed -= m_Wrapper.m_AvatarActionsCallbackInterface.OnWest;
                    @West.canceled -= m_Wrapper.m_AvatarActionsCallbackInterface.OnWest;
                }
                m_Wrapper.m_AvatarActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @Move.started += instance.OnMove;
                    @Move.performed += instance.OnMove;
                    @Move.canceled += instance.OnMove;
                    @Look.started += instance.OnLook;
                    @Look.performed += instance.OnLook;
                    @Look.canceled += instance.OnLook;
                    @ButtonSouth.started += instance.OnButtonSouth;
                    @ButtonSouth.performed += instance.OnButtonSouth;
                    @ButtonSouth.canceled += instance.OnButtonSouth;
                    @ButtonEast.started += instance.OnButtonEast;
                    @ButtonEast.performed += instance.OnButtonEast;
                    @ButtonEast.canceled += instance.OnButtonEast;
                    @ButtonWest.started += instance.OnButtonWest;
                    @ButtonWest.performed += instance.OnButtonWest;
                    @ButtonWest.canceled += instance.OnButtonWest;
                    @ButtonNorth.started += instance.OnButtonNorth;
                    @ButtonNorth.performed += instance.OnButtonNorth;
                    @ButtonNorth.canceled += instance.OnButtonNorth;
                    @TriggerRight.started += instance.OnTriggerRight;
                    @TriggerRight.performed += instance.OnTriggerRight;
                    @TriggerRight.canceled += instance.OnTriggerRight;
                    @TriggerLeft.started += instance.OnTriggerLeft;
                    @TriggerLeft.performed += instance.OnTriggerLeft;
                    @TriggerLeft.canceled += instance.OnTriggerLeft;
                    @ShoulderRight.started += instance.OnShoulderRight;
                    @ShoulderRight.performed += instance.OnShoulderRight;
                    @ShoulderRight.canceled += instance.OnShoulderRight;
                    @ShoulderLeft.started += instance.OnShoulderLeft;
                    @ShoulderLeft.performed += instance.OnShoulderLeft;
                    @ShoulderLeft.canceled += instance.OnShoulderLeft;
                    @SpecialRight.started += instance.OnSpecialRight;
                    @SpecialRight.performed += instance.OnSpecialRight;
                    @SpecialRight.canceled += instance.OnSpecialRight;
                    @SpecialLeft.started += instance.OnSpecialLeft;
                    @SpecialLeft.performed += instance.OnSpecialLeft;
                    @SpecialLeft.canceled += instance.OnSpecialLeft;
                    @North.started += instance.OnNorth;
                    @North.performed += instance.OnNorth;
                    @North.canceled += instance.OnNorth;
                    @South.started += instance.OnSouth;
                    @South.performed += instance.OnSouth;
                    @South.canceled += instance.OnSouth;
                    @East.started += instance.OnEast;
                    @East.performed += instance.OnEast;
                    @East.canceled += instance.OnEast;
                    @West.started += instance.OnWest;
                    @West.performed += instance.OnWest;
                    @West.canceled += instance.OnWest;
                }
            }
        }
        public AvatarActions @Avatar => new AvatarActions(this);

        // UI
        private readonly InputActionMap m_UI;
        private IUIActions m_UIActionsCallbackInterface;
        private readonly InputAction m_UI_Submit;
        private readonly InputAction m_UI_Cancel;
        private readonly InputAction m_UI_Point;
        private readonly InputAction m_UI_LeftClick;
        private readonly InputAction m_UI_MiddleClick;
        private readonly InputAction m_UI_RightClick;
        private readonly InputAction m_UI_ScrollWheel;
        public struct UIActions
        {
            private @AvatarControls m_Wrapper;
            public UIActions(@AvatarControls wrapper) { m_Wrapper = wrapper; }
            public InputAction @Submit => m_Wrapper.m_UI_Submit;
            public InputAction @Cancel => m_Wrapper.m_UI_Cancel;
            public InputAction @Point => m_Wrapper.m_UI_Point;
            public InputAction @LeftClick => m_Wrapper.m_UI_LeftClick;
            public InputAction @MiddleClick => m_Wrapper.m_UI_MiddleClick;
            public InputAction @RightClick => m_Wrapper.m_UI_RightClick;
            public InputAction @ScrollWheel => m_Wrapper.m_UI_ScrollWheel;
            public InputActionMap Get() { return m_Wrapper.m_UI; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(UIActions set) { return set.Get(); }
            public void SetCallbacks(IUIActions instance)
            {
                if (m_Wrapper.m_UIActionsCallbackInterface != null)
                {
                    @Submit.started -= m_Wrapper.m_UIActionsCallbackInterface.OnSubmit;
                    @Submit.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnSubmit;
                    @Submit.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnSubmit;
                    @Cancel.started -= m_Wrapper.m_UIActionsCallbackInterface.OnCancel;
                    @Cancel.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnCancel;
                    @Cancel.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnCancel;
                    @Point.started -= m_Wrapper.m_UIActionsCallbackInterface.OnPoint;
                    @Point.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnPoint;
                    @Point.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnPoint;
                    @LeftClick.started -= m_Wrapper.m_UIActionsCallbackInterface.OnLeftClick;
                    @LeftClick.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnLeftClick;
                    @LeftClick.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnLeftClick;
                    @MiddleClick.started -= m_Wrapper.m_UIActionsCallbackInterface.OnMiddleClick;
                    @MiddleClick.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnMiddleClick;
                    @MiddleClick.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnMiddleClick;
                    @RightClick.started -= m_Wrapper.m_UIActionsCallbackInterface.OnRightClick;
                    @RightClick.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnRightClick;
                    @RightClick.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnRightClick;
                    @ScrollWheel.started -= m_Wrapper.m_UIActionsCallbackInterface.OnScrollWheel;
                    @ScrollWheel.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnScrollWheel;
                    @ScrollWheel.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnScrollWheel;
                }
                m_Wrapper.m_UIActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @Submit.started += instance.OnSubmit;
                    @Submit.performed += instance.OnSubmit;
                    @Submit.canceled += instance.OnSubmit;
                    @Cancel.started += instance.OnCancel;
                    @Cancel.performed += instance.OnCancel;
                    @Cancel.canceled += instance.OnCancel;
                    @Point.started += instance.OnPoint;
                    @Point.performed += instance.OnPoint;
                    @Point.canceled += instance.OnPoint;
                    @LeftClick.started += instance.OnLeftClick;
                    @LeftClick.performed += instance.OnLeftClick;
                    @LeftClick.canceled += instance.OnLeftClick;
                    @MiddleClick.started += instance.OnMiddleClick;
                    @MiddleClick.performed += instance.OnMiddleClick;
                    @MiddleClick.canceled += instance.OnMiddleClick;
                    @RightClick.started += instance.OnRightClick;
                    @RightClick.performed += instance.OnRightClick;
                    @RightClick.canceled += instance.OnRightClick;
                    @ScrollWheel.started += instance.OnScrollWheel;
                    @ScrollWheel.performed += instance.OnScrollWheel;
                    @ScrollWheel.canceled += instance.OnScrollWheel;
                }
            }
        }
        public UIActions @UI => new UIActions(this);
        private int m_Player1SchemeIndex = -1;
        public InputControlScheme Player1Scheme
        {
            get
            {
                if (m_Player1SchemeIndex == -1) m_Player1SchemeIndex = asset.FindControlSchemeIndex("Player1");
                return asset.controlSchemes[m_Player1SchemeIndex];
            }
        }
        private int m_Player2SchemeIndex = -1;
        public InputControlScheme Player2Scheme
        {
            get
            {
                if (m_Player2SchemeIndex == -1) m_Player2SchemeIndex = asset.FindControlSchemeIndex("Player2");
                return asset.controlSchemes[m_Player2SchemeIndex];
            }
        }
        public interface IAvatarActions
        {
            void OnMove(InputAction.CallbackContext context);
            void OnLook(InputAction.CallbackContext context);
            void OnButtonSouth(InputAction.CallbackContext context);
            void OnButtonEast(InputAction.CallbackContext context);
            void OnButtonWest(InputAction.CallbackContext context);
            void OnButtonNorth(InputAction.CallbackContext context);
            void OnTriggerRight(InputAction.CallbackContext context);
            void OnTriggerLeft(InputAction.CallbackContext context);
            void OnShoulderRight(InputAction.CallbackContext context);
            void OnShoulderLeft(InputAction.CallbackContext context);
            void OnSpecialRight(InputAction.CallbackContext context);
            void OnSpecialLeft(InputAction.CallbackContext context);
            void OnNorth(InputAction.CallbackContext context);
            void OnSouth(InputAction.CallbackContext context);
            void OnEast(InputAction.CallbackContext context);
            void OnWest(InputAction.CallbackContext context);
        }
        public interface IUIActions
        {
            void OnSubmit(InputAction.CallbackContext context);
            void OnCancel(InputAction.CallbackContext context);
            void OnPoint(InputAction.CallbackContext context);
            void OnLeftClick(InputAction.CallbackContext context);
            void OnMiddleClick(InputAction.CallbackContext context);
            void OnRightClick(InputAction.CallbackContext context);
            void OnScrollWheel(InputAction.CallbackContext context);
        }
    }
}
