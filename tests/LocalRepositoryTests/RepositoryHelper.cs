using Sbeap.LocalRepository.Identity;
using Sbeap.LocalRepository.Repositories;
using Sbeap.TestData;
using Sbeap.TestData.Identity;

namespace LocalRepositoryTests;

public static class RepositoryHelper
{
    public static LocalAgencyRepository GetAgencyRepository()
    {
        ClearAllStaticData();
        return new LocalAgencyRepository();
    }

    public static LocalCaseworkRepository GetCaseworkRepository()
    {
        ClearAllStaticData();
        return new LocalCaseworkRepository();
    }

    public static LocalCustomerRepository GetCustomerRepository()
    {
        ClearAllStaticData();
        return new LocalCustomerRepository();
    }

    public static LocalOfficeRepository GetOfficeRepository()
    {
        ClearAllStaticData();
        return new LocalOfficeRepository();
    }

    public static LocalUserStore GetLocalUserStore()
    {
        ClearAllStaticData();
        return new LocalUserStore();
    }

    private static void ClearAllStaticData()
    {
        ContactData.ClearData();
        CaseworkData.ClearData();
        AgencyData.ClearData();
        ActionItemData.ClearData();
        ActionItemTypeData.ClearData();
        CustomerData.ClearData();
        OfficeData.ClearData();
        UserData.ClearData();
    }
}
