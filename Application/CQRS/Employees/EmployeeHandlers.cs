using Application.Abstractions.Services;

namespace Domain.CQRS.Employees;

public sealed class RegisterCommandHandlers(
    UserManager<User> _userManager,
    IMediator _mediator,
    IFileHelper _fileHelper,
    IUnitOfWork _unitOfWork,
    INotificationService _notificationService,
    IMailService _mailService) : IRequestHandler<RegisterCommand, Result<IEnumerable<string>>>
{
    public async Task<Result<IEnumerable<string>>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        // Compare the Password and ConfirmPassword fields.
        if (request.Register.Password != request.Register.ConfirmPassword)
        {
            return Result<IEnumerable<string>>.Fail("Confirm password doesn't match the password");
        }

        // check gender is valid or not.
        var genders = new List<string>()
        {
            "Single",
            "Married",
            "Divorced"
        };
        var isValid = genders.Contains(request.Register.MaritalStatus.Trim());
        if (!isValid)
        {
            return Result<IEnumerable<string>>.Fail("Invalid gender.");
        }

        // check is employement type exists.
        var employementType = await _unitOfWork
            .EmployementTypeRepository
            .GetAllReadOnly()
            .FirstOrDefaultAsync(type => type.Id == Convert.ToByte(request.Register.EmployementTypeId), cancellationToken);
        if (employementType is null)
        {
            return Result<IEnumerable<string>>.Fail($"Employement type doesn't exist with Id: {request.Register.EmployementTypeId}.");
        }

        // check is Employee type exists.
        var employeeType = await _unitOfWork
            .EmployeeTypeRepository
            .GetAllReadOnly()
            .FirstOrDefaultAsync(type => type.Id == Convert.ToByte(request.Register.EmployeeTypeId), cancellationToken);
        if (employeeType is null)
        {
            return Result<IEnumerable<string>>.Fail($"Employee type doesn't exist with Id: {request.Register.EmployeeTypeId}.");
        }

        var user = new User
        {
            Email = request.Register.Email,
            UserName = request.Register.Email,
            FirstName = request.Register.FirstName,
            LastName = request.Register.LastName,
            EmailConfirmed = false,
            PhoneNumber = string.Concat(request.Register.PhoneRegion, request.Register.Phone),
            PhoneNumberConfirmed = false,
            PlainPassword = request.Register.Password,
            UserTypeId = (byte)UserTypeEnum.Employee
        };

        var result = await _userManager.CreateAsync(user, request.Register.Password);

        if (result.Succeeded)
        {
            // save file
            string cvFileUrl = request.Register.File != null ? await _fileHelper.UploadFile("CV", request.Register.File) : "";

            // Handle employee.
            var employee = new AddEmployeeRequest()
            {
                UserId = new Guid(user.Id),
                Title = request.Register.Title,
                FirstName = request.Register.FirstName,
                LastName = request.Register.LastName,
                MaritalStatus = request.Register.MaritalStatus.Trim(),
                PinCode = request.Register.PinCode,
                PhoneNumber = request.Register.Phone,
                CountryCode = request.Register.PhoneRegion,
                Email = request.Register.Email,
                EmployeeTypeId = Convert.ToByte(request.Register.EmployeeTypeId),
                EmployementTypeId = Convert.ToByte(request.Register.EmployementTypeId),
                Latitude = request.Register.Latitude,
                Longitude = request.Register.Longitude,
                Country = request.Register.Country,
                CVFileURL = cvFileUrl,
            };

            var employeeResult = await _mediator.Send(new AddEmployeeCommand(employee));
            if (employeeResult.Succeeded)
            {
                // Add role to user
                await _userManager.AddToRoleAsync(user, nameof(RoleEnum.Employee));

                // send emails.
                _ = Task.Run(() =>
                {
                    _mailService.SendConfirmationEmailAsync(user.Email, Encryption.Crypt(user.Id.ToString()));
                    _mailService.SendConfirmationEmailToAdminAsync($"{user.FirstName} {user.LastName}", user.CreatedOnUtc.ToString("yyyy/MM/dd"));


                });

                // send notification.
                var notification = new Notification()
                {
                    Date = DateTime.UtcNow,
                    Type = (byte)NotificationTypeEnum.Employee,
                    Content = $"{employee.FirstName} {employee.LastName} has registered on {DateTime.UtcNow.ToString("yyyy-MM-dd")}",
                    EmployeeId = employeeResult.Data
                };
                await _unitOfWork.NotificationsRepository.Add(notification);
                await _unitOfWork.SaveChangesAsync();
                await _notificationService.TriggerNotification();
                //await  _mailService.SendConfirmationEmailToEmployeeAsync(user.Email, Encryption.Crypt(user.Id.ToString()));

                return Result<IEnumerable<string>>.Success(new List<string>(), "User created successfully!");
            }
        }
        return Result<IEnumerable<string>>.Fail(result.Errors.Select(e => e.Description).FirstOrDefault(), new List<string>());
    }
}

public sealed class LoginCommandHandlers(UserManager<User> _userManger, IConfiguration _configuration, IUnitOfWork _unitOfWork)
    : IRequestHandler<LoginCommand, Result<LoginResponse>>
{
    public async Task<Result<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        // login with email.
        var user = await _userManger.FindByEmailAsync(request.Login.Email);
        if (user is null)
        {
            return Result<LoginResponse>.Fail("There is no user.");
        }

        // check is password correct.
        var result = await _userManger.CheckPasswordAsync(user, request.Login.Password);
        if (!result)
        {
            return Result<LoginResponse>.Fail("Invalid credentials.");
        }

        // is employee exists.
        var employee = await _unitOfWork
            .EmployeeRepository
            .GetAll()
            .FirstOrDefaultAsync(x => x.UserId.Equals(new Guid(user.Id)), cancellationToken);
        if (employee is null)
        {
            return Result<LoginResponse>.Fail("There is no user.");
        }

        // add claims in jwt token.
        var claims = new[] {
                new Claim("Email", request.Login.Email),
                new Claim("EmployeeId", employee.Id.ToString()),
                new Claim("role", nameof(RoleEnum.Employee)),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AuthSettings:Key"]));
        var token = new JwtSecurityToken(
            issuer: _configuration["AuthSettings:Issuer"],
            audience: _configuration["AuthSettings:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddDays(1),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

        string tokenAsString = new JwtSecurityTokenHandler().WriteToken(token);

        LoginResponse response = new LoginResponse()
        {
            ExpireDate = token.ValidTo,
            Token = tokenAsString,
            Country = employee.Country,
            EmployeeTypeId = employee.EmployeeTypeId,
            EmployementTypeId = employee.EmployementTypeId,
            AccountStatus = employee.AccountStatus
        };

        // return response.
        return Result<LoginResponse>.Success(response, "User logged in successfully!");
    }
}
public sealed class AddEmployeeHandler(IUnitOfWork _unitOfWork) : IRequestHandler<AddEmployeeCommand, Result<int>>
{
    async Task<Result<int>> IRequestHandler<AddEmployeeCommand, Result<int>>.Handle(AddEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = new Employee()
        {
            UserId = request.Employee.UserId,
            Title = request.Employee.Title,
            FirstName = request.Employee.FirstName,
            LastName = request.Employee.LastName,
            MaritalStatus = request.Employee.MaritalStatus,
            PinCode = request.Employee.PinCode,
            PhoneNumber = request.Employee.PhoneNumber,
            CountryCode = request.Employee.CountryCode,
            Email = request.Employee.Email,
            EmployeeTypeId = request.Employee.EmployeeTypeId,
            EmployementTypeId = request.Employee.EmployementTypeId,
            CVFileURL = request.Employee.CVFileURL,
            NurseTypeId = (byte)NurseTypeEnum.UnKnown,
            PersonalLink = $"personalref/{request.Employee.UserId}",
            ProfessionalLink = $"professionalref/{request.Employee.UserId}",
        };

        await _unitOfWork.EmployeeRepository.Add(employee);
        await _unitOfWork.SaveChangesAsync();

        return Result<int>.Success(employee.Id);
    }
}

public sealed class LoginAPIsHandler(IUnitOfWork _unitOfWork) : IRequestHandler<SignUpAPIsQuery, Result<SignUpAPIsResponse>>
{
    public async Task<Result<SignUpAPIsResponse>> Handle(SignUpAPIsQuery request, CancellationToken cancellationToken)
    {
        var loginAPIsResponse = new SignUpAPIsResponse();

        // get employee type.
        loginAPIsResponse.EmployeeTypes = await _unitOfWork.EmployeeTypeRepository.GetEmployeeTypes().ToListAsync(cancellationToken);

        // get employement type.
        loginAPIsResponse.EmployementTypes = await _unitOfWork.EmployementTypeRepository.GetEmployementTypes().ToListAsync(cancellationToken);


        return Result<SignUpAPIsResponse>.Success(loginAPIsResponse, "Lists for login.");
    }
}

public sealed class AddVaccinationHandler(IUnitOfWork _unitOfWork, IFileHelper _fileHelper) : IRequestHandler<AddVaccinationQuery, Result<bool>>
{
    public async Task<Result<bool>> Handle(AddVaccinationQuery request, CancellationToken cancellationToken)
    {
        // check is employee exists.
        var employee = await _unitOfWork.EmployeeRepository.GetById(request.Request.EmployeeId);
        if (employee is null)
        {
            return Result<bool>.Fail("Employee doesn't exist.");
        }
        string message = string.Empty;
        // mean need to upload vaccination certificate.
        if (request.Request.IsVaccinated)
        {
            employee.VaccinationCertificateUrl = await _fileHelper.UploadFile("VaccinationCertificates", request.Request.File);
            message = "Vaccination";
        }
        //mean need to upload proof of exemption.
        else
        {
            employee.ProofOfExemptionUrl = await _fileHelper.UploadFile("ProofOfExemptions", request.Request.File);
            message = "Proof of exemption";
        }

        await _unitOfWork.SaveChangesAsync();

        return Result<bool>.Success(true, $"{message} details updated successfully.");
    }
}

public sealed class AddAddressHandler(IUnitOfWork _unitOfWork) : IRequestHandler<AddAddressCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(AddAddressCommand request, CancellationToken cancellationToken)
    {
        // check is employee exists.
        var employee = await _unitOfWork.EmployeeRepository.GetById(request.Address.EmployeeId);
        if (employee is null)
        {
            return Result<bool>.Fail("Employee doesn't exist.");
        }

        employee.Address = request.Address.Address;
        employee.Address2 = request.Address.Address2;
        employee.City = request.Address.City;
        employee.PinCode = request.Address.PinCode;
        employee.Latitude = request.Address.Latitude;
        employee.Longitude = request.Address.Longitude;
        employee.Country = request.Address.Country;

        await _unitOfWork.SaveChangesAsync();

        return Result<bool>.Success(true, "Address details updated successfully.");
    }
}

public sealed class AddShiftsHandler(IUnitOfWork _unitOfWork) : IRequestHandler<AddShiftsQuery, Result<bool>>
{
    public async Task<Result<bool>> Handle(AddShiftsQuery request, CancellationToken cancellationToken)
    {
        // check is employee exists.
        var employee = await _unitOfWork.EmployeeRepository.GetById(request.Request.EmployeeId);
        if (employee is null)
        {
            return Result<bool>.Fail("Employee doesn't exist.");
        }

        employee.NoOfShifts = request.Request.NoOfShifts;

        await _unitOfWork.SaveChangesAsync();

        return Result<bool>.Success(true, "No of shifts detail updated successfully.");
    }
}

public sealed class AddNMCRegistrationHandler(IUnitOfWork _unitOfWork) : IRequestHandler<AddNMCRegistrationCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(AddNMCRegistrationCommand request, CancellationToken cancellationToken)
    {
        // check nurse type exists.
        var isExist = Enum.IsDefined(typeof(NurseTypeEnum), request.Request.NurseTypeId);
        if (!isExist)
            return Result<bool>.Fail("Invalid nurse type.");

        // check is employee exists.
        var employee = await _unitOfWork.EmployeeRepository.GetById(request.Request.EmployeeId);
        if (employee is null)
        {
            return Result<bool>.Fail("Employee doesn't exist.");
        }

        employee.DateOfQualification = request.Request.DateOfQualification;
        employee.NurseTypeId = request.Request.NurseTypeId;
        employee.NMCPin = request.Request.NMCPin;
        employee.YearsOfExperience = request.Request.YearsOfExperience;
        employee.NMCPinStatus = (byte)NMCPinStatusEnum.Pending;

        await _unitOfWork.SaveChangesAsync();

        return Result<bool>.Success(true, "NMC registration details added successfully.");
    }
}

public sealed class AddQualificationHandler(IUnitOfWork _unitOfWork) : IRequestHandler<AddQualificationCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(AddQualificationCommand request, CancellationToken cancellationToken)
    {
        // check is employee exists.
        var employee = await _unitOfWork.EmployeeRepository.GetById(request.Request.EmployeeId);
        if (employee is null)
        {
            return Result<bool>.Fail("Employee doesn't exist.");
        }

        // validate the DateOfAward
        if (request.Request.DateOfAward.Date > DateTime.UtcNow.Date)
        {
            return Result<bool>.Fail("Can't accetp future date.");
        }

        var qualification = new Qualification()
        {
            EmployeeId = request.Request.EmployeeId,
            Course = request.Request.Course,
            DateOfAward = request.Request.DateOfAward,
            AwardingBody = request.Request.AwardingBody
        };

        await _unitOfWork.QualificationRepository.Add(qualification);
        await _unitOfWork.SaveChangesAsync();

        return Result<bool>.Success(true, "Qualification added successfully.");
    }
}

public sealed class DeleteQualificationHandler(IUnitOfWork _unitOfWork) : IRequestHandler<DeleteQualificationCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(DeleteQualificationCommand request, CancellationToken cancellationToken)
    {
        // check is qualification exists.
        var qualification = await _unitOfWork.QualificationRepository.GetById(request.Request.QualificationId);
        if (qualification is null)
        {
            return Result<bool>.Fail("Qualification doesn't exist.");
        }

        if (qualification.EmployeeId != request.Request.EmployeeId)
        {
            return Result<bool>.Fail("You're not allow to delete that qualification.");
        }

        _unitOfWork.QualificationRepository.DeleteByEntity(qualification);
        await _unitOfWork.SaveChangesAsync();

        return Result<bool>.Success(true, "Qualification deleted successfully.");
    }
}

public sealed class DeleteDbsDocumentHandler(IUnitOfWork _unitOfWork) : IRequestHandler<DeleteDbsDocumentCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(DeleteDbsDocumentCommand request, CancellationToken cancellationToken)
    {
        // check is employee exists.
        var employee = await _unitOfWork.EmployeeRepository.GetById(request.Request.EmployeeId);
        if (employee is null)
        {
            return Result<bool>.Fail("Employee doesn't exist.");
        }

        // get dbs document.
        var dbsDocument = await _unitOfWork
            .DbsDocumentRepository
            .GetAll()
            .FirstOrDefaultAsync(document => document.Id == request.Request.DocumentId && document.EmployeeId == request.Request.EmployeeId);
        if (dbsDocument is null)
        {
            return Result<bool>.Fail("Document doesn't exist.");
        }

        dbsDocument.IsActive = false;
        dbsDocument.IsDeleted = true;

        _unitOfWork.DbsDocumentRepository.Change(dbsDocument);
        await _unitOfWork.SaveChangesAsync();

        return Result<bool>.Success(true, "Document deleted successfully.");
    }
}

public sealed class GetQualificationsByEmployeeHandler(IUnitOfWork _unitOfWork)
    : IRequestHandler<GetQualificationsByEmployeeQuery, Result<List<GetQualificationsByEmployeeResponse>>>
{
    public async Task<Result<List<GetQualificationsByEmployeeResponse>>> Handle(GetQualificationsByEmployeeQuery request, CancellationToken cancellationToken)
    {
        // check is employee exists.
        var employee = await _unitOfWork.EmployeeRepository.GetById(request.EmployeeId);
        if (employee is null)
        {
            return Result<List<GetQualificationsByEmployeeResponse>>.Fail("Employee doesn't exist.");
        }

        // get qualifications by employee.
        var qualification = await _unitOfWork
            .QualificationRepository
            .GetQualificationsByEmployeeId(request.EmployeeId)
            .ToListAsync(cancellationToken);

        return Result<List<GetQualificationsByEmployeeResponse>>.Success(qualification, "Qualifications successfully collected.");
    }
}

public sealed class AddIsSubjectOfInvestigationHandler(IUnitOfWork _unitOfWork) : IRequestHandler<AddIsSubjectOfInvestigationQuery, Result<bool>>
{
    public async Task<Result<bool>> Handle(AddIsSubjectOfInvestigationQuery request, CancellationToken cancellationToken)
    {
        // check is employee exists.
        var employee = await _unitOfWork.EmployeeRepository.GetById(request.Request.EmployeeId);
        if (employee is null)
        {
            return Result<bool>.Fail("Employee doesn't exist.");
        }

        employee.IsSubjectOfInvestigation = request.Request.IsSubjectOfInvestigation;
        await _unitOfWork.SaveChangesAsync();

        return Result<bool>.Success(true, "Updated successfully.");
    }
}

public sealed class AddHaveQualificationHandler(IUnitOfWork _unitOfWork) : IRequestHandler<AddHaveQualificationQuery, Result<bool>>
{
    public async Task<Result<bool>> Handle(AddHaveQualificationQuery request, CancellationToken cancellationToken)
    {
        // check is employee exists.
        var employee = await _unitOfWork.EmployeeRepository.GetById(request.Request.EmployeeId);
        if (employee is null)
        {
            return Result<bool>.Fail("Employee doesn't exist.");
        }

        employee.HaveQualification = request.Request.HaveQualification;
        await _unitOfWork.SaveChangesAsync();

        return Result<bool>.Success(true, "Updated successfully.");
    }
}

public sealed class AddEmployementHandler(IUnitOfWork _unitOfWork) : IRequestHandler<AddEmployementCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(AddEmployementCommand request, CancellationToken cancellationToken)
    {
        // check is employee exists.
        var employee = await _unitOfWork.EmployeeRepository.GetById(request.Request.EmployeeId);
        if (employee is null)
        {
            return Result<bool>.Fail("Employee doesn't exist.");
        }

        if (request.Request.StartDate.Date > DateTime.UtcNow.Date)
        {
            return Result<bool>.Fail("Start date should be less than today.");
        }

        if (request.Request.EndDate.Date > DateTime.UtcNow.Date)
        {
            return Result<bool>.Fail("End date should be less than today.");
        }

        if (request.Request.StartDate.Date > request.Request.EndDate.Date)
        {
            return Result<bool>.Fail("Joining date should be less than leaving date.");
        }

        var employement = new Employement()
        {
            EmployeeId = Convert.ToInt32(request.Request.EmployeeId),
            CompanyName = request.Request.CompanyName,
            CompanyAddress = request.Request.CompanyAddress,
            EndDate = request.Request.EndDate,
            Position = request.Request.Position,
            ReasonForLeaving = request.Request.ReasonForLeaving,
            StartDate = request.Request.StartDate
        };

        await _unitOfWork.EmployementRepository.Add(employement);
        await _unitOfWork.SaveChangesAsync();

        return Result<bool>.Success(true, "Employement added successfully.");
    }
}

