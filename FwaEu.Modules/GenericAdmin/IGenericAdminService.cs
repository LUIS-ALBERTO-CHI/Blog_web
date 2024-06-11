namespace FwaEu.Modules.GenericAdmin
{
	public interface IGenericAdminService
	{
		IGenericAdminModelConfiguration GetConfiguration(string key);
	}
}
