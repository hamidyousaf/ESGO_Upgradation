namespace Infrastructure.Repositories;

internal sealed class EmployeeTypeRepository(IMapper _mapper, DbContext dbContext) : GenericRepository<EmployeeType>(dbContext), IEmployeeTypeRepository
{
    public IQueryable<GetEmployeeTypeByIdResponse> GetEmployeeTypeById(byte emplyeeTypeId)
    {
        return GetAllReadOnly()
                .Where(x => x.Id == emplyeeTypeId)
                .ProjectTo<GetEmployeeTypeByIdResponse>(_mapper.ConfigurationProvider);
    }

    public IQueryable<EmployeeTypeResponse> GetEmployeeTypes()
    {
        return GetAllReadOnly()
            .ProjectTo<EmployeeTypeResponse>(_mapper.ConfigurationProvider);
    }
}