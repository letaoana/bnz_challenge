using Microsoft.Playwright;
using System.Threading.Tasks;

namespace Bnz.UI.PageObjects
{
    public class PayeesPageObject
    {
        private readonly string pageHeader = "xpath=//span[text()='Payees']";
        private readonly string addButton = "xpath=//span[text()='Add']";
        private readonly string newPayeeFormAddButton = "xpath=//button[text()='Add']";
        private readonly string payeeName = "xpath=//input[@id='ComboboxInput-apm-name']";
        private readonly string bankCode = "xpath=//input[@id='apm-bank']";
        private readonly string branchCode = "xpath=//input[@id='apm-branch']";
        private readonly string accountNumber = "xpath=//input[@id='apm-account']";
        private readonly string suffix = "xpath=//input[@id='apm-suffix']";
        private readonly string addNewPayeeMessage = "xpath=//span[text()='Payee added']";
        private readonly string payeeList = ".js-payee-name";
        private readonly string nameHeader = "xpath=//span[text()='Name']";
        private readonly IPage page;

        public PayeesPageObject(IPage page)
        {
            this.page = page;
        }

        public async Task<bool> IsPageHeaderVisible()
        => await page.IsVisibleAsync(pageHeader);

        public async Task ClickAddButton()
        => await page.ClickAsync(addButton);

        public async Task ClickAddButtonOnNewPayeeForm()
        => await page.ClickAsync(newPayeeFormAddButton);

        public async Task SetPayeeName(string payeeName)
        {
            await page.TypeAsync(this.payeeName, payeeName);
            await page.PressAsync(this.payeeName, "Enter");
        }

        public async Task CreatePayee(string payeeName, string bankCode, string branchCode, string accountNumber, string suffix)
        {
            await ClickAddButton();
            await SetPayeeName(payeeName);
            await SetBankCode(bankCode);
            await SetBranchCode(branchCode);
            await SetAccountNumber(accountNumber);
            await SetSuffix(suffix);
            await ClickAddButtonOnNewPayeeForm();
        }

        private async Task SetBankCode(string bankCode)
        => await page.FillAsync(this.bankCode, bankCode);

        private async Task SetBranchCode(string branchCode)
        => await page.FillAsync(this.branchCode, branchCode);

        private async Task SetAccountNumber(string accountNumber)
        => await page.FillAsync(this.accountNumber, accountNumber);

        private async Task SetSuffix(string suffix)
        => await page.FillAsync(this.suffix, suffix);

        public async Task<bool> IsSuccessMessageDisplayed()
        => await page.IsVisibleAsync(addNewPayeeMessage);

        public async Task<bool> IsNewPayeeAddedInTheListOfPayees(string account)
        => await page.IsVisibleAsync($"xpath=//p[text()='{account}']");

        public async Task<string> GetFieldError()
        => await page.GetAttributeAsync(payeeName, "aria-label");

        public ILocator GetPayeeList()
        => page.Locator(payeeList);

        public async Task ClickNameHeader()
        => await page.ClickAsync(nameHeader);
    }
}