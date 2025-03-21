﻿namespace Application.Abstractions.Generic;

public interface IGenericRepository<TEntity> where TEntity : class
{
    IQueryable<TEntity> GetWithCondition(Expression<Func<TEntity, bool>> expression);
    IQueryable<TEntity> GetWithConditionReadOnly(Expression<Func<TEntity, bool>> expression);
    IQueryable<TEntity> GetAll();
    IQueryable<TEntity> GetAllReadOnly();
    Task<TEntity> GetById<T>(T id);
    Task Add(TEntity entity);
    void Change(TEntity entity);
    Task DeleteById<T>(T id);
    void DeleteByEntity(TEntity entity);
    Task DeleteRange<T>(List<T> ids);
    Task SoftDeleteById<T>(T id);
    Task SoftDelete(object entity);
    IQueryable<TEntity> GetNotDeleted();
    void ChangeRange(List<TEntity> entity);
    void RemoveRange(List<TEntity> entities);
    Task AddRangeAsync(List<TEntity> entities);
}