public sealed class GetEmployementsByEmployeeHandler(IUnitOfWork _unitOfWork) : IRequestHandler<GetEmployementsByEmployeeQuery, Result<List<GetEmployementsByEmployeeResponse>>>
{
    public async Task<Result<List<GetEmployementsByEmployeeResponse>>> Handle(GetEmployementsByEmployeeQuery request, CancellationToken cancellationToken)
    {
        // check is employee exists.
        var employee = await _unitOfWork.EmployeeRepository.GetById(request.EmployeeId);
        if (employee is null)
        {
            return Result<List<GetEmployementsByEmployeeResponse>>.Fail("Employee doesn't exist.");
        }

        // get Employements by employee.
        var employements = await _unitOfWork
            .EmployementRepository
            .GetEmployementsByEmployeeId(request.EmployeeId)
            .ToListAsync(cancellationToken);

        return Result<List<GetEmployementsByEmployeeResponse>>.Success(employements, "Employements successfully collected.");
    }
}

public sealed class DeleteEmployementHandler(IUnitOfWork _unitOfWork) : IRequestHandler<DeleteEmployementCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(DeleteEmployementCommand request, CancellationToken cancellationToken)
    {
        // check is qualification exists.
        var employement = await _unitOfWork.EmployementRepository.GetById(request.Request.EmployementId);
        if (employement is null)
        {
            return Result<bool>.Fail("Employement doesn't exist.");
        }

        if (employement.EmployeeId != request.Request.EmployeeId)
        {
            return Result<bool>.Fail("You're not allow to delete that employement.");
        }

        _unitOfWork.EmployementRepository.DeleteByEntity(employement);
        await _unitOfWork.SaveChangesAsync();

        return Result<bool>.Success(true, "Employement deleted successfully.");
    }
}

public sealed class AddWorkGapReasonHandler(IUnitOfWork _unitOfWork) : IRequestHandler<AddWorkGapReasonCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(AddWorkGapReasonCommand request, CancellationToken cancellationToken)
    {
        // check is employee exists.
        var employee = await _unitOfWork.EmployeeRepository.GetById(request.Request.EmployeeId);
        if (employee is null)
        {
            return Result<bool>.Fail("Employee doesn't exist.");
        }

        employee.WorkGapReason = request.Request.Reason;
        await _unitOfWork.SaveChangesAsync();

        return Result<bool>.Success(true, "Updated successfully.");
    }
}

public sealed class SendReferenceLinkToMailHandler(IUnitOfWork _unitOfWork, IMailService _mailService, IConfiguration _configuration)
    : IRequestHandler<SendReferenceLinkToMailCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(SendReferenceLinkToMailCommand request, CancellationToken cancellationToken)
    {
        // check nurse type exists.
        var isExist = Enum.IsDefined(typeof(ReferenceTypeEnum), request.Request.ReferenceType);
        if (!isExist)
            return Result<bool>.Fail("Invalid reference type.");

        // check is employee exists.
        var employee = await _unitOfWork.EmployeeRepository.GetById(request.Request.EmployeeId);
        if (employee is null)
        {
            return Result<bool>.Fail("Employee doesn't exist.");
        }

        string content = string.Empty;
        if (request.Request.ReferenceType == (byte)ReferenceTypeEnum.Personal)
        {
            // get personal email content.
            content = GetEmployeePersonalEmailContent(employee);
        }
        else
        {
            // get professional email content.
            content = GetEmployeeProfessionalEmailContent(employee);
        }

        string username = "Sir/Madam";
        string subject = "ESGO reference";

        // send email.
        _ = Task.Run(() =>
        {
            _mailService.Send(request.Request.Email, subject, content, username);
        });

        List<string> emails = employee.LinkSharedOnEmails.Split(", ").ToList();
        if (!emails.Any(x => x.ToUpper().Trim() == request.Request.Email.ToUpper().Trim()))
        {
            emails.RemoveAll(x => string.IsNullOrEmpty(x) || string.IsNullOrWhiteSpace(x));
            emails.Add(request.Request.Email);
            employee.LinkSharedOnEmails = string.Join(", ", emails);

            await _unitOfWork.SaveChangesAsync();
        }

        return Result<bool>.Success(true, "Email sent.");
    }
    private string GetEmployeePersonalEmailContent(Employee employee)
    {
        string content = $"{employee.FirstName} {employee.LastName} has applied to join ESGO as a flexible worker and has provided your email and consent to approach you for a reference.";
        content = content + "<P style='color: #000; font-weight: 400; font-size: 18px; padding: 20px 10px 0px 10px;'> I would be grateful if you could complete the Reference Request Form on the link below bearing in mind your knowledge of the applicant.</p>";
        content = content + "<P style='color: #000; font-weight: 400; font-size: 18px; padding: 20px 10px 0px 10px;'><B>Link :</B> <a href='[Ref_Link]'>[Ref_Link]</a> </p> ";
        content = content + "<P style='color: #000; font-weight: 400; font-size: 18px; padding: 20px 10px 0px 10px;'> We would appreciate your co-operation in completing the form with full and accurate information.</p>";
        content = content + "<P style='color: #000; font-weight: 400; font-size: 18px; padding: 20px 10px 0px 10px;'> Please note that under the Data Protection Act of 1998 and Freedom of Information Act 2000 candidates may request access to any information that is held on them.</p>";
        content = content + "<P style='color: #000; font-weight: 400; font-size: 18px; padding: 20px 10px 0px 10px;'> All references are handled in accordance with the ESGO Referencing Policy.</p>";
        content = content.Replace("[Ref_Link]", $"{_configuration["WebsiteSetting:Url"]}/{employee.PersonalLink}");

        return content;
    }
    private string GetEmployeeProfessionalEmailContent(Employee employee)
    {
        string content = $"{employee.FirstName} {employee.LastName} has applied to join ESGO as a flexible worker and has provided your email and consent to approach you for a reference.";
        content = content + "<P style='color: #000; font-weight: 400; font-size: 18px; padding: 20px 10px 0px 10px;'> I would be grateful if you could complete the Reference Request Form on the link below bearing in mind your knowledge of the applicant.</p>";
        content = content + "<P style='color: #000; font-weight: 400; font-size: 18px; padding: 20px 10px 0px 10px;'><B>Link :</B> <a href='[Ref_Link]'>[Ref_Link]</a> </p> ";
        content = content + "<P style='color: #000; font-weight: 400; font-size: 18px; padding: 20px 10px 0px 10px;'> We would appreciate your co-operation in completing the form with full and accurate information, in line with your organization's HR and referencing policies.</p>";
        content = content + "<P style='color: #000; font-weight: 400; font-size: 18px; padding: 20px 10px 0px 10px;'> Please note that under the Data Protection Act of 1998 and Freedom of Information Act 2000 candidates may request access to any information that is held on them.</p>";
        content = content + "<P style='color: #000; font-weight: 400; font-size: 18px; padding: 20px 10px 0px 10px;'> All references are handled in accordance with the ESGO Referencing Policy.</p>";
        content = content.Replace("[Ref_Link]", $"{_configuration["WebsiteSetting:Url"]}/{employee.ProfessionalLink}");

        return content;
    }
}

public sealed class GenerateReferenceLinkHandler(IUnitOfWork _unitOfWork, IConfiguration _configuration) : IRequestHandler<GenerateReferenceLinkCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(GenerateReferenceLinkCommand request, CancellationToken cancellationToken)
    {
        // check is employee exists.
        var employee = await _unitOfWork.EmployeeRepository.GetById(request.EmployeeId);
        if (employee is null)
        {
            return Result<bool>.Fail("Employee doesn't exist.");
        }

        if (string.IsNullOrEmpty(employee.PersonalLink) || string.IsNullOrWhiteSpace(employee.PersonalLink))
        {
            // generate personal url for employee.
            string personalUrl = _configuration["ReferenceLinksSetting:PersonalURL"];
            employee.PersonalLink = $"{personalUrl}{employee.EmployeeSecretId}";
        }

        if (string.IsNullOrEmpty(employee.ProfessionalLink) || string.IsNullOrWhiteSpace(employee.ProfessionalLink))
        {
            // generate professional url for employee.
            string professionalUrl = _configuration["ReferenceLinksSetting:ProfessionalURL"];
            employee.ProfessionalLink = $"{professionalUrl}{employee.EmployeeSecretId}";
        }

        // update employee
        await _unitOfWork.SaveChangesAsync();

        return Result<bool>.Success(true, "Links generated successfully.");
    }
}

public sealed class AddPersonalReferenceHandler(IUnitOfWork _unitOfWork) : IRequestHandler<AddPersonalReferenceCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(AddPersonalReferenceCommand request, CancellationToken cancellationToken)
    {
        // check is employee exists.
        var employee = await _unitOfWork.EmployeeRepository.GetAllReadOnly().FirstOrDefaultAsync(x => x.EmployeeSecretId.Equals(request.Request.EmployeeSecretId));
        if (employee is null)
        {
            return Result<bool>.Fail("Employee doesn't exist.");
        }

        var referenceExist = await _unitOfWork
            .ReferenceRepository
            .GetAllReadOnly()
            .FirstOrDefaultAsync(reference => reference.EmployeeId == employee.Id
                    && reference.ReferenceTypeId == (byte)ReferenceTypeEnum.Personal, cancellationToken);
        if (referenceExist is not null)
        {
            return Result<bool>.Fail("You can't multiple personal reference.");
        }

        var reference = new Reference()
        {
            EmployeeId = employee.Id,
            Name = request.Request.Name,
            Email = request.Request.Email,
            PhoneNumber = request.Request.PhoneNumber,
            CharacterProfile = request.Request.CharacterProfile,
            ReferenceTypeId = (byte)ReferenceTypeEnum.Personal,
            Status = (byte)PersonalReferenceStatusEnum.Pending
        };

        await _unitOfWork.ReferenceRepository.Add(reference);
        await _unitOfWork.SaveChangesAsync();

        return Result<bool>.Success(true, "Reference added successfully.");
    }
}

public sealed class AddProfessionalReferenceHandler(IUnitOfWork _unitOfWork) : IRequestHandler<AddProfessionalReferenceCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(AddProfessionalReferenceCommand request, CancellationToken cancellationToken)
    {

        if (request.Request.StartDate.Date > DateTime.UtcNow.Date)
        {
            return Result<bool>.Fail("Start date should be less than today.");
        }

        if (!request.Request.TillDate)
        {
            if (request.Request.EndDate.Date > DateTime.UtcNow.Date)
            {
                return Result<bool>.Fail("End date should be less than today.");
            }

            if (request.Request.StartDate.Date > request.Request.EndDate.Date)
            {
                return Result<bool>.Fail("Joining date should be less than leaving date.");
            }
        }

        // check is employee exists.
        var employee = await _unitOfWork
            .EmployeeRepository
            .GetAllReadOnly()
            .FirstOrDefaultAsync(employee => employee.EmployeeSecretId == request.Request.EmployeeSecretId);
        if (employee is null)
        {
            return Result<bool>.Fail("Employee doesn't exist.");
        }

        var referenceExist = await _unitOfWork
            .ReferenceRepository
            .GetAllReadOnly()
            .FirstOrDefaultAsync(reference => reference.EmployeeId == employee.Id
                        && reference.ReferenceTypeId == (byte)ReferenceTypeEnum.Professional, cancellationToken);
        if (referenceExist is not null)
        {
            return Result<bool>.Fail("You can't multiple professional reference.");
        }

        // add new reference
        var reference = new Reference()
        {
            EmployeeId = employee.Id,
            OrganizationName = request.Request.OrganizationName,
            OrganizationEmail = request.Request.OrganizationEmail,
            OrganizationPhoneNumber = request.Request.OrganizationPhoneNumber,
            JobTitle = request.Request.JobTitle,
            Position = request.Request.Position,
            BothWork = request.Request.BothWork,
            StartDate = request.Request.StartDate,
            EndDate = request.Request.EndDate,
            CharacterDescription = request.Request.CharacterDescription,
            TillDate = request.Request.TillDate,
            ReferenceTypeId = (byte)ReferenceTypeEnum.Professional,
            Status = (byte)ProfessionalReferenceStatusEnum.Pending
        };

        await _unitOfWork.ReferenceRepository.Add(reference);
        await _unitOfWork.SaveChangesAsync();

        return Result<bool>.Success(true, "Reference added successfully.");
    }
}
public sealed class GetEmployeeForPersonalBySecretIdHandler(IUnitOfWork _unitOfWork)
    : IRequestHandler<GetEmployeeForPersonalBySecretIdQuery, Result<GetEmployeeBySecretIdResponse>>
{
    public async Task<Result<GetEmployeeBySecretIdResponse>> Handle(GetEmployeeForPersonalBySecretIdQuery request, CancellationToken cancellationToken)
    {
        // check is employee exists.
        var employee = await _unitOfWork.EmployeeRepository.GetEmployeeBySecretId(request.EmployeeSecretId).FirstOrDefaultAsync(cancellationToken);
        if (employee is null)
        {
            return Result<GetEmployeeBySecretIdResponse>.Fail("Employee doesn't exist.");
        }
        // add check here to check reference is already exists or not.
        employee.CanAddReference = !await _unitOfWork
            .ReferenceRepository
            .GetAllReadOnly()
            .Where(reference => reference.ReferenceTypeId == (byte)ReferenceTypeEnum.Personal
                        && reference.EmployeeId == employee.Id)
            .AnyAsync(cancellationToken);

        return Result<GetEmployeeBySecretIdResponse>.Success(employee, "Employee successfully collected.");
    }
}
public sealed class GetEmployeeForProfessionalBySecretIdHandler(IUnitOfWork _unitOfWork)
    : IRequestHandler<GetEmployeeForProfessionalBySecretIdQuery, Result<GetEmployeeBySecretIdResponse>>
{
    public async Task<Result<GetEmployeeBySecretIdResponse>> Handle(GetEmployeeForProfessionalBySecretIdQuery request, CancellationToken cancellationToken)
    {
        // check is employee exists.
        var employee = await _unitOfWork.EmployeeRepository.GetEmployeeBySecretId(request.EmployeeSecretId).FirstOrDefaultAsync(cancellationToken);
        if (employee is null)
        {
            return Result<GetEmployeeBySecretIdResponse>.Fail("Employee doesn't exist.");
        }
        // add check here to check reference is already exists or not.
        employee.CanAddReference = !await _unitOfWork
            .ReferenceRepository
            .GetAllReadOnly()
            .Where(reference => reference.ReferenceTypeId == (byte)ReferenceTypeEnum.Professional
                        && reference.EmployeeId == employee.Id)
            .AnyAsync(cancellationToken);

        return Result<GetEmployeeBySecretIdResponse>.Success(employee, "Employee successfully collected.");
    }
}

public sealed class CanAddPersonalReferenceHandler(IUnitOfWork _unitOfWork)
    : IRequestHandler<CanAddPersonalReferenceQuery, Result<CanAddPersonalReferenceResponse>>
{
    public async Task<Result<CanAddPersonalReferenceResponse>> Handle(CanAddPersonalReferenceQuery request, CancellationToken cancellationToken)
    {
        // check is employee exists.
        var employee = await _unitOfWork.EmployeeRepository.GetEmployeeBySecretId(request.EmployeeSecretId).FirstOrDefaultAsync(cancellationToken);
        if (employee is null)
        {
            return Result<CanAddPersonalReferenceResponse>.Fail("Employee doesn't exist.");
        }

        var canReferenceAddResponse = new CanAddPersonalReferenceResponse();
        // check reference added or not.
        var isreferenceExist = await _unitOfWork
            .ReferenceRepository
            .GetAllReadOnly()
            .FirstOrDefaultAsync(reference => reference.EmployeeId == employee.Id
                            && reference.ReferenceTypeId == (byte)ReferenceTypeEnum.Personal, cancellationToken);
        if (isreferenceExist is null)
        {
            return Result<CanAddPersonalReferenceResponse>.Success(canReferenceAddResponse, "You can add personal reference.");
        }

        return Result<CanAddPersonalReferenceResponse>.Fail("Personal reference already added.");
    }
}

public sealed class CanAddProfessionalReferenceHandler(IUnitOfWork _unitOfWork)
    : IRequestHandler<CanAddProfessionalReferenceQuery, Result<CanAddProfessionalReferenceResponse>>
{
    public async Task<Result<CanAddProfessionalReferenceResponse>> Handle(CanAddProfessionalReferenceQuery request, CancellationToken cancellationToken)
    {
        // check is employee exists.
        var employee = await _unitOfWork.EmployeeRepository.GetEmployeeBySecretId(request.EmployeeSecretId).FirstOrDefaultAsync(cancellationToken);
        if (employee is null)
        {
            return Result<CanAddProfessionalReferenceResponse>.Fail("Employee doesn't exist.");
        }

        var canReferenceAddResponse = new CanAddProfessionalReferenceResponse();
        // check reference added or not.
        var isreferenceExist = await _unitOfWork
            .ReferenceRepository
            .GetAllReadOnly()
            .FirstOrDefaultAsync(reference => reference.EmployeeId == employee.Id
                            && reference.ReferenceTypeId == (byte)ReferenceTypeEnum.Professional, cancellationToken);
        if (isreferenceExist is null)
        {
            return Result<CanAddProfessionalReferenceResponse>.Success(canReferenceAddResponse, "You can add professional reference.");
        }

        return Result<CanAddProfessionalReferenceResponse>.Fail("Professional reference already added.");
    }
}

public sealed class GetEmployeeDocumentsHandler(IUnitOfWork _unitOfWork)
    : IRequestHandler<GetEmployeeDocumentsQuery, Result<List<GetEmployeeDocumentsResponse>>>
{
    public async Task<Result<List<GetEmployeeDocumentsResponse>>> Handle(GetEmployeeDocumentsQuery request, CancellationToken cancellationToken)
    {
        // get employee document by employee.
        var employeeDocument = _unitOfWork.EmployeeDocumentRepository.GetDocumentsByEmployee(request.EmployeeId);

        // get documents by category.
        byte categoryId = 1;
        var documents = _unitOfWork.DocumentRepository.GetDocumentByCategoryId(categoryId);

        var query = from document in documents
                    join employeeDocs in employeeDocument
                    on document.Id equals employeeDocs.DocumentId into Details
                    from m in Details.DefaultIfEmpty()
                    select new GetEmployeeDocumentsResponse
                    {
                        DocumentId = document.Id,
                        EmployeeDocumentId = m.Id,
                        DocumentName = document.Name,
                        DocumentUrl = m.DocumentUrl,
                        ExpiryDate = m.ExpiryDate,
                        Status = m.Status,
                        Reason = m.Reason
                    };

        var employements = await query.ToListAsync(cancellationToken);

        return Result<List<GetEmployeeDocumentsResponse>>.Success(employements, "Employee documents successfully collected.");
    }
}

