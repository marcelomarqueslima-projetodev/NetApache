namespace Banking.Account.Command.Domain.Common
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class BsonCollecctionAttribute : Attribute
    {
        public BsonCollecctionAttribute(string collectionName)
        {
            CollectionName = collectionName;
        }

        public string CollectionName { get; }


    }
}
