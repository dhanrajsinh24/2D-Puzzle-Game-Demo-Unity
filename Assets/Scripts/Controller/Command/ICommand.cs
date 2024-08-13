using System.Collections;

namespace IG.Command
{
    public interface ICommand
    {
        IEnumerator Execute();
        void Undo();
    }
}
