using Univisia.Finance.Data;
using Univisia.Finance.Data.Models;
using Microsoft.EntityFrameworkCore;
using Univisia.Finance.Data.Repositories;
using Univisia.Finance.Services;
using Univisia.Finance.Web.Controllers;
using Moq;
using Microsoft.AspNetCore.Identity;
using Univisia.Finance.Services.JWtFeature;
using Univisia.Finance.Services.Services;
using Univisia.Finance.Web.ViewModels.LoanView;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Security.Policy;

namespace Univisia.Finance.Test.Controllers
{
    public class LoanControllerTests
    {
        public DbContextOptions<FinanceDbContext> dbContextOptions;

        public LoanControllerTests()
        {
            dbContextOptions = new DbContextOptionsBuilder<FinanceDbContext>()
                                .UseInMemoryDatabase(databaseName: "UnivisiaFinanceDb")
                                .Options;
        }

        //[Fact]
        //public async Task Test_GetAllLoans()
        //{
        //    var expected = 2;
        //    Func<LoanController> loanController = CreateController;
        //    //if superadmin2@test.mail not admin expected 1 loan
        //    //if superadmin@test.mail Admin expected 2 loans
        //    var controller = new TestControllerBuilder()
        //        .WithIdentity("john.doe@mail.com", "superadmin@test.mail")
        //        .Build<LoanController>(loanController);

        //    var result = await controller.GetLoansAsync();
        //    Assert.Equal(expected, result.Count());
        //}

        [Fact]
        public async Task Test_AddLoan()
        {
            var expected = 3;
            Func<LoanController> loanController = CreateController;
            var controller = new TestControllerBuilder()
                .WithIdentity("john.doe@mail.com", "superadmin@test.mail")
                .Build<LoanController>(loanController);

            var loanView = new LoanViewModel()
            {
                CurrencyId = 1,
                CurrencyName = "USD",
                Person = "AddedPerson",
                Note = "AddedNote",
                Amount = 1000,
            };
            //if userId = 2 we have 1 out of total 3 loans seeded, after adding 1 loan expected should be 2
            //if userId = 1 role = "Admin" have total 2 seeded that admin can see, add 1, expected for admin 3
            var actionResult = await controller.AddLoanAsync(loanView) as OkResult;
            Assert.IsType<OkResult>(actionResult);

            var result = await controller.GetLoansAsync();
            Assert.Equal(expected, result.Count());
        }

        [Fact]
        public async Task Test_RemoveLoan()
        {
            Func<LoanController> loanController = CreateController;
            //conroller is creted with Identity of user with UserId = 2
            var controller = new TestControllerBuilder()
                .WithIdentity("john.doe@mail.com", "superadmin2@test.mail")
                .Build<LoanController>(loanController);

            //Loan with LoanId = 1, is creted in SeedDB() by Userid = 1, so should not be deleted
            var loanId = 1;
            var deleteLoanView = new DeleteLoanViewModel();
            deleteLoanView.LoanId = loanId;

            var actionResult = await controller.DeleteLoanAsync(deleteLoanView) as OkResult;
            //test will pass since Assert.IsNotType<OkResult>
            Assert.IsNotType<OkResult>(actionResult);
        }

        [Fact]
        public async Task Test_UpdateLoan()
        {
            SeedDb();
            LoanRepository loanRepository = new LoanRepository(new FinanceDbContext(dbContextOptions));
            GenericRepository<UserDetail> userDetailsRepository = new GenericRepository<UserDetail>(new FinanceDbContext(dbContextOptions));
            var loanService = new LoanService(loanRepository);
            var userManager = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
            var configuration = Mock.Of<Microsoft.Extensions.Configuration.IConfiguration>();
            var jwtHandler = new JwtHandler(configuration, userManager.Object);
            var userDetailsService = new UserDetailsService(userDetailsRepository, userManager.Object, jwtHandler);

            Func<LoanController> loanController = (() => new LoanController(loanService, userDetailsService));
            //conroller is creted with Identity of user with UserId = 3
            var controller = new TestControllerBuilder()
                .WithIdentity("john.doe@mail.com", "superadmin3@test.mail")
                .Build<LoanController>(loanController);

            //Loan with LoanId = 1, is creted in SeedDB() by Userid = 1, this loan should not be updated
            var loanId = 1;
            var loanView = new LoanViewModel();
            loanView.LoanId = loanId;
            loanView.Person = "EditPerson";
            loanView.Note = "EditNote";
            loanView.Amount = 1000000;
            //test will pass since Assert.IsNotType<OkResult>
            var actionResult = await controller.UpdateLoanAsync(loanView) as OkResult;
            Assert.IsNotType<OkResult>(actionResult);
        }

