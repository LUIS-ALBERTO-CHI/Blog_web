using FwaEu.Fwamework.Data;
using System.Threading;
using System.Threading.Tasks;

namespace FwaEu.Fwamework.Data
{
	public interface IDeletableFile : IFile
	{
		Task DeleteAsync();
	}
}
