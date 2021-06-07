namespace RedOwl.Engine
{
    public class SceneCollection : BetterKeyedCollection<string, SceneMetadata>
    {
        public SceneCollection(int capacity) : base(capacity) {}

        protected override string GetKeyForItem(SceneMetadata item) => item.name;
    }
}