        private LoanController CreateController()
        {
            SeedDb();
            LoanRepository loanRepository = new LoanRepository(new FinanceDbContext(dbContextOptions));
            GenericRepository<UserDetail> userDetailsRepository = new GenericRepository<UserDetail>(new FinanceDbContext(dbContextOptions));
            var loanService = new LoanService(loanRepository);
            var userManager = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
            var configuration = Mock.Of<Microsoft.Extensions.Configuration.IConfiguration>();
            var jwtHandler = new JwtHandler(configuration, userManager.Object);
            var userDetailsService = new UserDetailsService(userDetailsRepository, userManager.Object, jwtHandler);

            return new LoanController(loanService, userDetailsService);
        }

        private void SeedDb()
        {
            using FinanceDbContext financeDbContext = new FinanceDbContext(dbContextOptions);
            financeDbContext.Database.EnsureDeleted();

            var userDetails = new List<UserDetail>
            {
                new UserDetail
                    {
                    UserDetailId = 1,
                    Email = "superadmin@test.mail",
                    FirstName = "Test",
                    LastName = "Test",
                    DisplayName = "Test",
                    Description = "Test",
                    Status = "Admin",
                    UserId = "5f0d15b4-0c40-49e8-9352-422088f4e30e",
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    IsLoggedBySocialNetwork = false
                    },
                new UserDetail
                {
                    UserDetailId = 2,
                    Email = "superadmin2@test.mail",
                    FirstName = "Test",
                    LastName = "Test",
                    DisplayName = "Test",
                    Description = "Test",
                    Status = "",
                    UserId = "5f0d15b4-0c40-49e8-9352-422088f4e30f",
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    IsLoggedBySocialNetwork = false
                },
                new UserDetail
                {
                    UserDetailId = 3,
                    Email = "superadmin3@test.mail",
                    FirstName = "Test",
                    LastName = "Test",
                    DisplayName = "Test",
                    Description = "Test",
                    Status = "",
                    UserId = "5f0d15b4-0c40-49e8-9352-422088f4e30g",
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    IsLoggedBySocialNetwork = false
                }
            };
            var loans = new List<Loan>
            {
                new Loan
                {
                    LoanId = 1,
                    CurrencyId = 1,
                    Note = "Note1",
                    Person = "Person1",
                    Amount = 100,
                    CreatedBy = "superadmin@test.mail",
                    CreatedDate = DateTime.Now,
                    IsDeleted = false,
                    UserDetailId = 1

                },
                new Loan
                {
                    LoanId = 2,
                    CurrencyId = 2,
                    Note = "Note2",
                    Person = "Person2",
                    Amount = 200,
                    CreatedBy = "superadmin@test.mail",
                    CreatedDate = DateTime.Now,
                    IsDeleted = false,
                    UserDetailId = 1
                },
                new Loan
                {
                    LoanId = 3,
                    CurrencyId = 1,
                    Note = "Note3",
                    Person = "Person3",
                    Amount = 300,
                    CreatedBy = "superadmin2@test.mail",
                    CreatedDate = DateTime.Now,
                    IsDeleted = false,
                    UserDetailId = 2
                }
            };
            var users = new List<User>
            {
                new User
                {
                    FirstName = "Test",
                    LastName = "Test",
                }
            };
            var currencies = new List<Currency>
            {
                new Currency
                {
                    CurrencyId = 1, CurrencyName = "USD"
                },
                new Currency
                {
                    CurrencyId = 2, CurrencyName = "EUR"
                }
            };
            financeDbContext.AddRange(userDetails);
            financeDbContext.AddRange(loans);
            financeDbContext.AddRange(currencies);
            financeDbContext.AddRange(users);

            financeDbContext.SaveChanges();
        }
    }
}