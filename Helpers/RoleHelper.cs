using System.IO;

namespace Diploma.Helpers
{
    public static class RoleHelper
    {
        public static List<Role> GetRoles()
        {
            return Enum.GetValues(typeof(Role)).Cast<Role>().ToList();
        }

        public static string GetRoleName(Role r)
        {
            switch (r) {
                case Role.None:
                    return "Без роли";
                case Role.Jurist:
                    return "Юрист";
                case Role.Accountant:
                    return "Бухгалтео";
                case Role.HumanResources:
                    return "Кадровый отдел";
                case Role.TechSupport:
                    return "Тех. поддержка";
                case Role.Director:
                    return "Директор";
                default:
                    return "Админ";
            }
        }
    }
}
