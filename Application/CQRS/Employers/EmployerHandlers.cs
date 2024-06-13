using Application.Abstractions.Services;
using Domain.CQRS.Admins;
using Domain.Entities;

namespace Application.CQRS.Employers;

public sealed class EmployerRegisterHandler(UserManager<User> _userManger, IUnitOfWork _unitOfWork, IMailService _mailService)
    : IRequestHandler<EmployerRegisterCommand, Result<IEnumerable<string>>>
{
    public async Task<Result<IEnumerable<string>>> Handle(EmployerRegisterCommand request, CancellationToken cancellationToken)
    {
        // Compare the Password and ConfirmPassword fields.
        if (request.Request.Password != request.Request.ConfirmPassword)
        {
            return Result<IEnumerable<string>>.Fail("Confirm password doesn't match the password");
        }

        // check company type exist.
        var isExist = Enum.IsDefined(typeof(CompanyTypeEnum), request.Request.CompanyTypeId);
        if (!isExist)
            return Result<IEnumerable<string>>.Fail("Invalid company type.");

        if (!request.Request.ContactDetails.Any())
        {
            return Result<IEnumerable<string>>.Fail("Contact details is not provided.");
        }

        // begin transaction.
        var transaction = _unitOfWork.BeginTransaction();

        var user = new User
        {
            Email = request.Request.Email,
            UserName = request.Request.Email,
            FirstName = request.Request.CompanyName,
            LastName = request.Request.CompanyName,
            EmailConfirmed = false,
            PhoneNumber = "",
            PhoneNumberConfirmed = false,
            PlainPassword = request.Request.Password,
            UserTypeId = (byte)UserTypeEnum.Employer
        };

        var result = await _userManger.CreateAsync(user, request.Request.Password);

        if (result.Succeeded)
        {
            request.Request.UserId = new Guid(user.Id);
            var employeeResult = await AddEmployerAsync(request, cancellationToken);

            // commit the transaction.
            transaction.Commit();

            if (employeeResult.Succeeded)
            {
                // Add role to user
                await _userManger.AddToRoleAsync(user, nameof(RoleEnum.Employer));

                // send emails.
                _ = Task.Run(() =>
                {
                    _mailService.SendConfirmationEmailAsync(user.Email, Encryption.Crypt(user.Id.ToString()));


                });

                // send notification.
                var notification = new Notification()
                {
                    Date = DateTime.UtcNow,
                    Type = (byte)NotificationTypeEnum.Employer,
                    Content = $"{user.FirstName} {user.LastName} has registered on {DateTime.UtcNow.ToString("yyyy-MM-dd")}",
                    EmployerId = employeeResult.Data
                };
                await _unitOfWork.NotificationsRepository.Add(notification);
                await _unitOfWork.SaveChangesAsync();

                return Result<IEnumerable<string>>.Success(new List<string>(), "User created successfully!");
            }
        }
        return Result<IEnumerable<string>>.Fail("User did not create", result.Errors.Select(e => e.Description));
    }

    private async Task<Result<int>> AddEmployerAsync(EmployerRegisterCommand request, CancellationToken cancellationToken)
    {
        // map to employer.
        var employer = new Employer()
        {
            UserId = request.Request.UserId,
            Email = request.Request.Email,
            CompanyName = request.Request.CompanyName,
            Name = request.Request.CompanyName,
            JobTitle = request.Request.ContactDetails.FirstOrDefault().JobTitle,
            PhoneNumber = request.Request.ContactDetails.FirstOrDefault().PhoneNumber,
            CompanyNo = request.Request.CompanyNo,
            CompanyTypeId = request.Request.CompanyTypeId,
            AboutOrganization = request.Request.AboutOrganization,
            SiteName = request.Request.SiteName,
            PinCode = request.Request.PinCode,
            NearestLocation = request.Request.NearestLocation,
            Location = request.Request.Location,
            Address = request.Request.Address,
            Address2 = request.Request.Address2,
            IsHealthAndSafetyPolicy = request.Request.IsHealthAndSafetyPolicy
        };

        // save into database.
        await _unitOfWork.EmployerRepository.Add(employer);
        await _unitOfWork.SaveChangesAsync();

        // add contact details in db.
        await AddEmployerContactDetails(request.Request.ContactDetails, employer.Id);

        // add type of services.
        await AddEmployerTypeOfServices(request.Request.TypeOfServices, employer.Id);

        return Result<int>.Success(employer.Id);
    }

    private async Task AddEmployerTypeOfServices(List<byte> typeOfServices, int employerId)
    {
        List<TypeOfService> details = new List<TypeOfService>();
        foreach (var service in typeOfServices)
        {
            var detail = new TypeOfService()
            {
                TypeOfServiceId = service,
                EmployerId = employerId
            };
            details.Add(detail);
        }

        await _unitOfWork.TypeOfServiceRepository.AddRangeAsync(details);
        await _unitOfWork.SaveChangesAsync();
    }

    private async Task AddEmployerContactDetails(List<EmployerContactDetailRequest> contactDetails, int employerId)
    {
        List<EmployerContactDetail> details = new List<EmployerContactDetail>();
        foreach (var contact in contactDetails)
        {
            var detail = new EmployerContactDetail()
            {
                ContactName = contact.ContactName,
                Email = contact.Email,
                CountryCode = contact.CountryCode,
                PhoneNumber = contact.PhoneNumber,
                JobTitle = contact.JobTitle,
                EmployerId = employerId,
            };
            details.Add(detail);
        }

        await _unitOfWork.EmployerContactDetailRepository.AddRangeAsync(details);
        await _unitOfWork.SaveChangesAsync();
    }
}

public sealed class EmployerLoginCommandHandlers(UserManager<User> _userManger, IConfiguration _configuration, IUnitOfWork _unitOfWork)
    : IRequestHandler<EmployerLoginCommand, Result<EmployerLoginResponse>>
{
    public async Task<Result<EmployerLoginResponse>> Handle(EmployerLoginCommand request, CancellationToken cancellationToken)
    {
        // login with email.
        var user = await _userManger.FindByEmailAsync(request.Request.Email);
        if (user is null)
        {
            return Result<EmployerLoginResponse>.Fail("There is no user.");
        }

        // check is password correct.
        var result = await _userManger.CheckPasswordAsync(user, request.Request.Password);
        if (!result)
        {
            return Result<EmployerLoginResponse>.Fail("Invalid credentials.");
        }

        // get role.
        var role = _userManger.GetRolesAsync(user).Result.FirstOrDefault();
        if (role is null)
        {
            return Result<EmployerLoginResponse>.Fail("Role is not assigned.");
        }

        Claim[] claims;
        string organisationName = string.Empty;

        if (role == nameof(RoleEnum.SuperAdmin))
        {
            // add claims in jwt token.
            claims = new[] {
                new Claim("Email", request.Request.Email),
                new Claim("role", role),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };
        }
        else if (role == nameof(RoleEnum.Employer))
        {
            // is employer exist.
            var employer = await _unitOfWork
                .EmployerRepository
                .GetAllReadOnly()
                .Select(x => new { x.Id, x.UserId, x.CompanyName })
                .FirstOrDefaultAsync(x => x.UserId.Equals(new Guid(user.Id)), cancellationToken);
            if (employer is null)
            {
                return Result<EmployerLoginResponse>.Fail("There is no user.");
            }

            // add claims in jwt token.
            claims = new[] {
                new Claim("Email", request.Request.Email),
                new Claim("EmployerId", employer.Id.ToString()),
                new Claim("role", role),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };
            organisationName = employer.CompanyName;
        }
        else
        {
            return Result<EmployerLoginResponse>.Fail("Something went wrong.");
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AuthSettings:Key"]));
        var token = new JwtSecurityToken(
            issuer: _configuration["AuthSettings:Issuer"],
            audience: _configuration["AuthSettings:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddDays(1),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

        string tokenAsString = new JwtSecurityTokenHandler().WriteToken(token);

        EmployerLoginResponse response = new EmployerLoginResponse()
        {
            ExpireDate = token.ValidTo,
            Token = tokenAsString,
            Role = role,
            OrganisationName = organisationName
        };

        // return response.
        return Result<EmployerLoginResponse>.Success(response, "User logged in successfully!");
    }
}

public sealed class AddBookingHandler(IUnitOfWork _unitOfWork, IFileHelper _fileHelper, IMailService _mailService, IConfiguration _configuration) : IRequestHandler<AddBookingCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(AddBookingCommand request, CancellationToken cancellationToken)
    {
        // check is employee exists.
        var employer = await _unitOfWork.EmployerRepository.GetById(request.Request.EmployerId);
        if (employer is null)
        {
            return Result<bool>.Fail("Employer doesn't exist.");
        }

        // check user is activated.
        if (employer.AccountStatus != (byte)EmployerAccountStatusEnum.Active)
        {
            return Result<bool>.Fail("Employer status isn't active.");
        }

        string documentUrl = string.Empty;
        if (request.Request.File is not null)
        {
            documentUrl = await _fileHelper.UploadFile("BookingDocuments", request.Request.File);
        }

        Booking booking = new Booking()
        {
            EmployerId = Convert.ToInt32(request.Request.EmployerId),
            Details = request.Request.Details,
            Date = DateTime.UtcNow.Date,
            Status = (byte)BookingStatusEnum.New,
            DocumentUrl = documentUrl
        };

        await _unitOfWork.BookingRepository.Add(booking);
        await _unitOfWork.SaveChangesAsync();

        string subject = "Your booking has been placed :ESGO";
        string content = "Thank you for placing your booking with us.Your booking request is acknowledged and we will shortly update you on the booking status.";
        string username = employer.Name;

        string subject1 = $"{employer.CompanyName} booking details submitted";
        string content1 = $"OrganizationName has submitted new booking under Booking id BookingId";
        content1 = content1.Replace("OrganizationName", employer.CompanyName);
        content1 = content1.Replace("BookingId", booking.Id.ToString());
        string username1 = employer.Name;

        // send email to employer.
        _ = Task.Run(() =>
        {
            _mailService.Send(employer.Email, subject, content, username);
        });

        // send email to booking emails.
        if (_configuration["EmailSetting:FromEmail"] is not null)
        {
            _ = Task.Run(() =>
            {
                _mailService.Send(_configuration["EmailSetting:FromEmail"], subject1, content1, username1);
            });
        }

        return Result<bool>.Success(true, "Booking added successfully.");
    }
}

public sealed class AddOrganisationImageHandler(IUnitOfWork _unitOfWork, IFileHelper _fileHelper) : IRequestHandler<AddOrganisationImageCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(AddOrganisationImageCommand request, CancellationToken cancellationToken)
    {
        // check is employee exists.
        var employer = await _unitOfWork.EmployerRepository.GetById(request.Request.EmployerId);
        if (employer is null)
        {
            return Result<bool>.Fail("Employer doesn't exist.");
        }

        var url = await _fileHelper.UploadFile("Organisation Images", request.Request.File);

        employer.OrganizationImageUrl = url;
        _unitOfWork.EmployerRepository.Change(employer);
        await _unitOfWork.SaveChangesAsync();

        return Result<bool>.Success(true, "Image added successfully.");
    }
}

public sealed class AddOrganisationLogoHandler(IUnitOfWork _unitOfWork, IFileHelper _fileHelper) : IRequestHandler<AddOrganisationLogoCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(AddOrganisationLogoCommand request, CancellationToken cancellationToken)
    {
        // check is employee exists.
        var employer = await _unitOfWork.EmployerRepository.GetById(request.Request.EmployerId);
        if (employer is null)
        {
            return Result<bool>.Fail("Employer doesn't exist.");
        }

        var url = await _fileHelper.UploadFile("Organisation Logos", request.Request.File);

        employer.OrganizationLogoUrl = url;
        _unitOfWork.EmployerRepository.Change(employer);
        await _unitOfWork.SaveChangesAsync();

        return Result<bool>.Success(true, "Logo added successfully.");
    }
}

