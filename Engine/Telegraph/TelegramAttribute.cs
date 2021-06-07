using System;

namespace RedOwl.Engine
{
    [AttributeUsage(AttributeTargets.Property)]
    public class TelegramAttribute : Attribute
    {
        public string NameOverride;

        public TelegramAttribute(string nameOverride = "")
        {
            NameOverride = nameOverride;
        }
    }
}