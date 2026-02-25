using System;
using System.Collections.Generic;

using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;

using Microsoft.Xna.Framework;

using Mokus2D.Input;
using Mokus2D.Util;
using Mokus2D.Visual;

namespace Default.Namespace
{
    public class LevelBuilderBase : Updatable, IDisposable
    {
        public LevelBuilderBase(GameBase _game)
        {
            ClipFactory.debug_builder = this;
            createdObjects = new Dictionary<string, object>();
            defaultZ = 0;
            engineConfig = Box2DConfig.DefaultConfig;
            Settings.PositionIterations = engineConfig.PositionIterations;
            Settings.VelocityIterations = engineConfig.VelocityIterations;
            Settings.ContinuousPhysics = false;
            world = new World(engineConfig.Gravity);
            groundBody = BodyFactory.CreateBody(world, new Vector2(0f, 0f));
            maxWorldUpdateTime = 0.033333335f;
            physicsSpeed = 1f;
            game = _game;
            processors = new ArrayList();
            updater = new PhysicsUpdater(world);
            clips = new Dictionary<string, BodyClip>();
            AddProcessors();
        }

        public Box2DConfig EngineConfig
        {
            get => engineConfig; set => engineConfig = value;
        }

        public World World => world;

        public Body GroundBody => groundBody;

        public GameBase Game => game;

        public Dictionary<string, object> CreatedObjects => createdObjects;

        public float PhysicsSpeed
        {
            get => physicsSpeed; set => physicsSpeed = value;
        }

        public int DefaultZ
        {
            get => defaultZ; set => defaultZ = value;
        }

        public float SizeMult => engineConfig.SizeMultiplier;

        public Vector2 LevelSize
        {
            get => levelSize;
            set
            {
                levelSize = value;
                physicsLevelSize = levelSize * SizeMult;
            }
        }

        public Vector2 PhysicsLevelSize => physicsLevelSize;

        public Node Root => game.GameRoot;

        public void Dispose()
        {
        }

        public Body CreateCircleRadiusPositionRotationDynamic(float radius, Vector2 position, float rotation, bool dynamic)
        {
            return FarseerUtil.CreateCircle(world, radius, position, rotation, engineConfig.Density, dynamic);
        }

        public virtual void AddProcessors()
        {
            processors.Add(new CircleProcessor(this));
            processors.Add(new PolygonProcessor(this));
            processors.Add(new BodyProcessor(this));
            processors.Add(new EditorRevoluteJointProcessor(this));
        }

        public void RegisterObject(BodyClip bodyClip, string key)
        {
            createdObjects[key] = bodyClip;
        }

        public object GetObject(string key)
        {
            return !createdObjects.ContainsKey(key) ? null : createdObjects[key];
        }

        public void AddForeground(Node child)
        {
            Root.AddChild(child, 10);
        }

        public void AddBackground(Node child)
        {
            if (!game.Debug)
            {
                Root.AddChild(child, -10);
            }
        }

        public void AddChildBefore(Node child, Node before)
        {
            if (!game.Debug)
            {
                Root.AddChild(child, before.Layer - 1);
            }
        }

        public void AddChildAfter(Node child, Node after)
        {
            if (!game.Debug)
            {
                Root.AddChild(child, after.Layer);
            }
        }

        public Vector2 ToRootChild(Vector2 source, Node child)
        {
            Vector2 vector = child.LocalToGlobal(source, true);
            return Root.GlobalToLocal(vector, true);
        }

        public void ReorderChildBefore(Node child, Node node)
        {
            Root.RemoveChild(child);
            AddChildBefore(child, node);
        }

        public void ChangeChildLayer(Node child, int z)
        {
            if (!game.Debug)
            {
                Root.ChangeChildLayer(child, z);
            }
        }

        public void RemoveChild(Node child, bool cleanup)
        {
            Root.RemoveChild(child);
        }

        public void RemoveChild(Node child)
        {
            Root.RemoveChild(child);
        }

        public void Add(Node child, int z)
        {
            if (!game.Debug)
            {
                Root.AddChild(child, z);
            }
        }

        public Node ReplaceClipWithNode(Node clip, Node newClip)
        {
            newClip.ScaleX = clip.ScaleX;
            newClip.ScaleY = clip.ScaleY;
            newClip.Position = clip.Position;
            newClip.RotationRadians = clip.RotationRadians;
            ReplaceChildWith(clip, newClip);
            return newClip;
        }

        public Node ReplaceClipWith(Node clip, string clipName)
        {
            Sprite sprite = ClipFactory.CreateWithAnchor(clipName);
            return ReplaceClipWithNode(clip, sprite);
        }

        public void ReplaceChildWith(Node source, Node with)
        {
            CocosUtil.ReplaceNodeWithCleanup(source, with);
        }

        public Node AddChild(Node child)
        {
            return AddChild(child, 0);
        }

        public Node AddChild(Node child, int layer)
        {
            if (!game.Debug)
            {
                Root.AddChild(child, layer);
            }
            return child;
        }

        public BodyClip GetClip(string name)
        {
            return clips[name];
        }

        public Vector2 ToIPhoneVec(Vector2 point)
        {
            return ToVec(CocosUtil.toIPhone(point));
        }

        public Vector2 ToVec(Vector2 point)
        {
            return engineConfig.ToVec(point);
        }

        public Vector2 TouchRootVec(Touch touch)
        {
            Vector2 vector = CocosUtil.toIPhone(TouchRootPoint(touch));
            return ToVec(vector);
        }

        public Vector2 TouchRootPoint(Touch touch)
        {
            return CocosUtil.TouchPointInNode(touch, Root);
        }