public sealed class GetBookingsHandler(IUnitOfWork _unitOfWork) : IRequestHandler<GetBookingsQuery, Result<PaginationModel<GetBookingsResponse>>>
{
    public async Task<Result<PaginationModel<GetBookingsResponse>>> Handle(GetBookingsQuery request, CancellationToken cancellationToken)
    {
        // check is employee exists.
        var employer = await _unitOfWork.EmployerRepository.GetById(request.EmployerId);
        if (employer is null)
        {
            return Result<PaginationModel<GetBookingsResponse>>.Fail("Employer doesn't exist.");
        }

        var bookings = _unitOfWork.BookingRepository.GetBookings(request.EmployerId);

        // Add pagination
        PaginationModel<GetBookingsResponse> model = new PaginationModel<GetBookingsResponse>();
        if (request.PageSize > 0)
        {
            var result = new PagedList<GetBookingsResponse>(bookings, request.PageIndex, request.PageSize, false);
            model.PagedContent = result;
            model.PagingFilteringContext.LoadPagedList(result);
        }

        return Result<PaginationModel<GetBookingsResponse>>.Success(model, "Employer's booking list.");
    }
}

public sealed class GetEmployerByIdHandler(IUnitOfWork _unitOfWork) : IRequestHandler<GetEmployerByIdQuery, Result<GetEmployerByIdResponse>>
{
    public async Task<Result<GetEmployerByIdResponse>> Handle(GetEmployerByIdQuery request, CancellationToken cancellationToken)
    {
        // check is employee exists.
        var employer = await _unitOfWork.EmployerRepository.GetEmployerById(request.EmployerId).FirstOrDefaultAsync(cancellationToken);
        if (employer is null)
        {
            return Result<GetEmployerByIdResponse>.Fail("Employer doesn't exist.");
        }

        // get contack details.
        employer.ContactDetails = await _unitOfWork
            .EmployerContactDetailRepository
            .GetAllReadOnly()
            .Where(contact => contact.EmployerId == request.EmployerId)
            .Select(contact => new GetEmployerByIdContactDetailDto()
            {
                Id = contact.Id,
                ContactName = contact.ContactName,
                Email = contact.Email,
                CountryCode = contact.CountryCode,
                PhoneNumber = contact.PhoneNumber,
                JobTitle = contact.JobTitle
            }).ToListAsync(cancellationToken);

        //// get services details.
        //var x = await _unitOfWork
        //     .TypeOfServiceRepository
        //     .GetAllReadOnly()
        //     .Where(type => type.EmployerId == request.EmployerId)
        //     .Select(type => type.TypeOfServiceId)
        //     .ToArrayAsync(cancellationToken);
        //employer.Services = x.Select(value => Enum.GetName(typeof(TypeOfServiceEnum), value)).ToArray();
        employer.Services = await _unitOfWork
            .TypeOfServiceRepository
            .GetAllReadOnly()
            .Where(type => type.EmployerId == request.EmployerId)
            .Select(type => type.TypeOfServiceId).ToListAsync(cancellationToken);

        return Result<GetEmployerByIdResponse>.Success(employer, "Employer collected successfully.");
    }
}

public sealed class UpdateBookingHandler(IUnitOfWork _unitOfWork, IFileHelper _fileHelper) : IRequestHandler<UpdateBookingCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(UpdateBookingCommand request, CancellationToken cancellationToken)
    {
        // check is employee exists.
        var employer = await _unitOfWork.EmployerRepository.GetById(request.Request.EmployerId);
        if (employer is null)
        {
            return Result<bool>.Fail("Employer doesn't exist.");
        }

        // check user is activated.
        if (employer.AccountStatus != (byte)EmployerAccountStatusEnum.Active)
        {
            return Result<bool>.Fail("Employer status isn't active.");
        }

        // check booking exist.
        var booking = await _unitOfWork.BookingRepository.GetById(request.Request.Id);
        if (booking is null)
        {
            return Result<bool>.Fail($"Booking isn't exist with Id {request.Request.Id}.");
        }

        string documentUrl = "";
        if (request.Request.File is not null)
        {
            documentUrl = await _fileHelper.UploadFile("BookingDocuments", request.Request.File);
        }

        booking.Details = request.Request.Details;
        booking.DocumentUrl = string.IsNullOrEmpty(documentUrl) ? booking.DocumentUrl : documentUrl;

        _unitOfWork.BookingRepository.Change(booking);
        await _unitOfWork.IsCompleted();

        return Result<bool>.Success(true, "Booking updated successfully.");
    }
}

public sealed class AddShiftHandler(IUnitOfWork _unitOfWork) : IRequestHandler<AddShiftCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(AddShiftCommand request, CancellationToken cancellationToken)
    {
        // check is employee exists.
        var employer = await _unitOfWork.EmployerRepository.GetById(request.Request.EmployerId);
        if (employer is null)
        {
            return Result<bool>.Fail("Employer doesn't exist.");
        }

        // convert string to timeonly.
        TimeOnly.TryParse(request.Request.StartTime, out TimeOnly startTime);
        TimeOnly.TryParse(request.Request.EndTime, out TimeOnly endTime);

        // check starttime is in correct format.
        if (startTime.Ticks is 0)
        {
            return Result<bool>.Fail("Start time isn't in correct format.");
        }

        // check endtime is in correct format.
        if (endTime.Ticks is 0)
        {
            return Result<bool>.Fail("End time isn't in correct format.");
        }

        // add shift
        Shift shift = new Shift()
        {
            EmployerId = Convert.ToInt32(request.Request.EmployerId),
            Name = request.Request.Name,
            StartTime = startTime,
            EndTime = endTime
        };

        await _unitOfWork.ShiftRepository.Add(shift);
        await _unitOfWork.SaveChangesAsync();

        return Result<bool>.Success(true, "Shift added successfully.");
    }
}

public sealed class GetShiftsHandler(IUnitOfWork _unitOfWork) : IRequestHandler<GetShiftsQuery, Result<PaginationModel<GetShiftsResponse>>>
{
    public async Task<Result<PaginationModel<GetShiftsResponse>>> Handle(GetShiftsQuery request, CancellationToken cancellationToken)
    {
        // check is employee exists.
        var employer = await _unitOfWork.EmployerRepository.GetById(request.EmployerId);
        if (employer is null)
        {
            return Result<PaginationModel<GetShiftsResponse>>.Fail("Employer doesn't exist.");
        }

        // get shifts by employer id.
        var shifts = _unitOfWork.ShiftRepository.GetShiftsByEmployerId(request.EmployerId);

        // Add pagination
        PaginationModel<GetShiftsResponse> model = new PaginationModel<GetShiftsResponse>();
        if (request.PageSize > 0)
        {
            var result = new PagedList<GetShiftsResponse>(shifts, request.PageIndex, request.PageSize, false);
            model.PagedContent = result;
            model.PagingFilteringContext.LoadPagedList(result);
        }

        return Result<PaginationModel<GetShiftsResponse>>.Success(model, "shift list.");
    }
}

