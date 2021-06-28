using System;
using System.Runtime.CompilerServices;
using Sirenix.OdinInspector;
using UnityEngine;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;
using Attribute = System.Attribute;

namespace RedOwl.Engine
{
    [AttributeUsage(AttributeTargets.Struct)]
    public class ModelAttribute : Attribute
    {
        public string Title { get; private set; }
        public string Tag { get; private set; }
        
        public ModelAttribute(string title, string tag)
        {
            Title = title;
            Tag = tag;
        }
    }
    
    public abstract class Model<TData> : RedOwlScriptableObject, IYamlConvertible where TData : struct
    {
        // TODO: OnValidate - ID and Key are unique? How do we know which database?
        [SerializeField, HorizontalGroup("identification"), LabelWidth(25)]
        private int id;
        public int Id => id;
        
        [SerializeField, HorizontalGroup("identification"), LabelWidth(30), InlineButton("DoRename")]
        private string key;
        public string Key => key;

        [FoldoutGroup("Spec", Expanded = true), InlineProperty, HideLabel]
        public TData data;
        
        [Button(ButtonSizes.Large), HideInInlineEditors]
        private void CopyYamlToClipboard() => GUIUtility.systemCopyBuffer = AssetTools.Serializer.Serialize(this);

        private void DoRename() => AssetTools.Rename(this, $"{id}_{key}");
        
        #region YamlSerialization

        public void Read(IParser parser, Type expectedType, ObjectDeserializer nestedObjectDeserializer)
        {
            Log.Warn($"Unable to Deserialize Yaml for '{GetType().FullName}' you should probably be using '{typeof(TData).FullName}'");
            parser.SkipThisAndNestedEvents();
        }

        public void Write(IEmitter emitter, ObjectSerializer nestedObjectSerializer)
        {
            emitter.Emit(new MappingStart(null, null, true, MappingStyle.Block));
            
            emitter.Emit(new Scalar("kind")); 
            emitter.Emit(new Scalar(typeof(TData).Name));
            
            emitter.Emit(new Scalar("id")); 
            emitter.Emit(new Scalar($"{id}"));
            
            emitter.Emit(new Scalar("key")); 
            emitter.Emit(new Scalar(key));
            
            emitter.Emit(new Scalar("spec"));
            nestedObjectSerializer(data, typeof(TData));
            
            emitter.Emit(new MappingEnd());
        }
        
        #endregion
    }
    
    [Serializable, InlineProperty]
    public abstract class ModelReference<TDB, TModel, TData> : IYamlConvertible where TDB : Database<TModel, TData>, new() where TModel : Model<TData> where TData : struct
    {
        #region Overrides
        // This bit of code allows use to use Unity Inspector for overrides
        // but yaml deserialization uses _Overrides so we can flag usesOverrides on deserialization
        
        [ShowIf("@model != null")]
        [ToggleLeft, HorizontalGroup("model", 0.2f), LabelText("Override"), LabelWidth(74), OnValueChanged("_applyCleanOverride")]
        public bool usesOverrides;
        
        [ShowIf("usesOverrides")]
        [SerializeField, BoxGroup("Overrides"), HideLabel, InlineProperty]
        private TData overrides;

        #endregion
        
        #region Model
        
        [SerializeField, HideInInspector, HideLabel]
        private TModel model;
#if UNITY_EDITOR
        [ShowInInspector, HorizontalGroup("model", 0.5f), HideLabel, DisableIf("usesOverrides")]
        public TModel Model
        {
            get => model;
            set
            {
                model = value;
                _assign(model);
            }
        }
#endif
        
        private void _applyCleanOverride()
        {
            _assign(model);
        }

        private void _assign(TModel value)
        {
            if (value != null)
            {
                referenceId = value.Id;
                referenceKey = value.Key;
                overrides = value.data;            }
            else
            {
                referenceId = -1;
                referenceKey = "";
                usesOverrides = false;
                overrides = new TData();
            }
        }
        
        #endregion
        
        #region Value
        
        [ShowIf("@model != null")]
        [ReadOnly, HorizontalGroup("model"), HideLabel]
        public int referenceId = -1;
        
        public string referenceKey { get; set; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private TData GetFromDB() => referenceId == -1 ? Game.FindOrBind<TDB>().Get(referenceKey) : Game.FindOrBind<TDB>().Get(referenceId);
        
        public TData Value => usesOverrides ? overrides : GetFromDB();
        
        #endregion

        public static implicit operator TData(ModelReference<TDB, TModel, TData> self) => self.Value;

        #region YamlSerialization
        
        public void Read(IParser parser, Type expectedType, ObjectDeserializer nestedObjectDeserializer)
        {
            parser.Consume<MappingStart>();
            
            var fieldName = parser.Consume<Scalar>();
            if (fieldName.Value == "overrides")
            {
                usesOverrides = true;
                overrides = (TData)nestedObjectDeserializer(typeof(TData)); 
            }
            else
            {
                referenceKey = parser.Consume<Scalar>().Value;
            }
            parser.Consume<MappingEnd>();
        }

        public void Write(IEmitter emitter, ObjectSerializer nestedObjectSerializer)
        {
            emitter.Emit(new MappingStart(null, null, true, MappingStyle.Block));

            if (usesOverrides)
            {
                emitter.Emit(new Scalar("overrides"));
                nestedObjectSerializer(overrides, typeof(TData));
            }
            else
            {
                emitter.Emit(new Scalar("reference")); 
                emitter.Emit(new Scalar(model.Key));
            }
            
            emitter.Emit(new MappingEnd());
        }
        
        #endregion
    }
}