using Domain.SuperAdminModels.Tenant;
using Helper.Enum;
using System.Text;
using UseCase.SuperAdmin.ExportCsvTenantList;
using Helper.Extension;

namespace Interactor.SuperAdmin;

public class ExportCsvTenantListInteractor : IExportCsvTenantListInputPort
{
    private readonly ITenantRepository _tenantRepository;

    public ExportCsvTenantListInteractor(ITenantRepository tenantRepository)
    {
        _tenantRepository = tenantRepository;
    }

    public ExportCsvTenantListOutputData Handle(ExportCsvTenantListInputData inputData)
    {
        try
        {
            List<string> result = new();

            // get tenant data from database
            var tenantData = _tenantRepository.GetTenantList(inputData.SearchModel, inputData.SortDictionary, 0, 0, true);
            if (!tenantData.TenantList.Any())
            {
                return new ExportCsvTenantListOutputData(new(), ExportCsvTenantListStatus.NoData);
            }

            #region get index in csv file
            Dictionary<TenantEnum, string> columnNameDictionary = new();
            if (inputData.ColumnView.Any())
            {
                foreach (var item in inputData.ColumnView.Where(item => ColumnCsvName.ColumnNameTenantDictionary.ContainsKey(item)).ToList())
                {
                    columnNameDictionary.Add(item, ColumnCsvName.ColumnNameTenantDictionary[item]);
                }
            }
            else
            {
                columnNameDictionary = ColumnCsvName.ColumnNameTenantDictionary;
            }
            StringBuilder headerCsv = new();
            int index = 1;
            int columnCount = columnNameDictionary.Count;
            foreach (var column in columnNameDictionary)
            {
                headerCsv.Append(column.Value);
                if (index < columnCount)
                {
                    headerCsv.Append(",");
                }
                index++;
            }
            result.Add(headerCsv.ToString());
            #endregion

            #region add data to result csv
            foreach (var tenant in tenantData.TenantList)
            {
                StringBuilder tenantDataString = new();
                index = 1;
                columnCount = columnNameDictionary.Count;
                foreach (var column in columnNameDictionary)
                {
                    switch (column.Key)
                    {
                        case TenantEnum.CreateDate:
                            tenantDataString.Append(tenant.CreateDate.ToString("dd/MM/yyyy HH:mm"));
                            break;
                        case TenantEnum.TenantId:
                            tenantDataString.Append(tenant.TenantId.AsString());
                            break;
                        case TenantEnum.Domain:
                            tenantDataString.Append(tenant.SubDomain);
                            break;
                        case TenantEnum.AdminId:
                            tenantDataString.Append(tenant.AdminId.AsString());
                            break;
                        case TenantEnum.HospitalName:
                            tenantDataString.Append(tenant.Hospital);
                            break;
                        case TenantEnum.Type:
                            /// <summary>
                            /// 0: sharing; 1: dedicate(premium)
                            /// </summary>
                            if (tenant.Type == 1)
                            {
                                tenantDataString.Append("Dedicated");
                            }
                            else
                            {
                                tenantDataString.Append("Sharing");
                            }
                            break;
                        case TenantEnum.Size:
                            tenantDataString.Append(tenant.Size.AsString());
                            /// <summary>
                            /// 1: MB; 2: GB
                            /// </summary>
                            if (tenant.SizeType == 1)
                            {
                                tenantDataString.Append(" MB");
                            }
                            else
                            {
                                tenantDataString.Append(" GB");
                            }
                            break;
                        case TenantEnum.StorageFull:
                            tenantDataString.Append(tenant.StorageFull.AsString());
                            tenantDataString.Append(" %");
                            break;
                        case TenantEnum.StatusTenant:
                            /// <summary>
                            /// Undefined = 0
                            /// Pending = 1
                            /// Failed = 2
                            /// Running = 3
                            /// Stopping = 4
                            /// Stopped = 5
                            /// Shutting-down = 6
                            /// Terminated = 7
                            /// </summary>
                            switch (tenant.StatusTenant)
                            {
                                case 0:
                                    tenantDataString.Append("Undefined");
                                    break;
                                case 1:
                                    tenantDataString.Append("Pending");
                                    break;
                                case 2:
                                    tenantDataString.Append("Failed");
                                    break;
                                case 3:
                                    tenantDataString.Append("Running");
                                    break;
                                case 4:
                                    tenantDataString.Append("Stopping");
                                    break;
                                case 5:
                                    tenantDataString.Append("Stopped");
                                    break;
                                case 6:
                                    tenantDataString.Append("Shutting-down");
                                    break;
                                case 7:
                                    tenantDataString.Append("Terminated");
                                    break;
                            }
                            break;
                    }
                    if (index < columnCount)
                    {
                        tenantDataString.Append(",");
                    }
                    index++;
                }
                result.Add(tenantDataString.ToString());
            }
            #endregion
            return new ExportCsvTenantListOutputData(result, ExportCsvTenantListStatus.Successed);
        }
        finally
        {
            _tenantRepository.ReleaseResource();
        }
    }
}