public sealed class GetTimesheetsByStatusForEmployerHandler(IUnitOfWork _unitOfWork) : IRequestHandler<GetTimesheetsByStatusForEmployerQuery, Result<PaginationModel<GetTimesheetsByStatusForEmployerResponse>>>
{
    public async Task<Result<PaginationModel<GetTimesheetsByStatusForEmployerResponse>>> Handle(GetTimesheetsByStatusForEmployerQuery request, CancellationToken cancellationToken)
    {
        // check nurse type exists.
        var isExist = Enum.IsDefined(typeof(TimeSheetStatusEnum), request.Status);
        if (!isExist)
            return Result<PaginationModel<GetTimesheetsByStatusForEmployerResponse>>.Fail("Invalid nurse type.");

        // check is employee exists.
        var employer = await _unitOfWork.EmployerRepository.GetById(request.EmployerId);
        if (employer is null)
        {
            return Result<PaginationModel<GetTimesheetsByStatusForEmployerResponse>>.Fail("Employee doesn't exist.");
        }

        // get timesheets by status
        var timesheets = _unitOfWork.TimesheetRepository.GetTimesheetsByEmployer(request.EmployerId);
        var query = from timesheet in timesheets
                    join shift in _unitOfWork.ShiftRepository.GetAllReadOnly()
                    on timesheet.ShiftId equals shift.Id into Details
                    from m in Details.DefaultIfEmpty()
                    select new GetTimesheetsByStatusForEmployerResponse
                    {
                        TimesheetId = timesheet.TimesheetId,
                        JobId = timesheet.JobId,
                        EmployeeId = timesheet.EmployeeId,
                        EmployeeType = timesheet.EmployeeType,
                        TimesheetDate = timesheet.TimesheetDate,
                        StartTime = timesheet.StartTime,
                        EndTime = timesheet.EndTime,
                        EmployeeName = timesheet.EmployeeName,
                        BillableHours = timesheet.BillableHours,
                        BreakTime = timesheet.BreakTime,
                        TotalHours = timesheet.TotalHours,
                        HourlyRate = timesheet.HourlyRate,
                        Notes = timesheet.Notes,
                        Status = timesheet.Status,
                        OrganisationName = timesheet.OrganisationName,
                        JobDate = timesheet.JobDate,
                        JobCreatedDate = timesheet.JobCreatedDate,
                        ShiftId = timesheet.ShiftId,
                        ReviewedBy = timesheet.ReviewedBy,
                        Rating = timesheet.Rating,
                        ShiftName = timesheet.ShiftId == 0 ? "Custom" : m.Name
                    };
        switch (request.Status)
        {
            case (byte)TimeSheetStatusEnum.Pending: query = query.Where(x => x.Status == (byte)TimeSheetStatusEnum.Pending); break;
            case (byte)TimeSheetStatusEnum.Approved: query = query.Where(x => x.Status == (byte)TimeSheetStatusEnum.Approved); break;
            case (byte)TimeSheetStatusEnum.Rejected: query = query.Where(x => x.Status == (byte)TimeSheetStatusEnum.Rejected); break;
        }

        // Add pagination
        PaginationModel<GetTimesheetsByStatusForEmployerResponse> model = new PaginationModel<GetTimesheetsByStatusForEmployerResponse>();
        if (request.PageSize > 0)
        {
            var result = new PagedList<GetTimesheetsByStatusForEmployerResponse>(query, request.PageIndex, request.PageSize, false);
            model.PagedContent = result;
            model.PagingFilteringContext.LoadPagedList(result);
        }

        return Result<PaginationModel<GetTimesheetsByStatusForEmployerResponse>>.Success(model, "Timesheet list.");
    }
}

public sealed class GetShiftByIdHandler(IUnitOfWork _unitOfWork) : IRequestHandler<GetShiftByIdQuery, Result<GetShiftByIdResponse>>
{
    public async Task<Result<GetShiftByIdResponse>> Handle(GetShiftByIdQuery request, CancellationToken cancellationToken)
    {
        // check is employee exists.
        var employer = await _unitOfWork.EmployerRepository.GetById(request.EmployerId);
        if (employer is null)
        {
            return Result<GetShiftByIdResponse>.Fail("Employer doesn't exist.");
        }

        // get shift by id for employer.
        var shift = await _unitOfWork
            .ShiftRepository
            .GetShiftForEmployerById(request.ShiftId, request.EmployerId)
            .FirstOrDefaultAsync(cancellationToken);

        if (shift is null)
        {
            return Result<GetShiftByIdResponse>.Fail("Shift doesn't exist.");
        }

        return Result<GetShiftByIdResponse>.Success(shift, "shift collected.");
    }
}

public sealed class UpdateShiftHandler(IUnitOfWork _unitOfWork) : IRequestHandler<UpdateShiftCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(UpdateShiftCommand request, CancellationToken cancellationToken)
    {
        // convert string to timeonly.
        TimeOnly.TryParse(request.Request.StartTime, out TimeOnly startTime);
        TimeOnly.TryParse(request.Request.EndTime, out TimeOnly endTime);

        // check starttime is in correct format.
        if (startTime.Ticks is 0)
        {
            return Result<bool>.Fail("Start time isn't in correct format.");
        }

        // check endtime is in correct format.
        if (endTime.Ticks is 0)
        {
            return Result<bool>.Fail("End time isn't in correct format.");
        }

        // check shift is exist or not
        var shift = await _unitOfWork
            .ShiftRepository
            .GetAll()
            .FirstOrDefaultAsync(x => x.Id == request.Request.Id && x.EmployerId == request.Request.EmployerId, cancellationToken);
        if (shift is null)
        {
            return Result<bool>.Fail("Shift doesn't exist.");
        }

        // add shift
        shift.Name = request.Request.Name;
        shift.StartTime = startTime;
        shift.EndTime = endTime;

        await _unitOfWork.SaveChangesAsync();

        return Result<bool>.Success(true, "Shift updated successfully.");
    }
}

public sealed class DeleteShiftHandler(IUnitOfWork _unitOfWork) : IRequestHandler<DeleteShiftCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(DeleteShiftCommand request, CancellationToken cancellationToken)
    {
        // check shift is exist or not
        var shift = await _unitOfWork
            .ShiftRepository
            .GetAll()
            .FirstOrDefaultAsync(x => x.Id == request.Request.ShiftId && x.EmployerId == request.Request.EmployerId, cancellationToken);
        if (shift is null)
        {
            return Result<bool>.Fail("Shift doesn't exist.");
        }

        _unitOfWork.ShiftRepository.DeleteByEntity(shift);
        await _unitOfWork.SaveChangesAsync();

        return Result<bool>.Success(true, "Shift deleted successfully.");
    }
}

public sealed class DeleteBookingHandler(IUnitOfWork _unitOfWork) : IRequestHandler<DeleteBookingCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(DeleteBookingCommand request, CancellationToken cancellationToken)
    {
        // check Booking is exist or not
        var booking = await _unitOfWork
            .BookingRepository
            .GetAll()
            .FirstOrDefaultAsync(x => x.Id == request.Request.BookingId && x.EmployerId == request.Request.EmployerId, cancellationToken);
        if (booking is null)
        {
            return Result<bool>.Fail("Booking doesn't exist.");
        }

        _unitOfWork.BookingRepository.DeleteByEntity(booking);
        await _unitOfWork.SaveChangesAsync();

        return Result<bool>.Success(true, "Booking deleted successfully.");
    }
}

public sealed class ChangeTimesheetStatusHandler(IUnitOfWork _unitOfWork, IMailService _mailService) : IRequestHandler<ChangeTimesheetStatusCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(ChangeTimesheetStatusCommand request, CancellationToken cancellationToken)
    {
        // check is rating valid.
        if (request.Request.Rating is not 0)
        {
            if (request.Request.Rating < 1 || request.Request.Rating > 5)
            {
                return Result<bool>.Fail("Invalid rating.");
            }
        }

        // get timesheet by id.
        var timesheet = await _unitOfWork
            .TimesheetRepository
            .GetAll()
            .Include(x => x.AssignedJob)
            .Include(x => x.Job)
            .Include(x => x.Employer)
            .Include(x => x.Employee)
            .Where(x => x.Id == request.Request.TimesheetId
                    && x.EmployerId == request.Request.EmployerId
                    && x.Status == (byte)TimeSheetStatusEnum.Pending)
            .FirstOrDefaultAsync(cancellationToken);

        if (timesheet is null)
        {
            return Result<bool>.Fail("timesheet doesn't exist.");
        }

        string subject = string.Empty;
        string content = string.Empty;
        string timesheetStatus = string.Empty;

        if (request.Request.Status == (byte)TimeSheetStatusEnum.Approved)
        {
            timesheet.Status = (byte)TimeSheetStatusEnum.Approved;
            timesheet.ApprovalDate = DateTime.UtcNow;
            subject = $"{timesheet.Employer.CompanyName} Timesheet approved:ESGO";
            content = "The time sheet for the completed shift on JobDate from StartTime to EndTime is approved.";
            timesheetStatus = "approved";
        } 
        else if (request.Request.Status == (byte)TimeSheetStatusEnum.Rejected)
        {
            timesheet.Status = (byte)TimeSheetStatusEnum.Rejected;
            timesheet.RejectionDate = DateTime.UtcNow;
            timesheet.Reason = request.Request.Reason;
            subject = $"{timesheet.Employer.CompanyName} Timesheet rejected:ESGO";
            content = "UserName your time sheet for the completed shift on JobDate from StartTime to EndTime is rejected. Please login in to your ESGO account and resubmit.";
            timesheetStatus = "rejected";
        }
        else
        {
            return Result<bool>.Fail("Invalid status.");
        }

        // add employerfeedback.
        //var feedback = new TimesheetFeedback()
        //{
        //    EmployeeId = timesheet.EmployeeId,
        //    EmployerId = timesheet.EmployerId,
        //    Rating = request.Request.Rating,
        //    Comment = request.Request.Comment
        //};
        //await _unitOfWork.TimesheetFeedbackRepository.Add(feedback);
        timesheet.Rating = request.Request.Rating;
        timesheet.ReviewedBy = request.Request.ReviewedBy;

        _unitOfWork.TimesheetRepository.Change(timesheet);
        await _unitOfWork.SaveChangesAsync();

        // send email
        content = content.Replace("UserName", $"{timesheet.Employee.FirstName} {timesheet.Employee.LastName}");
        content = content.Replace("OrganizationName", timesheet.Employer.CompanyName);
        content = content.Replace("JobDate", timesheet.Job.Date.ToString("dd/MM/yyyy"));
        content = content.Replace("StartTime", timesheet.StartTime.ToString("hh:mm tt"));
        content = content.Replace("EndTime", timesheet.EndTime.ToString("hh:mm tt"));
        content = content.Replace("Status", timesheetStatus);

        if (!string.IsNullOrEmpty(subject))
        {
            // send email to employer.
            _ = Task.Run(() =>
            {
                _mailService.Send(timesheet.Employer.Email, subject, content, "");
            });
        }

        return Result<bool>.Success(true, "Status changed successfully.");
    }
}