public sealed class AddEmployeeDocumentHandler(IUnitOfWork _unitOfWork, IFileHelper _fileHelper) : IRequestHandler<AddEmployeeDocumentCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(AddEmployeeDocumentCommand request, CancellationToken cancellationToken)
    {
        // expiry date must be greater than now.
        if (request.Request.ExpiryDate.Date < DateTime.UtcNow.Date)
        {
            return Result<bool>.Fail("Expiry Date should be later than now.");
        }

        // check is employee exists.
        var employee = await _unitOfWork.EmployeeRepository.GetById(request.Request.EmployeeId);
        if (employee is null)
        {
            return Result<bool>.Fail("Employee doesn't exist.");
        }

        // remove if document already exist.
        var isExists = await _unitOfWork.EmployeeDocumentRepository
            .GetAll()
            .FirstOrDefaultAsync(document => document.EmployeeId == request.Request.EmployeeId
                            && document.DocumentId == request.Request.DocumentId);
        if (isExists is not null)
        {
            _unitOfWork.EmployeeDocumentRepository.DeleteByEntity(isExists);
        }

        var documentPath = await _fileHelper.UploadFile("Documents", request.Request.Document);
        var employeeDocument = new EmployeeDocument()
        {
            EmployeeId = Convert.ToInt32(request.Request.EmployeeId),
            DocumentId = request.Request.DocumentId,
            DocumentUrl = documentPath,
            UploadedDate = DateTime.UtcNow,
            ExpiryDate = request.Request.ExpiryDate,
            //Status = 0  ==> Before.
            Status = (byte)DocumentStatusEnum.VerificationUnderProcess
        };

        await _unitOfWork.EmployeeDocumentRepository.Add(employeeDocument);
        await _unitOfWork.SaveChangesAsync();

        return Result<bool>.Success(true, "Employee document added successfully.");
    }
}

public sealed class DeleteEmployeeDocumentHandler(IUnitOfWork _unitOfWork) : IRequestHandler<DeleteEmployeeDocumentCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(DeleteEmployeeDocumentCommand request, CancellationToken cancellationToken)
    {
        // check is qualification exists.
        var document = await _unitOfWork.EmployeeDocumentRepository.GetById(request.Request.EmployeeDocumentId);
        if (document is null)
        {
            return Result<bool>.Fail("Document doesn't exist.");
        }

        if (document.EmployeeId != request.Request.EmployeeId)
        {
            return Result<bool>.Fail("You're not allow to delete that document.");
        }

        _unitOfWork.EmployeeDocumentRepository.DeleteByEntity(document);
        await _unitOfWork.SaveChangesAsync();

        return Result<bool>.Success(true, "Document deleted successfully.");
    }
}

public sealed class AddDbsCertificateHandler(IUnitOfWork _unitOfWork, IFileHelper _fileHelper) : IRequestHandler<AddDbsCertificateCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(AddDbsCertificateCommand request, CancellationToken cancellationToken)
    {
        // expiry date must be greater than now.
        if (request.Request.ExpiryDate.Date < DateTime.UtcNow.Date)
        {
            return Result<bool>.Fail("Expiry Date should be later than now.");
        }

        // check is employee exists.
        var employee = await _unitOfWork.EmployeeRepository.GetById(request.Request.EmployeeId);
        if (employee is null)
        {
            return Result<bool>.Fail("Employee doesn't exist.");
        }

        var documentPath = await _fileHelper.UploadFile("DbsCertificates", request.Request.File);
        employee.DbsCertificateUrl = documentPath;
        employee.DbsExpiryDate = request.Request.ExpiryDate;
        employee.DbsCertificateStatus = (byte)DbsCertificateStatusEnum.Pending;

        _unitOfWork.EmployeeRepository.Change(employee);
        await _unitOfWork.SaveChangesAsync();

        return Result<bool>.Success(true, "Dbs added successfully.");
    }
}

public sealed class AddNationalInsuranceHandler(IUnitOfWork _unitOfWork, IFileHelper _fileHelper)
    : IRequestHandler<AddNationalInsuranceCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(AddNationalInsuranceCommand request, CancellationToken cancellationToken)
    {
        // check is employee exists.
        var employee = await _unitOfWork.EmployeeRepository.GetById(request.Request.EmployeeId);
        if (employee is null)
        {
            return Result<bool>.Fail("Employee doesn't exist.");
        }

        var documentPath = await _fileHelper.UploadFile("NationalInsurances", request.Request.File);
        employee.NationalInsuranceUrl = documentPath;
        employee.NationalInsuranceStatus = (byte)NationalInsuranceStatusEnum.Pending;

        _unitOfWork.EmployeeRepository.Change(employee);
        await _unitOfWork.SaveChangesAsync();

        return Result<bool>.Success(true, "Dbs added successfully.");
    }
}

public sealed class AddAccessNIHandler(IUnitOfWork _unitOfWork, IFileHelper _fileHelper) : IRequestHandler<AddAccessNICommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(AddAccessNICommand request, CancellationToken cancellationToken)
    {
        // expiry date must be greater than now.
        if (request.Request.ExpiryDate < DateTime.UtcNow)
        {
            return Result<bool>.Fail("Expiry Date should be later than now.");
        }

        // check is employee exists.
        var employee = await _unitOfWork.EmployeeRepository.GetById(request.Request.EmployeeId);
        if (employee is null)
        {
            return Result<bool>.Fail("Employee doesn't exist.");
        }

        if (!employee.Country.ToLower().Contains("ireland")) // Only nothern iceland country allow to add access ni.
        {
            return Result<bool>.Fail($"Employee with {employee.Country} do not allow to add Access NI.");
        }

        var documentPath = await _fileHelper.UploadFile("AceessNI", request.Request.File);
        employee.AccessNIUrl = documentPath;
        employee.AccessNIExpiryDate = request.Request.ExpiryDate;
        employee.AccessNIStatus = (byte)AccessNIStatusEnum.Pending;

        _unitOfWork.EmployeeRepository.Change(employee);
        await _unitOfWork.SaveChangesAsync();

        return Result<bool>.Success(true, "Aceess NI added successfully.");
    }
}

public sealed class AddDbsNumberHandler(IUnitOfWork _unitOfWork) : IRequestHandler<AddDbsNumberCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(AddDbsNumberCommand request, CancellationToken cancellationToken)
    {
        // check is employee exists.
        var employee = await _unitOfWork.EmployeeRepository.GetById(request.Request.EmployeeId);
        if (employee is null)
        {
            return Result<bool>.Fail("Employee doesn't exist.");
        }

        if (request.Request.HaveDbsNumber)
        {
            employee.DbsNumber = request.Request.DbsNumber;
            employee.DbsNumberStatus = (byte)DbsNumebrStatusEnum.Pending;
        }
        else
        {
            employee.DbsNumber = string.Empty;
            employee.DbsNumberStatus = (byte)DbsNumebrStatusEnum.Unknown;
        }

        employee.NationalInsuranceNumber = request.Request.NationalInsuranceNumber;
        employee.NationalInsuranceStatus = (byte)NationalInsuranceStatusEnum.Pending;
        employee.HaveDbsNumber = request.Request.HaveDbsNumber;

        _unitOfWork.EmployeeRepository.Change(employee);
        await _unitOfWork.SaveChangesAsync();

        return Result<bool>.Success(true, "Dbs added successfully.");
    }
}

public sealed class DeleteDbsCertificateHandler(IUnitOfWork _unitOfWork) : IRequestHandler<DeleteDbsCertificateCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(DeleteDbsCertificateCommand request, CancellationToken cancellationToken)
    {
        // check is employee exists.
        var employee = await _unitOfWork.EmployeeRepository.GetById(request.EmployeeId);
        if (employee is null)
        {
            return Result<bool>.Fail("Employee doesn't exist.");
        }

        employee.DbsCertificateUrl = string.Empty;
        employee.DbsCertificateStatus = (byte)DbsCertificateStatusEnum.Unknown;
        employee.DbsExpiryDate = null;

        _unitOfWork.EmployeeRepository.Change(employee);
        await _unitOfWork.SaveChangesAsync();

        return Result<bool>.Success(true, "Dbs deleted successfully.");
    }
}

public sealed class DeleteAccessNIHandler(IUnitOfWork _unitOfWork) : IRequestHandler<DeleteAccessNICommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(DeleteAccessNICommand request, CancellationToken cancellationToken)
    {
        // check is employee exists.
        var employee = await _unitOfWork.EmployeeRepository.GetById(request.EmployeeId);
        if (employee is null)
        {
            return Result<bool>.Fail("Employee doesn't exist.");
        }

        employee.AccessNIUrl = string.Empty;
        employee.AccessNIExpiryDate = null;
        employee.AccessNIStatus = (byte)AccessNIStatusEnum.Unknown;

        _unitOfWork.EmployeeRepository.Change(employee);
        await _unitOfWork.SaveChangesAsync();

        return Result<bool>.Success(true, "AccessNI deleted successfully.");
    }
}

public sealed class DeleteNationalInsuranceHandler(IUnitOfWork _unitOfWork) : IRequestHandler<DeleteNationalInsuranceCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(DeleteNationalInsuranceCommand request, CancellationToken cancellationToken)
    {
        // check is employee exists.
        var employee = await _unitOfWork.EmployeeRepository.GetById(request.EmployeeId);
        if (employee is null)
        {
            return Result<bool>.Fail("Employee doesn't exist.");
        }

        employee.NationalInsuranceUrl = string.Empty;
        employee.NationalInsuranceStatus = (byte)NationalInsuranceStatusEnum.Unknown;

        _unitOfWork.EmployeeRepository.Change(employee);
        await _unitOfWork.SaveChangesAsync();

        return Result<bool>.Success(true, "National Insurance deleted successfully.");
    }
}

public sealed class AddBankDetailsHandler(IUnitOfWork _unitOfWork) : IRequestHandler<AddBankDetailsCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(AddBankDetailsCommand request, CancellationToken cancellationToken)
    {
        // check is employee exists.
        var employee = await _unitOfWork.EmployeeRepository.GetById(request.Request.EmployeeId);
        if (employee is null)
        {
            return Result<bool>.Fail("Employee doesn't exist.");
        }

        employee.AccountName = request.Request.AccountName;
        employee.AccountNumber = request.Request.AccountNumber;
        employee.SortCode = request.Request.SortCode;
        employee.BankName = request.Request.BankName;

        _unitOfWork.EmployeeRepository.Change(employee);
        await _unitOfWork.SaveChangesAsync();

        return Result<bool>.Success(true, "Bank details added successfully.");
    }
}

public sealed class AddDocumentPolicyStatusHandler(IUnitOfWork _unitOfWork) : IRequestHandler<AddDocumentPolicyStatusCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(AddDocumentPolicyStatusCommand request, CancellationToken cancellationToken)
    {
        // check is employee exists.
        var employee = await _unitOfWork.EmployeeRepository.GetById(request.Request.EmployeeId);
        if (employee is null)
        {
            return Result<bool>.Fail("Employee doesn't exist.");
        }

        employee.Policy1 = request.Request.Policy1;
        employee.Policy2 = request.Request.Policy2;
        employee.Policy3 = request.Request.Policy3;
        employee.Policy4 = request.Request.Policy4;

        _unitOfWork.EmployeeRepository.Change(employee);
        await _unitOfWork.SaveChangesAsync();

        return Result<bool>.Success(true, "Policy details added successfully.");
    }
}

public sealed class GetDocumentPolicyInfoHandler(IUnitOfWork _unitOfWork)
    : IRequestHandler<GetDocumentPolicyInfoQuery, Result<GetDocumentPolicyInfoResponse>>
{
    public async Task<Result<GetDocumentPolicyInfoResponse>> Handle(GetDocumentPolicyInfoQuery request, CancellationToken cancellationToken)
    {
        // check is employee exists.
        var employee = await _unitOfWork.EmployeeRepository
            .GetDocumentPolicyInfoByEmployee(request.EmployeeId)
            .FirstOrDefaultAsync(cancellationToken);

        if (employee is null)
        {
            return Result<GetDocumentPolicyInfoResponse>.Fail("Employee doesn't exist.");
        }

        return Result<GetDocumentPolicyInfoResponse>.Success(employee, "Policy details collected successfully.");
    }
}

public sealed class UpdateEmployeeHandler(IUnitOfWork _unitOfWork, UserManager<User> _userManger) : IRequestHandler<UpdateEmployeeCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = await _unitOfWork.EmployeeRepository.GetById(request.Request.EmployeeId);
        if (employee is null)
        {
            return Result<bool>.Fail("Employee doesn't exist.");
        }

        // check the email already exists.
        var isexist = await _unitOfWork
            .EmployeeRepository
            .GetAllReadOnly()
            .AnyAsync(employee => employee.Email == request.Request.Email
                            && employee.Id != employee.Id);
        if (isexist)
        {
            return Result<bool>.Fail("Email already exist.");
        }

        // check employee type exists.
        var isTypeExist = await _unitOfWork
            .EmployeeTypeRepository
            .GetAllReadOnly()
            .AnyAsync(type => type.Id == request.Request.EmployeeTypeId);
        if (!isTypeExist)
        {
            return Result<bool>.Fail("Employee Type doesn't exist.");
        }

        // check employement type exist.
        var isEmployementTypeExist = await _unitOfWork
            .EmployementTypeRepository
            .GetAllReadOnly()
        .AnyAsync(type => type.Id == request.Request.EmployementTypeId);
        if (!isTypeExist)
        {
            return Result<bool>.Fail("Employement Type doesn't exist.");
        }

        // if employee account status is not activated, employee can change the EmployementTypeId.
        if (employee.AccountStatus != (byte)EmployeeAccountStatusEnum.Activated)
        {
            employee.EmployementTypeId = request.Request.EmployementTypeId;
        }

        // check gender is valid or not.
        var genders = new List<string>()
        {
            "Single",
            "Married",
            "Divorced"
        };
        var isValid = genders.Contains(request.Request.MaritalStatus.Trim());
        if (!isValid)
        {
            return Result<bool>.Fail("Invalid gender.");
        }

        // update the employee data.
        employee.Title = request.Request.Title;
        employee.FirstName = request.Request.FirstName;
        employee.LastName = request.Request.LastName;
        employee.MaritalStatus = request.Request.MaritalStatus.Trim();
        employee.EmployeeTypeId = request.Request.EmployeeTypeId;
        employee.Email = request.Request.Email;

        if (request.Request.EmployementTypeId == 1) // for Self Employed, we need UTRNumber.
        {
            if (string.IsNullOrEmpty(request.Request.UTRNumber)) // validate the UTR number.
            {
                return Result<bool>.Fail("UTR number is required for Self Employed.");
            }

            if (employee.UTRNumberStatus != (byte)UTRNumberStatusEnum.Approve) // we only update the utn number if it is not approved.
            {
                employee.UTRNumber = request.Request.UTRNumber;
                employee.UTRNumberStatus = (byte)UTRNumberStatusEnum.Pending;
            }
        }
        else if (request.Request.EmployementTypeId == 3) // For Limited Company, we need CompanyNumber
        {
            if (string.IsNullOrEmpty(request.Request.CompanyNumber)) // validate the Company number.
            {
                return Result<bool>.Fail("Company number is required for Limited Company.");
            }

            if (employee.UTRNumberStatus != (byte)CompanyNumberStatusEnum.Approve) // we only update the company number if it is not approved.
            {
                employee.CompanyNumber = request.Request.CompanyNumber;
                employee.CompanyNumberStatus = (byte)CompanyNumberStatusEnum.Pending;
            }
        }

        _unitOfWork.EmployeeRepository.Change(employee);

        // update user.
        var user = await _userManger.FindByIdAsync(employee.UserId.ToString());
        if (user is null)
        {
            return Result<bool>.Fail("User doesn't exist.");
        }

        user.FirstName = request.Request.FirstName;
        user.LastName = request.Request.LastName;
        user.Email = request.Request.Email;

        await _userManger.UpdateAsync(user);
        await _unitOfWork.SaveChangesAsync();

        return Result<bool>.Success(true, "Employee updated successfully.");
    }
}

public sealed class GetEmployeeByIdHandler(IUnitOfWork _unitOfWork) : IRequestHandler<GetEmployeeByIdQuery, Result<GetEmployeeByIdResponse>>
{
    public async Task<Result<GetEmployeeByIdResponse>> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
    {
        var employee = await _unitOfWork
            .EmployeeRepository
            .GetEmployeeById(request.EmployeeId)
            .FirstOrDefaultAsync(cancellationToken);

        if (employee is null)
        {
            return Result<GetEmployeeByIdResponse>.Fail("Employee doesn't exist.");
        }

        employee.IsRegistered = true;
        employee.IsAddressAdded = !string.IsNullOrEmpty(employee.Address);
        employee.IsNoOfShiftsAdded = employee.NoOfShifts > 0;
        employee.IsNMCAdded = employee.NMCPinStatus != (byte)NMCPinStatusEnum.Unknown ? true : false;
        employee.IsStarterFormAdded = await _unitOfWork.EmployeeStarterFormAnswerRepository.GetAllReadOnly().AnyAsync(answer => answer.EmployeeId == request.EmployeeId);
        employee.IsQualificationAdded = await _unitOfWork.QualificationRepository.GetAllReadOnly().AnyAsync(qualification => qualification.EmployeeId == request.EmployeeId);
        employee.IsEmployementAdded = await _unitOfWork.EmployementRepository.GetAllReadOnly().AnyAsync(employement => employement.EmployeeId == request.EmployeeId);
        employee.IsReferenceAdded = await _unitOfWork.ReferenceRepository.GetAllReadOnly().AnyAsync(reference => reference.EmployeeId == request.EmployeeId);
        employee.IsCertificateAdded = await _unitOfWork.EmployeeDocumentRepository.GetAllReadOnly().AnyAsync(document => document.EmployeeId == request.EmployeeId);
        employee.IsRightToWorkAdded = !string.IsNullOrEmpty(employee.BiometricResidenceCardUrl);
        employee.IsDBSAdded = !string.IsNullOrEmpty(employee.DbsCertificateUrl) || !string.IsNullOrEmpty(employee.NationalInsuranceNumber);
        employee.IsBankDetailsAdded = !string.IsNullOrEmpty(employee.BankName);
        employee.IsDocumentedAdded = employee.Policy1 || employee.Policy2 || employee.Policy3 || employee.Policy4 ? true : false;
        employee.IsAccessNIAdded = employee.AccessNIStatus == (byte)AccessNIStatusEnum.Unknown ? false : true;
        employee.IsAccessNIAdded = employee.AccessNIStatus == (byte)AccessNIStatusEnum.Unknown ? false : true;
        employee.IsAccessNIAdded = employee.AccessNIStatus == (byte)AccessNIStatusEnum.Unknown ? false : true;
        employee.PersonalReference = await _unitOfWork.ReferenceRepository.GetPersonalReferenceByEmployeeId(request.EmployeeId).FirstOrDefaultAsync(cancellationToken);
        employee.ProfessionalReference = await _unitOfWork.ReferenceRepository.GetProfessionalReferenceByEmployeeId(request.EmployeeId).FirstOrDefaultAsync(cancellationToken);
        employee.DbsDocuments = await _unitOfWork.DbsDocumentRepository.GetDbsDocumentsByEmployeeId(request.EmployeeId).ToListAsync(cancellationToken);

        return Result<GetEmployeeByIdResponse>.Success(employee, "Employee collected successfully.");
    }
}

