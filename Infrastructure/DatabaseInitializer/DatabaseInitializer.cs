namespace Infrastructure.DatabaseInitializers;

public class DatabaseInitializer(IConfiguration _configuration, ApplicationDbContext _dbContext, IServiceProvider _serviceProvider) : IDatabaseInitializer
{
    public async Task MigrateDbsAsync()
    {
        #region [Update the license db schema]
        await _dbContext.Database.MigrateAsync();
        #endregion
    }
    public async Task SeedDataAsync()
    {
        bool res = await AddRolesMeta();
        await AddSuperadminAsync();

        await addEmployeeTypes();
        await addEmployementTypes();
        await addDocumentData();
        await SeedStarterFormQuestion();
        await SeedDocumentTypes();
    }
    private async Task<bool> SeedDocumentTypes()
    {
        var isTypeExist = await _dbContext.DocumentTypes.AnyAsync();
        // check if the category exist.
        if (!isTypeExist)
        {
            var types = new List<DocumentType>()
            {
                new DocumentType{ Name = "Passport", GroupNo = "Group 1a", IsActive = true },
                new DocumentType{ Name = "Biometric residence permit", GroupNo = "Group 1a", IsActive = true},
                new DocumentType{ Name = "Current driving licence photocard - (full or provisional)", GroupNo = "Group 1a", IsActive = true},
                new DocumentType{ Name = "Birth certificate - issued within 12 months of birth", GroupNo = "Group 1a", IsActive = true},
                new DocumentType{ Name = "Adoption certificate", GroupNo = "Group 1a", IsActive = true},
                new DocumentType{ Name = "Current driving licence photocard - (full or provisional)", GroupNo= "Group 2a", IsActive = true},
                new DocumentType{ Name = "Current driving licence (full or provisional) - paper version (if issued before 1998)", GroupNo= "Group 2a", IsActive = true},
                new DocumentType{ Name = "Birth certificate - issued after time of birth", GroupNo = "Group 2a", IsActive = true},
                new DocumentType{ Name = "Marriage/civil partnership certificate", GroupNo = "Group 2a", IsActive = true},
                new DocumentType{ Name = "HM Forces ID card", GroupNo = "Group 2a", IsActive = true},
                new DocumentType{ Name = "Firearms licence", GroupNo = "Group 2a", IsActive = true},
                new DocumentType{ Name = "Mortgage statement", GroupNo = "Group 2b", IsActive = true},
                new DocumentType{ Name = "Bank or building society statement", GroupNo = "Group 2b", IsActive = true},
                new DocumentType{ Name = "Bank or building society account opening confirmation letter", GroupNo = "Group 2b", IsActive = true},
                new DocumentType{ Name = "Credit card statement", GroupNo = "Group 2b", IsActive = true},
            };

            await _dbContext.DocumentTypes.AddRangeAsync(types);
            await _dbContext.SaveChangesAsync();
        }
        return true;
    }
    private async Task<bool> addDocumentData()
    {
        var Document = await _dbContext.Documents.AnyAsync();
        // check if the category exist.
        if (!Document)
        {
            var documents = new List<Document>()
            {
                new Document{ Id = 1, Name = "Coronavirus (COVID-19) Infection Prevention and Control",  EmployeeTypeId = 1, Order = 1, IsActive = true, IsDeleted = false },
                new Document{ Id = 2, Name = "COSHH",  EmployeeTypeId = 1, Order = 2, IsActive = true, IsDeleted = false },
                new Document{ Id = 3, Name = "Equality, Diversity and Human Rights",  EmployeeTypeId = 1, Order = 3, IsActive = true, IsDeleted = false },
                new Document{ Id = 4, Name = "Fire Safety",  EmployeeTypeId = 1, Order = 4, IsActive = true, IsDeleted = false },
                new Document{ Id = 5, Name = "First Aid In The Workplace",  EmployeeTypeId = 1, Order = 5, IsActive = true, IsDeleted = false },
                new Document{ Id = 6, Name = "Health, Safety and Welfare",  EmployeeTypeId = 1, Order = 6, IsActive = true, IsDeleted = false },
                new Document{ Id = 7, Name = "Infection Prevention and Control (Level 1)",  EmployeeTypeId = 1, Order = 7, IsActive = true, IsDeleted = false },
                new Document{ Id = 8, Name = "Information Governance, Record Keeping and Caldicott Protocols",  EmployeeTypeId = 1, Order = 8, IsActive = true, IsDeleted = false },
                new Document{ Id = 9, Name = "Moving and Handling Level 1",  EmployeeTypeId = 1, Order = 9, IsActive = true, IsDeleted = false },
                new Document{ Id = 10, Name = "Privacy And Dignity In Health And Social Care",  EmployeeTypeId = 1, Order = 10, IsActive = true, IsDeleted = false },
                new Document{ Id = 11, Name = "Safeguarding Adults (SOCA) Level  Nurses ",  EmployeeTypeId = 1, Order = 11, IsActive = true, IsDeleted = false },
                new Document{ Id = 12, Name = "Handling Medication & Avoiding Drug Errors – Level 2",  EmployeeTypeId = 1, Order = 12, IsActive = true, IsDeleted = false },
                new Document{ Id = 13, Name = "Medication Management",  EmployeeTypeId = 1, Order = 13, IsActive = true, IsDeleted = false },
                new Document{ Id = 14, Name = "Coronavirus (COVID-19) Infection Prevention and Control",  EmployeeTypeId = 2, Order = 14, IsActive = true, IsDeleted = false },
                new Document{ Id = 15, Name = "COSHH",  EmployeeTypeId = 2, Order = 15, IsActive = true, IsDeleted = false },
                new Document{ Id = 16, Name = "Equality, Diversity and Human Rights",  EmployeeTypeId = 2, Order = 16, IsActive = true, IsDeleted = false },
                new Document{ Id = 17, Name = "Fire Safety",  EmployeeTypeId = 2, Order = 17, IsActive = true, IsDeleted = false },
                new Document{ Id = 18, Name = "First Aid In The Workplace",  EmployeeTypeId = 2, Order = 18, IsActive = true, IsDeleted = false },
                new Document{ Id = 19, Name = "Health, Safety and Welfare",  EmployeeTypeId = 2, Order = 19, IsActive = true, IsDeleted = false },
                new Document{ Id = 20, Name = "Infection Prevention and Control (Level 1)",  EmployeeTypeId = 2, Order = 20, IsActive = true, IsDeleted = false },
                new Document{ Id = 21, Name = "Information Governance, Record Keeping and Caldicott Protocols",  EmployeeTypeId = 2, Order = 21, IsActive = true, IsDeleted = false },
                new Document{ Id = 22, Name = "Moving and Handling Level 1",  EmployeeTypeId = 2, Order = 22, IsActive = true, IsDeleted = false },
                new Document{ Id = 23, Name = "Privacy And Dignity In Health And Social Care",  EmployeeTypeId = 2, Order = 23, IsActive = true, IsDeleted = false },
                new Document{ Id = 24, Name = "Safeguarding Adults (SOCA) Level  Nurses ",  EmployeeTypeId = 2, Order = 24, IsActive = true, IsDeleted = false },
                new Document{ Id = 25, Name = "Dementia  Awareness",  EmployeeTypeId = 2, Order = 25, IsActive = true, IsDeleted = false },
                new Document{ Id = 26, Name = "Promoting Person Centered Care in Health and Social Care",  EmployeeTypeId = 2, Order = 26, IsActive = true, IsDeleted = false },
                new Document{ Id = 27, Name = "Dysphagia  Care",  EmployeeTypeId = 2, Order = 27, IsActive = true, IsDeleted = false },

            };

            await _dbContext.Documents.AddRangeAsync(documents);
            await _dbContext.SaveChangesAsync();
        }
        return true;
    }