public sealed class GetNextBookingIdHandler(IUnitOfWork _unitOfWork) : IRequestHandler<GetNextBookingIdQuery, Result<GetNextBookingIdResponse>>
{
    public async Task<Result<GetNextBookingIdResponse>> Handle(GetNextBookingIdQuery request, CancellationToken cancellationToken)
    {
        // get next booking id.
        var isexist = await _unitOfWork
            .BookingRepository
            .GetAllReadOnly().AnyAsync();
        int bookingId = 1;
        if (isexist)
        {
            bookingId = await _unitOfWork
            .BookingRepository
            .GetAllReadOnly().MaxAsync(booking => booking.Id) + 1;
        }

        var result = new GetNextBookingIdResponse()
        {
            Id = bookingId,
            Date = DateTime.UtcNow.Date
        };

        return Result<GetNextBookingIdResponse>.Success(result, "Next booking id collected successfully.");
    }
}

public sealed class UpdateTimesheatHandler(IUnitOfWork _unitOfWork) : IRequestHandler<UpdateTimesheatCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(UpdateTimesheatCommand request, CancellationToken cancellationToken)
    {
        // check is employee exists.
        var employer = await _unitOfWork.EmployerRepository.GetById(request.EmployerId);
        if (employer is null)
        {
            return Result<bool>.Fail("Employer doesn't exist.");
        }

        // check user is activated.
        if (employer.AccountStatus != (byte)EmployerAccountStatusEnum.Active)
        {
            return Result<bool>.Fail("Employer status isn't active.");
        }

        // check job exist.
        var job = await _unitOfWork.JobRepository.GetById(request.Request.JobId);
        if (job is null)
        {
            return Result<bool>.Fail($"Job isn't exist with Id {request.Request.JobId}.");
        }

        // check timesheet exist or not.
        var responceTimesheet = await _unitOfWork.TimesheetRepository.GetById(request.Request.Id);
        if (responceTimesheet is null)
        {
            return Result<bool>.Fail("TImesheet isn't exist.");
        }

        // update the timesheet by entity.



        _unitOfWork.TimesheetRepository.Change(responceTimesheet);
        await _unitOfWork.SaveChangesAsync();

        return Result<bool>.Success(true, "Timesheat updated successfully.");
    }
}

public sealed class UpdateEmployerHandler(IUnitOfWork _unitOfWork) : IRequestHandler<UpdateEmployerCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(UpdateEmployerCommand request, CancellationToken cancellationToken)
    {
        // check is contact details provide or not.
        if (request.Request.ContactDetails.Count is 0)
        {
            return Result<bool>.Fail("Contact detail is not provided.");
        }

        // check company type exist.
        var isExist = Enum.IsDefined(typeof(CompanyTypeEnum), request.Request.CompanyTypeId);
        if (!isExist)
            return Result<bool>.Fail("Invalid company type.");

        // check is email already used.
        var isEmailUsed = await _unitOfWork
            .EmployerRepository
            .GetAllReadOnly()
            .AnyAsync(employer => employer.Id != request.Request.EmployerId && employer.Email.ToLower() == request.Request.Email.ToLower(), cancellationToken);
        if (isEmailUsed)
        {
            return Result<bool>.Fail("This email is already exist.");
        }

        // check is employee exists.
        var employer = await _unitOfWork.EmployerRepository.GetById(request.Request.EmployerId);
        if (employer is null)
        {
            return Result<bool>.Fail("Employer doesn't exist.");
        }

        // update employer.
        employer.Email = request.Request.Email;
        employer.CompanyName = request.Request.CompanyName;
        employer.PhoneNumber = request.Request.PhoneNumber;
        employer.CompanyTypeId = request.Request.CompanyTypeId;
        employer.PinCode = request.Request.PinCode;
        employer.SiteName = request.Request.SiteName;
        employer.CompanyNo = request.Request.CompanyNo;
        employer.JobTitle = request.Request.JobTitle;
        employer.Location = request.Request.Location;
        employer.Address = request.Request.Address;
        employer.Address2 = request.Request.Address2;
        employer.AboutOrganization = request.Request.AboutOrganization;
        _unitOfWork.EmployerRepository.Change(employer);

        // add contact details.
        var existingDetails = await _unitOfWork
                    .EmployerContactDetailRepository
                    .GetAllReadOnly()
                    .Where(detail => detail.EmployerId == request.Request.EmployerId)
                    .ToListAsync(cancellationToken);
        _unitOfWork.EmployerContactDetailRepository.RemoveRange(existingDetails);

        List<EmployerContactDetail> details = new List<EmployerContactDetail>();
        foreach (var contact in request.Request.ContactDetails)
        {
            var detail = new EmployerContactDetail()
            {
                ContactName = contact.ContactName,
                Email = contact.Email,
                CountryCode = contact.CountryCode,
                PhoneNumber = contact.PhoneNumber,
                JobTitle = contact.JobTitle,
                EmployerId = request.Request.EmployerId,
            };
            details.Add(detail);
        }

        await _unitOfWork.EmployerContactDetailRepository.AddRangeAsync(details);

        // add type of services.
        var existingServices = await _unitOfWork
                .TypeOfServiceRepository
                .GetAllReadOnly()
                .Where(detail => detail.EmployerId == request.Request.EmployerId)
                .ToListAsync(cancellationToken);
        _unitOfWork.TypeOfServiceRepository.RemoveRange(existingServices);

        List<TypeOfService> services = new List<TypeOfService>();
        foreach (var service in request.Request.TypeOfServices)
        {
            var detail = new TypeOfService()
            {
                TypeOfServiceId = service,
                EmployerId = request.Request.EmployerId
            };
            services.Add(detail);
        }
        await _unitOfWork.TypeOfServiceRepository.AddRangeAsync(services);

        await _unitOfWork.SaveChangesAsync();
        return Result<bool>.Success(true, "Employer updated successfully.");
    }
}

public sealed class GetTimesheetByIdForEmployerHandler(IUnitOfWork _unitOfWork) : IRequestHandler<GetTimesheetByIdForEmployerQuery, Result<GetTimesheetByIdResponce>>
{
    public async Task<Result<GetTimesheetByIdResponce>> Handle(GetTimesheetByIdForEmployerQuery request, CancellationToken cancellationToken)
    {
        // get timesheet by id.
        var timesheet = await _unitOfWork.TimesheetRepository.GetAllReadOnly()
            .Include(timesheet => timesheet.Employee)
            .Include(timesheet => timesheet.Employer)
            .Include(timesheet => timesheet.AssignedJob)
            .Where(timesheet => timesheet.Id == request.TimesheetId && timesheet.EmployerId == request.EmployerId)
            .Select(timesheet => new GetTimesheetByIdResponce()
            {
                Id = timesheet.Id,
                OrganisationName = timesheet.Employer.CompanyName,
                BreakTime = timesheet.BreakTime,
                Date = timesheet.Date,
                StartTime = timesheet.Date.Date.Add(timesheet.StartTime.ToTimeSpan()),
                EndTime = timesheet.Date.Date.Add(timesheet.EndTime.ToTimeSpan()),
                HourlyRate = timesheet.HourlyRate,
                BillableHours = timesheet.BillableHours,
                Status = timesheet.Status,
                Notes = timesheet.Notes,
                Reason = timesheet.Reason
            })
            .FirstOrDefaultAsync(cancellationToken);
        if (timesheet is null)
        {
            return Result<GetTimesheetByIdResponce>.Fail("Timesheet is not exist.");
        }

        return Result<GetTimesheetByIdResponce>.Success(timesheet, "Timesheet collected.");
    }
}

