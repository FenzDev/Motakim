using System.Collections.Generic;

namespace Motakim
{
    internal class ComponentsGroup : List<IComponent>, IComparer<ComponentsGroup>
    {
        internal ComponentsGroup(int index, Entity entity)
        {
            Index = index;
            Entity = entity;
        }

        internal int Index;
        internal Entity Entity;

        public int Compare(ComponentsGroup x, ComponentsGroup y)
        {
            return x.Index.CompareTo(y);
        }
    }
}
