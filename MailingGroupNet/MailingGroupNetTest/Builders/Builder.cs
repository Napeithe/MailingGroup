using System;
using System.Collections.Generic;
using System.Text;
using Model.Database;

namespace MailingGroupNetTest.Builders
{
    public interface IBuilder<TItem> where TItem : new()
    {
        TItem BuildAndSave();
    }

    public abstract class Builder<TBuilder, TItem> : IBuilder<TItem> where TBuilder : Builder<TBuilder, TItem>
        where TItem : new()
    {
        protected readonly MailingGroupContext Context;
        protected TItem State;

        protected Builder()
        {
            Init();
        }

        protected Builder(MailingGroupContext context)
        {
            Context = context;
            Init();
        }

        public virtual TItem Build()
        {
            return State;
        }

        public virtual TItem BuildAndSave()
        {
            Save();
            return State;
        }

        public TBuilder Save()
        {
            Context.Add(State);
            Context.SaveChanges();
            return (TBuilder)this;
        }

        public TBuilder With(Action<TItem> operation)
        {
            operation.Invoke(State);
            return (TBuilder)this;
        }

        private void Init()
        {
            State = new TItem();
        }

        public TItem Get()
        {
            return State;
        }

        public TItem As(TItem item)
        {
            return State = item;
        }

        public TBuilder For(Action<TBuilder> operation)
        {
            operation.Invoke((TBuilder)this);
            return (TBuilder)this;
        }

    }

    public class BuilderSaveException : Exception
    {
        public BuilderSaveException(string msg) : base(msg)
        {
        }
    }
}