public sealed class GetJobsByStatusForEmployerHandler(IUnitOfWork _unitOfWork) : IRequestHandler<GetJobsByStatusForEmployerQuery, Result<PaginationModel<GetJobsByStatusResponse>>>
{
    public async Task<Result<PaginationModel<GetJobsByStatusResponse>>> Handle(GetJobsByStatusForEmployerQuery request, CancellationToken cancellationToken)
    {
        // check is status valid.
        var isJobStatusValid = Enum.IsDefined(typeof(JobStatusEnum), request.Status);
        if (!isJobStatusValid)
        {
            return Result<PaginationModel<GetJobsByStatusResponse>>.Fail("Invalid status.");
        }

        // this is use to just intialize.
        IQueryable<GetJobsByStatusResponse> query = _unitOfWork
            .JobRepository
            .GetAllReadOnly()
            .Select(x => new GetJobsByStatusResponse());

        if (request.Status == (byte)JobStatusEnum.Open)
        {
            // get confirmed jobsids
            var jobIds = await _unitOfWork.AssignedJobRepository.GetAllReadOnly()
                .Where(assignedJob => assignedJob.EmployerId == request.EmployerId
                                        && (assignedJob.JobStatus == (byte)JobStatusEnum.Confirmed
                                        || assignedJob.JobStatus == (byte)JobStatusEnum.Completed
                                        || assignedJob.JobStatus == (byte)JobStatusEnum.UnSuccessful))
                .Select(assignedJob => assignedJob.JobId)
                .ToArrayAsync(cancellationToken);

            var jobs = _unitOfWork
                .JobRepository
                .GetAllReadOnly()
                    .Include(job => job.Employer)
                    .Include(job => job.EmployeeType)
                .Where(job => !jobIds.Contains(job.Id) && job.EmployerId == request.EmployerId && job.Date.Date >= DateTime.UtcNow.Date)
                // also check job should exclude with cancel status.
                .Where(x => x.Status != (byte)JobStatusEnum.Cancelled)
                .Select(job => new GetJobsByStatusResponse()
                {
                    JobId = job.Id,
                    OrganisationName = job.Employer.CompanyName,
                    ShiftStartTime = job.Date.Date.Add(job.ShiftStartTime.ToTimeSpan()),
                    ShiftEndTime = job.Date.Date.Add(job.ShiftEndTime.ToTimeSpan()),
                    StartDate = job.Date.Date,
                    JobDate = job.Date.Date,
                    Location = job.Employer.Location,
                    StartTime = job.Date.Date.Add(job.ShiftStartTime.ToTimeSpan()),
                    EndTime = job.Date.Date.Add(job.ShiftEndTime.ToTimeSpan()),
                    EmployeeTypeId = job.EmployeeTypeId,
                    EmployeeType = job.EmployeeType.Name,
                    ShiftId = job.ShiftId,
                    EmployeeName = string.Empty,
                    BreakTime = job.BreakTime,
                    JobStatus = (byte)JobStatusEnum.Open,
                    JobTypeId = job.JobTypeId,
                    HourlyRate = job.HourlyRate,
                    JobCreatedDate = job.CreatedDate.Date,
                    IsDummy = job.IsDummy,
                    IsFixedRate = job.IsFixedRate,
                    FixedRate = job.FixedRate,
                    FixedRateAfterCommission = job.FixedRateAfterCommission,
                });

            query = from job in jobs
                    join shift in _unitOfWork.ShiftRepository.GetAllReadOnly()
                    on job.ShiftId equals shift.Id into Details
                    from m in Details.DefaultIfEmpty()

                    let appliedEmployees =
                        (
                          from c in _unitOfWork.AssignedJobRepository.GetAllReadOnly().Include(assignedjob => assignedjob.Employee)
                          where job.JobId == c.JobId && c.JobStatus == (byte)JobStatusEnum.Applied
                          select $"{c.Employee.FirstName} {c.Employee.LastName}"
                        ).ToList()


                    let waitingEmployees =
                        (
                          from c in _unitOfWork.AssignedJobRepository.GetAllReadOnly().Include(assignedjob => assignedjob.Employee)
                          where job.JobId == c.JobId && c.JobStatus == (byte)JobStatusEnum.Open
                          select $"{c.Employee.FirstName} {c.Employee.LastName}"
                        ).ToList()

                    let cancelledEmployees =
                        (
                          from c in _unitOfWork.AssignedJobRepository.GetAllReadOnly().Include(assignedjob => assignedjob.Employee)
                          where job.JobId == c.JobId && c.JobStatus == (byte)JobStatusEnum.Cancelled
                          select $"{c.Employee.FirstName} {c.Employee.LastName}"
                        ).ToList()


                    let applicantCount =
                        (
                          from c in _unitOfWork.AssignedJobRepository.GetAllReadOnly()
                          where job.JobId == c.JobId
                          select c
                        ).Count()

                    select new GetJobsByStatusResponse
                    {
                        JobId = job.JobId,
                        OrganisationName = job.OrganisationName,
                        ShiftStartTime = job.ShiftStartTime,
                        JobDate = job.JobDate,
                        ShiftEndTime = job.ShiftEndTime,
                        StartDate = job.StartDate,
                        Location = job.Location,
                        StartTime = job.ShiftStartTime,
                        EndTime = job.ShiftEndTime,
                        EmployeeTypeId = job.EmployeeTypeId,
                        EmployeeType = job.EmployeeType,
                        EmployeeName = job.EmployeeName,
                        ShiftId = job.ShiftId,
                        BreakTime = job.BreakTime,
                        JobStatus = job.JobStatus,
                        JobTypeId = job.JobTypeId,
                        HourlyRate = job.HourlyRate,
                        JobCreatedDate = job.JobCreatedDate,
                        AssignedJobId = job.AssignedJobId,
                        ShiftDescription = job.ShiftId == 0 ? "Custom" : m.Name,
                        Applied = appliedEmployees.Count,
                        AppliedEmployees = appliedEmployees,
                        Waiting = waitingEmployees.Count,
                        WaitingEmployees = waitingEmployees,
                        Canceled = cancelledEmployees.Count,
                        CancelledEmployees = cancelledEmployees,
                        Applicant = applicantCount,
                        IsDummy = job.IsDummy,
                        IsFixedRate = job.IsFixedRate,
                        FixedRate = job.FixedRate,
                        FixedRateAfterCommission = job.FixedRateAfterCommission,
                    };
        }
        else if (request.Status == (byte)JobStatusEnum.Cancelled)
        {
            var jobs = _unitOfWork
                .JobRepository
                .GetAllReadOnly()
                    .Include(job => job.Employer)
                    .Include(job => job.EmployeeType)
                .Where(job => job.EmployerId == request.EmployerId && job.Status == (byte)JobStatusEnum.Cancelled)
                .Select(job => new GetJobsByStatusResponse()
                {
                    JobId = job.Id,
                    OrganisationName = job.Employer.CompanyName,
                    ShiftStartTime = job.Date.Date.Add(job.ShiftStartTime.ToTimeSpan()),
                    ShiftEndTime = job.Date.Date.Add(job.ShiftEndTime.ToTimeSpan()),
                    StartDate = job.Date.Date,
                    JobDate = job.Date.Date,
                    Location = job.Employer.Location,
                    StartTime = job.Date.Date.Add(job.ShiftStartTime.ToTimeSpan()),
                    EndTime = job.Date.Date.Add(job.ShiftEndTime.ToTimeSpan()),
                    EmployeeTypeId = job.EmployeeTypeId,
                    EmployeeType = job.EmployeeType.Name,
                    ShiftId = job.ShiftId,
                    EmployeeName = string.Empty,
                    BreakTime = job.BreakTime,
                    JobStatus = (byte)JobStatusEnum.Open,
                    JobTypeId = job.JobTypeId,
                    HourlyRate = job.HourlyRate,
                    JobCreatedDate = job.CreatedDate.Date,
                    CancellationReason = job.CancellationReason,
                    IsDummy = job.IsDummy,
                    IsFixedRate = job.IsFixedRate,
                    FixedRate = job.FixedRate,
                    FixedRateAfterCommission = job.FixedRateAfterCommission,
                });

            query = from job in jobs
                    join shift in _unitOfWork.ShiftRepository.GetAllReadOnly()
                    on job.ShiftId equals shift.Id into Details
                    from m in Details.DefaultIfEmpty()

                    select new GetJobsByStatusResponse
                    {
                        JobId = job.JobId,
                        OrganisationName = job.OrganisationName,
                        ShiftStartTime = job.ShiftStartTime,
                        JobDate = job.JobDate,
                        ShiftEndTime = job.ShiftEndTime,
                        StartDate = job.StartDate,
                        Location = job.Location,
                        StartTime = job.ShiftStartTime,
                        EndTime = job.ShiftEndTime,
                        EmployeeTypeId = job.EmployeeTypeId,
                        EmployeeType = job.EmployeeType,
                        EmployeeName = job.EmployeeName,
                        ShiftId = job.ShiftId,
                        BreakTime = job.BreakTime,
                        JobStatus = job.JobStatus,
                        JobTypeId = job.JobTypeId,
                        HourlyRate = job.HourlyRate,
                        JobCreatedDate = job.JobCreatedDate,
                        AssignedJobId = job.AssignedJobId,
                        ShiftDescription = job.ShiftId == 0 ? "Custom" : m.Name,
                        CancellationReason = job.CancellationReason,
                        IsDummy = job.IsDummy,
                        IsFixedRate = job.IsFixedRate,
                        FixedRate = job.FixedRate,
                        FixedRateAfterCommission = job.FixedRateAfterCommission,
                    };
        }
        else
        {
            // get jobs by status.
            var jobs = _unitOfWork.AssignedJobRepository.GetJobsByStatus(request.Status).Where(job => job.EmployerId == request.EmployerId);


            // get timesheets by status
            query = from job in jobs
                    join shift in _unitOfWork.ShiftRepository.GetAllReadOnly()
                    on job.ShiftId equals shift.Id into Details
                    from m in Details.DefaultIfEmpty()

                    join timesheet in _unitOfWork.TimesheetRepository.GetAllReadOnly() on job.AssignedJobId equals timesheet.AssignedJobId into tempJoin2
                    from tempItem2 in tempJoin2.DefaultIfEmpty()

                    select new GetJobsByStatusResponse
                    {
                        OrganisationName = job.OrganisationName,
                        AssignedJobId = job.AssignedJobId,
                        JobId = job.JobId,
                        StartDate = job.StartDate,
                        ShiftStartTime = job.ShiftStartTime,
                        ShiftEndTime = job.ShiftEndTime,
                        JobDate = job.JobDate,
                        HourlyRate = job.HourlyRate,
                        JobCreatedDate = job.JobCreatedDate,
                        Location = job.Location,
                        StartTime = job.ShiftStartTime,
                        EndTime = job.ShiftEndTime,
                        EmployeeTypeId = job.EmployeeTypeId,
                        EmployeeType = job.EmployeeType,
                        ShiftId = job.ShiftId,
                        EmployeeName = job.EmployeeName,
                        EmployeeId = job.EmployeeId,
                        JobStatus = job.JobStatus,
                        BreakTime = job.BreakTime,
                        ShiftDescription = job.ShiftId == 0 ? "Custom" : m.Name,
                        Status = tempItem2 != null ? tempItem2.Status : null,
                        BillableHours = tempItem2 != null ? tempItem2.BillableHours : null,
                        Notes = tempItem2 != null ? tempItem2.Notes : null,
                        Rating = tempItem2 != null ? tempItem2.Rating : null,
                        ReviewedBy = tempItem2 != null ? tempItem2.ReviewedBy : "",
                        TimesheetId = tempItem2 != null ? tempItem2.Id : 0,
                        IsDummy = job.IsDummy,
                        IsFixedRate = job.IsFixedRate,
                        FixedRate = job.FixedRate,
                        FixedRateAfterCommission = job.FixedRateAfterCommission,
                    };
        }

        query = query.OrderBy(job => job.JobDate.Date);
        // Add pagination
        PaginationModel<GetJobsByStatusResponse> model = new PaginationModel<GetJobsByStatusResponse>();
        if (request.PageSize > 0)
        {
            var result = new PagedList<GetJobsByStatusResponse>(query, request.PageIndex, request.PageSize, false);
            model.PagedContent = result;
            model.PagingFilteringContext.LoadPagedList(result);
        }

        return Result<PaginationModel<GetJobsByStatusResponse>>.Success(model, "Jobs list collected.");
    }
}


