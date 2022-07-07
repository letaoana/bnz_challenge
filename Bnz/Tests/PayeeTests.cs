using Bnz.UI.PageObjects;
using NUnit.Framework;
using Shouldly;
using System.Threading.Tasks;

namespace Bnz.UI.Tests.Tests
{
    [TestFixture]
    [FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
    [Parallelizable(ParallelScope.Fixtures)]
    [TestFixture(TargetedBrowser.Chrome)]
    [TestFixture(TargetedBrowser.Firefox)]
    public class PayeeTests : PlaywrightBase
    {
        private PayeesPageObject payeesPageObject;
        private readonly string pageUrl = "https://www.demo.bnz.co.nz/client/payees";

        public PayeeTests(TargetedBrowser targetedBrowser) : base(TargetedBrowser)
        {
            TargetedBrowser = targetedBrowser;
        }

        [Test]
        public async Task VerifyCanNavigateToPayeesPage()
        {
            await Page.GotoAsync("https://www.demo.bnz.co.nz/client/");
            var clientPageObject = new ClientPageObject(Page);
            await clientPageObject.ClickMenuButton();
            await clientPageObject.ClickPayeesButton();

            payeesPageObject = new PayeesPageObject(Page);
            var isPageHeaderVisible = await payeesPageObject.IsPageHeaderVisible();
            isPageHeaderVisible.ShouldBeTrue();
            Page.Url.ShouldBe(pageUrl);
        }

        [Test]
        public async Task VerifyCanAddNewPayee()
        {
            await Page.GotoAsync(pageUrl);
            payeesPageObject = new PayeesPageObject(Page);
            await payeesPageObject.CreatePayee("M", "10", "1010", "1010101", "01");

            var isSuccessMessageDisplayed = await payeesPageObject.IsSuccessMessageDisplayed();
            isSuccessMessageDisplayed.ShouldBeTrue();
            var isNewPayeeAddedInTheListOfPayees = await payeesPageObject.IsNewPayeeAddedInTheListOfPayees("10-1010-1010101-01");
            isNewPayeeAddedInTheListOfPayees.ShouldBeTrue();
        }

        [Test]
        public async Task VerifyPayeeNameIsARequiredField()
        {
            await Page.GotoAsync(pageUrl);
            payeesPageObject = new PayeesPageObject(Page);
            await payeesPageObject.ClickAddButton();
            await payeesPageObject.ClickAddButtonOnNewPayeeForm();
            var fieldError = await payeesPageObject.GetFieldError();
            fieldError.ShouldStartWith("Payee Name is a required field.");
            var payeeName = "L";
            await payeesPageObject.SetPayeeName(payeeName);
            fieldError = await payeesPageObject.GetFieldError();
            fieldError.ShouldBeNull();
        }

        [Test]
        public async Task VerifyThatPayeesCanBeSortedByName()
        {
            await Page.GotoAsync(pageUrl);
            payeesPageObject = new PayeesPageObject(Page);

            var payeeList = payeesPageObject.GetPayeeList().AllTextContentsAsync().Result;
            Assert.That(payeeList, Is.Ordered.Ascending);
            await payeesPageObject.ClickNameHeader();
            payeeList = payeesPageObject.GetPayeeList().AllTextContentsAsync().Result;
            Assert.That(payeeList, Is.Ordered.Descending);
        }
    }
}