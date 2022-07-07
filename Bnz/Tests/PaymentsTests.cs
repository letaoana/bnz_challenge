using Bnz.UI.PageObjects;
using Bnz.UI.Utils;
using NUnit.Framework;
using Shouldly;
using System.Threading.Tasks;

namespace Bnz.UI.Tests.Tests
{
    [FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
    [Parallelizable(ParallelScope.Fixtures)]
    [TestFixture(TargetedBrowser.Chrome)]
    [TestFixture(TargetedBrowser.Firefox)]
    public class PaymentsTests : PlaywrightBase
    {
        private const int TransferAmount = 500;
        private const string EverydayAccount = "Everyday";
        private const string BillsAccount = "Bills";

        public PaymentsTests(TargetedBrowser targetedBrowser) : base(TargetedBrowser)
        {
            TargetedBrowser = targetedBrowser;
        }

        [Test]
        public async Task VerifyCurrentBalanceOfEverydayAccountAndBillsAccountAreCorrect()
        {
            await Page.GotoAsync("https://www.demo.bnz.co.nz/client");

            var paymentsPageObject = new PaymentsPageObject(Page);
            var oldEverydayAccountBalance = await paymentsPageObject.GetAccountBalance(EverydayAccount);
            var expectedEverydayAccountBalance = BalanceCalculator.SubtractFromBalance(oldEverydayAccountBalance[0], TransferAmount);

            var oldBillsAccountBalance = await paymentsPageObject.GetAccountBalance(BillsAccount);
            var expectedBillsAccountBalance = BalanceCalculator.AddToBalance(oldBillsAccountBalance[0], TransferAmount);

            await Page.GotoAsync("https://www.demo.bnz.co.nz/client/payments");
            await paymentsPageObject.Transfer(EverydayAccount, BillsAccount, TransferAmount);

            var newEverydayAccountBalance = await paymentsPageObject.GetAccountBalance(EverydayAccount);
            var newBillsAccountBalance = await paymentsPageObject.GetAccountBalance(BillsAccount);

            double.Parse(newBillsAccountBalance[0]).ShouldBe(expectedBillsAccountBalance);
            double.Parse(newEverydayAccountBalance[0]).ShouldBe(expectedEverydayAccountBalance);
        }
    }
}