public sealed class GetEmployeeStarterFormAnswersHandler(IUnitOfWork _unitOfWork) : IRequestHandler<GetStarterFormAnswersQuery, Result<List<GetStarterFormAnswerResponse>>>
{
    public async Task<Result<List<GetStarterFormAnswerResponse>>> Handle(GetStarterFormAnswersQuery request, CancellationToken cancellationToken)
    {
        var answers = await _unitOfWork
            .EmployeeStarterFormAnswerRepository
            .GetWithConditionReadOnly(answer => answer.EmployeeId == request.EmployeeId)
             .Select(answer => new GetStarterFormAnswerResponse
             {
                 QuestionId = answer.QuestionId,
                 YesOrNo = answer.YesOrNo
             })
            .ToListAsync(cancellationToken);

        return Result<List<GetStarterFormAnswerResponse>>.Success(answers, "Collected successfully.");
    }
}

public sealed class AddProfileImageHandler(IUnitOfWork _unitOfWork, IFileHelper _fileHelper) : IRequestHandler<AddProfileImageCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(AddProfileImageCommand request, CancellationToken cancellationToken)
    {
        //check employee exists.
        var employee = await _unitOfWork.EmployeeRepository.GetById(request.Request.EmployeeId);
        if (employee is null)
        {
            return Result<bool>.Fail("Employee doesn't exist.");
        }

        // save image.
        var image = await _fileHelper.UploadFile("Profiles", request.Request.File);

        employee.ProfileImageUrl = image;

        _unitOfWork.EmployeeRepository.Change(employee);
        await _unitOfWork.SaveChangesAsync();
        return Result<bool>.Success(true, "Profile image added successfully.");
    }
}

public sealed class AddCVHandler(IUnitOfWork _unitOfWork, IFileHelper _fileHelper) : IRequestHandler<AddCVCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(AddCVCommand request, CancellationToken cancellationToken)
    {
        //check employee exists.
        var employee = await _unitOfWork.EmployeeRepository.GetById(request.Request.EmployeeId);
        if (employee is null)
        {
            return Result<bool>.Fail("Employee doesn't exist.");
        }

        // save image.
        var image = await _fileHelper.UploadFile("CV", request.Request.File);

        employee.CVFileURL = image;

        _unitOfWork.EmployeeRepository.Change(employee);
        await _unitOfWork.SaveChangesAsync();
        return Result<bool>.Success(true, "CV added successfully.");
    }
}

public sealed class AddStarterFormHandler(IUnitOfWork _unitOfWork) : IRequestHandler<AddStarterFormCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(AddStarterFormCommand request, CancellationToken cancellationToken)
    {
        //check employee exists.
        var employee = await _unitOfWork.EmployeeRepository.GetById(request.Request.EmployeeId);
        if (employee is null)
        {
            return Result<bool>.Fail("Employee doesn't exist.");
        }

        // delete if already exist.
        var getExistingAnswers = await _unitOfWork
            .EmployeeStarterFormAnswerRepository
            .GetWithCondition(answer => answer.EmployeeId == request.Request.EmployeeId)
            .ToListAsync(cancellationToken);

        _unitOfWork.EmployeeStarterFormAnswerRepository.RemoveRange(getExistingAnswers);

        // add employee answers.
        var answers = new List<EmployeeStarterFormAnswer>();
        var questions = await _unitOfWork
            .StarterFormQuestionRepository
            .GetAllReadOnly()
            .ToListAsync(cancellationToken);
        foreach (var question in request.Request.QuestionAnswers)
        {
            var isexist = questions.Any(x => x.Id == question.QuestionId);
            if (!isexist)
            {
                return Result<bool>.Fail("Question doesn't exist.");
            }

            if (question.QuestionId == 9 && !question.YesOrNo) // if question is: Q6Do you have a P45?, Delete p45DocumentUrl
            {
                employee.P45DocumentUrl = string.Empty;
            }

            answers.Add(new EmployeeStarterFormAnswer()
            {
                QuestionId = question.QuestionId,
                YesOrNo = question.YesOrNo,
                EmployeeId = employee.Id
            });
        }

        await _unitOfWork.EmployeeStarterFormAnswerRepository.AddRangeAsync(answers);
        // add national insurance number.
        employee.NationalInsuranceNumber = request.Request.NationalInsuranceNumber;
        employee.NationalInsuranceNumberStatus = (byte)NationalInsuranceNumberStatusEnum.Pending;

        _unitOfWork.EmployeeRepository.Change(employee);
        await _unitOfWork.SaveChangesAsync();
        return Result<bool>.Success(true, "Added successfully.");
    }
}

public sealed class AddP45DocumentHandler(IUnitOfWork _unitOfWork, IFileHelper _fileHelper) : IRequestHandler<AddP45DocumentCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(AddP45DocumentCommand request, CancellationToken cancellationToken)
    {
        //check employee exists.
        var employee = await _unitOfWork.EmployeeRepository.GetById(request.Request.EmployeeId);
        if (employee is null)
        {
            return Result<bool>.Fail("Employee doesn't exist.");
        }

        // save image.
        var image = await _fileHelper.UploadFile("P45 Documents", request.Request.File);

        employee.P45DocumentUrl = image;

        _unitOfWork.EmployeeRepository.Change(employee);
        await _unitOfWork.SaveChangesAsync();
        return Result<bool>.Success(true, "P45 document added successfully.");
    }
}

public sealed class AddBiometricResidenceCardHandler(IUnitOfWork _unitOfWork, IFileHelper _fileHelper)
    : IRequestHandler<AddBiometricResidenceCardCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(AddBiometricResidenceCardCommand request, CancellationToken cancellationToken)
    {
        //check employee exists.
        var employee = await _unitOfWork.EmployeeRepository.GetById(request.Request.EmployeeId);
        if (employee is null)
        {
            return Result<bool>.Fail("Employee doesn't exist.");
        }

        // save image.
        var image = await _fileHelper.UploadFile("Biometric Residence Cards", request.Request.File);

        employee.BiometricResidenceCardUrl = image;
        employee.BiometricResidenceCardStatus = (byte)BiometricResidenceCardStatusEnum.Pending;

        _unitOfWork.EmployeeRepository.Change(employee);
        await _unitOfWork.SaveChangesAsync();
        return Result<bool>.Success(true, "Biometric Residence Card added successfully.");
    }
}

public sealed class AddPassportHandler(IUnitOfWork _unitOfWork, IFileHelper _fileHelper) : IRequestHandler<AddPassportCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(AddPassportCommand request, CancellationToken cancellationToken)
    {
        //check employee exists.
        var employee = await _unitOfWork.EmployeeRepository.GetById(request.Request.EmployeeId);

        if (employee is null)
        {
            return Result<bool>.Fail("Employee doesn't exist.");
        }

        // save image.
        var image = await _fileHelper.UploadFile("Passports", request.Request.File);

        employee.PassportUrl = image;
        employee.PassportStatus = (byte)PassportStatusEnum.Pending;

        _unitOfWork.EmployeeRepository.Change(employee);
        await _unitOfWork.SaveChangesAsync();
        return Result<bool>.Success(true, "Passport added successfully.");
    }
}

public sealed class DeleteBiometricResidenceCardHandler(IUnitOfWork _unitOfWork) : IRequestHandler<DeleteBiometricResidenceCardCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(DeleteBiometricResidenceCardCommand request, CancellationToken cancellationToken)
    {
        //check employee exists.
        var employee = await _unitOfWork.EmployeeRepository.GetById(request.EmployeeId);
        if (employee is null)
        {
            return Result<bool>.Fail("Employee doesn't exist.");
        }

        employee.BiometricResidenceCardUrl = string.Empty;
        employee.BiometricResidenceCardStatus = (byte)BiometricResidenceCardStatusEnum.Unknown;
        employee.BiometricResidenceCardRejectionReason = string.Empty;

        _unitOfWork.EmployeeRepository.Change(employee);
        await _unitOfWork.SaveChangesAsync();
        return Result<bool>.Success(true, "Biometric Residence Card deleted successfully.");
    }
}

public sealed class DeletePassportHandler(IUnitOfWork _unitOfWork) : IRequestHandler<DeletePassportCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(DeletePassportCommand request, CancellationToken cancellationToken)
    {
        //check employee exists.
        var employee = await _unitOfWork.EmployeeRepository.GetById(request.EmployeeId);
        if (employee is null)
        {
            return Result<bool>.Fail("Employee doesn't exist.");
        }

        employee.PassportUrl = string.Empty;
        employee.PassportStatus = (byte)PassportStatusEnum.Unknown;
        employee.PassportRejectionReason = string.Empty;

        _unitOfWork.EmployeeRepository.Change(employee);
        await _unitOfWork.SaveChangesAsync();
        return Result<bool>.Success(true, "Passport deleted successfully.");
    }
}

public sealed class AddRightToWorkHandler(IUnitOfWork _unitOfWork) : IRequestHandler<AddRightToWorkCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(AddRightToWorkCommand request, CancellationToken cancellationToken)
    {
        // check is gender valid.
        var isExist = Enum.IsDefined(typeof(GenderEnum), request.Request.Gender);
        if (!isExist)
            return Result<bool>.Fail("Invalid gender.");

        //check employee exists.
        var employee = await _unitOfWork.EmployeeRepository.GetById(request.Request.EmployeeId);
        if (employee is null)
        {
            return Result<bool>.Fail("Employee doesn't exist.");
        }

        if (request.Request.DateOfBirth.Date > DateTime.UtcNow.Date)
        {
            return Result<bool>.Fail("Date of birth shouldn't be the future date.");
        }

        employee.DateOfBirth = request.Request.DateOfBirth;
        employee.Gender = request.Request.Gender;
        employee.Nationality = request.Request.Nationality;

        _unitOfWork.EmployeeRepository.Change(employee);
        await _unitOfWork.SaveChangesAsync();
        return Result<bool>.Success(true, "Details added successfully.");
    }
}

public sealed class GetAssignedJobsHandler(IUnitOfWork _unitOfWork)
    : IRequestHandler<GetAssignedJobsQuery, Result<PaginationModel<GetAssignedJobsResponse>>>
{
    public async Task<Result<PaginationModel<GetAssignedJobsResponse>>> Handle(GetAssignedJobsQuery request, CancellationToken cancellationToken)
    {
        // check is employee exists.
        var employee = await _unitOfWork.EmployeeRepository.GetById(request.EmployeeId);
        if (employee is null)
        {
            return Result<PaginationModel<GetAssignedJobsResponse>>.Fail("Employee doesn't exist.");
        }

        var jobs = _unitOfWork.AssignedJobRepository.GetAssignedJobsByEmployeeId(request.EmployeeId)
                                .Where(x =>
                             x.Date.Date >= DateTime.UtcNow.Date
                            && x.EmployeeTypeId == employee.EmployeeTypeId
                            && x.PostalCode.ToLower() == employee.PinCode.ToLower())
            .Select(x => new GetAssignedJobsResponse()
            {
                AssignedJobId = x.AssignedJobId,
                OrganisationName = x.OrganisationName,
                PostalCode = x.PostalCode,
                Date = x.Date.Date,
                EmployeeTypeId = x.EmployeeTypeId,
                HourlyRate = x.HourlyRate,
                ShiftTime = x.ShiftTime,
                IsFixedRate = x.IsFixedRate,
                FixedRate = x.FixedRate,
                FixedRateAfterCommission = x.FixedRateAfterCommission,
                IsUrgent = x.IsUrgent,
                SelfCommission = x.SelfCommission,
                HourlyRateAfterSelfCommission = x.HourlyRateAfterSelfCommission,
                PayrollCommission = x.PayrollCommission,
                HourlyRateAfterPayrollCommission = x.HourlyRateAfterPayrollCommission,
                LimitedCommission = x.LimitedCommission,
                HourlyRateAfterLimitedCommission = x.HourlyRateAfterLimitedCommission,
            }); ;

        // filter accourding to parameters.
        if (request.FromDate.Date != DateTime.MinValue.Date)
        {
            jobs = jobs.Where(job => job.Date.Date >= request.FromDate.Date);
        }
        if (request.MinRate != 0)
        {
            jobs = jobs.Where(job => job.HourlyRate >= request.MinRate);
        }
        if (!string.IsNullOrEmpty(request.PostalCode))
        {
            jobs = jobs.Where(job => job.PostalCode.ToLower() == request.PostalCode.ToLower());
        }

        jobs = jobs.OrderBy(job => job.Date);
        // Add pagination
        PaginationModel<GetAssignedJobsResponse> model = new PaginationModel<GetAssignedJobsResponse>();
        if (request.PageSize > 0)
        {
            var result = new PagedList<GetAssignedJobsResponse>(jobs, request.PageIndex, request.PageSize, false);
            model.PagedContent = result;
            model.PagingFilteringContext.LoadPagedList(result);
        }

        return Result<PaginationModel<GetAssignedJobsResponse>>.Success(model, "Assigned jobs list.");
    }
}

public sealed class GetUrgentJobsHandler(IUnitOfWork _unitOfWork)
    : IRequestHandler<GetUrgentJobsQuery, Result<PaginationModel<GetUrgentJobsResponse>>>
{
    public async Task<Result<PaginationModel<GetUrgentJobsResponse>>> Handle(GetUrgentJobsQuery request, CancellationToken cancellationToken)
    {
        // check is employee exists.
        var employee = await _unitOfWork.EmployeeRepository.GetById(request.EmployeeId);
        if (employee is null)
        {
            return Result<PaginationModel<GetUrgentJobsResponse>>.Fail("Employee doesn't exist.");
        }

        // check is job assigned or not. 
        var jobIds = await _unitOfWork.AssignedJobRepository.GetAllReadOnly().Where(x => x.EmployeeId == request.EmployeeId).Select(x => x.JobId).ToListAsync(cancellationToken);

        var jobs = _unitOfWork
            .JobRepository
            .GetAllReadOnly()
            .Include(x => x.Employer)
            .Where(x => !jobIds.Contains(x.Id)
                            && x.JobTypeId == (byte)JobTypeEnum.UrgentJob
                            && x.Date.Date >= DateTime.UtcNow.Date
                            && x.EmployeeTypeId == employee.EmployeeTypeId
                            && x.Employer.PinCode.ToLower() == employee.PinCode.ToLower()
                            // also check job should exclude with cancel status.
                            && x.Status != (byte)JobStatusEnum.Cancelled)
            .Select(x => new GetUrgentJobsResponse()
            {
                JobId = x.Id,
                Date = x.Date.Date,
                HourlyRate = x.HourlyRate,
                IsFixedRate = x.IsFixedRate,
                FixedRate = x.FixedRate,
                FixedRateAfterCommission = x.FixedRateAfterCommission,
                OrganisationName = x.Employer.CompanyName,
                PostalCode = x.Employer.PinCode,
                ShiftTime = $"{x.ShiftStartTime} - {x.ShiftEndTime}",
                IsDummy = x.IsDummy,
                IsUrgent = x.JobTypeId == (byte)JobTypeEnum.UrgentJob ? true : false,
                SelfCommission = x.SelfCommission,
                HourlyRateAfterSelfCommission = x.HourlyRateAfterSelfCommission,
                PayrollCommission = x.PayrollCommission,
                HourlyRateAfterPayrollCommission = x.HourlyRateAfterPayrollCommission,
                LimitedCommission = x.LimitedCommission,
                HourlyRateAfterLimitedCommission = x.HourlyRateAfterLimitedCommission,
            });

        // filter accourding to parameters.
        if (request.FromDate.Date != DateTime.MinValue.Date)
        {
            jobs = jobs.Where(job => job.Date.Date >= request.FromDate.Date);
        }
        if (request.MinRate != 0)
        {
            jobs = jobs.Where(job => job.HourlyRate >= request.MinRate);
        }
        if (!string.IsNullOrEmpty(request.PostalCode))
        {
            jobs = jobs.Where(job => job.PostalCode.ToLower() == request.PostalCode.ToLower());
        }

        jobs = jobs.OrderBy(job => job.Date);

        // Add pagination
        PaginationModel<GetUrgentJobsResponse> model = new PaginationModel<GetUrgentJobsResponse>();
        if (request.PageSize > 0)
        {
            var result = new PagedList<GetUrgentJobsResponse>(jobs, request.PageIndex, request.PageSize, false);
            model.PagedContent = result;
            model.PagingFilteringContext.LoadPagedList(result);
        }

        return Result<PaginationModel<GetUrgentJobsResponse>>.Success(model, "Urgent jobs list.");
    }
}

public sealed class GetAllJobsHandler(IUnitOfWork _unitOfWork)
    : IRequestHandler<GetAllJobsQuery, Result<PaginationModel<GetAllJobsResponse>>>
{
    public async Task<Result<PaginationModel<GetAllJobsResponse>>> Handle(GetAllJobsQuery request, CancellationToken cancellationToken)
    {
        // check is employee exists.
        var employee = await _unitOfWork.EmployeeRepository.GetById(request.EmployeeId);
        if (employee is null)
        {
            return Result<PaginationModel<GetAllJobsResponse>>.Fail("Employee doesn't exist.");
        }

        // check is job assigned or not. 
        var jobIds = await _unitOfWork.AssignedJobRepository.GetAllReadOnly().Where(x => x.EmployeeId == request.EmployeeId).Select(x => x.JobId).ToListAsync(cancellationToken);

        var jobs = _unitOfWork
            .JobRepository
            .GetAllReadOnly()
            .Include(x => x.Employer)
            .Where(x => !jobIds.Contains(x.Id)
                            && x.Date.Date >= DateTime.UtcNow.Date
                            && x.EmployeeTypeId == employee.EmployeeTypeId
                            && x.Employer.PinCode.ToLower() == employee.PinCode.ToLower()
                            // also check job should exclude with cancel status.
                            && x.Status != (byte)JobStatusEnum.Cancelled)

            .Select(x => new GetAllJobsResponse()
            {
                JobId = x.Id,
                Date = x.Date.Date,
                HourlyRate = x.HourlyRate,
                IsFixedRate = x.IsFixedRate,
                FixedRate = x.FixedRate,
                FixedRateAfterCommission = x.FixedRateAfterCommission,
                OrganisationName = x.Employer.CompanyName,
                PostalCode = x.Employer.PinCode,
                ShiftTime = $"{x.ShiftStartTime} - {x.ShiftEndTime}",
                IsUrgent = x.JobTypeId == (byte)JobTypeEnum.UrgentJob ? true : false,
                SelfCommission = x.SelfCommission,
                HourlyRateAfterSelfCommission = x.HourlyRateAfterSelfCommission,
                PayrollCommission = x.PayrollCommission,
                HourlyRateAfterPayrollCommission = x.HourlyRateAfterPayrollCommission,
                LimitedCommission = x.LimitedCommission,
                HourlyRateAfterLimitedCommission = x.HourlyRateAfterLimitedCommission,
            });

        // filter accourding to parameters.
        if (request.FromDate.Date != DateTime.MinValue.Date)
        {
            jobs = jobs.Where(job => job.Date.Date >= request.FromDate.Date);
        }
        if (request.MinRate != 0)
        {
            jobs = jobs.Where(job => job.HourlyRate >= request.MinRate);
        }
        if (!string.IsNullOrEmpty(request.PostalCode))
        {
            jobs = jobs.Where(job => job.PostalCode.ToLower() == request.PostalCode.ToLower());
        }

        jobs = jobs.OrderBy(job => job.Date);
        // Add pagination
        PaginationModel<GetAllJobsResponse> model = new PaginationModel<GetAllJobsResponse>();
        if (request.PageSize > 0)
        {
            var result = new PagedList<GetAllJobsResponse>(jobs, request.PageIndex, request.PageSize, false);
            model.PagedContent = result;
            model.PagingFilteringContext.LoadPagedList(result);
        }

        return Result<PaginationModel<GetAllJobsResponse>>.Success(model, "All jobs list.");
    }
}

