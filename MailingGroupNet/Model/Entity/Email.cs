namespace Model.Entity
{
    public class Email : Entity
    {
        public string Name { get; set; }
        public int MailingGroupId { get; set; }
        public MailingGroup MailingGroup { get; set; }
    }
}