public sealed class GetAPIsForJobForEmployerHandler(IUnitOfWork _unitOfWork) : IRequestHandler<GetAPIsForJobForEmployerQuery, Result<GetAPIsForJobForEmployerResponce>>
{
    public async Task<Result<GetAPIsForJobForEmployerResponce>> Handle(GetAPIsForJobForEmployerQuery request, CancellationToken cancellationToken)
    {
        var result = new GetAPIsForJobForEmployerResponce();

        // get employee types.
        result.EmployeeTypes = await _unitOfWork
            .EmployeeTypeRepository
            .GetAllReadOnly()
            .Select(type => new EmployeeTypeDto()
            {
                Id = type.Id,
                Name = type.Name,
                Description = type.Description,
                MinRate = type.MinRate
            }).ToListAsync(cancellationToken);

        // get shifts.
        result.Shifts = await _unitOfWork
            .ShiftRepository
            .GetAllReadOnly()
            .Where(shift => shift.EmployerId == request.EmployerId)
            .Select(shift => new ShiftDto()
            {
                Id = shift.Id,
                Name = shift.Name,
                StartTime = DateTime.Now.Date.Add(shift.StartTime.ToTimeSpan()),
                EndTime = DateTime.Now.Date.Add(shift.EndTime.ToTimeSpan())
            }).ToListAsync(cancellationToken);

        // get job types from enum.
        result.JobTypes = Enum.GetValues(typeof(JobTypeEnum))
                           .Cast<JobTypeEnum>()
                           .Select(e => new JobTypeDto() { Id = (byte)e, Name = GetEnumDescription(e) })
                           .ToList();

        // get employee categories from enum.
        result.EmployeeCategories = Enum.GetValues(typeof(EmployeeCategoryEnum))
                           .Cast<EmployeeCategoryEnum>()
                           .Select(e => new EmployeeCategoryDto() { Id = (byte)e, Name = GetEnumDescription(e) })
                           .ToList();

        // get break time from enum.
        result.BreakTime = Enum.GetValues(typeof(BreakTimeEnum))
                           .Cast<BreakTimeEnum>()
                           .Select(e => (byte)e)
                           .ToList();

        return Result<GetAPIsForJobForEmployerResponce>.Success(result, "Data collected successfully.");
    }
    static string GetEnumDescription(Enum value)
    {
        var fieldInfo = value.GetType().GetField(value.ToString());
        var attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute))!;

        return attribute == null ? value.ToString() : attribute.Description;
    }
}

public sealed class GetShiftsForJobHandler(IUnitOfWork _unitOfWork) : IRequestHandler<GetShiftsForJobQuery, Result<List<GetShiftsForJobResponce>>>
{
    public async Task<Result<List<GetShiftsForJobResponce>>> Handle(GetShiftsForJobQuery request, CancellationToken cancellationToken)
    {
        var jobShifts = await _unitOfWork
            .JobRepository
            .GetAllReadOnly()
                .Include(job => job.EmployeeType)
            .Where(job => job.EmployerId == request.EmployerId)
            .GroupBy(job => new { job.JobTypeId, job.EmployeeTypeId, job.ShiftStartTime, job.ShiftEndTime })
            .Select(job => new GetShiftsForJobResponce()
            {
                ShiftId = job.Select(x => x.ShiftId).First(),
                ShiftStartTime = job.Select(x => x.Date).First().Date.Date.Add(job.Key.ShiftStartTime.ToTimeSpan()),
                ShiftEndTime = job.Select(x => x.Date).First().Date.Date.Add(job.Key.ShiftEndTime.ToTimeSpan()),
                JobTypeId = job.Key.JobTypeId,
                EmployeeType = job.Select(x => x.EmployeeType.Name).First(),
                EmployeeTypeId = job.Key.EmployeeTypeId,
                HourlyRate = job.Select(x => x.HourlyRate).First(),
                BreakTime = job.Select(x => x.BreakTime).First()
            })
            .ToListAsync(cancellationToken);

        var query = from jobShift in jobShifts
                    join shift in _unitOfWork.ShiftRepository.GetAllReadOnly()
                    on jobShift.ShiftId equals shift.Id into Details
                    from m in Details.DefaultIfEmpty()
                    select new GetShiftsForJobResponce
                    {
                        ShiftId = jobShift.ShiftId,
                        ShiftEndTime = jobShift.ShiftEndTime,
                        ShiftStartTime = jobShift.ShiftStartTime,
                        JobTypeId = jobShift.JobTypeId,
                        EmployeeType = jobShift.EmployeeType,
                        EmployeeTypeId = jobShift.EmployeeTypeId,
                        HourlyRate = jobShift.HourlyRate,
                        BreakTime = jobShift.BreakTime,
                        ShiftName = jobShift.ShiftId == 0 ? "Custom" : m.Name
                    };

        var result = query.ToList();
        return Result<List<GetShiftsForJobResponce>>.Success(result, "Data collected successfully.");
    }
}

public sealed class UpdateJobHandler(IUnitOfWork _unitOfWork, IConfiguration _configuration, IMailService _mailService) : IRequestHandler<UpdateJobCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(UpdateJobCommand request, CancellationToken cancellationToken)
    {
        // begin the transaction here.
        var transaction = _unitOfWork.BeginTransaction();

        // check employer exist.
        var employer = await _unitOfWork.EmployerRepository.GetById(request.Request.EmployerId);
        if (employer is null)
        {
            return Result<bool>.Fail("Employer doesn't exist.");
        }

        // get job by id.
        var job = await _unitOfWork.JobRepository.GetById(request.Request.JobId);
        if (job is null)
        {
            return Result<bool>.Fail("Job doesn't exist.");
        }

        if (request.Request.Date.Date < DateTime.UtcNow.Date)
        {
            return Result<bool>.Fail("Date is not valid.");
        }

        // check employee type exist.
        var employeeType = await _unitOfWork.EmployeeTypeRepository.GetById(request.Request.EmployeeTypeId);
        if (employeeType is null)
        {
            return Result<bool>.Fail("Employee type doesn't exist.");
        }

        // check is status code valid.
        var isExist = Enum.IsDefined(typeof(EmployeeCategoryEnum), request.Request.EmployeeCategoryId);
        if (!isExist)
        {
            return Result<bool>.Fail("Invalid Employee Category status code.");
        }

        // convert string to timeonly.
        TimeOnly.TryParse(request.Request.ShiftStartTime, out TimeOnly startTime);
        TimeOnly.TryParse(request.Request.ShiftEndTime, out TimeOnly endTime);

        // check starttime is in correct format.
        if (startTime.Ticks is 0)
        {
            return Result<bool>.Fail("Start time isn't in correct format.");
        }

        // check endtime is in correct format.
        if (endTime.Ticks is 0)
        {
            return Result<bool>.Fail("End time isn't in correct format.");
        }

        // check shift exist.
        TimeOnly shiftStartTime = startTime;
        TimeOnly shiftEndTime = endTime;
        var isShiftExist = await _unitOfWork.ShiftRepository.GetById(request.Request.ShiftId);
        if (isShiftExist is not null)
        {
            shiftStartTime = isShiftExist.StartTime;
            shiftEndTime = isShiftExist.EndTime;
        }

        // check hourly rate is valid.
        if (employeeType.MinRate > request.Request.HourlyRate)
        {
            return Result<bool>.Fail($"Rate should be greater than {employeeType.MinRate}.");
        }

        // check break time is valid.
        var isBreakTimeValid = Enum.IsDefined(typeof(BreakTimeEnum), request.Request.BreakTime);
        if (!isBreakTimeValid)
        {
            return Result<bool>.Fail("Invalid break time.");
        }

        var getJobMinutesPerDay = DateHelper.GetTotalMinutes(startTime, endTime);
        var getJobHoursPerDay = DateHelper.GetTotalHours(startTime, endTime);
        var breakMinutes = (double)request.Request.BreakTime;
        var ratePerMinute = (double)(request.Request.HourlyRate / 60);

        var CostPerShift = ((getJobMinutesPerDay) - breakMinutes) * ratePerMinute;

        job.Date = request.Request.Date;
        job.EmployeeTypeId = request.Request.EmployeeTypeId;
        job.EmployeeCategoryId = request.Request.EmployeeCategoryId;
        job.ShiftId = request.Request.ShiftId;
        job.ShiftStartTime = shiftStartTime;
        job.ShiftEndTime = shiftEndTime;
        job.BreakTime = request.Request.BreakTime;
        job.CostPershift = (decimal)CostPerShift;
        job.CostPershiftPerDay = (decimal)CostPerShift;
        job.JobHoursPerDay = (decimal)getJobHoursPerDay;
        job.IsFixedRate = request.Request.IsFixedRate;


        // calculate rates
        job.HourlyRate = request.Request.HourlyRate;

        job.SelfCommission = employer.SelfCommission;
        job.HourlyRateAfterSelfCommission = request.Request.HourlyRate - (request.Request.HourlyRate * employer.SelfCommission / 100);

        job.PayrollCommission = employer.PayrollCommission;
        job.HourlyRateAfterPayrollCommission = request.Request.HourlyRate - (request.Request.HourlyRate * employer.PayrollCommission / 100);

        job.LimitedCommission = employer.LimitedCommission;
        job.HourlyRateAfterLimitedCommission = request.Request.HourlyRate - (request.Request.HourlyRate * employer.LimitedCommission / 100);

        if (job.IsFixedRate)
        {
            job.FixedRate = request.Request.FixedRate;
            var commssion = Convert.ToDecimal(_configuration["JobSettings:FixedRateCommission"] ?? "0");
            job.FixedRateAfterCommission = job.FixedRate - (job.FixedRate * commssion / 100);
        }
        else
        {
            job.FixedRate = 0;
            job.FixedRateAfterCommission = 0;
        }
        // check employer exist.
        var employee = await _unitOfWork.EmployeeRepository.GetById(request.Request.EmployeeId);
        if (employee is not null)
        {
            var assignedJob = new AssignedJob()
            {
                JobId = job.Id,
                EmployeeId = request.Request.EmployeeId,
                EmployerId = job.EmployerId,
                HourWorked = job.JobHoursPerDay,
                JobStatus = (byte)JobStatusEnum.Open,
                IsSelected = true,
                SelectedDate = DateTime.UtcNow
            };

            await _unitOfWork.AssignedJobRepository.Add(assignedJob);

            // send email to employee for job confirmation.
            string subject = $"Confirm your shift at {employer.CompanyName}:ESGO";
            string content = "You have been assigned the shift you have applied for  on JobDate from StartTime to EndTime Please confirm you are still available by clicking the link to avoid missing out .    </p><p style=\"color: #000; font-weight: 100; font-size: 20px; padding: 20px 10px 0px 10px;\"> Click here to confirm </p>   <p style=\"color:#000;font-weight:600;text-align: center;font-size:20px;padding:20px 10px 0px 10px;\">  <a href=\"Url\" style=\"background: #ED1C24 0% 0% no-repeat padding-box;border-radius: 6px;opacity: 1;color: #fff;padding: 10px 20px;width:100px;margin:0px auto;text-decoration: none;\"> Proceed </a></p>  ";
            content = content.Replace("OrganizationName", employer.CompanyName);
            content = content.Replace("JobDate", job.Date.ToString("yyyy-MM-dd"));
            content = content.Replace("StartTime", job.ShiftStartTime.ToString("hh:mm tt"));
            content = content.Replace("EndTime", job.ShiftEndTime.ToString("hh:mm tt"));
            content = content.Replace("Url", $"{_configuration["WebsiteSetting:Url"]}/jobconfirmation/{Encryption.Crypt(assignedJob.Id.ToString())}");
            string username = employer.Name;

            _ = Task.Run(() =>
            {
                _mailService.Send(employee.Email, subject, content, username);
            });
        }

        await _unitOfWork.SaveChangesAsync();
        // end the transaction here.
        transaction.Commit();

        return Result<bool>.Success(true, "Jobs updated successfully.");
    }
}

