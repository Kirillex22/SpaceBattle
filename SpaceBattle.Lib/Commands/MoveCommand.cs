namespace SpaceBattleLib;

public class MoveCommand : ICommand
    {
        private IMovable movable;
        public MoveCommand(IMovable movable)
        {
            this.movable = movable;
        }
        public void Execute()
        {
            movable.position += movable.velocity;
        }

    }