using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEngine;

namespace RedOwl.Core.Editor
{
    public class FlexibleLayoutProcessor : OdinPropertyProcessor<FlexibleLayout>
    {
        public override void ProcessMemberProperties(List<InspectorPropertyInfo> propertyInfos)
        {
            for (int i = 0; i < propertyInfos.Count; i++)
            {
                if (propertyInfos[i].PropertyName == "m_Padding")
                {
                    propertyInfos[i].GetEditableAttributesList().AddRange(new Attribute[]
                        {new InlinePropertyAttribute(), new HideLabelAttribute(), new BoxGroupAttribute("Padding")});
                    propertyInfos.Add(propertyInfos[i]);
                    propertyInfos.RemoveAt(i);
                }
            }
            propertyInfos.Remove("m_ChildAlignment");
        }
    }
}