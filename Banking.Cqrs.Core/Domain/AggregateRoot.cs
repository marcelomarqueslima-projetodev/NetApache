using Banking.Cqrs.Core.Events;

namespace Banking.Cqrs.Core.Domain
{
    public abstract class AggregateRoot
    {
        public string Id { get; set; } =string.Empty;
        private int version = -1;
        List<BaseEvent> changes = new List<BaseEvent>();

        public int GetVersio()
        {
            return version;
        }

        public void SetVersion()
        {
            this.version = version;
        }

        public List<BaseEvent> GetUncommitedChances()
        {
            return changes;
        }

        public void MarkChangesAsCommited()
        {
            changes.Clear();
        }

        public void ApplyChanges(BaseEvent @event, bool isNewEvent)
        {
            try
            {
                var ClaseDeEvento = @event.GetType();
                var method = GetType().GetMethod("Apply", new[] { ClaseDeEvento });
                method.Invoke(this, new object[] { @event });

            }
            catch (Exception ex)
            {

                throw;
            }
            finally
            {
                if (isNewEvent)
                {
                    changes.Add(@event);
                }
            }
        }

        public void RaiseEvent(BaseEvent @event)
        {
            ApplyChanges(@event, true);
        }

        public void ReplayEvent(IEnumerable<BaseEvent> events)
        {
            foreach (var evt in events)
            {
                ApplyChanges(evt, false);
            }
        }
    }
}
