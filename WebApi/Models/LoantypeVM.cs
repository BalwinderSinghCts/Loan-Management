namespace WebApi.Models
{
    public class LoantypeVM
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public static List<LoantypeVM> GetLoanTypes()
        {
            List<LoantypeVM> list = new List<LoantypeVM>();
            list.Add(new LoantypeVM() { Id = 1, Name = "Personal Loan" });
            list.Add(new LoantypeVM() { Id = 2, Name = "Home Loan" });
            list.Add(new LoantypeVM() { Id = 3, Name = "Car Loan" });
            list.Add(new LoantypeVM() { Id = 4, Name = "Bike Loan" });
            list.Add(new LoantypeVM() { Id = 5, Name = "Gold Loan" });
            return list;
        }
    }
}
