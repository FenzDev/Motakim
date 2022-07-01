using System;
using System.Collections.Generic;
using System.Linq;

namespace Motakim 
{
    public class Entity : ICloneable
    {
        internal List<Entity> ChildrenList = new List<Entity>(); 
        internal List<Component> ComponentsList = new List<Component>();
        internal HashSet<string> TagHashSet = new HashSet<string>();
        public bool Enabled { get; set; } = true;
        public string Name { get; set; }
        public Scene Scene { get; internal set; }
        public Entity Parent { get; internal set; }
        public List<Entity> Children => new List<Entity>(ChildrenList);
        public List<Entity> FirstChildren => ChildrenList.Where(child => child.Parent == this).ToList(); 
        public IReadOnlyList<Component> Components => ComponentsList.AsReadOnly();
        public IReadOnlyList<string> Tags;
        
        private void UpdateEntityRoot(Entity entity)
        {
            entity.ChildrenList = FindEntityChildren(entity).ToList();
            if (entity.Parent != null)
            {
                FindEntityChildren(entity.Parent);
            }
            // public List<Entity> Children => new List<Entity>(ChildrenList);
            // public List<Entity> ChildrenRoot => ChildrenList.Where(child => child.Parent == this).ToList(); 
        }
        internal IEnumerable<Entity> FindEntityChildren(Entity entity) => ChildrenList.Where(child => child.Parent == entity).SelectMany(child => FindEntityChildren(child).Prepend(child));
        internal void AddedToScene()
        {
            foreach (var component in ComponentsList)
            {
                component.OnEntityAdded();
            }
            foreach (var child in ChildrenList)
            {
                child.AddedToScene();
            }
        }
        internal void RemovedFromScene()
        {
            foreach (var component in ComponentsList)
            {
                component.OnEntityRemoved();
            }
            foreach (var child in ChildrenList)
            {
                child.RemovedFromScene();
            }
        }