public sealed class DeleteJobHandler(IUnitOfWork _unitOfWork) : IRequestHandler<DeleteJobCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(DeleteJobCommand request, CancellationToken cancellationToken)
    {
        var job = await _unitOfWork
            .JobRepository
            .GetAllReadOnly()
            .Where(job => job.Id == request.Request.JobId && job.EmployerId == request.Request.EmployerId)
            .FirstOrDefaultAsync(cancellationToken);
        if (job is null)
        {
            return Result<bool>.Fail("Data collected successfully.");
        }

        job.CancellationReason = request.Request.Reason!;
        job.CancellationDate = DateTime.UtcNow.Date;
        job.Status = (byte)JobStatusEnum.Cancelled;

        _unitOfWork.JobRepository.Change(job);
        await _unitOfWork.SaveChangesAsync();

        return Result<bool>.Success(true, "Job deleted successfully.");
    }
}

public sealed class GetJobByIdHandler(IUnitOfWork _unitOfWork) : IRequestHandler<GetJobByIdForEmployerQuery, Result<GetJobByIdForEmployerResponce>>
{
    public async Task<Result<GetJobByIdForEmployerResponce>> Handle(GetJobByIdForEmployerQuery request, CancellationToken cancellationToken)
    {
        var job = await _unitOfWork
            .JobRepository
            .GetAllReadOnly()
            .Where(job => job.Id == request.JobId && job.EmployerId == request.EmployerId)
            .FirstOrDefaultAsync(cancellationToken);
        if (job is null)
        {
            return Result<GetJobByIdForEmployerResponce>.Fail("Job is not found.");
        }

        var result = new GetJobByIdForEmployerResponce()
        {
            Id = job.Id,
            EmployerId = job.EmployerId,
            JobTypeId = job.JobTypeId,
            EmployeeTypeId = job.EmployeeTypeId,
            BookingId = job.BookingId,
            Date = job.Date,
            ShiftId = job.ShiftId,
            ShiftStartTime = job.Date.Date.Add(job.ShiftStartTime.ToTimeSpan()),
            ShiftEndTime = job.Date.Date.Add(job.ShiftEndTime.ToTimeSpan()),
            HourlyRate = job.HourlyRate,
            BreakTime = job.BreakTime,
            CostPershift = job.CostPershift,
            EmployeeCategoryId = job.EmployeeCategoryId,
            JobDescription = job.JobDescription,
            IsDummy = job.IsDummy,
            FixedRate = job.FixedRate,
            FixedRateAfterCommission = job.FixedRateAfterCommission,
            IsFixedRate = job.IsFixedRate,
        };

        result.Applicants = await _unitOfWork.AssignedJobRepository.GetAllReadOnly()
            .Where(assignedJob => assignedJob.JobId == job.Id && assignedJob.JobStatus == (byte)JobStatusEnum.Applied)
            .CountAsync(cancellationToken);

        return Result<GetJobByIdForEmployerResponce>.Success(result, "Job collected successfully.");
    }
}

public sealed class RemoveEmployeeFromAssignedJobByIdHandler(IUnitOfWork _unitOfWork) : IRequestHandler<RemoveEmployeeFromAssignedJobByIdQuery, Result<bool>>
{
    public async Task<Result<bool>> Handle(RemoveEmployeeFromAssignedJobByIdQuery request, CancellationToken cancellationToken)
    {
        var assignedJob = await _unitOfWork
            .AssignedJobRepository
            .GetAllReadOnly()
            .Where(job => job.Id == request.AssignedJobId && job.EmployerId == request.EmployerId)
            .FirstOrDefaultAsync(cancellationToken);
        if (assignedJob is null)
        {
            return Result<bool>.Fail("Assigned Job is not found.");
        }

        assignedJob.JobStatus = (byte)JobStatusEnum.Cancelled;
        assignedJob.IsSelected = false;

        _unitOfWork.AssignedJobRepository.Change(assignedJob);
        await _unitOfWork.SaveChangesAsync();

        return Result<bool>.Success(true, "Assigned Job deleted successfully.");
    }
}

public sealed class GetApplicantsByJobIdHandler(IUnitOfWork _unitOfWork) : IRequestHandler<GetApplicantsByJobIdQuery, Result<GetApplicantsByJobIdResponce>>
{
    public async Task<Result<GetApplicantsByJobIdResponce>> Handle(GetApplicantsByJobIdQuery request, CancellationToken cancellationToken)
    {
        // get job by id.
        var jobs = _unitOfWork
            .JobRepository
            .GetAllReadOnly()
                .Include(job => job.EmployeeType)
            .Where(job => job.Id == request.JobId && job.EmployerId == request.EmployerId);

        // get shift details.
        var query = from j in jobs
                    join shift in _unitOfWork.ShiftRepository.GetAllReadOnly()
                    on j.ShiftId equals shift.Id into Details
                    from m in Details.DefaultIfEmpty()
                    select new GetApplicantsByJobIdResponce
                    {
                        JobId = j.Id,
                        Date = j.Date.Date,
                        ShiftName = j.ShiftId == 0 ? "Custom" : m.Name,
                        StartTime = j.Date.Date.Add(j.ShiftStartTime.ToTimeSpan()),
                        EndTime = j.Date.Date.Add(j.ShiftEndTime.ToTimeSpan()),
                        HourlyRate = j.HourlyRate,
                        EmployeeTypeId = j.EmployeeTypeId,
                        EmployeeType = j.EmployeeType.Name,
                        JobHoursPerDay = j.JobHoursPerDay
                    };

        var job = await query.FirstOrDefaultAsync(cancellationToken);
        if (job is null)
        {
            return Result<GetApplicantsByJobIdResponce>.Fail("Job is not found.");
        }

        var hourWorked = await _unitOfWork
            .AssignedJobRepository
            .GetAllReadOnly()
            .Where(assignedJob => assignedJob.JobId == request.JobId && assignedJob.JobStatus == (byte)JobStatusEnum.Completed)
            .GroupBy(assignedJob => new { assignedJob.EmployeeId })
            .Select(assignedJob => new
            {
                assignedJob.Key.EmployeeId,
                HourWorked = assignedJob.Sum(x => x.HourWorked)
            })
            .ToListAsync(cancellationToken);

        // get applications.
        job.Applicants = await _unitOfWork
            .AssignedJobRepository
            .GetAllReadOnly()
                .Include(assignedJob => assignedJob.Employee)
                .Include(assignedJob => assignedJob.Job)
            .Where(assignedJob => assignedJob.JobId == request.JobId)
            .Select(assignedJob => new ApplicantsGetApplicantsByJobIdResponce()
            {
                AssignedJobId = assignedJob.Id,
                EmployeeId = assignedJob.EmployeeId,
                Employee = $"{assignedJob.Employee.FirstName} {assignedJob.Employee.LastName}",
                JobStatus = assignedJob.JobStatus,
                IsSelected = assignedJob.JobStatus == (byte)JobStatusEnum.Applied ? assignedJob.IsSelected : true
            })
            .ToListAsync(cancellationToken);

        foreach (var (applicants, index) in job.Applicants.Select((v, i) => (v, i)))
        {
            // Adding 1 to start the serial number from 1
            applicants.SerialNumber = index + 1;
            // Assuming there's a property named HourWorked to sum
            applicants.HourWorked = hourWorked.Where(x => x.EmployeeId == applicants.EmployeeId).Sum(x => x.HourWorked);
        };


        return Result<GetApplicantsByJobIdResponce>.Success(job, "Applicants collected successfully.");
    }
}

public sealed class SelectEmployeeByAssignedJobIdHandler(IUnitOfWork _unitOfWork) : IRequestHandler<SelectEmployeeByAssignedJobIdQuery, Result<bool>>
{
    public async Task<Result<bool>> Handle(SelectEmployeeByAssignedJobIdQuery request, CancellationToken cancellationToken)
    {
        // get assigned job by id.
        var assignedJob = await _unitOfWork
            .AssignedJobRepository
            .GetAllReadOnly()
            .Where(assignedJob => assignedJob.Id == request.Request.AssignedJobId
                                    && assignedJob.EmployerId == request.Request.EmployerId
                                    && (assignedJob.JobStatus == (byte)JobStatusEnum.Applied || assignedJob.JobStatus == (byte)JobStatusEnum.Cancelled))
            .FirstOrDefaultAsync(cancellationToken);
        if (assignedJob is null)
        {
            return Result<bool>.Fail("Assigned Job is not found.");
        }

        assignedJob.JobStatus = (byte)JobStatusEnum.Confirmed;
        assignedJob.IsSelected = true;
        assignedJob.SelectedDate = DateTime.UtcNow;

        _unitOfWork.AssignedJobRepository.Change(assignedJob);
        await _unitOfWork.SaveChangesAsync();

        return Result<bool>.Success(true, "Applicants selected successfully.");
    }
}