public sealed class GetFavouriteJobsHandler(IUnitOfWork _unitOfWork)
    : IRequestHandler<GetFavouriteJobsQuery, Result<PaginationModel<GetFavouriteJobsResponse>>>
{
    public async Task<Result<PaginationModel<GetFavouriteJobsResponse>>> Handle(GetFavouriteJobsQuery request, CancellationToken cancellationToken)
    {
        // check is employee exists.
        var employee = await _unitOfWork.EmployeeRepository.GetById(request.EmployeeId);
        if (employee is null)
        {
            return Result<PaginationModel<GetFavouriteJobsResponse>>.Fail("Employee doesn't exist.");
        }

        // get favourite employers.
        var favouriteEmployerIds = await _unitOfWork
            .EmployeeFavouriteRepository
            .GetAllReadOnly()
            .Where(favourite => favourite.EmployeeId == request.EmployeeId)
            .Select(favourite => favourite.EmployerId)
            .ToListAsync(cancellationToken);

        // check is job assigned or not. 
        var jobIds = await _unitOfWork.AssignedJobRepository.GetAllReadOnly().Where(x => x.EmployeeId == request.EmployeeId).Select(x => x.JobId).ToListAsync(cancellationToken);

        var jobs = _unitOfWork
            .JobRepository
            .GetAllReadOnly()
            .Include(x => x.Employer)
            .Where(x => !jobIds.Contains(x.Id)
                            && favouriteEmployerIds.Contains(x.EmployerId)
                            && x.Date.Date >= DateTime.UtcNow.Date
                            && x.EmployeeTypeId == employee.EmployeeTypeId
                            && x.Employer.PinCode.ToLower() == employee.PinCode.ToLower()
                            // also check job should exclude with cancel status.
                            && x.Status != (byte)JobStatusEnum.Cancelled)
            .Select(x => new GetFavouriteJobsResponse()
            {
                JobId = x.Id,
                Date = x.Date.Date,
                HourlyRate = x.HourlyRate,
                IsFixedRate = x.IsFixedRate,
                FixedRate = x.FixedRate,
                FixedRateAfterCommission = x.FixedRateAfterCommission,
                OrganisationName = x.Employer.CompanyName,
                PostalCode = x.Employer.PinCode,
                ShiftTime = $"{x.ShiftStartTime} - {x.ShiftEndTime}",
                IsUrgent = x.JobTypeId == (byte)JobTypeEnum.UrgentJob ? true : false,
                SelfCommission = x.SelfCommission,
                HourlyRateAfterSelfCommission = x.HourlyRateAfterSelfCommission,
                PayrollCommission = x.PayrollCommission,
                HourlyRateAfterPayrollCommission = x.HourlyRateAfterPayrollCommission,
                LimitedCommission = x.LimitedCommission,
                HourlyRateAfterLimitedCommission = x.HourlyRateAfterLimitedCommission,
            });

        // filter accourding to parameters.
        if (request.FromDate.Date != DateTime.MinValue.Date)
        {
            jobs = jobs.Where(job => job.Date.Date >= request.FromDate.Date);
        }
        if (request.MinRate != 0)
        {
            jobs = jobs.Where(job => job.HourlyRate >= request.MinRate);
        }
        if (!string.IsNullOrEmpty(request.PostalCode))
        {
            jobs = jobs.Where(job => job.PostalCode.ToLower() == request.PostalCode.ToLower());
        }

        jobs = jobs.OrderBy(job => job.Date);
        // Add pagination
        PaginationModel<GetFavouriteJobsResponse> model = new PaginationModel<GetFavouriteJobsResponse>();
        if (request.PageSize > 0)
        {
            var result = new PagedList<GetFavouriteJobsResponse>(jobs, request.PageIndex, request.PageSize, false);
            model.PagedContent = result;
            model.PagingFilteringContext.LoadPagedList(result);
        }

        return Result<PaginationModel<GetFavouriteJobsResponse>>.Success(model, "Favourite jobs list.");
    }
}

public sealed class GetAppliedJobsHandler(IUnitOfWork _unitOfWork)
    : IRequestHandler<GetAppliedJobsQuery, Result<PaginationModel<GetAppliedJobsResponse>>>
{
    public async Task<Result<PaginationModel<GetAppliedJobsResponse>>> Handle(GetAppliedJobsQuery request, CancellationToken cancellationToken)
    {
        // check is employee exists.
        var employee = await _unitOfWork.EmployeeRepository.GetById(request.EmployeeId);
        if (employee is null)
        {
            return Result<PaginationModel<GetAppliedJobsResponse>>.Fail("Employee doesn't exist.");
        }

        // check is job assigned or not. 
        var jobs = _unitOfWork.AssignedJobRepository.GetAppliedJobsByEmployeeId(request.EmployeeId)
                        .Where(x =>
                             x.Date.Date >= DateTime.UtcNow.Date
                            && x.EmployeeTypeId == employee.EmployeeTypeId
                            && x.PostalCode.ToLower() == employee.PinCode.ToLower())
            .Select(x => new GetAppliedJobsResponse()
            {
                AssignedJobId = x.AssignedJobId,
                OrganisationName = x.OrganisationName,
                PostalCode = x.PostalCode,
                Date = x.Date.Date,
                EmployeeTypeId = x.EmployeeTypeId,
                HourlyRate = x.HourlyRate,
                FixedRateAfterCommission = x.FixedRateAfterCommission,
                FixedRate = x.FixedRate,
                IsFixedRate = x.IsFixedRate,
                ShiftTime = x.ShiftTime,
                IsUrgent = x.IsUrgent,
                IsSelected = x.IsSelected,
                SelfCommission = x.SelfCommission,
                HourlyRateAfterSelfCommission = x.HourlyRateAfterSelfCommission,
                PayrollCommission = x.PayrollCommission,
                HourlyRateAfterPayrollCommission = x.HourlyRateAfterPayrollCommission,
                LimitedCommission = x.LimitedCommission,
                HourlyRateAfterLimitedCommission = x.HourlyRateAfterLimitedCommission,
            }); ;

        // filter accourding to parameters.
        if (request.FromDate.Date != DateTime.MinValue.Date)
        {
            jobs = jobs.Where(job => job.Date.Date >= request.FromDate.Date);
        }
        if (request.MinRate != 0)
        {
            jobs = jobs.Where(job => job.HourlyRate >= request.MinRate);
        }
        if (!string.IsNullOrEmpty(request.PostalCode))
        {
            jobs = jobs.Where(job => job.PostalCode.ToLower() == request.PostalCode.ToLower());
        }

        jobs = jobs.OrderBy(job => job.Date);
        // Add pagination
        PaginationModel<GetAppliedJobsResponse> model = new PaginationModel<GetAppliedJobsResponse>();
        if (request.PageSize > 0)
        {
            var result = new PagedList<GetAppliedJobsResponse>(jobs, request.PageIndex, request.PageSize, false);
            model.PagedContent = result;
            model.PagingFilteringContext.LoadPagedList(result);
        }

        return Result<PaginationModel<GetAppliedJobsResponse>>.Success(model, "Applied jobs list.");
    }
}


public sealed class GetJobCountsHandler(IUnitOfWork _unitOfWork) : IRequestHandler<GetJobCountsQuery, Result<GetJobCountsResponce>>
{
    public async Task<Result<GetJobCountsResponce>> Handle(GetJobCountsQuery request, CancellationToken cancellationToken)
    {
        // check is employee exists.
        var employee = await _unitOfWork.EmployeeRepository.GetById(request.EmployeeId);
        if (employee is null)
        {
            return Result<GetJobCountsResponce>.Fail("Employee doesn't exist.");
        }

        GetJobCountsResponce result = new GetJobCountsResponce();

        var jobIds = await _unitOfWork.AssignedJobRepository.GetAllReadOnly().Where(x => x.EmployeeId == request.EmployeeId).Select(x => x.JobId).ToListAsync(cancellationToken);

        // get favourite employers.
        var favouriteEmployerIds = await _unitOfWork
            .EmployeeFavouriteRepository
            .GetAllReadOnly()
            .Where(favourite => favourite.EmployeeId == request.EmployeeId)
            .Select(favourite => favourite.EmployerId)
            .ToListAsync(cancellationToken);

        // get count favourite jobs
        var totalFavouriteJobs = await _unitOfWork
            .JobRepository
            .GetAllReadOnly()
            .Include(x => x.Employer)
            .Where(x => !jobIds.Contains(x.Id)
                            && favouriteEmployerIds.Contains(x.EmployerId)
                            && x.Date.Date >= DateTime.UtcNow.Date
                            && x.EmployeeTypeId == employee.EmployeeTypeId
                            && x.Employer.PinCode.ToLower() == employee.PinCode.ToLower()
                            // also check job should exclude with cancel status.
                            && x.Status != (byte)JobStatusEnum.Cancelled)
            .CountAsync(cancellationToken);


        var jobs = await _unitOfWork
            .JobRepository
            .GetAllReadOnly()
            .Where(x => !jobIds.Contains(x.Id)

                            && x.Date.Date >= DateTime.UtcNow.Date
                            && x.EmployeeTypeId == employee.EmployeeTypeId
                            && x.Employer.PinCode.ToLower() == employee.PinCode.ToLower()
                            // also check job should exclude with cancel status.
                            && x.Status != (byte)JobStatusEnum.Cancelled)
            .GroupBy(x => x.JobTypeId)
            .Select(x => new
            {
                JobTypeId = x.Key,
                Count = x.Count()
            })
            .ToListAsync(cancellationToken);

        result.TotalAllJobs = jobs.Sum(x => x.Count);
        result.TotalFavouriteJobs = totalFavouriteJobs;
        result.TotalUrgentJobs = jobs.Where(x => x.JobTypeId == (byte)JobTypeEnum.UrgentJob).Sum(x => x.Count);

        return Result<GetJobCountsResponce>.Success(result, "Data collected.");
    }
}
public sealed class GetConfirmedJobsHandler(IUnitOfWork _unitOfWork) : IRequestHandler<GetConfirmedJobsQuery, Result<PaginationModel<GetConfirmedJobsResponse>>>
{
    public async Task<Result<PaginationModel<GetConfirmedJobsResponse>>> Handle(GetConfirmedJobsQuery request, CancellationToken cancellationToken)
    {
        // check is employee exists.
        var employee = await _unitOfWork.EmployeeRepository.GetById(request.EmployeeId);
        if (employee is null)
        {
            return Result<PaginationModel<GetConfirmedJobsResponse>>.Fail("Employee doesn't exist.");
        }

        // get confirmed job
        var jobs = _unitOfWork.AssignedJobRepository.GetConfirmedJobsByEmployeeId(request.EmployeeId)
                          .Where(x =>
                             x.Date.Date >= DateTime.UtcNow.Date
                            && x.EmployeeTypeId == employee.EmployeeTypeId
                            && x.PostalCode.ToLower() == employee.PinCode.ToLower())
            .Select(x => new GetConfirmedJobsResponse()
            {
                AssignedJobId = x.AssignedJobId,
                OrganisationName = x.OrganisationName,
                PostalCode = x.PostalCode,
                Date = x.Date.Date,
                EmployeeTypeId = x.EmployeeTypeId,
                IsFixedRate = x.IsFixedRate,
                FixedRate = x.FixedRate,
                FixedRateAfterCommission = x.FixedRateAfterCommission,
                HourlyRate = x.HourlyRate,
                ShiftTime = x.ShiftTime,
                IsUrgent = x.IsUrgent,
                SelfCommission = x.SelfCommission,
                HourlyRateAfterSelfCommission = x.HourlyRateAfterSelfCommission,
                PayrollCommission = x.PayrollCommission,
                HourlyRateAfterPayrollCommission = x.HourlyRateAfterPayrollCommission,
                LimitedCommission = x.LimitedCommission,
                HourlyRateAfterLimitedCommission = x.HourlyRateAfterLimitedCommission,
            });

        // filter accourding to parameters.
        if (request.FromDate.Date != DateTime.MinValue.Date)
        {
            jobs = jobs.Where(job => job.Date.Date >= request.FromDate.Date);
        }
        if (request.MinRate != 0)
        {
            jobs = jobs.Where(job => job.HourlyRate >= request.MinRate);
        }
        if (!string.IsNullOrEmpty(request.PostalCode))
        {
            jobs = jobs.Where(job => job.PostalCode.ToLower() == request.PostalCode.ToLower());
        }

        jobs = jobs.OrderBy(job => job.Date);

        // Add pagination
        PaginationModel<GetConfirmedJobsResponse> model = new PaginationModel<GetConfirmedJobsResponse>();
        if (request.PageSize > 0)
        {
            var result = new PagedList<GetConfirmedJobsResponse>(jobs, request.PageIndex, request.PageSize, false);
            model.PagedContent = result;
            model.PagingFilteringContext.LoadPagedList(result);
        }

        return Result<PaginationModel<GetConfirmedJobsResponse>>.Success(model, "Confirmed jobs list.");
    }
}

public sealed class GetTimesheetsByStatusHandler(IUnitOfWork _unitOfWork)
    : IRequestHandler<GetTimesheetsByStatusQuery, Result<PaginationModel<GetTimesheetsByStatusResponse>>>
{
    public async Task<Result<PaginationModel<GetTimesheetsByStatusResponse>>> Handle(GetTimesheetsByStatusQuery request, CancellationToken cancellationToken)
    {
        // check nurse type exists.
        var isExist = Enum.IsDefined(typeof(TimeSheetStatusEnum), request.Status);
        if (!isExist)
            return Result<PaginationModel<GetTimesheetsByStatusResponse>>.Fail("Invalid nurse type.");

        // check is employee exists.
        var employer = await _unitOfWork.EmployeeRepository.GetById(request.EmployeeId);
        if (employer is null)
        {
            return Result<PaginationModel<GetTimesheetsByStatusResponse>>.Fail("Employee doesn't exist.");
        }

        // get timesheets by status
        var query = _unitOfWork.TimesheetRepository.GetTimesheetsByEmployee(request.EmployeeId);
        switch (request.Status)
        {
            case (byte)TimeSheetStatusEnum.Pending: query = query.Where(timesheet => timesheet.Status == (byte)TimeSheetStatusEnum.Pending || timesheet.Status == (byte)TimeSheetStatusEnum.NotSubmitted); break;
            case (byte)TimeSheetStatusEnum.Approved: query = query.Where(timesheet => timesheet.Status == request.Status); break;
            case (byte)TimeSheetStatusEnum.Rejected: query = query.Where(timesheet => timesheet.Status == request.Status); break;
        }

        // Add pagination
        PaginationModel<GetTimesheetsByStatusResponse> model = new PaginationModel<GetTimesheetsByStatusResponse>();
        if (request.PageSize > 0)
        {
            var result = new PagedList<GetTimesheetsByStatusResponse>(query, request.PageIndex, request.PageSize, false);
            model.PagedContent = result;
            model.PagingFilteringContext.LoadPagedList(result);
        }

        return Result<PaginationModel<GetTimesheetsByStatusResponse>>.Success(model, "Timesheet list.");
    }
}

public sealed class GetCompletedJobsByEmployeeHandler(IUnitOfWork _unitOfWork)
    : IRequestHandler<GetCompletedJobsByEmployeeQuery, Result<PaginationModel<GetCompletedJobsResponse>>>
{
    public async Task<Result<PaginationModel<GetCompletedJobsResponse>>> Handle(GetCompletedJobsByEmployeeQuery request, CancellationToken cancellationToken)
    {
        // check is employee exists.
        var employee = await _unitOfWork.EmployeeRepository.GetById(request.EmployeeId);
        if (employee is null)
        {
            return Result<PaginationModel<GetCompletedJobsResponse>>.Fail("Employee doesn't exist.");
        }

        // get confirmed job
        var jobs = _unitOfWork.AssignedJobRepository
            .GetAllReadOnly()
                .Include(x => x.Job)
                    .ThenInclude(x => x.EmployeeType)
                .Include(x => x.Job)
                    .ThenInclude(x => x.Employer)
                .Where(x => x.JobStatus == (byte)JobStatusEnum.Completed && x.EmployeeId == request.EmployeeId)
                .Where(x => x.Job.EmployeeTypeId == employee.EmployeeTypeId
                            && x.Employer.PinCode.ToLower() == employee.PinCode.ToLower())
            .Select(x => new GetCompletedJobsResponse()
            {
                AssignedJobId = x.Id,
                Date = x.Job.Date.Date,
                EmployeeTypeId = x.Job.EmployeeTypeId,
                StartDate = x.Job.Date,
                Location = x.Employer.Location,
                ShiftStartTime = x.Job.ShiftStartTime,
                ShiftEndTime = x.Job.ShiftEndTime,
                EmployeeType = x.Job.EmployeeType.Name,
                PinCode = x.Employer.PinCode,
                HourlyRate = x.Job.HourlyRate,
                IsFixedRate = x.Job.IsFixedRate,
                FixedRate = x.Job.FixedRate,
                FixedRateAfterCommission = x.Job.FixedRateAfterCommission,
                JobStatus = x.JobStatus,
                IsUrgent = x.Job.JobTypeId == (byte)JobTypeEnum.UrgentJob ? true : false,
                SelfCommission = x.Job.SelfCommission,
                HourlyRateAfterSelfCommission = x.Job.HourlyRateAfterSelfCommission,
                PayrollCommission = x.Job.PayrollCommission,
                HourlyRateAfterPayrollCommission = x.Job.HourlyRateAfterPayrollCommission,
                LimitedCommission = x.Job.LimitedCommission,
                HourlyRateAfterLimitedCommission = x.Job.HourlyRateAfterLimitedCommission,
            });

        // filter accourding to parameters.
        if (request.FromDate.Date != DateTime.MinValue.Date)
        {
            jobs = jobs.Where(job => job.Date.Date >= request.FromDate.Date);
        }
        if (request.MinRate != 0)
        {
            jobs = jobs.Where(job => job.HourlyRate >= request.MinRate);
        }
        if (!string.IsNullOrEmpty(request.PostalCode))
        {
            jobs = jobs.Where(job => job.PinCode.ToLower() == request.PostalCode.ToLower());
        }

        jobs = jobs.OrderBy(job => job.Date);
        // Add pagination
        PaginationModel<GetCompletedJobsResponse> model = new PaginationModel<GetCompletedJobsResponse>();
        if (request.PageSize > 0)
        {
            var result = new PagedList<GetCompletedJobsResponse>(jobs, request.PageIndex, request.PageSize, false);
            model.PagedContent = result;
            model.PagingFilteringContext.LoadPagedList(result);
        }
        var timesheets = await _unitOfWork
            .TimesheetRepository
            .GetAllReadOnly()
            .Where(timesheet => model.PagedContent.Select(x => x.AssignedJobId).Contains(timesheet.AssignedJobId))
            .Select(timesheet => new
            {
                timesheet.AssignedJobId,
                timesheet.Status
            })
            .ToListAsync(cancellationToken);
        model.PagedContent.ForEach(assignedJob =>
        {
            if (timesheets.Any(timesheet => timesheet.AssignedJobId == assignedJob.AssignedJobId && timesheet.Status == (byte)TimeSheetStatusEnum.NotSubmitted))
            {
                assignedJob.Status = "Timesheet not submitted";
            }
            else if (timesheets.Any(timesheet => timesheet.AssignedJobId == assignedJob.AssignedJobId && timesheet.Status == (byte)TimeSheetStatusEnum.Pending))
            {
                assignedJob.Status = "Timesheet Created";
            }
            else if (timesheets.Any(timesheet => timesheet.AssignedJobId == assignedJob.AssignedJobId && timesheet.Status == (byte)TimeSheetStatusEnum.Approved))
            {
                assignedJob.Status = "Timesheet Approved";
            }
            else if (timesheets.Any(timesheet => timesheet.AssignedJobId == assignedJob.AssignedJobId && timesheet.Status == (byte)TimeSheetStatusEnum.Rejected))
            {
                assignedJob.Status = "Timesheet Rejected";
            }
        });
        return Result<PaginationModel<GetCompletedJobsResponse>>.Success(model, "Completed jobs list.");
    }
}

