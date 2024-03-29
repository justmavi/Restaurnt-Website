﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Restaurant_Website.Domain.Core;

namespace Restaurant_Website.Services.Interfaces
{
    public interface IVacancyService
    {
        Task<IEnumerable<VacancyLang>> GetTranslationsAsync(string lang);
        Task<Vacancy> GetByIdAsync(int id);

        Task<bool> CreateAsync(Vacancy vacancy);
        Task<bool> EditAsync(Vacancy vacancy);
        Task<bool> DeleteAsync(Vacancy vacancy);
    }
}
