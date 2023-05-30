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
        return new LocalCaseworkRepository(new LocalActionItemRepository());
    }

    public static LocalCustomerRepository GetCustomerRepository()
    {
        ClearAllStaticData();
        return new LocalCustomerRepository(new LocalContactRepository(),
            new LocalCaseworkRepository(new LocalActionItemRepository()));
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

    public static LocalActionItemTypeRepository GetActionItemTypeRepository()
    {
        ClearAllStaticData();
        return new LocalActionItemTypeRepository();
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