public sealed class ChangeJobStatusToConfirmHandler(IUnitOfWork _unitOfWork, IMailService _mailService) : IRequestHandler<ChangeJobStatusToConfirmQuery, Result<bool>>
{
    public async Task<Result<bool>> Handle(ChangeJobStatusToConfirmQuery request, CancellationToken cancellationToken)
    {
        // check is employee exists.
        var employee = await _unitOfWork.EmployeeRepository.GetById(request.EmployeeId);
        if (employee is null)
        {
            return Result<bool>.Fail("Employee doesn't exist.");
        }

        // get assigned job.
        var assignedJob = await _unitOfWork.AssignedJobRepository
            .GetWithCondition(assignedJob => assignedJob.Id == request.AssignedJobId
                            && assignedJob.EmployeeId == request.EmployeeId)
                .Include(assignedJob => assignedJob.Job)
            .FirstOrDefaultAsync(cancellationToken);
        if (assignedJob is null)
        {
            return Result<bool>.Fail("Job doesn't exist.");
        }

        assignedJob.JobStatus = (byte)JobStatusEnum.Confirmed;
        assignedJob.ConfirmationDate = DateTime.UtcNow;

        _unitOfWork.AssignedJobRepository.Change(assignedJob);
        await _unitOfWork.SaveChangesAsync();

        // send email.
        string subject = $"Important protocols - ESGO";
        var content = " <p style='color:#000;font-weight:00;font-size:14px;padding:0px 10px 0px 20px' > First of all, A warm welcome to you and thank you for registering with ESGO.  We value your hard work and effort put forward to take care of an individual who requires your care. </p>";
        content = content + "<p style='color:#000;font-weight:00;font-size:14px;padding:0px 10px 0px 20px'>This job requires a lot of patience and care as you are helping to safe guard another human being who is not strong enough to protect themselves.You are providing attention to those who require considerable help.  We would like to point out some important protocols that should be followed at all times. </p>";
        content = content + "<p style='color:#000;font-weight:00;font-size:14px;padding:0px 10px 0px 20px'> The below listed points are some important protocols which should be followed at all times while on duty. </p>";
        content = content + "<ol style='color:#000;font-weight:00;font-size:14px;padding:0px 10px 0px 20px' >";
        content = content + "  <li > Do not use mobile phones while at work.</ li >";
        content = content + "      <li > Good hygiene and tidy uniform</ li >";
        content = content + "        <li > Do not use regional language in front of a resident or in the care home vicinity.</ li >";
        content = content + "           <li > Be punctual </ li >";
        content = content + "    <li > Patience and care is a must have attitude towards a resident.</ li >";
        content = content + "      <li > There is no space for assumptions.Any doubts to be clarified as soon as possible with the a senior staff or in-house RGN.</ li >";
        content = content + "        <li > Reporting and recording any medical errors or any symptoms observed.</ li >";
        content = content + "            <li>   No personal relationships  </li >";
        content = content + "  <li > Above all, a good smile, gentle talk is a big medicine for every resident.</ li >";
        content = content + "    </ol >";
        content = content + "  <p style='color:#000;font-weight:00;font-size:14px;padding:0px 10px 0px 20px'> As a care provider, there are certain things which you have to be careful about, when working at a care home.Any safe guarding error will be recorded and you will have to provide a statement for the same. This will affect you in future as it will be reflected in your DBS certificate if the error has caused serious injury or loss of life.Care homes are very vigilant in monitoring resident’s health and wellbeing.</ p >";
        content = content + "   <p style='color:#000;font-weight:00;font-size:14px;padding:0px 10px 0px 20px'> Also, if you need a reference from ESGO or from the care home. Its a must that you perform in the best possible way. We will be providing a true statement regarding your performance and reliability.</ p >";
        content = content + "   <p style='color:#000;font-weight:00;font-size:14px;padding:0px 10px 0px 20px'> We are giving this information to make you aware of the seriousness of the job and also the consequences. We at ESGO are always there for you if you need any advice or help.We urge you to put the best of your ability and sincerity.  </p >";
        content = content + "  <p style='color:#000;font-weight:00;font-size:14px;padding:0px 10px 0px 20px'> We look forward for the best from your part and we are confident that you will adhere to all the rules   </ p > ";
        content = content + "< p> Welcome again.</p>";
        string username = $"{employee.FirstName} {employee.LastName}";

        _ = Task.Run(() =>
        {
            _mailService.Send(employee.Email, subject, content, username);
        });

        return Result<bool>.Success(true, "Job status changed.");
    }
}

public sealed class GetAssignedJobByIdHandler(IUnitOfWork _unitOfWork) : IRequestHandler<GetAssignedJobByIdQuery, Result<GetAssignedJobByIdResponce>>
{
    public async Task<Result<GetAssignedJobByIdResponce>> Handle(GetAssignedJobByIdQuery request, CancellationToken cancellationToken)
    {
        // check is employee exists.
        var employer = await _unitOfWork.EmployeeRepository.GetById(request.EmployeeId);
        if (employer is null)
        {
            return Result<GetAssignedJobByIdResponce>.Fail("Employee doesn't exist.");
        }

        // get assigned job.
        var assignedJob = await _unitOfWork.AssignedJobRepository.GetAssignedJobById(request.AssignedJobId, request.EmployeeId).FirstOrDefaultAsync(cancellationToken);
        if (assignedJob is null)
        {
            return Result<GetAssignedJobByIdResponce>.Fail("Job doesn't exist.");
        }

        assignedJob.Employer.IsFavourite = await _unitOfWork
            .EmployeeFavouriteRepository
            .GetAllReadOnly()
            .Where(favourite => favourite.EmployeeId == request.EmployeeId && favourite.EmployerId == assignedJob.EmployerId)
            .AnyAsync(cancellationToken);

        return Result<GetAssignedJobByIdResponce>.Success(assignedJob, "Job collected.");
    }
}

public sealed class GetJobByIdHandler(IUnitOfWork _unitOfWork) : IRequestHandler<GetJobByIdQuery, Result<GetJobByIdResponce>>
{
    public async Task<Result<GetJobByIdResponce>> Handle(GetJobByIdQuery request, CancellationToken cancellationToken)
    {
        // check is employee exists.
        var employer = await _unitOfWork.EmployeeRepository.GetById(request.EmployeeId);
        if (employer is null)
        {
            return Result<GetJobByIdResponce>.Fail("Employee doesn't exist.");
        }

        // get assigned job.
        var job = await _unitOfWork.JobRepository.GetJobById(request.JobId).FirstOrDefaultAsync(cancellationToken);

        if (job is null)
        {
            return Result<GetJobByIdResponce>.Fail("Job doesn't exist.");
        }

        job.IsFavourite = await _unitOfWork
            .EmployeeFavouriteRepository
            .GetAllReadOnly()
            .Where(favourite => favourite.EmployeeId == request.EmployeeId && favourite.EmployerId == job.EmployerId)
            .AnyAsync(cancellationToken);


        return Result<GetJobByIdResponce>.Success(job, "Job collected.");
    }
}

public sealed class GetJobByIdForHomePageHandler(IUnitOfWork _unitOfWork) : IRequestHandler<GetJobByIdForHomePageQuery, Result<GetJobByIdResponce>>
{
    public async Task<Result<GetJobByIdResponce>> Handle(GetJobByIdForHomePageQuery request, CancellationToken cancellationToken)
    {
        // get assigned job.
        var job = await _unitOfWork.JobRepository.GetJobById(request.JobId).FirstOrDefaultAsync(cancellationToken);

        if (job is null)
        {
            return Result<GetJobByIdResponce>.Fail("Job doesn't exist.");
        }

        return Result<GetJobByIdResponce>.Success(job, "Job collected.");
    }
}

public sealed class GetTimesheetByIdHandler(IUnitOfWork _unitOfWork) : IRequestHandler<GetTimesheetByIdQuery, Result<GetTimesheetByIdResponce>>
{
    public async Task<Result<GetTimesheetByIdResponce>> Handle(GetTimesheetByIdQuery request, CancellationToken cancellationToken)
    {
        // check is employee exists.
        var employer = await _unitOfWork.EmployeeRepository.GetById(request.EmployeeId);
        if (employer is null)
        {
            return Result<GetTimesheetByIdResponce>.Fail("Employee doesn't exist.");
        }

        // get assigned job.
        var timesheet = await _unitOfWork.TimesheetRepository.GetTimesheetById(request.TimesheetId, request.EmployeeId).FirstOrDefaultAsync(cancellationToken);
        if (timesheet is null)
        {
            return Result<GetTimesheetByIdResponce>.Fail("Job doesn't exist.");
        }

        return Result<GetTimesheetByIdResponce>.Success(timesheet, "timesheet collected.");
    }
}

public sealed class GetDocumentTypesHandler(IUnitOfWork _unitOfWork) : IRequestHandler<GetDocumentTypesQuery, Result<GetDocumentTypesResponce>>
{
    public async Task<Result<GetDocumentTypesResponce>> Handle(GetDocumentTypesQuery request, CancellationToken cancellationToken)
    {
        GetDocumentTypesResponce result = new GetDocumentTypesResponce();

        // get document types job.
        var types = await _unitOfWork.DocumentTypeRepository.GetDocumentTypes().ToListAsync(cancellationToken);
        result.DocumentOneType = types.Where(type => type.GroupNo != "Group 2b").ToList();
        result.DocumentTwoType = types;
        result.DocumentThreeType = types;

        return Result<GetDocumentTypesResponce>.Success(result, "Document types collected.");
    }
}

public sealed class GetPendingJobCountsHandler(IUnitOfWork _unitOfWork) : IRequestHandler<GetPendingJobCountsQuery, Result<GetPendingJobCountsResponce>>
{
    public async Task<Result<GetPendingJobCountsResponce>> Handle(GetPendingJobCountsQuery request, CancellationToken cancellationToken)
    {
        // check is employee exists.
        var employee = await _unitOfWork.EmployeeRepository.GetById(request.EmployeeId);
        if (employee is null)
        {
            return Result<GetPendingJobCountsResponce>.Fail("Employee doesn't exist.");
        }

        GetPendingJobCountsResponce result = new GetPendingJobCountsResponce();

        result.TotalAssignedJobs = await _unitOfWork.AssignedJobRepository.GetAssignedJobsByEmployeeId(request.EmployeeId)
            .Where(x => x.Date.Date >= DateTime.UtcNow.Date && x.EmployeeTypeId == employee.EmployeeTypeId && x.PostalCode.ToLower() == employee.PinCode.ToLower())
            .CountAsync(cancellationToken);

        result.TotalConfirmedJobs = await _unitOfWork.AssignedJobRepository.GetConfirmedJobsByEmployeeId(request.EmployeeId)
            .Where(x => x.Date.Date >= DateTime.UtcNow.Date && x.EmployeeTypeId == employee.EmployeeTypeId && x.PostalCode.ToLower() == employee.PinCode.ToLower())
            .CountAsync(cancellationToken);

        result.TotalAppliedJobs = await _unitOfWork.AssignedJobRepository.GetAppliedJobsByEmployeeId(request.EmployeeId)
             .Where(x => x.Date.Date >= DateTime.UtcNow.Date && x.EmployeeTypeId == employee.EmployeeTypeId && x.PostalCode.ToLower() == employee.PinCode.ToLower())
             .CountAsync(cancellationToken);

        return Result<GetPendingJobCountsResponce>.Success(result, "Data collected.");
    }
}

public sealed class GetTimesheetCountsHandler(IUnitOfWork _unitOfWork) : IRequestHandler<GetTimesheetCountsQuery, Result<GetTimesheetCountsResponce>>
{
    public async Task<Result<GetTimesheetCountsResponce>> Handle(GetTimesheetCountsQuery request, CancellationToken cancellationToken)
    {
        GetTimesheetCountsResponce result = new GetTimesheetCountsResponce();

        result.TotalPendingTimesheets = await _unitOfWork.TimesheetRepository.GetAllReadOnly().Where(timesheet => (timesheet.Status == (byte)TimeSheetStatusEnum.NotSubmitted || timesheet.Status == (byte)TimeSheetStatusEnum.Pending) && timesheet.EmployeeId == request.EmployeeId).CountAsync(cancellationToken);
        result.TotalApprovedTimesheets = await _unitOfWork.TimesheetRepository.GetAllReadOnly().Where(timesheet => timesheet.Status == (byte)TimeSheetStatusEnum.Approved && timesheet.EmployeeId == request.EmployeeId).CountAsync(cancellationToken);
        result.TotalRejectedTimesheets = await _unitOfWork.TimesheetRepository.GetAllReadOnly().Where(timesheet => timesheet.Status == (byte)TimeSheetStatusEnum.Rejected && timesheet.EmployeeId == request.EmployeeId).CountAsync(cancellationToken);

        return Result<GetTimesheetCountsResponce>.Success(result, "Data collected.");
    }
}

public sealed class SubmitTimesheetHandler(IUnitOfWork _unitOfWork, IMailService _mailService) : IRequestHandler<SubmitTimesheetCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(SubmitTimesheetCommand request, CancellationToken cancellationToken)
    {
        // check is employee exists.
        var employee = await _unitOfWork.EmployeeRepository.GetById(request.Request.EmployeeId);
        if (employee is null)
        {
            return Result<bool>.Fail("Employee doesn't exist.");
        }

        // get assigned job.
        var timesheet = await _unitOfWork.TimesheetRepository
            .GetAll()
                .Include(timesheet => timesheet.Job)
                .Include(timesheet => timesheet.Employer)
            .Where(timesheet => timesheet.Id == request.Request.TimesheetId)
            .FirstOrDefaultAsync(cancellationToken);
        if (timesheet is null)
        {
            return Result<bool>.Fail("Timesheet doesn't exist.");
        }

        // calculate billable hours
        var startTime = TimeOnly.FromDateTime(request.Request.StartTime);
        var endTime = TimeOnly.FromDateTime(request.Request.EndTime);

        var totalMinutes = DateHelper.GetTotalMinutes(startTime, endTime);
        var time = TimeSpan.FromMinutes(totalMinutes);
        var totalHours = new TimeOnly((int)time.TotalHours, (int)time.Minutes);
        //var billableHours = new TimeOnly((int)time.TotalHours, (int)time.Minutes - timesheet.BreakTime);
        var billableHours = totalHours.AddMinutes(-timesheet.BreakTime);
        var billableHourInDecimal = DateHelper.ConvertToDecimal(billableHours);

        timesheet.Notes = request.Request.Notes;
        timesheet.StartTime = startTime;
        timesheet.EndTime = endTime;
        timesheet.BillableHours = billableHours;
        timesheet.BillableHourInDecimal = billableHourInDecimal;
        timesheet.TotalHours = totalHours;
        timesheet.TotalAmount = billableHourInDecimal * timesheet.HourlyRate;
        timesheet.OrginalTotalAmount = billableHourInDecimal * timesheet.HourlyRate;
        timesheet.TotalHolidayAmount = billableHourInDecimal * timesheet.Job.HolidayPayRate;
        timesheet.Status = (byte)TimeSheetStatusEnum.Pending;

        _unitOfWork.TimesheetRepository.Change(timesheet);
        await _unitOfWork.SaveChangesAsync();

        // send email.
        string subject = "Timesheet verification pending:ESGO";
        string content = "EmployeeName has submitted their timesheet for the shift completed on JobDate from StartTime and EndTime. Please login into your account and approve it.";
        content = content.Replace("EmployeeName", $"{employee.FirstName} {employee.LastName}");
        content = content.Replace("JobDate", timesheet.Job.Date.ToString("dd/MM/yyyy"));
        content = content.Replace("StartTime", request.Request.StartTime.ToString("hh:mm tt"));
        content = content.Replace("EndTime", request.Request.EndTime.ToString("hh:mm tt"));

        string username = $"{employee.FirstName} {employee.LastName}";

        string[] emails = await _unitOfWork.EmployerContactDetailRepository.GetAllReadOnly()
            .Where(contact => contact.EmployerId == timesheet.EmployerId)
            .Select(contact => contact.Email)
            .ToArrayAsync(cancellationToken);

        if (!string.IsNullOrEmpty(subject))
        {
            // send email to employer.
            _ = Task.Run(() =>
            {
                _mailService.Send(timesheet.Employer.Email, subject, content, username);
            });

            // send email to contact emails.
            emails.Select(email => Task.Run(() =>
            {
                _mailService.Send(email, subject, content, username);
            })).ToList();
        }

        return Result<bool>.Success(true, "timesheet submited.");
    }
}

public sealed class AddDbsDocumentsHandler(IUnitOfWork _unitOfWork, IFileHelper _fileHelper) : IRequestHandler<AddDbsDocumentsCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(AddDbsDocumentsCommand request, CancellationToken cancellationToken)
    {
        // check document number is valid.
        if (request.Request.DocumentNumber < 1 && request.Request.DocumentNumber > 3)
        {
            return Result<bool>.Fail("Document number doesn't correct.");
        }

        // check is employee exists.
        var employee = await _unitOfWork.EmployeeRepository.GetById(request.Request.EmployeeId);
        if (employee is null)
        {
            return Result<bool>.Fail("Employee doesn't exist.");
        }

        // check document type valid.
        var istypeExist = await _unitOfWork.DocumentTypeRepository.GetAllReadOnly().AnyAsync(type => type.Id == request.Request.DocumentTypeId);
        if (!istypeExist)
        {
            return Result<bool>.Fail("Document type doesn't exist.");
        }

        var folder = "DbsDocuments";
        var fileUrl = await _fileHelper.UploadFile(folder, request.Request.File);

        // chack if documnet already exist then edit it.
        var isAlreadyExist = await _unitOfWork
            .DbsDocumentRepository
            .GetAll()
            .FirstOrDefaultAsync(document => document.EmployeeId == request.Request.EmployeeId && document.DocumentNumber == request.Request.DocumentNumber);
        if (isAlreadyExist is null)
        {
            var dbsDocument = new DbsDocument()
            {
                EmployeeId = request.Request.EmployeeId,
                Url = fileUrl,
                DocumentTypeId = request.Request.DocumentTypeId,
                DocumentNumber = request.Request.DocumentNumber,
                Status = (byte)DbsDocumentStatusEnum.Pending
            };
            await _unitOfWork.DbsDocumentRepository.Add(dbsDocument);
        }
        else
        {
            isAlreadyExist.Url = fileUrl;
            isAlreadyExist.DocumentTypeId = request.Request.DocumentTypeId;
            isAlreadyExist.Status = (byte)DbsDocumentStatusEnum.Pending;
            _unitOfWork.DbsDocumentRepository.Change(isAlreadyExist);
        }
        await _unitOfWork.SaveChangesAsync();

        return Result<bool>.Success(true, "Document added.");
    }
}

