namespace Infrastructure.Repositories;

internal sealed class ReferenceRepository(DbContext dbContext, IMapper _mapper) : GenericRepository<Reference>(dbContext), IReferenceRepository
{
    public IQueryable<GetPersonalReferenceByEmployeeIdResponce> GetPersonalReferenceByEmployeeId(int employeeId)
    {
        return GetAllReadOnly()
                .Where(x => x.EmployeeId == employeeId && x.ReferenceTypeId == (byte) ReferenceTypeEnum.Personal)
                .ProjectTo<GetPersonalReferenceByEmployeeIdResponce>(_mapper.ConfigurationProvider);
    }

    public IQueryable<GetProfessionalReferenceByEmployeeIdResponce> GetProfessionalReferenceByEmployeeId(int employeeId)
    {
        return GetAllReadOnly()
                .Where(x => x.EmployeeId == employeeId && x.ReferenceTypeId == (byte) ReferenceTypeEnum.Professional)
                .ProjectTo<GetProfessionalReferenceByEmployeeIdResponce>(_mapper.ConfigurationProvider);
    }
}