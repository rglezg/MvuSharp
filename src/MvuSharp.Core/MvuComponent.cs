namespace MvuSharp
{
    public delegate (TModel, CommandHandler<TMsg>) InitHandler<TModel, TMsg, in TArgs>(TArgs args);

    public delegate (TModel, CommandHandler<TMsg>) UpdateHandler<TModel, TMsg>(TModel model, TMsg msg);
    public abstract class MvuComponent<TModel, TMsg, TArgs>
    {
        public readonly InitHandler<TModel, TMsg, TArgs> Init;
        public readonly UpdateHandler<TModel, TMsg> Update;

        protected MvuComponent(
            InitHandler<TModel, TMsg, TArgs> init, 
            UpdateHandler<TModel, TMsg> update)
        {
            Init = init;
            Update = update;
        }
    }
}