public sealed class ApplyJobHandler(IUnitOfWork _unitOfWork) : IRequestHandler<ApplyJobQuery, Result<bool>>
{
    public async Task<Result<bool>> Handle(ApplyJobQuery request, CancellationToken cancellationToken)
    {
        var job = await _unitOfWork.JobRepository.GetById(request.Request.JobId);
        if (job is null)
        {
            return Result<bool>.Fail("Job doesn't exist.");
        }

        var assignedJob = new AssignedJob()
        {
            JobId = job.Id,
            EmployeeId = request.EmployeeId,
            EmployerId = job.EmployerId,
            AppliedDate = DateTime.UtcNow.Date,
            HourWorked = job.JobHoursPerDay,
            JobStatus = (byte)JobStatusEnum.Applied,
            IsSelected = false
        };

        await _unitOfWork.AssignedJobRepository.Add(assignedJob);
        await _unitOfWork.SaveChangesAsync();

        return Result<bool>.Success(true, "Job Applied.");
    }
}

public sealed class GetEmployeeAllDetailsByIdForEmployeeHandler(IUnitOfWork _unitOfWork) : IRequestHandler<GetEmployeeAllDetailsByIdForEmployeeQuery, Result<GetEmployeeAllDetailsByIdForEmployeeResponce>>
{
    public async Task<Result<GetEmployeeAllDetailsByIdForEmployeeResponce>> Handle(GetEmployeeAllDetailsByIdForEmployeeQuery request, CancellationToken cancellationToken)
    {
        // check is employee exist or not.
        var employee = await _unitOfWork
            .EmployeeRepository
            .GetAllReadOnly()
                .Include(employee => employee.EmployementType)
            .Where(employee => employee.Id == request.EmployeeId)
            .Select(employee => new GetEmployeeAllDetailsByIdForEmployeeResponce()
            {
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = employee.Email,
                PhoneNumber = employee.PhoneNumber,
                PinCode = employee.PinCode,
                EmployementTypeDescription = employee.EmployementType.Name,
                BiometricResidenceCardUrl = employee.BiometricResidenceCardUrl,
                PassportUrl = employee.PassportUrl,
                P45DocumentUrl = employee.P45DocumentUrl,
                AccessNIUrl = employee.AccessNIUrl,
                ProfileImageUrl = employee.ProfileImageUrl
            })
            .FirstOrDefaultAsync(cancellationToken);
        if (employee is null)
        {
            return Result<GetEmployeeAllDetailsByIdForEmployeeResponce>.Fail("employee doesn't exist.");
        }

        if (!string.IsNullOrEmpty(employee.BiometricResidenceCardUrl)) // check is card added then add 25.
        {
            employee.ProfilePercentage = employee.ProfilePercentage + 25;
        }
        if (!string.IsNullOrEmpty(employee.PassportUrl)) // check is passport added then add 25.
        {
            employee.ProfilePercentage = employee.ProfilePercentage + 25;
        }
        if (!string.IsNullOrEmpty(employee.P45DocumentUrl)) // check is passport added then add 25.
        {
            employee.ProfilePercentage = employee.ProfilePercentage + 25;
        }
        if (!string.IsNullOrEmpty(employee.AccessNIUrl)) // check is passport added then add 25.
        {
            employee.ProfilePercentage = employee.ProfilePercentage + 25;
        }

        // get payments details.
        var jobs = await _unitOfWork
            .AssignedJobRepository
            .GetAllReadOnly()
                .Include(assignedJob => assignedJob.Job)
            .Where(assignedJob => assignedJob.EmployeeId == request.EmployeeId)
            .ToListAsync(cancellationToken);
        employee.TotalJobs = jobs.Where(assignedJob => assignedJob.JobStatus == (byte)JobStatusEnum.Completed).Count();
        employee.TotalHours = jobs.Where(assignedJob => assignedJob.JobStatus == (byte)JobStatusEnum.Completed).Select(job => job.Job.JobHoursPerDay).Sum();
        employee.AmountPending = jobs.Where(assignedJob => assignedJob.JobStatus == (byte)JobStatusEnum.Completed).Select(job => job.Job.HourlyRate).Sum();
        employee.AmountCredited = 0;

        // get job information.
        employee.ActiveJobs = jobs.Where(assignedJob => assignedJob.JobStatus == (byte)JobStatusEnum.Open).Count();
        employee.AppliedJobs = jobs.Where(assignedJob => assignedJob.JobStatus == (byte)JobStatusEnum.Applied).Count();
        employee.RejectedJobs = jobs.Where(assignedJob => assignedJob.JobStatus == (byte)JobStatusEnum.UnSuccessful).Count();
        employee.CancelledJobs = jobs.Where(assignedJob => assignedJob.JobStatus == (byte)JobStatusEnum.Cancelled).Count();

        employee.Experiences = await _unitOfWork.EmployementRepository.GetAllReadOnly().Where(employement => employement.EmployeeId == request.EmployeeId)
            .Select(employement => new EmployementDto()
            {
                CompanyName = employement.CompanyName,
                StartDate = employement.StartDate.Date,
                EndDate = employement.EndDate.Date
            })
            .ToListAsync(cancellationToken);

        return Result<GetEmployeeAllDetailsByIdForEmployeeResponce>.Success(employee, "Employee collected successfully.");
    }
}

public sealed class HomeJobSearchHandler(IUnitOfWork _unitOfWork)
    : IRequestHandler<HomeJobSearchQuery, Result<PaginationModel<HomeJobSearchResponce>>>
{
    public async Task<Result<PaginationModel<HomeJobSearchResponce>>> Handle(HomeJobSearchQuery request, CancellationToken cancellationToken)
    {
        // check is employee exists.
        var employee = await _unitOfWork.EmployeeRepository.GetById(request.Request.EmployeeId);
        if (employee is null)
        {
            return Result<PaginationModel<HomeJobSearchResponce>>.Fail("Employee doesn't exist.");
        }

        // this is use to just intialize.
        IQueryable<HomeJobSearchResponce> jobs = _unitOfWork
            .JobRepository
            .GetAllReadOnly()
            .Select(x => new HomeJobSearchResponce());

        // check is job assigned or not. 
        var jobIds = await _unitOfWork.AssignedJobRepository.GetAllReadOnly().Where(x => x.EmployeeId == request.Request.EmployeeId).Select(x => x.JobId).ToListAsync(cancellationToken);

        if (request.Request.TabName == "all")
        {
            jobs = _unitOfWork
            .JobRepository
            .GetAllReadOnly()
            .Include(x => x.Employer)
            .Where(x => !jobIds.Contains(x.Id)
                            && x.Date.Date >= DateTime.UtcNow.Date
                            && x.EmployeeTypeId == employee.EmployeeTypeId
                            && x.Employer.PinCode.ToLower() == employee.PinCode.ToLower()
                            // filter accourding to parameters.
                            && request.Request.FromDate.Date != DateTime.MinValue.Date ? x.Date.Date >= request.Request.FromDate.Date : true
                            && request.Request.MinRate != 0 ? x.HourlyRate >= request.Request.MinRate : true
                            && !string.IsNullOrEmpty(request.Request.PostalCode) ? x.Employer.PinCode.ToLower() == request.Request.PostalCode.ToLower() : true)
            .Select(x => new HomeJobSearchResponce()
            {
                JobId = x.Id,
                Date = x.Date.Date,
                HourlyRate = x.HourlyRate,
                OrganisationName = x.Employer.CompanyName,
                PostalCode = x.Employer.PinCode,
                ShiftTime = $"{x.ShiftStartTime} - {x.ShiftEndTime}",
                IsUrgent = x.JobTypeId == (byte)JobTypeEnum.UrgentJob ? true : false
            });
        }
        else if (request.Request.TabName == "favourite")
        {
            // get favourite employers.
            var favouriteEmployerIds = await _unitOfWork
                .EmployeeFavouriteRepository
                .GetAllReadOnly()
                .Where(favourite => favourite.EmployeeId == request.Request.EmployeeId)
                .Select(favourite => favourite.EmployerId)
                .ToListAsync(cancellationToken);

            jobs = _unitOfWork
                .JobRepository
                .GetAllReadOnly()
                .Include(x => x.Employer)
                .Where(x => !jobIds.Contains(x.Id)
                                && favouriteEmployerIds.Contains(x.EmployerId)
                                && x.Date.Date >= DateTime.UtcNow.Date
                                && x.EmployeeTypeId == employee.EmployeeTypeId
                                && x.Employer.PinCode.ToLower() == employee.PinCode.ToLower())
                .Select(x => new HomeJobSearchResponce()
                {
                    JobId = x.Id,
                    Date = x.Date.Date,
                    HourlyRate = x.HourlyRate,
                    OrganisationName = x.Employer.CompanyName,
                    PostalCode = x.Employer.PinCode,
                    ShiftTime = $"{x.ShiftStartTime} - {x.ShiftEndTime}",
                    IsUrgent = x.JobTypeId == (byte)JobTypeEnum.UrgentJob ? true : false
                });
        }
        else if (request.Request.TabName == "urgent")
        {
            jobs = _unitOfWork
                .JobRepository
                .GetAllReadOnly()
                .Include(x => x.Employer)
                .Where(x => !jobIds.Contains(x.Id)
                                && x.JobTypeId == (byte)JobTypeEnum.UrgentJob
                                && x.Date.Date >= DateTime.UtcNow.Date
                                && x.EmployeeTypeId == employee.EmployeeTypeId
                                && x.Employer.PinCode.ToLower() == employee.PinCode.ToLower()
                            // filter accourding to parameters.
                            && request.Request.FromDate.Date != DateTime.MinValue.Date ? x.Date.Date >= request.Request.FromDate.Date : true
                            && request.Request.MinRate != 0 ? x.HourlyRate >= request.Request.MinRate : true
                            && !string.IsNullOrEmpty(request.Request.PostalCode) ? x.Employer.PinCode.ToLower() == request.Request.PostalCode.ToLower() : true)
                .Select(x => new HomeJobSearchResponce()
                {
                    JobId = x.Id,
                    Date = x.Date.Date,
                    HourlyRate = x.HourlyRate,
                    OrganisationName = x.Employer.CompanyName,
                    PostalCode = x.Employer.PinCode,
                    ShiftTime = $"{x.ShiftStartTime} - {x.ShiftEndTime}",
                    IsDummy = x.IsDummy,
                    IsUrgent = x.JobTypeId == (byte)JobTypeEnum.UrgentJob ? true : false
                });
        }
        else
        {
            return Result<PaginationModel<HomeJobSearchResponce>>.Fail("Invalid tab.");
        }


        // Add pagination
        PaginationModel<HomeJobSearchResponce> model = new PaginationModel<HomeJobSearchResponce>();
        if (request.Request.PageSize > 0)
        {
            var result = new PagedList<HomeJobSearchResponce>(jobs, request.Request.PageIndex, request.Request.PageSize, false);
            model.PagedContent = result;
            model.PagingFilteringContext.LoadPagedList(result);
        }

        return Result<PaginationModel<HomeJobSearchResponce>>.Success(model, "jobs list.");
    }
}

public sealed class PendingJobSearchHandler(IUnitOfWork _unitOfWork)
    : IRequestHandler<PendingJobSearchQuery, Result<PaginationModel<PendingJobSearchResponce>>>
{
    public async Task<Result<PaginationModel<PendingJobSearchResponce>>> Handle(PendingJobSearchQuery request, CancellationToken cancellationToken)
    {
        // check is employee exists.
        var employee = await _unitOfWork.EmployeeRepository.GetById(request.Request.EmployeeId);
        if (employee is null)
        {
            return Result<PaginationModel<PendingJobSearchResponce>>.Fail("Employee doesn't exist.");
        }

        // this is use to just intialize.
        IQueryable<PendingJobSearchResponce> jobs = _unitOfWork
            .JobRepository
            .GetAllReadOnly()
            .Select(x => new PendingJobSearchResponce());

        if (request.Request.TabName == "applied")
        {
            jobs = _unitOfWork.AssignedJobRepository.GetAppliedJobsByEmployeeId(request.Request.EmployeeId)
                .Select(x => new PendingJobSearchResponce()
                {
                    AssignedJobId = x.AssignedJobId,
                    OrganisationName = x.OrganisationName,
                    PostalCode = x.PostalCode,
                    Date = x.Date,
                    HourlyRate = x.HourlyRate,
                    ShiftTime = x.ShiftTime,
                    IsUrgent = x.IsUrgent,
                    IsSelected = x.IsSelected,
                })
                .Where(x =>     // filter accourding to parameters.
                            request.Request.FromDate.Date != DateTime.MinValue.Date ? x.Date.Date >= request.Request.FromDate.Date : true
                            && request.Request.MinRate != 0 ? x.HourlyRate >= request.Request.MinRate : true
                            && !string.IsNullOrEmpty(request.Request.PostalCode) ? x.PostalCode.ToLower() == request.Request.PostalCode.ToLower() : true);
        }
        else if (request.Request.TabName == "assigned")
        {
            jobs = _unitOfWork.AssignedJobRepository.GetAssignedJobsByEmployeeId(request.Request.EmployeeId)
                .Select(x => new PendingJobSearchResponce()
                {
                    AssignedJobId = x.AssignedJobId,
                    OrganisationName = x.OrganisationName,
                    PostalCode = x.PostalCode,
                    Date = x.Date,
                    HourlyRate = x.HourlyRate,
                    ShiftTime = x.ShiftTime,
                    IsUrgent = x.IsUrgent
                })
                .Where(x =>     // filter accourding to parameters.
                            request.Request.FromDate.Date != DateTime.MinValue.Date ? x.Date.Date >= request.Request.FromDate.Date : true
                            && request.Request.MinRate != 0 ? x.HourlyRate >= request.Request.MinRate : true
                            && !string.IsNullOrEmpty(request.Request.PostalCode) ? x.PostalCode.ToLower() == request.Request.PostalCode.ToLower() : true);
        }
        else if (request.Request.TabName == "confirmed")
        {
            jobs = _unitOfWork.AssignedJobRepository.GetConfirmedJobsByEmployeeId(request.Request.EmployeeId)
                .Select(x => new PendingJobSearchResponce()
                {
                    AssignedJobId = x.AssignedJobId,
                    OrganisationName = x.OrganisationName,
                    PostalCode = x.PostalCode,
                    Date = x.Date,
                    HourlyRate = x.HourlyRate,
                    ShiftTime = x.ShiftTime,
                    IsUrgent = x.IsUrgent
                })
                .Where(x =>     // filter accourding to parameters.
                            request.Request.FromDate.Date != DateTime.MinValue.Date ? x.Date.Date >= request.Request.FromDate.Date : true
                            && request.Request.MinRate != 0 ? x.HourlyRate >= request.Request.MinRate : true
                            && !string.IsNullOrEmpty(request.Request.PostalCode) ? x.PostalCode.ToLower() == request.Request.PostalCode.ToLower() : true);
        }
        else
        {
            return Result<PaginationModel<PendingJobSearchResponce>>.Fail("Invalid tab.");
        }


        // Add pagination
        PaginationModel<PendingJobSearchResponce> model = new PaginationModel<PendingJobSearchResponce>();
        if (request.Request.PageSize > 0)
        {
            var result = new PagedList<PendingJobSearchResponce>(jobs, request.Request.PageIndex, request.Request.PageSize, false);
            model.PagedContent = result;
            model.PagingFilteringContext.LoadPagedList(result);
        }

        return Result<PaginationModel<PendingJobSearchResponce>>.Success(model, "jobs list.");
    }
}

public sealed class CompletedJobSearchHandler(IUnitOfWork _unitOfWork)
    : IRequestHandler<CompletedJobSearchQuery, Result<PaginationModel<CompletedJobSearchResponce>>>
{
    public async Task<Result<PaginationModel<CompletedJobSearchResponce>>> Handle(CompletedJobSearchQuery request, CancellationToken cancellationToken)
    {
        // check is employee exists.
        var employee = await _unitOfWork.EmployeeRepository.GetById(request.Request.EmployeeId);
        if (employee is null)
        {
            return Result<PaginationModel<CompletedJobSearchResponce>>.Fail("Employee doesn't exist.");
        }

        // get confirmed job
        var jobs = _unitOfWork.AssignedJobRepository.GetCompletedJobsByEmployee(request.Request.EmployeeId)
                            .Select(x => new CompletedJobSearchResponce()
                            {
                                AssignedJobId = x.AssignedJobId,
                                StartDate = x.StartDate,
                                Location = x.Location,
                                ShiftStartTime = x.ShiftStartTime,
                                ShiftEndTime = x.ShiftEndTime,
                                EmployeeType = x.EmployeeType,
                                PinCode = x.PinCode,
                                HourlyRate = x.HourlyRate,
                                Status = x.Status,
                                JobStatus = x.JobStatus,
                                IsUrgent = x.IsUrgent,
                            })
                .Where(x =>     // filter accourding to parameters.
                            request.Request.FromDate.Date != DateTime.MinValue.Date ? x.StartDate.Date >= request.Request.FromDate.Date : true
                            && request.Request.MinRate != 0 ? x.HourlyRate >= request.Request.MinRate : true
                            && !string.IsNullOrEmpty(request.Request.PostalCode) ? x.PinCode.ToLower() == request.Request.PostalCode.ToLower() : true);

        // Add pagination
        PaginationModel<CompletedJobSearchResponce> model = new PaginationModel<CompletedJobSearchResponce>();
        if (request.Request.PageSize > 0)
        {
            var result = new PagedList<CompletedJobSearchResponce>(jobs, request.Request.PageIndex, request.Request.PageSize, false);
            model.PagedContent = result;
            model.PagingFilteringContext.LoadPagedList(result);
        }

        return Result<PaginationModel<CompletedJobSearchResponce>>.Success(model, "jobs list.");
    }
}

