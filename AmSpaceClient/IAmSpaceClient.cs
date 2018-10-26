﻿using AmSpaceModels;
using AmSpaceModels.Enums;
using AmSpaceModels.Idp;
using AmSpaceModels.JobMap;
using AmSpaceModels.Organization;
using AmSpaceModels.Performance;
using AmSpaceModels.Sap;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace AmSpaceClient
{
    public interface IAmSpaceClient : IDisposable
    {
        Task<bool> LoginRequestAsync(string userName, SecureString password, IAmSpaceEnvironment environment);
        Task<Profile> GetProfileAsync();
        Task<Profile> GetProfileByIdAsync(long Id);
        Task<IEnumerable<Competency>> GetCompetenciesAsync();
        Task<IEnumerable<Level>> GetLevelsAsync();
        Task<CompetencyAction> GetCompetencyActionsAsync(long competencyId);
        Task<bool> UpdateActionAsync(UpdateAction model, long competencyId);
        Task<bool> LogoutRequestAsync();
        Task<BitmapSource> GetAvatarAsync(string link);
        Task<IEnumerable<AmspaceDomain>> GetOrganizationStructureAsync();
        Task<IEnumerable<AmspaceUser>> GetDomainUsersAsync(int domainId);
        /// <summary>
        /// Allows only Create and Update users
        /// </summary>
        /// <param name="user"></param>
        /// <returns>True in case of successful request</returns>
        Task<bool> PutUserAsync(SapUser user);
        /// <summary>
        /// Enables all changes to Domains: Create, Update and Disable Domains. To deactivate Domain,
        /// set "Status" prop false and "ParentDomainId" to null
        /// </summary>
        /// <param name="domain"></param>
        /// <returns>True in case of successful request</returns>
        Task<bool> PutDomainAsync(SapDomain domain);
        Task<bool> DisableUserAsync(SapUserDelete userToDelete);
        Task<IEnumerable<Position>> GetPositionsInLevelsAsync(IEnumerable<Level> levels);
        Task<IEnumerable<Position>> GetPositionsAsync();
        Task<IEnumerable<People>> GetPeopleAsync();
        Task<IEnumerable<People>> GetPeopleInPositionsAsync(IEnumerable<Position> positions);
        Task<IEnumerable<Kpi>> GetFinancialKpiAsync(ContractSearch userContract);
        Task<IEnumerable<Kpi>> GetNonFinancialKpiAsync(ContractSearch userContract);
        Task<IEnumerable<Goal>> GetGoalsAsync(ContractSearch userContract, Roadmap roadmap);
        Task<Roadmaps> GetRoadmapsAsync(ContractSearch userContract);
        Task<Kpi> CreateFinancialKpiAsync(ContractSearch userContract, Kpi kpi);
        Task<Kpi> CreateNonFinancialKpiAsync(ContractSearch userContract, Kpi kpi);
        Task<Kpi> UpdateFinancialKpiAsync(ContractSearch userContract, Kpi kpi);
        Task<Kpi> UpdateNonFinancialKpiAsync(ContractSearch userContract, Kpi kpi);
        Task<Roadmap> CreateRoadmapAsync(ContractSearch userContract, Roadmap roadmap);
        Task<Roadmap> UpdateRoadmapAsync(ContractSearch userContract, Roadmap roadmap);
        Task<Goal> CreateGoalAsync(ContractSearch userContract, Roadmap roadmap, GoalNew goal);
        Task<Goal> UpdateGoalAsync(ContractSearch userContract, Roadmap roadmap, Goal goal);
        Task<bool> DeleteGoalAsync(ContractSearch userContract, Roadmap roadmap, Goal goal);
        Task<bool> DeleteFinancialKpiAsync(ContractSearch userContract, Kpi kpi);
        Task<bool> DeleteNonFinancialKpiAsync(ContractSearch userContract, Kpi kpi);
        Task<IEnumerable<Brand>> GetBrandsAsync();
        Task<IEnumerable<Country>> GetCountriesAsync(Brand brand);
        Task<IEnumerable<SearchUserResult>> FindUser(string query, Brand brand, OrganizationGroup orgGroup, UserStatus status, string domain);
        Task<IEnumerable<OrganizationGroup>> GetOrganizationGroupsAsync();
        Task<IEnumerable<GoalWeight>> UpdateGoalsWeight(IEnumerable<GoalWeight> weights, ContractSearch userContract, Roadmap roadmap);
        Task<TemporaryAccount> CreateTemporaryAccount(TemporaryAccount accountInfo);
        Task<ExternalAccount> CreateExternalAccount(ExternalAccount accountInfo);
        Task<IEnumerable<JobMap>> FindJobMapAsync(string country, string brand, int level, string position);
        Task<JobDescription> UpdateJobDescriptionAsync(JobDescription jobDescription);
        Task<bool> DeleteJobResponsibilityAsync(JobResponsibility responsibility);
        Task<List<JobResponsibility>> GetJobResponsibilitiesAsync(JobMap jobMap);
        Task<JobResponsibility> CreateJobResponsibilityAsync(JobResponsibility responsibility);
    }
}