    private async Task<bool> addEmployeeTypes()
    {
        var isCategoryExist = await _dbContext.EmployeeTypes.AnyAsync();
        // check if the category exist.
        if (!isCategoryExist)
        {
            var categories = new List<EmployeeType>()
            {
                new EmployeeType{ Id = 1, Name = "Nurses", Description = "Nursing can be described as both an art and a science; a heart and a mind", MinRate = (decimal)27.00, ParentId = 0, Order = 1, IsActive = true, IsDeleted = false },
                new EmployeeType{ Id = 2, Name = "Carers", Description = "a family member or paid helper who regularly looks after a child or a sick, elderly, or disabled person.", MinRate = (decimal)13.00, ParentId = 0, Order = 2, IsActive = true, IsDeleted = false },
                
            };

            await _dbContext.EmployeeTypes.AddRangeAsync(categories);
            await _dbContext.SaveChangesAsync();
        }
        return true;
    }  

    private async Task<bool> SeedStarterFormQuestion()
    {
        var isexist = await _dbContext.StarterFormQuestions.AnyAsync();
        // check if the category exist.
        if (!isexist)
        {
            var questions = new List<StarterFormQuestion>()
            {
                new StarterFormQuestion{ Question = "Do you have another job or in receipt of a state, works or private pension?", Order = 1, IsDeleted = false },
                new StarterFormQuestion{ Question = "Jobseekers allowance or incapacity benefits or employment and support allowance", Order = 2, IsDeleted = false },
                new StarterFormQuestion{ Question = "Jobseekers allowance or incapacity benefits or employment and support allowance", Order = 3, IsDeleted = false },
                new StarterFormQuestion{ Question = "Tell us if any of the following statements apply to you:", Order = 4, IsDeleted = false },
                new StarterFormQuestion{ Question = "Plan 1 : You have Plan 1 if any of the following apply:  you lived in Northern Ireland when you started your course  you lived in England or Wales and started your course before 1 September 2012", Order = 5, IsDeleted = false },
                new StarterFormQuestion{ Question = "Plan 2 : You have a Plan 2 if:  You lived in England or Wales and started your course on or after 1 September 2012.", Order = 6, IsDeleted = false },
                new StarterFormQuestion{ Question = "Plan 4 : You have a Plan 4 if:  You lived in Scotland and applied through the Students Award Agency Scotland (SAAS) when you started your course.", Order = 7, IsDeleted = false },
                new StarterFormQuestion{ Question = "Postgraduate Loan (England and Wales only) : You have a Postgraduate Loan if any of the following apply:  You lived in England and started your Postgraduate Master’s course on or after 1 August 2016  You lived in Wales and started your Postgraduate Master’s course on or after 1 August 2017  You lived in England or Wales and started your Postgraduate Doctoral course on or after 1 August 2018", Order = 8, IsDeleted = false },
            };

            await _dbContext.StarterFormQuestions.AddRangeAsync(questions);
            await _dbContext.SaveChangesAsync();
        }
        return true;
    }        
    private async Task<bool> addEmployementTypes()
    {
        var isCategoryExist = await _dbContext.EmployementTypes.AnyAsync();
        // check if the category exist.
        if (!isCategoryExist)
        {
            var employementType = new List<EmployementType>()
            {
                new EmployementType{ Id = 1, Name = "Self Employed", Description = "Self Employed", Order = 1, IsActive = true, IsDeleted = false },
                new EmployementType{Id = 2, Name = "Payroll", Description = "Payroll", Order = 2, IsActive = true, IsDeleted = false},
                new EmployementType{ Id = 3, Name = "Limited Company", Description = "Limited Company", Order = 3, IsActive = true, IsDeleted = false }
            };

            await _dbContext.EmployementTypes.AddRangeAsync(employementType);
            await _dbContext.SaveChangesAsync();
        }
        return true;
    }
    private async Task<bool> RoleNameExists(string Name, RoleManager<Roles> RoleService)
    {
        var role_obj = await RoleService.FindByNameAsync(Name);
        if (role_obj != null)
            return true;
        else
            return false;
    }
    public async Task<bool> AddRolesMeta()
    {
        var isSuccessfull = true;
        var _roleManager = _serviceProvider.GetRequiredService<RoleManager<Roles>>();
        var rolesList = Enum.GetValues(typeof(RoleEnum)).Cast<RoleEnum>().ToList();

        foreach (var role in rolesList)
        {
            var roleExist = await RoleNameExists(role.GetEnumDescription(), _roleManager);
            if (!roleExist)
            {
                var result = await _roleManager.CreateAsync(new Roles()
                {
                    Name = role.GetEnumDescription(),
                    Description = role.GetEnumDescription()
                });
                if (!result.Succeeded)
                {
                    isSuccessfull = false;
                    break;
                }
            }
        }
        return isSuccessfull;
    }
    public async Task AddSuperadminAsync()
    {
        var _userManager = _serviceProvider.GetRequiredService<UserManager<User>>();
        var uniqueGuid = Guid.NewGuid();
        var superUserName = _configuration["SystemadminUser:UserName"];
#pragma warning disable CS8604 // Possible null reference argument.
        var user = await _userManager.FindByNameAsync(superUserName);
#pragma warning restore CS8604 // Possible null reference argument.
        if (user == null)
        {
#pragma warning disable CS8601 // Possible null reference assignment.
#pragma warning disable CS8604 // Possible null reference argument.
            var userResult = await _userManager.CreateAsync(new User()
            {
                Id = uniqueGuid.ToString(),
                FirstName = _configuration["SystemadminUser:FirstName"],
                LastName = _configuration["SystemadminUser:LastName"],
                Email = _configuration["SystemadminUser:Email"],
                UserName = superUserName,
                EmailConfirmed = true,
                UserTypeId = (byte) UserTypeEnum.Admin
            }, _configuration["SystemadminUser:Password"]);
#pragma warning restore CS8604 // Possible null reference argument.
            if (userResult.Succeeded)
            {
                user = await _userManager.FindByNameAsync(superUserName);
                var addToRoleResult = await _userManager.AddToRoleAsync(user, nameof(RoleEnum.SuperAdmin));
            }
#pragma warning restore CS8601 // Possible null reference assignment.

        }
    }

}