public sealed class GetOpenJobsForHomePageHandler(IUnitOfWork _unitOfWork)
    : IRequestHandler<GetOpenJobsForHomePageQuery, Result<PaginationModel<GetOpenJobsForHomePageResponce>>>
{
    public async Task<Result<PaginationModel<GetOpenJobsForHomePageResponce>>> Handle(GetOpenJobsForHomePageQuery request, CancellationToken cancellationToken)
    {
        // check is job assigned or not. 
        var jobIds = await _unitOfWork.AssignedJobRepository.GetAllReadOnly().Select(x => x.JobId).ToListAsync(cancellationToken);

        var jobs = _unitOfWork
            .JobRepository
            .GetAllReadOnly()
            .Include(x => x.Employer)
            .Where(x => !jobIds.Contains(x.Id)
                            && x.Date.Date >= DateTime.UtcNow.Date
                            && x.EmployeeTypeId == request.Request.EmployeeTypeId)
            .Select(x => new GetOpenJobsForHomePageResponce()
            {
                JobId = x.Id,
                Date = x.Date.Date,
                HourlyRate = x.HourlyRate,
                OrganisationName = x.Employer.CompanyName,
                PostalCode = x.Employer.PinCode,
                ShiftTime = $"{x.ShiftStartTime} - {x.ShiftEndTime}",
                IsUrgent = x.JobTypeId == (byte)JobTypeEnum.UrgentJob ? true : false,
                IsDummy = x.IsDummy,
                CreatedDate = x.CreatedDate.Date,
                Location = x.Employer.Location,
                EmployeeTypeId = x.EmployeeTypeId
            });

        // Add pagination
        PaginationModel<GetOpenJobsForHomePageResponce> model = new PaginationModel<GetOpenJobsForHomePageResponce>();
        if (request.Request.PageSize > 0)
        {
            var result = new PagedList<GetOpenJobsForHomePageResponce>(jobs, request.Request.PageIndex, request.Request.PageSize, false);
            model.PagedContent = result;
            model.PagingFilteringContext.LoadPagedList(result);
        }

        return Result<PaginationModel<GetOpenJobsForHomePageResponce>>.Success(model, "jobs list.");
    }
}

public sealed class WebGuestJobSearchHandler(IUnitOfWork _unitOfWork)
    : IRequestHandler<WebGuestJobSearchQuery, Result<PaginationModel<WebGuestJobSearchResponce>>>
{
    public async Task<Result<PaginationModel<WebGuestJobSearchResponce>>> Handle(WebGuestJobSearchQuery request, CancellationToken cancellationToken)
    {
        // check is job assigned or not. 
        var jobIds = await _unitOfWork.AssignedJobRepository.GetAllReadOnly().Select(x => x.JobId).ToListAsync(cancellationToken);

        var jobs = _unitOfWork
            .JobRepository
            .GetAllReadOnly()
            .Include(x => x.Employer)
            .Where(x => !jobIds.Contains(x.Id)
                            && x.Date.Date >= DateTime.UtcNow.Date
                            && x.EmployeeTypeId == request.Request.EmployeeTypeId)
            .Select(x => new WebGuestJobSearchResponce()
            {
                JobId = x.Id,
                Date = x.Date.Date,
                HourlyRate = x.HourlyRate,
                OrganisationName = x.Employer.CompanyName,
                PostalCode = x.Employer.PinCode,
                ShiftTime = $"{x.ShiftStartTime} - {x.ShiftEndTime}",
                IsUrgent = x.JobTypeId == (byte)JobTypeEnum.UrgentJob ? true : false,
                IsDummy = x.IsDummy,
                CreatedDate = x.CreatedDate.Date,
                Location = x.Employer.Location,
                EmployeeTypeId = x.EmployeeTypeId
            });

        // filter accourding to parameters.
        if (request.Request.FromDate.Date != DateTime.MinValue.Date)
        {
            jobs = jobs.Where(job => job.Date.Date >= request.Request.FromDate.Date);
        }
        if (request.Request.MinRate != 0)
        {
            jobs = jobs.Where(job => job.HourlyRate >= request.Request.MinRate);
        }
        if (!string.IsNullOrEmpty(request.Request.PostalCode))
        {
            jobs = jobs.Where(job => job.PostalCode.ToLower() == request.Request.PostalCode.ToLower());
        }

        jobs = jobs.OrderBy(job => job.Date);
        // Add pagination
        PaginationModel<WebGuestJobSearchResponce> model = new PaginationModel<WebGuestJobSearchResponce>();
        if (request.Request.PageSize > 0)
        {
            var result = new PagedList<WebGuestJobSearchResponce>(jobs, request.Request.PageIndex, request.Request.PageSize, false);
            model.PagedContent = result;
            model.PagingFilteringContext.LoadPagedList(result);
        }

        return Result<PaginationModel<WebGuestJobSearchResponce>>.Success(model, "jobs list.");
    }
}

public sealed class GetCalenderJobsHandler(IUnitOfWork _unitOfWork)
    : IRequestHandler<GetCalenderJobsQuery, Result<GetCalenderJobsResponce>>
{
    public async Task<Result<GetCalenderJobsResponce>> Handle(GetCalenderJobsQuery request, CancellationToken cancellationToken)
    {
        // check is employee exists.
        var employee = await _unitOfWork.EmployeeRepository.GetById(request.Request.EmployeeId);
        if (employee is null)
        {
            return Result<GetCalenderJobsResponce>.Fail("Employee doesn't exist.");
        }

        var result = new GetCalenderJobsResponce();

        // get applied jobs.
        result.AppliedJobs = await _unitOfWork.AssignedJobRepository.GetAppliedJobsByEmployeeId(request.Request.EmployeeId)
                .Select(x => new JobForGetCalenderJobsResponce()
                {
                    AssignedJobId = x.AssignedJobId,
                    OrganisationName = x.OrganisationName,
                    PostalCode = x.PostalCode,
                    Date = x.Date,
                    HourlyRate = x.HourlyRate,
                    ShiftTime = x.ShiftTime,
                    IsUrgent = x.IsUrgent,
                    IsSelected = x.IsSelected,
                })
                .Where(x => x.Date.Date >= request.Request.FromDate.Date && x.Date.Date <= request.Request.ToDate.Date)
                .ToListAsync(cancellationToken);

        // get assigned jobs.
        result.AssignedJobs = await _unitOfWork.AssignedJobRepository.GetAssignedJobsByEmployeeId(request.Request.EmployeeId)
                .Select(x => new JobForGetCalenderJobsResponce()
                {
                    AssignedJobId = x.AssignedJobId,
                    OrganisationName = x.OrganisationName,
                    PostalCode = x.PostalCode,
                    Date = x.Date,
                    HourlyRate = x.HourlyRate,
                    ShiftTime = x.ShiftTime,
                    IsUrgent = x.IsUrgent
                })
                .Where(x => x.Date.Date >= request.Request.FromDate.Date && x.Date.Date <= request.Request.ToDate.Date)
                .ToListAsync(cancellationToken);

        // get confirmed jobs
        result.ConfirmedJobs = await _unitOfWork.AssignedJobRepository.GetConfirmedJobsByEmployeeId(request.Request.EmployeeId)
                .Select(x => new JobForGetCalenderJobsResponce()
                {
                    AssignedJobId = x.AssignedJobId,
                    OrganisationName = x.OrganisationName,
                    PostalCode = x.PostalCode,
                    Date = x.Date,
                    HourlyRate = x.HourlyRate,
                    ShiftTime = x.ShiftTime,
                    IsUrgent = x.IsUrgent
                })
                .Where(x => x.Date.Date >= request.Request.FromDate.Date && x.Date.Date <= request.Request.ToDate.Date)
                .ToListAsync(cancellationToken);

        return Result<GetCalenderJobsResponce>.Success(result, "jobs list.");
    }
}

public sealed class AddToFavouriteHandler(IUnitOfWork _unitOfWork)
    : IRequestHandler<AddToFavouriteCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(AddToFavouriteCommand request, CancellationToken cancellationToken)
    {
        // check employer exist.
        var employer = await _unitOfWork.EmployerRepository.GetById(request.Request.EmployerId);
        if (employer is null)
        {
            return Result<bool>.Fail("Employer doesn't exist.");
        }

        // check is employee exists.
        var employee = await _unitOfWork.EmployeeRepository.GetById(request.Request.EmployeeId);
        if (employee is null)
        {
            return Result<bool>.Fail("Employee doesn't exist.");
        }

        // check is already added to favourite. 
        var favourite = await _unitOfWork
            .EmployeeFavouriteRepository
            .GetAllReadOnly()
            .Where(favourite => favourite.EmployerId == request.Request.EmployerId && favourite.EmployeeId == request.Request.EmployeeId)
            .FirstOrDefaultAsync(cancellationToken);

        if (favourite is null)
        {
            // add favourite
            var data = new EmployeeFavourite()
            {
                EmployerId = request.Request.EmployerId,
                EmployeeId = request.Request.EmployeeId
            };
            await _unitOfWork.EmployeeFavouriteRepository.Add(data);
        }
        else
        {
            _unitOfWork.EmployeeFavouriteRepository.DeleteByEntity(favourite);
        }
        await _unitOfWork.SaveChangesAsync();

        return Result<bool>.Success(true, "changed successfully.");
    }
}

public sealed class EmailVerificationHandler(IUnitOfWork _unitOfWork, UserManager<User> _userManager) : IRequestHandler<EmailVerificationQuery, Result<bool>>
{
    public async Task<Result<bool>> Handle(EmailVerificationQuery request, CancellationToken cancellationToken)
    {
        // decode the token.
        var userId = Encryption.DeCrypt(request.Token);
        if (string.IsNullOrEmpty(userId))
        {
            return Result<bool>.Fail("Invalid token.");
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return Result<bool>.Fail("Invalid token.");
        }

        user.EmailConfirmed = true;

        await _unitOfWork.SaveChangesAsync();

        return Result<bool>.Success(true, "Email is confirmed.");
    }
}

public sealed class GetAssignedJobByEncryptIdHandler(IUnitOfWork _unitOfWork) : IRequestHandler<GetAssignedJobByEncryptIdQuery, Result<GetAssignedJobByEncryptIdResponce>>
{
    public async Task<Result<GetAssignedJobByEncryptIdResponce>> Handle(GetAssignedJobByEncryptIdQuery request, CancellationToken cancellationToken)
    {
        // decode the AssignedJobId.
        var assignedJobId = Encryption.DeCrypt(request.AssignedJobId);
        if (string.IsNullOrEmpty(assignedJobId))
        {
            return Result<GetAssignedJobByEncryptIdResponce>.Fail("Invalid AssignedJobId.");
        }

        var assignedJob = await _unitOfWork.AssignedJobRepository
            .GetAllReadOnly()
                .Include(assignedJob => assignedJob.Job)
                .Include(assignedJob => assignedJob.Employer)
            .FirstOrDefaultAsync(assignedJob => assignedJob.Id == Convert.ToInt32(assignedJobId), cancellationToken);
        if (assignedJob is null)
        {
            return Result<GetAssignedJobByEncryptIdResponce>.Fail("Invalid AssignedJobId.");
        }

        var result = new GetAssignedJobByEncryptIdResponce()
        {
            AssignedJobId = assignedJob.Id,
            JobDate = assignedJob.Job.Date.Date,
            Shift = $"{assignedJob.Job.ShiftStartTime.ToString("hh:mm tt")} - {assignedJob.Job.ShiftEndTime.ToString("hh:mm tt")}",
            Status = assignedJob.JobStatus,
            CompanyName = assignedJob.Employer.CompanyName,
            JobId = assignedJob.JobId
        };

        return Result<GetAssignedJobByEncryptIdResponce>.Success(result, "Date collected.");
    }
}

public sealed class ChangeJobStatusToConfirmWithEmailHandler(IUnitOfWork _unitOfWork, IMailService _mailService) : IRequestHandler<ChangeJobStatusToConfirmWithEmailQuery, Result<bool>>
{
    public async Task<Result<bool>> Handle(ChangeJobStatusToConfirmWithEmailQuery request, CancellationToken cancellationToken)
    {
        // decode the AssignedJobId.
        var assignedJobId = Encryption.DeCrypt(request.AssignedJobId);
        if (string.IsNullOrEmpty(assignedJobId))
        {
            return Result<bool>.Fail("Invalid AssignedJobId.");
        }

        // get assigned job.
        var assignedJob = await _unitOfWork.AssignedJobRepository
            .GetWithCondition(assignedJob => assignedJob.Id == Convert.ToInt32(assignedJobId) && assignedJob.JobStatus == (byte)JobStatusEnum.Open)
                .Include(assignedJob => assignedJob.Job)
            .FirstOrDefaultAsync(cancellationToken);
        if (assignedJob is null)
        {
            return Result<bool>.Fail("Job doesn't exist.");
        }

        assignedJob.JobStatus = (byte)JobStatusEnum.Confirmed;
        assignedJob.ConfirmationDate = DateTime.UtcNow;

        _unitOfWork.AssignedJobRepository.Change(assignedJob);
        await _unitOfWork.SaveChangesAsync();

        var employee = await _unitOfWork.EmployeeRepository.GetById(assignedJob.EmployeeId);
        if (employee is not null)
        {
            // send email.
            string subject = $"Important protocols - ESGO";
            var content = " <p style='color:#000;font-weight:00;font-size:14px;padding:0px 10px 0px 20px' > First of all, A warm welcome to you and thank you for registering with ESGO.  We value your hard work and effort put forward to take care of an individual who requires your care. </p>";
            content = content + "<p style='color:#000;font-weight:00;font-size:14px;padding:0px 10px 0px 20px'>This job requires a lot of patience and care as you are helping to safe guard another human being who is not strong enough to protect themselves.You are providing attention to those who require considerable help.  We would like to point out some important protocols that should be followed at all times. </p>";
            content = content + "<p style='color:#000;font-weight:00;font-size:14px;padding:0px 10px 0px 20px'> The below listed points are some important protocols which should be followed at all times while on duty. </p>";
            content = content + "<ol style='color:#000;font-weight:00;font-size:14px;padding:0px 10px 0px 20px' >";
            content = content + "  <li > Do not use mobile phones while at work.</ li >";
            content = content + "      <li > Good hygiene and tidy uniform</ li >";
            content = content + "        <li > Do not use regional language in front of a resident or in the care home vicinity.</ li >";
            content = content + "           <li > Be punctual </ li >";
            content = content + "    <li > Patience and care is a must have attitude towards a resident.</ li >";
            content = content + "      <li > There is no space for assumptions.Any doubts to be clarified as soon as possible with the a senior staff or in-house RGN.</ li >";
            content = content + "        <li > Reporting and recording any medical errors or any symptoms observed.</ li >";
            content = content + "            <li>   No personal relationships  </li >";
            content = content + "  <li > Above all, a good smile, gentle talk is a big medicine for every resident.</ li >";
            content = content + "    </ol >";
            content = content + "  <p style='color:#000;font-weight:00;font-size:14px;padding:0px 10px 0px 20px'> As a care provider, there are certain things which you have to be careful about, when working at a care home.Any safe guarding error will be recorded and you will have to provide a statement for the same. This will affect you in future as it will be reflected in your DBS certificate if the error has caused serious injury or loss of life.Care homes are very vigilant in monitoring resident’s health and wellbeing.</ p >";
            content = content + "   <p style='color:#000;font-weight:00;font-size:14px;padding:0px 10px 0px 20px'> Also, if you need a reference from ESGO or from the care home. Its a must that you perform in the best possible way. We will be providing a true statement regarding your performance and reliability.</ p >";
            content = content + "   <p style='color:#000;font-weight:00;font-size:14px;padding:0px 10px 0px 20px'> We are giving this information to make you aware of the seriousness of the job and also the consequences. We at ESGO are always there for you if you need any advice or help.We urge you to put the best of your ability and sincerity.  </p >";
            content = content + "  <p style='color:#000;font-weight:00;font-size:14px;padding:0px 10px 0px 20px'> We look forward for the best from your part and we are confident that you will adhere to all the rules   </ p > ";
            content = content + "< p> Welcome again.</p>";
            string username = $"{employee.FirstName} {employee.LastName}";

            _ = Task.Run(() =>
            {
                _mailService.Send(employee.Email, subject, content, username);
            });
        }
        return Result<bool>.Success(true, "Job status changed.");
    }
}

public sealed class SendEmailForResetPasswordHandlers(UserManager<User> _userManger, IConfiguration _configuration, IUnitOfWork _unitOfWork, IMailService _mailService)
    : IRequestHandler<SendEmailForResetPasswordCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(SendEmailForResetPasswordCommand request, CancellationToken cancellationToken)
    {
        // login with email.
        var user = await _userManger.FindByEmailAsync(request.Email);
        if (user is null)
        {
            return Result<bool>.Fail("There is no user.");
        }

        string url = string.Empty;
        if (user.UserTypeId == (byte)UserTypeEnum.Employee && user.UserTypeId == request.UserTypeId)
        {
            url = _configuration["WebsiteSetting:EmployeeResetPasswordUrl"] ?? string.Empty;
        }
        else if (user.UserTypeId == (byte)UserTypeEnum.Employer && user.UserTypeId == request.UserTypeId)
        {
            url = _configuration["WebsiteSetting:EmployerResetPasswordUrl"] ?? string.Empty;
        }
        else
        {
            return Result<bool>.Fail("There is no user.");
        }

        if (url is null)
        {
            return Result<bool>.Fail("Something went wrong. Please contact to support.");
        }
        url = url + Encryption.Crypt(user.Id.ToString());

        string username = $"{user.FirstName} {user.LastName}";
        string subject = "Reset password";
        string content = @"<P style='color: #000; font-weight: 100; font-size: 20px; padding: 0px 10px 0px 10px;'>We are sending you this e-mail because you have requested a password reset. Click on the below link to create a new password.</p>";
        content = content + "<P style='color: #000; font-weight: 100; font-size: 20px; padding: 20px 10px 20px 10px; text-align:center'><a href='" + url + "' style='padding:10px;background-color:#eb2126;color:white'>Reset Password</a> </p> ";

        // send email.
        _ = Task.Run(() =>
        {
            _mailService.Send(request.Email, subject, content, username);
        });

        return Result<bool>.Success(true, "Email sent successfully!");
    }
}

public sealed class ResetPasswordHandlers(UserManager<User> _userManger, IUnitOfWork _unitOfWork) : IRequestHandler<ResetPasswordCommand, Result<IEnumerable<string>>>
{
    public async Task<Result<IEnumerable<string>>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        // check eser exists.
        var userId = Encryption.DeCrypt(request.Token);
        if (string.IsNullOrEmpty(userId))
        {
            return Result<IEnumerable<string>>.Fail("Invalid token.");
        }

        var user = await _userManger.FindByIdAsync(userId);
        if (user is null)
        {
            return Result<IEnumerable<string>>.Fail("Invalid token.");
        }

        // Compare the Password and ConfirmPassword fields.
        if (request.Password.Trim() != request.ConfirmPassword.Trim())
        {
            return Result<IEnumerable<string>>.Fail("Password doesn't match its confirmation.");
        }

        // reset the password.
        var result = await _userManger.ChangePasswordAsync(user, user.PlainPassword, request.Password);
        if (result.Succeeded)
        {
            // update plain password.
            user.PlainPassword = request.Password.Trim();
            await _unitOfWork.SaveChangesAsync();
            return Result<IEnumerable<string>>.Success(new List<string>(), "Password has been reset successfully!");
        }


        return Result<IEnumerable<string>>.Fail("Something went wrong", result.Errors.Select(e => e.Description));
    }
}