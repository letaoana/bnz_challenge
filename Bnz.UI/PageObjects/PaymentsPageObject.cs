using Microsoft.Playwright;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bnz.UI.PageObjects
{
    public class PaymentsPageObject
    {
        public readonly IPage page;
        private readonly string fromAccount = "//button[@data-monitoring-label='Transfer Form From Chooser']";
        private readonly string toAccount = "//button[@data-monitoring-label='Transfer Form To Chooser']";
        private readonly string accountSearch = "//input[@placeholder='Search']";
        private readonly string oldBalance = "//p[@class='balance-1-1-67']";
        private readonly string transferButton = "//button[contains(@class,'Button-component-88 Button-component-106 Button-normalSize-96 Button-midblueColor-92 Button-solidVariant-89 Button-solidVariant-107')]//span[contains(@class,'Button-wrapper-98')]";
        private readonly string newBalance = ".js-account-current";
        private readonly string closeAccountDetails = "//span[@aria-label='Close account details']";
        private readonly string amount = "input[name='amount']";

        public PaymentsPageObject(IPage page)
        {
            this.page = page;
        }

        public async Task<IReadOnlyList<string>> GetOldBalanceAmounts()
        => await page.Locator(oldBalance).AllTextContentsAsync();

        public async Task<IReadOnlyList<string>> GetAccountBalance(string account)
        {
            await ClickAccount(account);
            var accountBalance = await page.Locator(newBalance).AllTextContentsAsync();
            await ClickCloseAccountDetailsButton();
            return accountBalance;
        }

        public async Task Transfer(string fromAccount, string toAccount, double transferAmount)
        {
            await SelectFromAccount(fromAccount);
            await SelectToAccount(toAccount);
            await SetAmount(transferAmount);
            await ClickTransferButton();
        }

        public async Task SetAmount(double transferAmount)
        => await page.FillAsync(amount, transferAmount.ToString());

        public async Task SelectToAccount(string account)
        {
            await page.ClickAsync(toAccount);
            await page.FillAsync(accountSearch, account);
            await page.ClickAsync($"//p[normalize-space()='{account}']");
        }

        public async Task SelectFromAccount(string account)
        {
            await page.ClickAsync(fromAccount);
            await page.FillAsync(accountSearch, account);
            await page.ClickAsync($"//p[normalize-space()='{account}']");
        }

        public async Task ClickTransferButton()
        => await page.ClickAsync(transferButton);

        public async Task ClickAccount(string account)
        => await page.ClickAsync($"//h3[normalize-space()='{account}']");

        public async Task ClickCloseAccountDetailsButton()
        => await page.ClickAsync(closeAccountDetails);
    }
}