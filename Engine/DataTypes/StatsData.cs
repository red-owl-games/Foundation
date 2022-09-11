using System;
using System.Collections.Generic;
using System.Text;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;

namespace RedOwl.Engine
{
    public enum StatNames
    {
        Agility,
        Athletics,
        Armor,
        ArmorClass,
        ArmorPenetration,
        Attack,
        AttackSpeed,
        Awesome,
        Block,
        Charisma,
        Chi,
        Concentration,
        Constitution,
        CriticalStrike,
        DamageReduction,
        Defence,
        Dexterity,
        Dodge,
        Endurance,
        Energy,
        Focus,
        Fortitude,
        Fury,
        HealingPower,
        Health,
        HitChance,
        Hunger,
        Intellect,
        Intelligence,
        Knowledge,
        Luck,
        Magic,
        MagicDefence,
        Mana,
        Parry,
        Perception,
        Power,
        Rage,
        Resistance,
        Sanity,
        Size,
        Sleep,
        Smarts,
        Special,
        Speed,
        SpellCriticalStrike,
        SpellHitChance,
        SpellPenetration,
        SpellPower,
        Spirit,
        Stamina,
        Strength,
        Water,
        Wisdom,
        Willpower,
        Vitality,
    }
    
    public class StatBuilder
    {
        private StatCollection _collection;
        private bool _vital = false;
        private string _name = "";
        private double _base = 1;
        private double _current = -1;
        private string[] _expressions = Array.Empty<string>();

        public StatBuilder(StatCollection collection)
        {
            _collection = collection;
        }

        public StatBuilder Vital()
        {
            _vital = true;
            return this;
        }

        public StatBuilder Name(Enum name)
        {
            _name = name.ToString();
            return this;
        }
        
        public StatBuilder Name(string name)
        {
            _name = name;
            return this;
        }

        public StatBuilder Base(double value)
        {
            _base = value;
            return this;
        }
        
        public StatBuilder Current(double value)
        {
            _current = value;
            return this;
        }

        public StatBuilder Expressions(params string[] expressions)
        {
            _expressions = expressions;
            return this;
        }

        public StatCollection Done()
        {
            _collection.Add(new Stat(_collection, _vital, _name, _base, _current, _expressions));
            return _collection;
        }
    }

    [Serializable]
    public class Stat
    {
        [NonSerialized]
        internal StatCollection Collection;
        
        [HorizontalGroup("Settings")]
        [SerializeField, LabelWidth(60)] 
        private string name;
        public string Name => name;
        
        [HorizontalGroup("Settings",0.2f)]
        [SerializeField, ToggleLeft, LabelWidth(50)] 
        private bool vital;

        [SerializeField, LabelWidth(60)]
        private double baseline;
        
        [SerializeField, HideInInspector]
        private double current;
        
        public Stat(StatCollection collection, bool vital, string name, double baseline, double current, IEnumerable<string> expressions)
        {
            this.Collection = collection;
            this.vital = vital;
            this.name = name;
            this.baseline = baseline;
            this.current = current;
            this.expressions = new List<string>(expressions);
        }

        public event Action<double> OnCurrentValueChanged;

        [HorizontalGroup("Values")]
        [ShowInInspector, LabelWidth(60), ShowIf("vital")]
        public double Current {
            get => vital ? current < 0 ? Value : math.clamp(current, 0, Value) : Value;
            set
            {
                if (!vital) return;
                value = math.clamp(value, 0, Value);
                if (Math.Abs(current - value) < math.EPSILON_DBL) return;
                current = value;
                OnCurrentValueChanged?.Invoke(current);
            }
        }
        
        [HorizontalGroup("Values")]
        [ShowInInspector, LabelWidth(60)]
        public double Value
        {
            get => baseline + Mod;
            set => baseline = value - Mod;
        }
        
        [ShowInInspector]
        [ProgressBar(0, 1), LabelText("Current %"), LabelWidth(60), ShowIf("vital")]
        public double CurrentPercent
        {
            get => Current / Value;
            set => Current = value * Value;
        }
        
        [SerializeField, PropertyOrder(10)] private List<string> expressions;
        private double Mod
        {
            get
            {
                var interpreter = Collection.GetInterpreter();
                foreach (var expression in expressions)
                {
                    try
                    {
                        interpreter.Eval(expression);
                    }
                    catch (Exception e)
                    {
                        Log.Warn(e.ToString());
                    }
                }

                return interpreter.Eval("mod");
            }
        }

        public void Add(string expression) => expressions.Add(expression);

        public void Remove(string expression) => expressions.Remove(expression);
        
        [ButtonGroup("Buttons"), Button, PropertyOrder(20)]
        public void Increment() => baseline += 1;
        [ButtonGroup("Buttons"), Button, PropertyOrder(20)]
        public void Decrement() => baseline -= 1;

        [ButtonGroup("Buttons"), Button, ShowIf("vital"), PropertyOrder(20)]
        public void SetToMax() => Current = Value;

        [ButtonGroup("Buttons"), Button, ShowIf("vital"), PropertyOrder(20)]
        public void SetToMin() => Current = 0;

        public override string ToString()
        {
            return vital ? $"{Name}: {Current} / {Value}" : $"{Name}: {Value}";
        }
    }

    /// <summary>
    /// var stats = new StatCollection()
    ///     .New().Name("Level").Base(5).Done()
    ///     .New().Vital().Name(StatNames.Health).Base(10).Expressions("mod += stats.Level * 5").Done()
    ///     .New().Vital().Name("Mana").Base(10).Done()
    /// Log.Always($"{stats[StatNames.Health]}");
    /// FileController.Internal.Write(filepath, stats);
    /// </summary>
    [Serializable]
    public class StatCollection : BetterKeyedCollection<string, Stat>
    {
        public MathInterpreter GetInterpreter()
        {
            var output = new MathInterpreter();
            foreach (var stat in this)
            {
                output.AddFunction($"stats.{stat.Name}", () => stat.Value);
            }
            return output;
        }
        
        protected override string GetKeyForItem(Stat item) => item.Name;

        public StatBuilder New() => new(this);
        
        public Stat this[Enum key] => this[key.ToString()];

        public void Merge(StatCollection other)
        {
            // TODO: Overlay data from other to this one to merge saved data with hardcoded data
        }

        public override void OnAfterDeserialize()
        {
            base.OnAfterDeserialize();
            foreach (var stat in this)
            {
                stat.Collection = this;
            }
        }
        
        public override string ToString()
        {
            var builder = new StringBuilder("Stats:\n");
            foreach (var stat in this)
            {
                builder.Append($"\t{stat}\n");
            }
            return builder.ToString();
        }
    }
    
    [CreateAssetMenu(fileName = "Stats", menuName = "Red Owl/Stats Data")]
    public class StatsData : ScriptableObject
    {
        [HideLabel, InlineProperty, ListDrawerSettings(CustomAddFunction = "CustomAdd")]
        public StatCollection Stats;

        private void CustomAdd() => Stats.New().Done();

        [ButtonGroup("Buttons"), Button, PropertyOrder(-10)]
        private void Print() => Log.Always($"{Stats}");
    }
}