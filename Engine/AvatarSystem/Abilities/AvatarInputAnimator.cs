
/*
namespace RedOwl.Engine
{
    public class AvatarInputAnimator : AvatarAbility
    {
        public override int Priority { get; } = 10000000;
        
        public string moveX = "MoveX";
        public string moveY = "MoveY";
        public string lookX = "LookX";
        public string lookY = "LookY";

        public string north = "North";
        public string south = "South";
        public string east = "East";
        public string west = "West";
        
        public string buttonSouth = "A";
        public string buttonEast = "B";
        public string buttonWest = "X";
        public string buttonNorth = "Y";
        
        public string triggerRight = "Fire";
        public string triggerLeft = "Break";
        
        public string shoulderRight = "Right";
        public string shoulderLeft = "Left";
        
        public string specialRight = "Start";
        public string specialLeft = "Select";

        private int _moveX;
        private int _moveY;
        private int _lookX;
        private int _lookY;

        private int _north;
        private int _northTrigger;
        private int _south;
        private int _southTrigger;
        private int _east;
        private int _eastTrigger;
        private int _west;
        private int _westTrigger;

        private int _buttonSouth;
        private int _buttonSouthTrigger;
        private int _buttonEast;
        private int _buttonEastTrigger;
        private int _buttonWest;
        private int _buttonWestTrigger;
        private int _buttonNorth;
        private int _buttonNorthTrigger;
        private int _triggerRight;
        private int _triggerRightTrigger;
        private int _triggerLeft;
        private int _triggerLeftTrigger;
        private int _shoulderRight;
        private int _shoulderRightTrigger;
        private int _shoulderLeft;
        private int _shoulderLeftTrigger;
        private int _specialRight;
        private int _specialRightTrigger;
        private int _specialLeft;
        private int _specialLeftTrigger;
        
        private int _velocityX;
        private int _velocityY;
        private int _grounded;
        private int _anyGround;

        private AnimatorManager _manager;
        
        private void RegisterButton(string name, out int parameter, out int parameterTrigger)
        {
            _manager.RegisterBool(name, out parameter);
            _manager.RegisterTrigger(name, out parameterTrigger);
        }
        
        private void SetButtonParameter(ButtonStates state, int parameter, int parameterTrigger)
        {
            if (state == ButtonStates.Pressed)
            {
                _manager.SetTrigger(parameterTrigger);
            }
            else
            {
                _manager.SetBool(parameter, state == ButtonStates.Held);
            }
        }

        public override void OnStart()
        {
            if (Avatar.animator == null)
            {
                Unlocked = false;
                return;
            }
            _manager = new AnimatorManager(Avatar.animator);

            _manager.RegisterFloat(moveX, out _moveX);
            _manager.RegisterFloat(moveY, out _moveY);
            _manager.RegisterFloat(lookX, out _lookX);
            _manager.RegisterFloat(lookY, out _lookY);
            
            RegisterButton(north, out _north, out _northTrigger);
            RegisterButton(south, out _south, out _southTrigger);
            RegisterButton(east, out _east, out _eastTrigger);
            RegisterButton(west, out _west, out _westTrigger);
            
            RegisterButton(buttonSouth, out _buttonSouth, out _buttonSouthTrigger);
            RegisterButton(buttonEast, out _buttonEast, out _buttonEastTrigger);
            RegisterButton(buttonWest, out _buttonWest, out _buttonWestTrigger);
            RegisterButton(buttonNorth, out _buttonNorth, out _buttonNorthTrigger);
            
            RegisterButton(triggerRight, out _triggerRight, out _triggerRightTrigger);
            RegisterButton(triggerLeft, out _triggerLeft, out _triggerLeftTrigger);
            
            RegisterButton(shoulderRight, out _shoulderRight, out _shoulderRightTrigger);
            RegisterButton(shoulderLeft, out _shoulderLeft, out _shoulderLeftTrigger);
            
            RegisterButton(specialRight, out _specialRight, out _specialRightTrigger);
            RegisterButton(specialLeft, out _specialLeft, out _specialLeftTrigger);
        }

        public override void HandleInput(ref AvatarInput input)
        {
            _manager.SetFloat(_moveX, input.Move.x);
            _manager.SetFloat(_moveY, input.Move.y);
            _manager.SetFloat(_lookX, input.Look.x);
            _manager.SetFloat(_lookY, input.Look.y);
            
            SetButtonParameter(input.North, _north, _northTrigger);
            SetButtonParameter(input.South, _south, _southTrigger);
            SetButtonParameter(input.East, _east, _eastTrigger);
            SetButtonParameter(input.West, _west, _westTrigger);
            
            SetButtonParameter(input.ButtonSouth, _buttonSouth, _buttonSouthTrigger);
            SetButtonParameter(input.ButtonEast, _buttonEast, _buttonEastTrigger);
            SetButtonParameter(input.ButtonWest, _buttonWest, _buttonWestTrigger);
            SetButtonParameter(input.ButtonNorth, _buttonNorth, _buttonNorthTrigger);
            
            SetButtonParameter(input.TriggerRight, _triggerRight, _triggerRightTrigger);
            SetButtonParameter(input.TriggerLeft, _triggerLeft, _triggerLeftTrigger);
            
            SetButtonParameter(input.ShoulderRight, _shoulderRight, _shoulderRightTrigger);
            SetButtonParameter(input.ShoulderLeft, _shoulderLeft, _shoulderLeftTrigger);
            
            SetButtonParameter(input.SpecialRight, _specialRight, _specialRightTrigger);
            SetButtonParameter(input.SpecialLeft, _specialLeft, _specialLeftTrigger);
        }
    }
}
*/