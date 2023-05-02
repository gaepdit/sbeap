using Sbeap.LocalRepository.Identity;
using Sbeap.LocalRepository.Repositories;
using Sbeap.TestData;
using Sbeap.TestData.Identity;

namespace LocalRepositoryTests;

public static class RepositoryHelper
{
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
        AgencyData.ClearData();
        OfficeData.ClearData();
        UserData.ClearData();
    }
}