        public void AddTags(params string[] tags)
        {
            foreach (var tag in tags)
            {
                AddTag(tag);
            }
        }
        public void AddTag(string tag)
        {
            if (TagHashSet.Contains(tag)) return;

            TagHashSet.Add(tag);
            
            if (Scene != null)
            { 
                AddTag(tag);
            }
        }
        public void RemoveTags(params string[] tags)
        {
            foreach (var tag in tags)
            {
                RemoveTag(tag);
            }
        }
        public void RemoveTag(string tag)
        {
            if (!TagHashSet.Contains(tag)) return;

            TagHashSet.Remove(tag);
            
            if (Scene != null)
            {
                RemoveTag(tag);
            }
        }
        public bool HasTag(string tag)
        {
            return TagHashSet.Contains(tag);
        }
        public bool HasTags(params string[] tags)
        {
            var hasTag = false;
            foreach (var tag in tags)
            {
                hasTag |= TagHashSet.Contains(tag);
            }
            return hasTag;
        }
        public TComponent AddComponent<TComponent>() where TComponent : Component, new() 
        {
            return AddComponent(new TComponent());
        }
        public TComponent AddComponent<TComponent>(TComponent component) where TComponent : Component
        {
            if (HasComponent<TComponent>()) return GetComponent<TComponent>();

            component.Entity = this;
            ComponentsList.Add(component);

            if (component is IUpdate && Scene != null)
            {
                var uGroup = Scene.UpdateComponentGroups.Find(group => group.Entity == this);
                if (uGroup == null)
                {
                    uGroup = new ComponentsGroup(Scene.EntitiesList.IndexOf(this), this);
                    Scene.UpdateComponentGroups.Add(uGroup);
                }
                uGroup.Add(component);
            }
            else if (component is IRender && Scene != null)
            {
                var rGroup = Scene.RenderComponentGroups.Find(group => group.Entity == this);
                if (rGroup == null)
                {
                    rGroup = new ComponentsGroup(Scene.EntitiesList.IndexOf(this), this);
                    Scene.RenderComponentGroups.Add(rGroup);
                }
                rGroup.Add(component);
            }

            component.OnAdded();
            return component;
        }
        public void RemoveComponent<TComponent>() where TComponent : Component
        {
            if (!HasComponent<TComponent>()) return;

            var index = ComponentsList.FindIndex(component => component is TComponent);
            var component = ComponentsList[index];

            var entity = component;
            entity.Entity = null;
            ComponentsList.RemoveAt(index);
            
            if (component is IUpdate && Scene != null)
            {
                var uGroup = Scene.UpdateComponentGroups.Find(group => group.Entity == this);
                if (uGroup != null)
                {
                    uGroup.Remove(component);
                }
            }
            else if (component is IRender && Scene != null)
            {
                var rGroup = Scene.RenderComponentGroups.Find(group => group.Entity == this);
                if (rGroup != null)
                {
                    rGroup.Remove(component);
                }
            }

            entity.OnRemoved();
        }
        public TComponent GetComponent<TComponent>() where TComponent : Component
        {
            return (TComponent)ComponentsList.Find(c => c is TComponent);
        }
        public bool TryGetComponent<TComponent>(out TComponent component) where TComponent : Component 
        {
            component = (TComponent)ComponentsList.Find(c => c is TComponent);
            return component != null;
        }
        public bool HasComponent<TComponent>() where TComponent : Component
        {
            return (TComponent)ComponentsList.Find(c => c is TComponent) != null;
        }
        public bool HasComponent<TComponent>(out TComponent compoent) where TComponent : Component
        {
            var _component = (TComponent)ComponentsList.Find(c => c is TComponent);
            compoent = _component;
            return _component != null;
        }
        public void Remove()
        {
            if (Scene == null) return;

            Scene.RemoveEntity(this);
        }
        public Entity CreateEntity(string name = "") 
        {
            var entity = new Entity() { Name = name };
            entity.AddComponent<Transform>();

            PutEntity(entity);
            return entity;
        }
        public void PutEntity(Entity entity)
        {
            entity.Scene = this.Scene;
            entity.Parent = this;
            foreach (var child in entity.Children)
            {
                entity.Scene = this.Scene;
            }

            if (Scene != null)
            {
                int idx;
                if (Scene.EntitiesList.Contains(entity))
                {
                    idx = Scene.EntitiesList.IndexOf(entity);
                    Scene.EntitiesList.RemoveAt(idx);    
                    Scene.EntitiesList.RemoveRange(idx, entity.ChildrenList.Count);    
                }
                
                idx = Scene.EntitiesList.IndexOf(this) + ChildrenList.Count + 1;

                Scene.EntitiesList.Insert(idx, entity);
                Scene.EntitiesList.InsertRange(idx + 1, entity.ChildrenList);

                Scene.UpdateEntityRoot(entity);
            }
            else
            {
                if (ChildrenList.Contains(entity))
                {
                    var idx = ChildrenList.IndexOf(entity);
                    ChildrenList.RemoveAt(idx);    
                    ChildrenList.RemoveRange(idx, entity.ChildrenList.Count);    
                }

                ChildrenList.Add(entity);
                ChildrenList.AddRange(entity.ChildrenList);

                UpdateEntityRoot(entity);
            }
        }
        public void PutEntityBefore(Entity before, Entity entity)
        {
            if (Scene != null) Scene.PutEntityBefore(before, entity);
            else
            {

                if (ChildrenList.Contains(entity))
                {
                    var idx = ChildrenList.IndexOf(entity);
                    ChildrenList.RemoveAt(idx);
                    ChildrenList.RemoveRange(idx, entity.ChildrenList.Count);
                }

                entity.Scene = this.Scene;
                entity.Parent = before.Parent;
                foreach (var child in entity.Children)
                {
                    entity.Scene = this.Scene;
                }
                ChildrenList.Insert(ChildrenList.IndexOf(before), entity);
                ChildrenList.InsertRange(ChildrenList.IndexOf(before), entity.ChildrenList);
                UpdateEntityRoot(entity);
            }
        }
        public void PutEntityBefore(int before, Entity entity)
        {
            PutEntityBefore(FirstChildren[before], entity);
        }
        public void PutEntityAfter(Entity afterObj, Entity entity)
        {
            if (Scene != null) Scene.PutEntityAfter(afterObj, entity);
            else
            {
                if (ChildrenList.Contains(entity))
                {
                    var idx = ChildrenList.IndexOf(entity);
                    ChildrenList.RemoveAt(idx);
                    ChildrenList.RemoveRange(idx, entity.ChildrenList.Count);
                }

                entity.Scene = this.Scene;
                entity.Parent = afterObj.Parent;
                foreach (var child in entity.Children)
                {
                    entity.Scene = this.Scene;
                }
                ChildrenList.Insert(ChildrenList.IndexOf(afterObj) + 1, entity);
                ChildrenList.InsertRange(ChildrenList.IndexOf(afterObj) + 1, entity.ChildrenList);
                UpdateEntityRoot(entity);
            }
        }
        public void PutEntityAfter(int after, Entity entity)
        {
            PutEntityAfter(FirstChildren[after], entity);
        }
        
        public override string ToString() => $"Entity: {Name}";
        public Entity Duplicate() => (Entity)Clone();
        public object Clone() 
        {
            Entity clone = (Entity)MemberwiseClone();
            var scene = clone.Scene;
            clone.Scene = null;
            clone.Parent = null;

            for (var i = 0; i < ChildrenList.Count; i++)
            {
                clone.ChildrenList[i] = (Entity)ChildrenList[i].Clone();
                clone.ChildrenList[i].Scene = null;
            }

            clone.ComponentsList.Clear();
            foreach (var component in ComponentsList)
            {
                var cloneComponent = (Component)component.Clone();
                if (scene != null)
                    cloneComponent.OnRemoved();
                clone.ComponentsList.Add(cloneComponent);
            }

            return clone;
        }
    }

}