public sealed class GetEmployeeDetailsByIdHandler(IUnitOfWork _unitOfWork) : IRequestHandler<GetEmployeeDetailsByIdQuery, Result<GetEmployeeDetailsByIdResponce>>
{
    public async Task<Result<GetEmployeeDetailsByIdResponce>> Handle(GetEmployeeDetailsByIdQuery request, CancellationToken cancellationToken)
    {
        // get employee by id.
        var employee = await _unitOfWork
            .EmployeeRepository
            .GetAllReadOnly()
            .Where(employee => employee.Id == request.EmployeeId)
            .Select(employee => new GetEmployeeDetailsByIdResponce
            {
                Id = employee.Id,
                FullName = $"{employee.FirstName} {employee.LastName}",
                ProfileImageUrl = employee.ProfileImageUrl,
                DbsNumber = employee.DbsNumber,
                DbsCertificateUrl = employee.DbsCertificateUrl,
                DbsCertificateRejectionReason = employee.DbsCertificateRejectionReason,
                DbsCertificateStatus = employee.DbsCertificateStatus,
                DbsExpiryDate = employee.DbsExpiryDate,
                DbsNumberStatus = employee.DbsNumberStatus,
                NMCPin = employee.NMCPin,
                EmployeeTypeId = employee.EmployeeTypeId,
                IsNMCPinVerified = employee.NMCPinStatus == (byte)NMCPinStatusEnum.Approve ? true : false,
                IsReferenceVerified = false, // need to handle.
                IsRightToWorkVerified = !string.IsNullOrEmpty(employee.BiometricResidenceCardUrl) && !string.IsNullOrEmpty(employee.PassportUrl) ? true : false,
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (employee is null)
        {
            return Result<GetEmployeeDetailsByIdResponce>.Fail("Assigned Job is not found.");
        }

        // check is employee favourite.
        employee.IsFavourite = await _unitOfWork.EmployerFavouriteRepository.GetAllReadOnly()
            .Where(favourite => favourite.EmployeeId == request.EmployeeId && favourite.EmployerId == request.EmployerId)
            .AnyAsync(cancellationToken);

        // get employee ratings.
        employee.Rating = await _unitOfWork.TimesheetRepository.GetAllReadOnly()
            .Where(timesheet => timesheet.EmployeeId == request.EmployeeId)
            .Select(timesheet => timesheet.Rating)
            .DefaultIfEmpty() // Handle case where there are no matching timesheets
            .AverageAsync(rating => rating, cancellationToken);
        employee.Rating = Math.Round(employee.Rating);

        // get employee document by employee id.
        employee.TrainingDocuments = await _unitOfWork.EmployeeDocumentRepository.GetAllReadOnly()
                .Include(employeeDocument => employeeDocument.Document)
            .Where(employeeDocument => employeeDocument.EmployeeId == request.EmployeeId)
            .Select(employeeDocument => new TrainingDocumenForGetEmployeeDetailsByIdResponce()
            {
                EmployeeDocumentId = employeeDocument.Id,
                DocumentName = employeeDocument.Document.Name,
                DocumentUrl = employeeDocument.DocumentUrl,
                ExpiryDate = employeeDocument.ExpiryDate.Date,
                Status = employeeDocument.Status,
                Reason = employeeDocument.Reason
            })
            .ToListAsync(cancellationToken);

        // get dbs documents.
        employee.DbsDocuments = await _unitOfWork.DbsDocumentRepository.GetAllReadOnly()
                .Include(dbs => dbs.DocumentType)
            .Where(dbs => dbs.EmployeeId == request.EmployeeId)
            .Select(dbs => new DbsForGetEmployeeDetailsByIdResponce()
            {
                DocumentType = dbs.DocumentType.Name,
                GroupNo = dbs.DocumentType.GroupNo,
                Url = dbs.Url,
                DocumentTypeId = dbs.DocumentTypeId,
                DocumentNumber = dbs.DocumentNumber,
                Status = dbs.Status,
                RejectionReason = dbs.RejectionReason
            })
            .ToListAsync(cancellationToken);
        employee.IsAllDbsDocumentsVerified = employee.DbsDocuments.Count == employee.DbsDocuments.Where(x => x.Status == (byte)DbsDocumentStatusEnum.Approve).Count() ? true : false;

        // get experiences.
        employee.Experiences = await _unitOfWork.EmployementRepository.GetAllReadOnly()
            .Where(employement => employement.EmployeeId == request.EmployeeId)
            .Select(employement => new EmployementDto()
            {
                CompanyName = employement.CompanyName,
                StartDate = employement.StartDate.Date,
                EndDate = employement.EndDate.Date
            })
            .ToListAsync(cancellationToken);

        // get feedbacks.
        employee.Feedbacks = await _unitOfWork.FeedbackRepository.GetFeedbacksByEmployeeId(request.EmployeeId).ToListAsync(cancellationToken);

        return Result<GetEmployeeDetailsByIdResponce>.Success(employee, "Applicants selected successfully.");
    }
}

public sealed class AddToFavouriteForEmployerHandler(IUnitOfWork _unitOfWork)
    : IRequestHandler<AddToFavouriteForEmployerCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(AddToFavouriteForEmployerCommand request, CancellationToken cancellationToken)
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
            .EmployerFavouriteRepository
            .GetAllReadOnly()
            .Where(favourite => favourite.EmployerId == request.Request.EmployerId && favourite.EmployeeId == request.Request.EmployeeId)
            .FirstOrDefaultAsync(cancellationToken);

        if (favourite is null)
        {
            // add favourite
            var data = new EmployerFavourite()
            {
                EmployerId = request.Request.EmployerId,
                EmployeeId = request.Request.EmployeeId
            };
            await _unitOfWork.EmployerFavouriteRepository.Add(data);
        }
        else
        {
            _unitOfWork.EmployerFavouriteRepository.DeleteByEntity(favourite);
        }
        await _unitOfWork.SaveChangesAsync();

        return Result<bool>.Success(true, "changed successfully.");
    }
}

public sealed class DeleteFeedbackHandler(IUnitOfWork _unitOfWork) : IRequestHandler<DeleteFeedbackCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(DeleteFeedbackCommand request, CancellationToken cancellationToken)
    {
        // check is employee exists.
        var feedback = await _unitOfWork.FeedbackRepository.GetById(request.FeedbackId);
        if (feedback is null)
        {
            return Result<bool>.Fail("Feedback doesn't exist.");
        }

        _unitOfWork.FeedbackRepository.DeleteByEntity(feedback);
        await _unitOfWork.SaveChangesAsync();

        return Result<bool>.Success(true, "Feedback deleted.");
    }
}

public sealed class UpdateFeedbackHandler(IUnitOfWork _unitOfWork) : IRequestHandler<UpdateFeedbackCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(UpdateFeedbackCommand request, CancellationToken cancellationToken)
    {
        // check is employee exists.
        var feedback = await _unitOfWork.FeedbackRepository.GetById(request.Request.FeedbackId);
        if (feedback is null)
        {
            return Result<bool>.Fail("Feedback doesn't exist.");
        }

        feedback.Description = request.Request.Description;

        _unitOfWork.FeedbackRepository.Change(feedback);
        await _unitOfWork.SaveChangesAsync();

        return Result<bool>.Success(true, "Feedback updated.");
    }
}

public sealed class GetEmployeesWorkedUnderEmployerHandler(IUnitOfWork _unitOfWork) : IRequestHandler<GetEmployeesWorkedUnderEmployerQuery, Result<List<GetEmployeesWorkedUnderEmployerResponce>>>
{
    public async Task<Result<List<GetEmployeesWorkedUnderEmployerResponce>>> Handle(GetEmployeesWorkedUnderEmployerQuery request, CancellationToken cancellationToken)
    {
        // get employee ids from assigned job.
        var employeeIds = await _unitOfWork.AssignedJobRepository.GetAllReadOnly()
            .Where(assignedJob => assignedJob.EmployerId == request.EmployerId)
            .Select(assignedJob => assignedJob.EmployeeId)
            .ToListAsync(cancellationToken);

        // get employee details by employee ids.
        var employees = await _unitOfWork.EmployeeRepository.GetAllReadOnly()
            .Include(employee => employee.EmployeeType)
            .Where(employee => employeeIds.Contains(employee.Id))
            .Select(employee => new GetEmployeesWorkedUnderEmployerResponce()
            {
                Id = employee.Id,
                EmployeeType = employee.EmployeeType.Name,
                Name = $"{employee.FirstName} {employee.LastName}"
            })
            .ToListAsync(cancellationToken);

        return Result<List<GetEmployeesWorkedUnderEmployerResponce>>.Success(employees, "Employees collected.");
    }
}

public sealed class GetFavouriteEmployeesHandler(IUnitOfWork _unitOfWork) : IRequestHandler<GetFavouriteEmployeesQuery, Result<List<GetFavouriteEmployeesResponce>>>
{
    public async Task<Result<List<GetFavouriteEmployeesResponce>>> Handle(GetFavouriteEmployeesQuery request, CancellationToken cancellationToken)
    {
        // get employee ids from employer favourites.
        var employeeIds = await _unitOfWork.EmployerFavouriteRepository.GetAllReadOnly()
            .Where(assignedJob => assignedJob.EmployerId == request.EmployerId)
            .Select(assignedJob => assignedJob.EmployeeId)
            .ToListAsync(cancellationToken);

        // get employee details by employee ids.
        var employees = await _unitOfWork.EmployeeRepository.GetAllReadOnly()
            .Include(employee => employee.EmployeeType)
            .Where(employee => employeeIds.Contains(employee.Id))
            .Select(employee => new GetFavouriteEmployeesResponce()
            {
                Id = employee.Id,
                EmployeeType = employee.EmployeeType.Name,
                Name = $"{employee.FirstName} {employee.LastName}"
            })
            .ToListAsync(cancellationToken);

        return Result<List<GetFavouriteEmployeesResponce>>.Success(employees, "Employees collected.");
    }
}