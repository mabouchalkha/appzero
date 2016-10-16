﻿namespace Sarwa.Core.Common.Contracts
{
    public interface IDataRepositoryFactory
    {
        T GetDataRepository<T>() where T : IBaseRepository;
    }
}
