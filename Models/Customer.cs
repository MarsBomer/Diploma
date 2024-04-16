public class Customer
{
    public Guid Id { get; set; }
    public string OrganizationName { get; set; }
    public string Inn {  get; set; }
    public string Ogrn { get; set; }    
    public string Kpp { get; set; }
    public string Address { get; set; }
    public string ContactPhone { get; set; }
    public string ContactName { get; set; }
    public Employee ResponsibleEmployee { get; set; }
}