using System.Collections.ObjectModel;
using Chess.Pieces;

namespace Chess.Moves
{
    public abstract class Movement
    {        
        public abstract bool CanMove(Field target, ObservableCollection<ChessPieceViewModel> activeState);

        public virtual bool CanMove(Field target)
        {
            return CanMove(target, Formation.Pieces);
        }
    }
}