        public Vector2 ToIPhonePoint(Vector2 vec)
        {
            return CocosUtil.toIPhone(ToPoint(vec));
        }

        public Vector2 ToIPadPoint(Vector2 vec)
        {
            return CocosUtil.toIPad(ToPoint(vec));
        }

        public void ToPointsPoints(List<Vector2> vecs, List<Vector2> points)
        {
            for (int i = 0; i < vecs.Count; i++)
            {
                points.Add(ToPoint(vecs[i]));
            }
        }

        public Vector2 ToPoint(Vector2 vec)
        {
            return engineConfig.ToPoint(vec);
        }

        public void DestroyFixturesData(Body body)
        {
            foreach (Fixture fixture in body.FixtureList)
            {
                fixture.UserData = null;
            }
        }

        public void RemoveBody(Body body)
        {
            DestroyFixturesData(body);
            world.RemoveBody(body);
        }

        public void ProcessLevel(Level level)
        {
            Hashtable levelProperties = level.LevelProperties;
            LevelSize = new Vector2(levelProperties.GetFloat("Width"), levelProperties.GetFloat("Height"));
            foreach (object obj in level.Items)
            {
                Hashtable hashtable = (Hashtable)obj;
                ArrayList arrayList = (ArrayList)hashtable["children"];
                foreach (object obj2 in arrayList)
                {
                    Hashtable hashtable2 = (Hashtable)obj2;
                    if (!ProcessItem(hashtable2))
                    {
                        DebugLog.MISSING("Item Not Processed `" + hashtable2.GetString("config/type") + "` ");
                    }
                }
            }
        }

        public bool ProcessItem(Hashtable item)
        {
            bool flag = false;
            foreach (object obj in processors)
            {
                TypeProcessorBase typeProcessorBase = (TypeProcessorBase)obj;
                if (typeProcessorBase.Match(item))
                {
                    flag = true;
                    object obj2 = typeProcessorBase.ProcessItem(item);
                    if (obj2 != null)
                    {
                        ProcessCreatedPhysicsItem(obj2, item);
                    }
                }
            }
            return flag;
        }

        public virtual string GetViewType(Hashtable config)
        {
            return config.Exists("iPhoneViewType")
                ? config.GetString("iPhoneViewType")
                : config.Exists("viewType") ? config.GetString("viewType") : null;
        }

        private string GetClipType(Hashtable config)
        {
            return config.Exists("iPhoneClipType")
                ? config.GetString("iPhoneClipType")
                : config.Exists("clipType") ? config.GetString("clipType") : null;
        }

        public virtual LevelBuilderBase GetBuilder()
        {
            return this;
        }

        public BodyClip CreateItem(object physics, Hashtable item)
        {
            Hashtable hashtable = item.GetHashtable("config");
            string viewType = GetViewType(hashtable);
            if (viewType != null || hashtable.Exists("createClip") || hashtable.Exists("clipType"))
            {
                Node node = null;
                if (viewType != null && !viewType.Equals("null"))
                {
                    node = ClipFactory.CreateWithAnchor(viewType);
                    Vector2 vector = hashtable.GetVector("scale");
                    node.ScaleX = vector.X;
                    node.ScaleY = vector.Y;
                    if (hashtable.GetBool("flipX"))
                    {
                        node.ScaleX *= -1f;
                    }
                    if (hashtable.GetBool("flipY"))
                    {
                        node.ScaleY *= -1f;
                    }
                    int num = hashtable.Exists("z") ? hashtable.GetInt("z") : DefaultZ;
                    Add(node, num);
                    Vector2 vector2 = item.GetVector("position");
                    node.Position = ToIPadPoint(vector2);
                    node.RotationRadians = -Maths.ToRadians(hashtable.GetFloat("rotation", 0f));
                }
                if (!hashtable.Exists("skipClip"))
                {
                    string clipType = GetClipType(hashtable);
                    Type type = (clipType != null) ? Type.GetType(NamespacePrefix + clipType) : typeof(BodyClip);
                    if (type == null)
                    {
                        DebugLog.errorFmt("type not found {0}", null, [clipType]);
                        return null;
                    }
                    return (BodyClip)ReflectUtil.CreateInstance(type, [this, physics, node, hashtable]);
                }
            }
            return null;
        }

        public void ProcessCreatedPhysicsItem(object physics, Hashtable item)
        {
            BodyClip bodyClip = CreateItem(physics, item);
            if (bodyClip != null && item.Exists("config/id"))
            {
                string @string = item.GetString("config/id");
                clips[@string] = bodyClip;
            }
        }

        public float ToRotationVec(Vector2 vec)
        {
            return ToRotation(Maths.atan2Vec(vec));
        }

        public float ToRotation(float angle)
        {
            return MathHelper.ToDegrees(-angle);
        }

        public override void Update(float time)
        {
            float num = Math.Min(time, maxWorldUpdateTime);
            world.Step(num * physicsSpeed);
            updater.Update(time);
        }

        public const int FOREGROUND = 10;

        public const float MAX_UPDATE_TIME = 0.033333335f;

        public const int BACKGROUND = -10;

        public string NamespacePrefix;

        protected Dictionary<string, BodyClip> clips;

        protected Hashtable createdBodies;

        protected Dictionary<string, object> createdObjects;

        protected Vector2 currentOffset;

        protected int defaultZ;

        protected Box2DConfig engineConfig;

        protected GameBase game;

        protected Body groundBody;

        protected Hashtable lastItem;

        private Vector2 levelSize;

        protected float maxWorldUpdateTime;

        private Vector2 physicsLevelSize;

        protected float physicsSpeed;

        protected ArrayList processedBodies;

        protected ArrayList processors;

        protected PhysicsUpdater updater;

        protected World world;
    }
}

