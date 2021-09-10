namespace Chatto.BLL.Interfaces
{
	public interface IServiceCreator
	{
		IUserService CreateUserService();

		IMessageService CreateMessageService();
	}
}
