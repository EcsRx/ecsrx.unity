using System;
using System.Collections.Generic;
using System.Linq;
using ModestTree;
using ModestTree.Util;

namespace Zenject
{
    // This class is only used inside EditorWindowDependencyRoot, since in most cases
    // people aren't going to want it
    // However, you can include it yourself if you want to use it
    public class GuiRenderableManager
    {
        List<RenderableInfo> _renderables;

        public GuiRenderableManager(
            [Inject(Optional = true, Source = InjectSources.Local)]
            List<IGuiRenderable> renderables,
            [Inject(Optional = true, Source = InjectSources.Local)]
            List<ModestTree.Util.Tuple<Type, int>> priorities)
        {
            _renderables = new List<RenderableInfo>();

            foreach (var renderable in renderables)
            {
                // Note that we use zero for unspecified priority
                // This is nice because you can use negative or positive for before/after unspecified
                var matches = priorities
                    .Where(x => renderable.GetType().DerivesFromOrEqual(x.First))
                    .Select(x => x.Second).ToList();

                int priority = matches.IsEmpty() ? 0 : matches.Single();

                _renderables.Add(
                    new RenderableInfo(renderable, priority));
            }

            _renderables = _renderables.OrderBy(x => x.Priority).ToList();

            foreach (var renderable in _renderables.Select(x => x.Renderable).GetDuplicates())
            {
                Assert.That(false, "Found duplicate IGuiRenderable with type '{0}'".Fmt(renderable.GetType()));
            }
        }

        public void OnGui()
        {
            foreach (var renderable in _renderables)
            {
                try
                {
#if PROFILING_ENABLED
                    using (ProfileBlock.Start("{0}.Initialize()", renderable.Renderable.GetType().Name()))
#endif
                    {
                        renderable.Renderable.GuiRender();
                    }
                }
                catch (Exception e)
                {
                    throw Assert.CreateException(
                        e, "Error occurred while initializing IGuiRenderable with type '{0}'", renderable.Renderable.GetType().Name());
                }
            }
        }

        class RenderableInfo
        {
            public IGuiRenderable Renderable;
            public int Priority;

            public RenderableInfo(IGuiRenderable renderable, int priority)
            {
                Renderable = renderable;
                Priority = priority;
            }
        }
    }
}
