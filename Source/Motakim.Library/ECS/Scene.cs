using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Motakim 
{
    public abstract class Scene
    {
        internal List<ComponentsGroup> RenderComponentGroups = new List<ComponentsGroup>();
        internal List<ComponentsGroup> UpdateComponentGroups = new List<ComponentsGroup>();
        internal Dictionary<string, List<Entity>> EntityTagGroups = new Dictionary<string, List<Entity>>();
        internal List<Entity> EntitiesList { get; private set; } 
        public string Name;
        public IReadOnlyList<Entity> Entities => new List<Entity>(EntitiesList).AsReadOnly(); 
        public IReadOnlyList<Entity> RootEntities => EntitiesList.Where(entity => entity.Parent == null).ToList().AsReadOnly(); 
        public bool IsLoaded { get; internal set; }
        public Color Background;
        public bool IsCameraFree = false;
        public Rectangle CameraBounds = new Rectangle(0, 0, (int)(Game.DisplayWidth / Game.ScenePixelScaling), (int)(Game.DisplayHeight / Game.ScenePixelScaling));
        public Vector2 Camera;
        public Entity CameraTarget { get; set; }

        internal protected virtual bool LoadWithGame => false;
        internal protected virtual void Load() {}
        internal protected virtual void Unload() {}
        internal protected virtual void Entering() {}
        internal protected virtual void Leaving() {}

        ~Scene()
        {
            Dispose();
        }
        internal virtual void Initialize() 
        {
            EntitiesList = new List<Entity>();

            Load();
            IsLoaded = true;
        }
        internal virtual void Dispose() 
        {
            Unload();
            IsLoaded = false;

            var entities = RootEntities;
            foreach (var entity in entities)
            {
                RemoveEntity(entity);
            }
            
            EntitiesList = null;

            Camera = Vector2.Zero;  
        }
        internal void UpdateEntityRoot(Entity entity)
        {
            entity.ChildrenList = FindEntityChildren(entity).ToList();
            if (entity.Parent != null)
            {
                UpdateEntityRoot(entity.Parent);
            }
            // public List<Entity> Children => new List<Entity>(ChildrenList);
            // public List<Entity> ChildrenRoot => ChildrenList.Where(child => child.Parent == this).ToList(); 
        }
        internal IEnumerable<Entity> FindEntityChildren(Entity entity) 
        {
            return EntitiesList.Where(child => child.Parent == entity).SelectMany(child => FindEntityChildren(child).Prepend(child));
        } 
        internal Matrix GetCameraMatrix()
        {
            return Matrix.CreateLookAt(new Vector3((Camera - new Vector2(-Game.DisplayWidth / 2f, -Game.DisplayHeight / 2f)) * Game.ScenePixelScaling, 0f), Vector3.Zero, Vector3.Up);
        }  
        private void RemoveEntity(Entity entity, bool first)
        {
            var idx = EntitiesList.IndexOf(entity);
            EntitiesList.RemoveAt(idx);
            
            entity.Scene = null;

            foreach (var child in entity.ChildrenList)
            {
                RemoveEntity(child, false);
            }
            
            var uGroup = UpdateComponentGroups.FindIndex(group => group.Entity == entity);
            var rGroup = RenderComponentGroups.FindIndex(group => group.Entity == entity);

            if (uGroup > -1)
            {
                UpdateComponentGroups.RemoveAt(uGroup);
            }
            if (rGroup > -1)
            {
                RenderComponentGroups.RemoveAt(rGroup);
            }
            
            foreach (var tag in entity.TagHashSet)
            {
                var group = EntityTagGroups[tag];
                group.Remove(entity);

                if (group.Count == 0) EntityTagGroups.Remove(tag);
            }

            if (first)
            {
                entity.RemovedFromScene();
                UpdateEntityRoot(entity);
                
                var uCGAfter = UpdateComponentGroups.FindIndex(group => group.Index > idx);
                var rCGAfter = RenderComponentGroups.FindIndex(group => group.Index > idx);
                if (uCGAfter > -1)
                {
                    for (var i = uCGAfter; i < UpdateComponentGroups.Count; i++)
                    {
                        UpdateComponentGroups[i].Index =+ entity.ChildrenList.Count + 1;
                    }
                }
                if (rCGAfter > -1)
                {
                    for (var i = rCGAfter; i < RenderComponentGroups.Count; i++)
                    {
                        RenderComponentGroups[i].Index =+ entity.ChildrenList.Count + 1;
                    }
                }
            }    
        }
        private void AddEntity(Entity entity, Entity parent)
        {
            entity.Scene = this;
            entity.Parent = parent;
            foreach (var child in entity.Children)
            {
                entity.Scene = this;
            }

            var uGroup = UpdateComponentGroups.Find(group => group.Entity == entity);
            var rGroup = RenderComponentGroups.Find(group => group.Entity == entity);
            foreach (var component in entity.ComponentsList)
            {
                if (component is IUpdate)
                {
                    if (uGroup == null)
                    {
                        uGroup = new ComponentsGroup(EntitiesList.IndexOf(entity), entity);
                        UpdateComponentGroups.Add(uGroup);
                        UpdateComponentGroups.Sort();
                    }

                    uGroup.Add(component);
                }
                else if (component is IRender)
                {
                    if (rGroup == null)
                    {
                        rGroup = new ComponentsGroup(EntitiesList.IndexOf(entity), entity);
                        RenderComponentGroups.Add(rGroup);
                        RenderComponentGroups.Sort();
                    }

                    rGroup.Add(component);
                }
            }

            foreach (var tag in entity.TagHashSet)
            {
                if (!EntityTagGroups.ContainsKey(tag))
                {
                    EntityTagGroups.Add(tag, new List<Entity>());
                }
                EntityTagGroups[tag].Add(entity);
            }

        }
        internal void AddEntityTag(Entity entity, string tag)
        {
            if (!EntityTagGroups.ContainsKey(tag))
            {
                EntityTagGroups.Add(tag, new List<Entity>());
            }
            EntityTagGroups[tag].Add(entity);
        }
        internal void RemoveEntityTag(Entity entity, string tag)
        {
            if (!EntityTagGroups.ContainsKey(tag)) return;
            
            EntityTagGroups[tag].Remove(entity);
            if (EntityTagGroups[tag].Count == 0)
            {
                EntityTagGroups.Remove(tag);
            } 
        }
        public override string ToString() => $"Scene: {Name}";

        public bool HasEntity(Entity entity)
        {
            if (entity.Equals(null)) return false;
            return EntitiesList.Contains(entity);
        }
        public bool HasEntity(string name)
        {
            return EntitiesList.Find(entity => entity.Name == name) != null;
        }
        public Entity GetEntityWithName(string name)
        {
            return EntitiesList.Find(entity => entity.Name == name);
        }
        public List<Entity> GetEntitiesWithTag(string tag)
        {
            if (!EntityTagGroups.ContainsKey(tag)) return new List<Entity>();
            
            return EntityTagGroups[tag];
        }
        public Entity FindEntity(Predicate<Entity> condition) => EntitiesList.Find(condition);
        public List<Entity> FindEntities(Predicate<Entity> condition) => EntitiesList.FindAll(condition);
        public Entity CreateEntity(string name = "", Vector2 position = default)
        {
            var entity = new Entity() { Name = name };
            entity.AddComponent(new Transform(position));

            PutEntity(entity);
            return entity;
        }
        public void RemoveEntity(Entity entity)
        {
            RemoveEntity(entity, true);
        }
        public void PutEntity(Entity entity)
        {
            if (EntitiesList.Contains(entity))
            {
                RemoveEntity(entity);
            }

            EntitiesList.Add(entity);
            AddEntity(entity, null);

            entity.AddedToScene();

            UpdateEntityRoot(entity);
        }
        public void PutEntityBefore(Entity before, Entity entity)
        {
            if (EntitiesList.Contains(entity))
            {
                RemoveEntity(entity);
            }

            var idx = EntitiesList.IndexOf(before);
            EntitiesList.Insert(idx, entity);
            EntitiesList.InsertRange(idx, entity.ChildrenList);

            AddEntity(entity, before.Parent);
            
            entity.AddedToScene();
            
            UpdateEntityRoot(entity);
        }
        public void PutEntityBefore(int before, Entity entity)
        {
            PutEntityBefore(RootEntities[before], entity);
        }
        public void PutEntityAfter(Entity after, Entity entity)
        {
            if (EntitiesList.Contains(entity))
            {
                RemoveEntity(entity);
            }

            var idx = EntitiesList.IndexOf(after);
            EntitiesList.Insert(idx + 1, entity);
            EntitiesList.InsertRange(idx + 1, entity.ChildrenList);
            
            AddEntity(entity, after.Parent);

            entity.AddedToScene();

            UpdateEntityRoot(entity);
        }
        public void PutEntityAfter(int after, Entity entity)
        {
            PutEntityAfter(RootEntities[after], entity);
        }
        
    }

}