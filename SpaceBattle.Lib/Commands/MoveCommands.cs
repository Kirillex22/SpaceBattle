namespace SpaceBattleLib;

public class Move : ICommand
    {
        private IMovable movable;
        public Move(IMovable movable)
        {
            this.movable = movable;
        }
        public void Execute()
        {
            movable.position += movable.velocity;
        }

    }