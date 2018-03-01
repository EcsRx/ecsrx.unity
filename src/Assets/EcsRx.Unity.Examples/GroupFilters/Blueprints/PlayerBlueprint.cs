using EcsRx.Blueprints;
using EcsRx.Entities;
using EcsRx.Unity.Examples.GroupFilters.Components;

namespace EcsRx.Unity.Examples.GroupFilters.Blueprints
{
    public class PlayerBlueprint : IBlueprint
    {
        public string Name { get; private set; }
        public int Score { get; private set; }

        public PlayerBlueprint(string name, int score)
        {
            Name = name;
            Score = score;
        }

        public void Apply(IEntity entity)
        {
            var scoreComponent = new HasScoreComponent { Name = Name };
            scoreComponent.Score.Value = Score;
            entity.AddComponent(scoreComponent);
        }
    }
}