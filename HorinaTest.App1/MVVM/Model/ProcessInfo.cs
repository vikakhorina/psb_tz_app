namespace HorinaTest.App1.MVVM.Model
{
    public class ProcessInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Prioriry { get; set; }

        public ProcessInfo(int id, string name, string prioriry)
        {
            Id = id;
            Name = name;
            Prioriry = prioriry;
        }
    }
}
