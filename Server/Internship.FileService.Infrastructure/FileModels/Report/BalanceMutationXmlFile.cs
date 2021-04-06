namespace Internship.FileService.Infrastructure.FileModels.Report
{
    public class BalanceMutationXmlFile
    {
        public string DebtorIBAN { get; set; }
        public string CreditorIBAN { get; set; }
        public decimal Amount { get; set; }
        public string OriginalReference { get; set; }
    }
}
