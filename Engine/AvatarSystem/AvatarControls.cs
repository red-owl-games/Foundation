// GENERATED AUTOMATICALLY FROM 'Packages/com.redowl.foundation/Resources/AvatarControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace RedOwl.Core
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
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Look"",
                    ""type"": ""Value"",
                    ""id"": ""efc3eda3-75ab-446c-be89-363d2c621c85"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
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
                    ""groups"": "";Gamepad"",
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
                    ""groups"": "";Keyboard&Mouse"",
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
                    ""groups"": "";Keyboard&Mouse"",
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
                    ""groups"": "";Keyboard&Mouse"",
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
                    ""groups"": "";Keyboard&Mouse"",
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
                    ""groups"": "";Keyboard&Mouse"",
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
                    ""groups"": "";Keyboard&Mouse"",
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
                    ""groups"": "";Keyboard&Mouse"",
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
                    ""groups"": "";Keyboard&Mouse"",
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
                    ""groups"": "";Gamepad"",
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
                    ""groups"": ""Keyboard&Mouse"",
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
                    ""groups"": "";Touch"",
                    ""action"": ""ButtonSouth"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""90deca70-cc3a-4113-afa7-cfd2f821a8fc"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""ButtonEast"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""97804e13-3f07-40dd-ad5d-ea838fb1199e"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""ButtonEast"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b9fb4cd3-d55a-4e9e-8b69-d4ff3be98904"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8d7cc1f2-3110-47d6-9330-b68bff610a26"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": ""NormalizeVector2"",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a03bbaa1-499a-45be-8606-9c886cc67793"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
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
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""ButtonWest"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""774f46f2-b9e5-449e-a592-58be11d0ebde"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
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
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""ButtonNorth"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b7d55099-6c96-479c-ade6-7f940d3214b3"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""TriggerRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ae8d6883-a114-49cf-a1ed-32cb08d18346"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""TriggerRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a65c5d72-5149-4313-aea4-1bce7b19edc9"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
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
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""TriggerLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fcc44f62-50e1-4480-9da2-2d9f3601e5a7"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
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
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""ShoulderRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""773ebcb9-2baf-4760-bee8-c80f4c562703"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
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
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""ShoulderLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6af8756a-96c6-4df5-b03c-4c37362edcf9"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
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
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""SpecialRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""10ddff50-417b-4614-9c4c-c1063163a177"",
                    ""path"": ""<Gamepad>/select"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
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
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""SpecialLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""598c92da-cb6f-4f5a-8d8e-396e16f992ab"",
                    ""path"": ""<Gamepad>/dpad/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
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
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""North"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c5c7081f-7206-4fc8-8b7e-c91e7dfb887d"",
                    ""path"": ""<Gamepad>/dpad/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
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
                    ""groups"": """",
                    ""action"": ""South"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""37884f2e-f7f5-477f-bdd6-ec7359b886bb"",
                    ""path"": ""<Gamepad>/dpad/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
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
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""East"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""22bf7bd0-80bd-45f0-8caa-d83fd380e442"",
                    ""path"": ""<Gamepad>/dpad/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
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
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""West"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard&Mouse"",
            ""bindingGroup"": ""Keyboard&Mouse"",
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
                }
            ]
        },
        {
            ""name"": ""Gamepad"",
            ""bindingGroup"": ""Gamepad"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Touch"",
            ""bindingGroup"": ""Touch"",
            ""devices"": [
                {
                    ""devicePath"": ""<Touchscreen>"",
                    ""isOptional"": false,
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
        private int m_KeyboardMouseSchemeIndex = -1;
        public InputControlScheme KeyboardMouseScheme
        {
            get
            {
                if (m_KeyboardMouseSchemeIndex == -1) m_KeyboardMouseSchemeIndex = asset.FindControlSchemeIndex("Keyboard&Mouse");
                return asset.controlSchemes[m_KeyboardMouseSchemeIndex];
            }
        }
        private int m_GamepadSchemeIndex = -1;
        public InputControlScheme GamepadScheme
        {
            get
            {
                if (m_GamepadSchemeIndex == -1) m_GamepadSchemeIndex = asset.FindControlSchemeIndex("Gamepad");
                return asset.controlSchemes[m_GamepadSchemeIndex];
            }
        }
        private int m_TouchSchemeIndex = -1;
        public InputControlScheme TouchScheme
        {
            get
            {
                if (m_TouchSchemeIndex == -1) m_TouchSchemeIndex = asset.FindControlSchemeIndex("Touch");
                return asset.controlSchemes[m_TouchSchemeIndex];
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
    }
}
