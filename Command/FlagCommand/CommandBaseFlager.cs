using todoCOM.Flags;

namespace todoCOM.Command.FlagCommand
{
    public abstract class CommandBaseFlag : CommandBase
    {
        public abstract bool GetFlag(out OptionFlags flag);

        protected CommandBaseFlag()
        {
        }
    }
}