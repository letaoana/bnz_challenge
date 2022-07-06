using Microsoft.Playwright;
using System.Threading.Tasks;

namespace Bnz.UI.PageObjects
{
    public class ClientPageObject
    {
        private readonly IPage page;
        private readonly string menuButton = "xpath=//span[text()='Menu']";
        private readonly string payeesbutton = "xpath=//span[text()='Payees']";

        public ClientPageObject(IPage page)
        {
            this.page = page;
        }

        public async Task ClickMenuButton()
        => await page.ClickAsync(menuButton);

        public async Task ClickPayeesButton()
        => await page.ClickAsync(payeesbutton);
    }
}