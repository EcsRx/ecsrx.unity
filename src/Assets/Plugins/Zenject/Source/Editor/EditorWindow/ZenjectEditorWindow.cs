using System;
using ModestTree;
using UnityEditor;
using UnityEngine;

namespace Zenject
{
    public abstract class ZenjectEditorWindow : EditorWindow
    {
        [Inject]
        [NonSerialized]
        Kernel _kernel = null;

        [Inject]
        [NonSerialized]
        GuiRenderableManager _guiRenderableManager = null;

        [NonSerialized]
        DiContainer _container;

        [NonSerialized]
        Exception _fatalError;

        [NonSerialized]
        GUIStyle _errorTextStyle;

        GUIStyle ErrorTextStyle
        {
            get
            {
                if (_errorTextStyle == null)
                {
                    _errorTextStyle = new GUIStyle(GUI.skin.label);
                    _errorTextStyle.fontSize = 18;
                    _errorTextStyle.normal.textColor = Color.red;
                    _errorTextStyle.wordWrap = true;
                    _errorTextStyle.alignment = TextAnchor.MiddleCenter;
                }

                return _errorTextStyle;
            }
        }

        protected DiContainer Container
        {
            get { return _container; }
        }

        public void OnEnable()
        {
            if (_fatalError != null)
            {
                return;
            }

            Initialize();
        }

        void Initialize()
        {
            Assert.IsNull(_container);

            _container = new DiContainer(new DiContainer[] { StaticContext.Container });

            // Make sure we don't create any game objects since editor windows don't have a scene
            _container.AssertOnNewGameObjects = true;

            ZenjectManagersInstaller.Install(_container);

            _container.Bind<Kernel>().AsSingle();
            _container.Bind<GuiRenderableManager>().AsSingle();
            _container.BindInstance(this);

            InstallBindings();

            _container.QueueForInject(this);
            _container.ResolveRoots();

            _kernel.Initialize();
        }

        public void OnDisable()
        {
            if (_fatalError != null)
            {
                return;
            }

            _kernel.Dispose();
        }

        public void Update()
        {
            if (_fatalError != null)
            {
                return;
            }

            try
            {
                _kernel.Tick();
            }
            catch (Exception e)
            {
                Log.ErrorException(e);
                _fatalError = e;
            }

            // We might also consider only calling Repaint when changes occur
            Repaint();
        }

        public void OnGUI()
        {
            if (_fatalError != null)
            {
                var labelWidth = 600;
                var labelHeight = 200;

                GUI.Label(new Rect(Screen.width / 2 - labelWidth / 2, Screen.height / 3 - labelHeight / 2, labelWidth, labelHeight), "Unrecoverable error occurred!  \nSee log for details.", ErrorTextStyle);

                var buttonWidth = 100;
                var buttonHeight = 50;
                var offset = new Vector2(0, 100);

                if (GUI.Button(new Rect(Screen.width / 2 - buttonWidth / 2 + offset.x, Screen.height / 3 - buttonHeight / 2 + offset.y, buttonWidth, buttonHeight), "Reload"))
                {
                    ExecuteFullReload();
                }
            }
            else
            {
                try
                {
                    if (_guiRenderableManager != null)
                    {
                        _guiRenderableManager.OnGui();
                    }
                }
                catch (Exception e)
                {
                    Log.ErrorException(e);
                    _fatalError = e;
                }
            }
        }

        void ExecuteFullReload()
        {
            _kernel = null;
            _guiRenderableManager = null;
            _container = null;
            _fatalError = null;

            Initialize();
        }

        public abstract void InstallBindings();
    }
}
